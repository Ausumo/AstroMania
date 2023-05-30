using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using System.Drawing;
using UnityEngine.UIElements;
using Unity.Burst;

[BurstCompile]
public struct NoiseGenJob : IJobParallelFor
{
    [ReadOnly] public int Size;
    [ReadOnly] public float Scale;
    [ReadOnly] public float ScaleMutliplier;
    [ReadOnly] public float FreuqencX;
    [ReadOnly] public float FrequencY;
    [ReadOnly] public Vector2 Offset;

    [WriteOnly] public NativeArray<float> HeightMap;

    [BurstCompile]
    public void Execute(int index)
    {
        var x = index / Size;
        var y = index % Size;

        float posX = (x * Scale + Offset.x);
        float posY = (y * Scale + Offset.y);

        HeightMap[index] = Mathf.PerlinNoise(posX * FreuqencX, posY * FrequencY) * ScaleMutliplier;
    }
}
