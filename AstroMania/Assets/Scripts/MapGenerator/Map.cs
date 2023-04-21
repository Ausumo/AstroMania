using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("HeightMap")]
    private float[,] heightMap;
    private float[,] craterMap;

    [SerializeField]
    private Terrain terrain;

    //biome map
    [Header("Texture Map")]
    [SerializeField]
    private Material Sand;
    [SerializeField]
    private Material Stone;
    [SerializeField]
    private Material Ground;


    //public Texture2D texture;


    /// <summary>
    /// Set the Size, Heights and Maps to the Terrain
    /// </summary>
    /// <param name="size"></param>
    /// <param name="scale"></param>
    /// <param name="offset"></param>
    public void GenerateMap(int size, float scale, float scaleMultiplier, float frequencX, float frequencY, Vector2 offset, AnimationCurve craterCurve, float craterSize, float craterDetails)
    {
        if (craterCurve == null)
            return;

        heightMap = NoiseGenerator.CreateNoiseMap(size, scale, scaleMultiplier, frequencX, frequencY, offset);
        craterMap = CraterGenerator.CreateCraterMap(size, craterSize, craterDetails, craterCurve);

        //texture = new Texture2D(127, 127, TextureFormat.RGBA32, false);

        terrain.terrainData.heightmapResolution = size;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                heightMap[x, y] += craterMap[x, y];

                //texture.SetPixel(x, y, new Color(craterMap[x, y], 0, 0, 255));
                //texture.Apply();
            }
        }

        terrain.terrainData.SetHeights(0, 0, heightMap);
    }
}
 
