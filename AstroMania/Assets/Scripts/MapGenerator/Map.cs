using Unity.Collections;
using UnityEngine;
using Unity.Jobs;
using System;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using System.Drawing;
using Unity.Mathematics;
using Unity.VisualScripting.Dependencies.Sqlite;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Map : MonoBehaviour
{
    [Header("HeightMap")]
    private float[,] heightMap;
    private float[,] craterMap;

    [SerializeField]
    private Terrain _terrain;

    private float[,,] _myAlphamap;
    private float angle;

    /// <summary>
    /// Set the Size, Heights and Maps to the Terrain
    /// </summary>
    /// <param name="size"></param>
    /// <param name="scale"></param>
    /// <param name="offset"></param>
    public void GenerateMap(int size, float scale, float scaleMultiplier, float frequencX, float frequencY, Vector2 offset, AnimationCurve craterCurve, float craterSize, float craterDetails, Vector2 craterPosition)
    {

        if (!_terrain.terrainData)
            return;

        if (craterCurve == null)
            return;


        int terrainSize = _terrain.terrainData.heightmapResolution;
        var alphamapWidth = _terrain.terrainData.alphamapWidth;
        var alphamapHeight = _terrain.terrainData.alphamapHeight;

        NativeCurve test = new NativeCurve(craterCurve, 200);
       
        var randomizeCraterDetails = UnityEngine.Random.Range(-craterDetails, craterDetails);

        float[,] noiseDest = NoiseMapJob(terrainSize, size, scale, scaleMultiplier, frequencX, frequencY, offset);
        
        float[,] craterDest = CraterMapJob(terrainSize, size, test, craterSize, randomizeCraterDetails, craterPosition);

        //float[,,] textureDest = AlphamapJob(alphamapWidth, alphamapHeight, 2, )


        _terrain.terrainData.heightmapResolution = size;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                noiseDest[x, y] += craterDest[x, y];
            }
        }

        _terrain.terrainData.SetHeights(0, 0, noiseDest);

        //Texture


        float[,,] _alphamap = new float[alphamapWidth, alphamapHeight, 2];

        for (int y = 0; y < alphamapHeight; y++)
        {
            for (int x = 0; x < alphamapWidth; x++)
            {
                float normX = x * 1.0f / (alphamapWidth - 1);
                float normY = y * 1.0f / (alphamapHeight - 1);
                var angle = _terrain.terrainData.GetSteepness(normX, normY);

                var frac = angle / 90.0;
                _alphamap[x, y, 0] = (float)frac;
                _alphamap[x, y, 1] = (float)(1 - frac);
            }
        }

        _terrain.terrainData.SetAlphamaps(0, 0, _alphamap);

    }

    #region NoiseMapJob
    //Start and Running the NoiseMap Generator Job
    private float[,] NoiseMapJob(int terrainSize, int size, float scale, float scaleMultiplier, float frequencX, float frequencY, Vector2 offset)
    {
        var heights = new NativeArray<float>(terrainSize * terrainSize, Allocator.Persistent);
        var job = new NoiseGenJob()
        {
            Size = size,
            Scale = scale,
            ScaleMutliplier = scaleMultiplier,
            FreuqencX = frequencX,
            FrequencY = frequencY,
            Offset = offset,

            HeightMap = heights
        };

        JobHandle jobHandle = job.Schedule(heights.Length, 64);

        jobHandle.Complete();

        float[] src = heights.ToArray();
        float[,] noiseDest = new float[terrainSize, terrainSize];
        int byteLength = Buffer.ByteLength(src);
        Buffer.BlockCopy(src, 0, noiseDest, 0, byteLength);

        heights.Dispose();

        return noiseDest;
    }
    #endregion

    #region CraterMapJob
    //Start and Running the CraterMap Generator Job
    private float[,] CraterMapJob(int terrainSize, int size, NativeCurve craterCurve, float craterSize, float randomizeCraterDetails, Vector2 position)
    {
        var crater = new NativeArray<float>(terrainSize * terrainSize, Allocator.Persistent);
        var job = new CraterGenJob()
        {
            Size = size,
            CraterSize = craterSize,
            CraterCurve = craterCurve,
            RandomizeCraterDetails = randomizeCraterDetails,
            position = position,

            CraterMap = crater
        };

        JobHandle jobHandle = job.Schedule(crater.Length, 64);

        jobHandle.Complete();

        float[] src = crater.ToArray();
        float[,] craterDest = new float[terrainSize, terrainSize];
        int byteLength = Buffer.ByteLength(src);
        Buffer.BlockCopy(src, 0, craterDest, 0, byteLength);

        crater.Dispose();

        return craterDest;
    }
    #endregion

    //public float [,,] AlphamapJob(int alphamapWidth, int alphamapHeight, int layers)
    //{
    //    var alphamap = new NativeArray<float3>(alphamapWidth * alphamapHeight, Allocator.Persistent);



    //    return 
    //}

}

