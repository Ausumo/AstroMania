using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using System.Drawing;
using UnityEngine.UIElements;

public struct NoiseGenJob : IJobParallelFor
{
    public int Size;
    public float Scale;
    public float ScaleMutliplier;
    public float FreuqencX;
    public float FrequencY;
    public Vector2 Offset;

    public NativeArray<float> HeightMap;
    public void Execute(int index)
    {
        var x = index / Size;
        var y = index % Size;

        float posX = (x * Scale + Offset.x);
        float posY = (y * Scale + Offset.y);

        HeightMap[index] = Mathf.PerlinNoise(posX * FreuqencX, posY * FrequencY) * ScaleMutliplier;
    }
}
