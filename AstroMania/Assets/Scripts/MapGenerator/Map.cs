using System.Xml.Schema;
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
    public void GenerateMap(float scale, float scaleMultiplier, float frequencX, float frequencY, Vector2 offset)
    {
        heightMap = NoiseGenerator.CreateNoiseMap(scale, scaleMultiplier, frequencX, frequencY, offset);

        terrain.terrainData.heightmapResolution = 513;

        terrain.terrainData.SetHeights(0, 0, heightMap);
    }
}
 
