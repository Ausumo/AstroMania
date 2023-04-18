using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("HeightMap")]
    public float[,] heightMap;
    public float[,,] alphaMap;

    public Terrain terrain;

    [Header("AlphaMap")]
    private const float WATER = 0.0f;
    private const float SAND = 0.3f;
    private const float GRASS = 0.7f;
    private const float SNOW = 0.9f;

    /// <summary>
    /// Set the Size and Heights to the Terrain
    /// </summary>
    /// <param name="size"></param>
    /// <param name="scale"></param>
    /// <param name="offset"></param>
    public void GenerateMap(float scale, float scaleMultiplier, float frequencX, float frequencY, Vector2 offset)
    {
        heightMap = NoiseGenerator.CreateNoiseMap(scale, scaleMultiplier, frequencX, frequencY, offset);

        var alphaResolution = terrain.terrainData.alphamapResolution;

        alphaMap = new float[alphaResolution, alphaResolution, 4];

        terrain.terrainData.heightmapResolution = 513;

        //for (int x = 0; x < 513; x++)
        //{
        //    for (int y = 0; y < 513; y++)
        //    {
        //        alphaMap[x , y, GRASS];
        //    }
        //}

        terrain.terrainData.SetHeights(0, 0, heightMap);
        terrain.terrainData.SetAlphamaps(0, 0, alphaMap);
    }
}
 
