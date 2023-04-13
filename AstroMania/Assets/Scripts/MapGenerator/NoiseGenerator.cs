using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseGenerator
{
    /// <summary>
    /// Create the NoiseMap
    /// </summary>
    /// <param name="size"></param>
    /// <param name="scale"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public static float[,] CreateNoiseMap(Vector2 size, float scale, Vector2 offset)
    {
        int sizeX = (int)size.x;
        int sizeY = (int)size.y;

        float[,] noiseMap =  new float[sizeX, sizeY];

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                float posX = x * scale + offset.x;
                float posY = y * scale + offset.y;

                noiseMap[x, y] = Mathf.PerlinNoise(posX, posY);

            }
        }

        return noiseMap;
    }
}
