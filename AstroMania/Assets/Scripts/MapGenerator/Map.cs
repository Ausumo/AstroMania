using Unity.VisualScripting;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("HeightMap")]
    public float[,] heightMap;

    public Terrain terrain;

    /// <summary>
    /// Set the Size and Heights to the Terrain
    /// </summary>
    /// <param name="size"></param>
    /// <param name="scale"></param>
    /// <param name="offset"></param>
    public void GenerateMap(Vector2 size, float scale, float scaleMultiplier, Vector2 offset)
    {

        heightMap = NoiseGenerator.CreateNoiseMap(size, scale, scaleMultiplier, offset);

        terrain.terrainData.heightmapResolution = (int)size.x + 1;

        terrain.terrainData.SetHeights(0, 0, heightMap);
    }
}
 
