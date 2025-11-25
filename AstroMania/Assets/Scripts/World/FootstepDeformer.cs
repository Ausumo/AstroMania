using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Deforms meshes or Terrain where the player steps (footprints).
/// - Attach to the player (or a foot GameObject)
/// - Raycasts downward and deforms either MeshFilters or Terrain at the hit point
/// - Deformations recover over time
/// </summary>
public class FootstepDeformer : MonoBehaviour
{
    [FormerlySerializedAs("stepRadius")]
    [Tooltip("Radius of the deformation in meters")]
    public float stepRadius = 0.5f;

    [FormerlySerializedAs("stepDepth")]
    [Tooltip("Base indentation depth in meters (positive = up, negative = down). Actual depth scales with impact force.)")]
    public float stepDepth = 0.05f;

    [FormerlySerializedAs("recoverSpeed")]
    [Tooltip("Speed at which the mesh/terrain recovers to its original state (meters per second)")]
    public float recoverSpeed = 1f;

    [FormerlySerializedAs("stepDistance")]
    [Tooltip("Minimum distance between two automatic steps")]
    public float stepDistance = 0.6f;

    [FormerlySerializedAs("stepCooldown")]
    [Tooltip("Minimum time between two automatic steps")]
    public float stepCooldown = 0.15f;

    [FormerlySerializedAs("groundLayer")]
    [Tooltip("LayerMask for walkable/deformable objects")]
    public LayerMask groundLayer = ~0; // default: everything

    [FormerlySerializedAs("impactDepthFactor")]
    [Tooltip("Factor to convert impact force into a depth multiplier")]
    public float impactDepthFactor = 0.8f;

    [FormerlySerializedAs("minImpactMultiplier")]
    [Tooltip("Minimum multiplier applied to stepDepth when impact is very small")]
    public float minImpactMultiplier = 0.5f;

    [FormerlySerializedAs("maxImpactMultiplier")]
    [Tooltip("Maximum multiplier to avoid excessive deformation")]
    public float maxImpactMultiplier = 3f;

    // Internal state
    private Vector3 _lastStepPosition;
    private float _lastStepTime;

    // Track which terrains we've cloned for runtime edits so we don't modify assets on disk
    private readonly HashSet<Terrain> _clonedTerrains = new HashSet<Terrain>();

    // Data structure for storing per-mesh info
    private class DeformableMesh
    {
        public Mesh mesh; // instance of the mesh we modify
        public Vector3[] originalVertices; // original reference
        public Vector3[] modifiedVertices; // current state
        public Transform transform;
        public MeshCollider meshCollider; // optional
    }

    // Mapping MeshFilter -> DeformableMesh
    private readonly Dictionary<MeshFilter, DeformableMesh> _meshes = new Dictionary<MeshFilter, DeformableMesh>();

    private void Start()
    {
        _lastStepPosition = transform.position;
        _lastStepTime = -stepCooldown;
    }

    private void Update()
    {
        // Recover modified meshes each frame. Automatic step triggering is removed.
        // Call `TriggerStepAtPosition(...)` from the place where footstep sounds or animation events are handled
        // (AnimationEvent, Audio callback, or PlayerMovement) to synchronize sound and deformation.
        RecoverMeshes();
    }

