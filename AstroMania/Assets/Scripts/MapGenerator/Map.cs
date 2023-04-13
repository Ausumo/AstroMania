using Unity.VisualScripting;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("HeightMap")]
    public float[,] heightMap;

    public Texture2D texture;
    public Material mat;

    public Terrain terrain;

    public void GenerateMap(Vector2 size, float scale, Vector2 offset)
    {
        texture = new Texture2D((int)size.x, (int)size.y, TextureFormat.ARGB32, false);

        heightMap = NoiseGenerator.CreateNoiseMap(size, scale, offset);

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                texture.SetPixel(x, y, new Color(heightMap[x, y], 0, 0, 1));
                texture.Apply();

                mat.mainTexture = texture;

                

            }
        }

    }
}
 
