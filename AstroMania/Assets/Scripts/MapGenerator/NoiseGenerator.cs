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
    public static float[,] CreateNoiseMap(int size, float scale, float scaleMultiplier, float frequencX, float frequencY, Vector2 offset)
    {

        float[,] noiseMapLayerOne = new float[size, size];

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float posX = (x * scale + offset.x)/* * 0.1f*/;
                float posY = (y * scale + offset.y)/* * 0.1f*/;

                noiseMapLayerOne[x, y] = Mathf.PerlinNoise(posX * frequencX, posY * frequencY) * scaleMultiplier;
            }
        }

        return noiseMapLayerOne;
    }

}