    /// <summary>
    /// Public API: trigger a step at a world position with a given impact force.
    /// impactForce should be positive; larger values create deeper/wider deformations.
    /// </summary>
    public void TriggerStepAtPosition(Vector3 worldPosition, float impactForce)
    {
        // Map impact force to a depth multiplier
        float multiplier = Mathf.Clamp(impactForce * impactDepthFactor, minImpactMultiplier, maxImpactMultiplier);
        float effectiveDepth = stepDepth * multiplier;

        // Perform a short downward raycast and apply deformation with the computed depth
        Ray ray = new Ray(worldPosition + Vector3.up * 0.3f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f, groundLayer, QueryTriggerInteraction.Ignore))
        {
            // Terrain handling: operate on a runtime copy so the original asset is not modified
            var terrain = hit.collider.GetComponent<Terrain>();
            if (terrain != null)
            {
                EnsureRuntimeTerrainCopy(terrain);
                ApplyDeformationTerrain(terrain, hit.point, effectiveDepth);
                return;
            }

            var meshFilter = hit.collider.GetComponent<MeshFilter>();
            if (meshFilter == null) return;

            DeformableMesh dm = GetOrCreateDeformableMesh(meshFilter);
            ApplyDeformation(dm, hit.point, hit.normal, effectiveDepth);
        }
    }

    /// <summary>
    /// Backwards-compatible helper that triggers a default step at the provided world position.
    /// </summary>
    private void TryMakeStep(Vector3 worldPosition)
    {
        TriggerStepAtPosition(worldPosition, 1f);
    }

    /// <summary>
    /// Ensure the terrain has a runtime instance of TerrainData so modifications won't be saved to the original asset.
    /// </summary>
    private void EnsureRuntimeTerrainCopy(Terrain terrain)
    {
        if (terrain == null) return;
        if (_clonedTerrains.Contains(terrain)) return;

        TerrainData original = terrain.terrainData;
        if (original == null) return;

        TerrainData runtimeData = Instantiate(original);
        runtimeData.name = original.name + "_RuntimeInstance";
        terrain.terrainData = runtimeData;

        _clonedTerrains.Add(terrain);
    }

    /// <summary>
    /// Get or create a DeformableMesh for a MeshFilter. Instantiates the mesh so the original asset is not modified.
    /// </summary>
    private DeformableMesh GetOrCreateDeformableMesh(MeshFilter mf)
    {
        if (_meshes.TryGetValue(mf, out DeformableMesh dm)) return dm;

        Mesh shared = mf.sharedMesh;
        if (shared == null) return null;

        Mesh instance = Instantiate(shared);
        instance.name = shared.name + "_DeformInstance";

        // Assign instance to MeshFilter and MeshCollider if present
        mf.mesh = instance;
        var meshCollider = mf.GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = instance;
        }

        dm = new DeformableMesh
        {
            mesh = instance,
            originalVertices = instance.vertices,
            modifiedVertices = instance.vertices,
            transform = mf.transform,
            meshCollider = meshCollider
        };

        _meshes[mf] = dm;
        return dm;
    }

    /// <summary>
    /// Applies a local indentation to a mesh at the given world hit point.
    /// depth specifies the indentation depth in meters.
    /// </summary>
    private void ApplyDeformation(DeformableMesh dm, Vector3 hitPointWorld, Vector3 hitNormalWorld, float depth)
    {
        if (dm == null) return;

        Vector3 localHit = dm.transform.InverseTransformPoint(hitPointWorld);

        Vector3[] verts = dm.modifiedVertices;
        Vector3[] orig = dm.originalVertices;

        bool changed = false;

        Vector3 localNormal = dm.transform.InverseTransformDirection(hitNormalWorld).normalized;

        for (int i = 0; i < verts.Length; i++)
        {
            Vector3 v = verts[i];
            float dist = Vector3.Distance(v, localHit);
            if (dist > stepRadius) continue;

            float falloff = 1f - (dist / stepRadius);
            float displacement = -depth * falloff; // negative = indent

            Vector3 target = orig[i] + localNormal * displacement;

            if ((verts[i] - target).sqrMagnitude > 0.000001f)
            {
                verts[i] = target;
                changed = true;
            }
        }

        if (changed)
        {
            dm.modifiedVertices = verts;
            dm.mesh.vertices = verts;
            dm.mesh.RecalculateBounds();
            dm.mesh.RecalculateNormals();

            if (dm.meshCollider != null)
            {
                dm.meshCollider.sharedMesh = null;
                dm.meshCollider.sharedMesh = dm.mesh;
            }
        }
    }

    /// <summary>
    /// Applies an indentation to a Terrain at the given world position.
    /// Modifies a region of the terrain's heightmap based on stepRadius and provided depth.
    /// </summary>
    private void ApplyDeformationTerrain(Terrain terrain, Vector3 hitPointWorld, float depth)
    {
        if (terrain == null) return;

        TerrainData tData = terrain.terrainData;
        Vector3 tPos = terrain.transform.position;

        int heightmapRes = tData.heightmapResolution;
        float terrainWidth = tData.size.x;
        float terrainLength = tData.size.z;
        float terrainHeight = tData.size.y;

        // Normalized coordinates (0..1) of the hit point relative to terrain
        float normX = Mathf.InverseLerp(tPos.x, tPos.x + terrainWidth, hitPointWorld.x);
        float normZ = Mathf.InverseLerp(tPos.z, tPos.z + terrainLength, hitPointWorld.z);

        int centerX = Mathf.RoundToInt(normX * (heightmapRes - 1));
        int centerZ = Mathf.RoundToInt(normZ * (heightmapRes - 1));

        int radiusSamplesX = Mathf.CeilToInt((stepRadius / terrainWidth) * heightmapRes);
        int radiusSamplesZ = Mathf.CeilToInt((stepRadius / terrainLength) * heightmapRes);

        int xFrom = Mathf.Clamp(centerX - radiusSamplesX, 0, heightmapRes - 1);
        int zFrom = Mathf.Clamp(centerZ - radiusSamplesZ, 0, heightmapRes - 1);
        int xTo = Mathf.Clamp(centerX + radiusSamplesX, 0, heightmapRes - 1);
        int zTo = Mathf.Clamp(centerZ + radiusSamplesZ, 0, heightmapRes - 1);

        int width = xTo - xFrom + 1;
        int height = zTo - zFrom + 1;

        if (width <= 0 || height <= 0) return;

        // Get the heights block (normalized 0..1)
        float[,] heights = tData.GetHeights(xFrom, zFrom, width, height);

        // Loop over block and modify heights based on distance falloff
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                int sampleX = xFrom + x;
                int sampleZ = zFrom + z;

                // World position of the sample
                float sampleWorldX = Mathf.Lerp(tPos.x, tPos.x + terrainWidth, (float)sampleX / (heightmapRes - 1));
                float sampleWorldZ = Mathf.Lerp(tPos.z, tPos.z + terrainLength, (float)sampleZ / (heightmapRes - 1));

                float dist = Vector2.Distance(new Vector2(sampleWorldX, sampleWorldZ), new Vector2(hitPointWorld.x, hitPointWorld.z));
                if (dist > stepRadius) continue;

                float falloff = 1f - (dist / stepRadius);

                // Current height in meters
                float currentHeightMeters = heights[z, x] * terrainHeight;
                // Displacement in meters (negative for indentation)
                float displacement = -depth * falloff;
                float newHeightMeters = currentHeightMeters + displacement;
                // Convert back to normalized height
                float newNormalized = Mathf.Clamp01(newHeightMeters / terrainHeight);

                heights[z, x] = newNormalized;
            }
        }

        // Apply modified heights
        tData.SetHeights(xFrom, zFrom, heights);

        // Force TerrainCollider to update so physics matches the heightmap change
        var terrainCollider = terrain.GetComponent<TerrainCollider>();
        if (terrainCollider != null)
        {
            var data = terrain.terrainData;
            terrainCollider.enabled = false;
            terrainCollider.terrainData = data;
            terrainCollider.enabled = true;

            Physics.SyncTransforms();
            terrain.Flush();

            // Deferred refresh next frame in case the engine needs one frame to rebuild internal structures
            StartCoroutine(RefreshTerrainColliderNextFrame(terrainCollider, data));
        }
    }

    private IEnumerator RefreshTerrainColliderNextFrame(TerrainCollider tc, TerrainData data)
    {
        yield return null;
        if (tc == null) yield break;
        tc.enabled = false;
        tc.terrainData = data;
        tc.enabled = true;
        Physics.SyncTransforms();
    }

    /// <summary>
    /// Recover modified meshes back to their original state over time.
    /// Note: Terrain recovery is not implemented here; terrain modifications remain for the runtime instance.
    /// </summary>
    private void RecoverMeshes()
    {
        if (recoverSpeed <= 0f) return;

        foreach (var kv in _meshes)
        {
            DeformableMesh dm = kv.Value;
            Vector3[] verts = dm.modifiedVertices;
            Vector3[] orig = dm.originalVertices;

            bool changed = false;

            for (int i = 0; i < verts.Length; i++)
            {
                if (verts[i] == orig[i]) continue;

                verts[i] = Vector3.MoveTowards(verts[i], orig[i], recoverSpeed * Time.deltaTime);
                if ((verts[i] - orig[i]).sqrMagnitude > 0.000001f) changed = true;
            }

            if (changed)
            {
                dm.modifiedVertices = verts;
                dm.mesh.vertices = verts;
                dm.mesh.RecalculateBounds();
                dm.mesh.RecalculateNormals();

                if (dm.meshCollider != null)
                {
                    dm.meshCollider.sharedMesh = null;
                    dm.meshCollider.sharedMesh = dm.mesh;
                }
            }
        }
    }

    private void OnDestroy()
    {
        // Cleanup instantiated meshes to avoid memory leaks
        foreach (var kv in _meshes)
        {
            if (kv.Value.mesh != null)
            {
#if UNITY_EDITOR
                DestroyImmediate(kv.Value.mesh);
#else
                Destroy(kv.Value.mesh);
#endif
            }
        }
        _meshes.Clear();
    }
}
