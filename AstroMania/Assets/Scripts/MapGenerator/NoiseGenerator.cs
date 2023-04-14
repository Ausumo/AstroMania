using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
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
    public static float[,] CreateNoiseMap(float scale, float scaleMultiplier, float frequencX, float frequencY, Vector2 offset)
    {
        int sizeX = 513;
        int sizeY = 513;

        float[,] noiseMap = new float[sizeX, sizeY];

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                float posX = (x * scale + offset.x) * 0.1f;
                float posY = (y * scale + offset.y) * 0.1f;

                noiseMap[x, y] = Mathf.PerlinNoise(posX * frequencX, posY * frequencY) * scaleMultiplier;
            }
        }

        return noiseMap;
    }
}
