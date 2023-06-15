using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[BurstCompile]
public struct CraterGenJob : IJobParallelFor
{

    [ReadOnly] public int Size;
    [ReadOnly] public float CraterSize;
    [ReadOnly] public float RandomizeCraterDetails;
    [ReadOnly] public NativeCurve CraterCurve;
    [ReadOnly] public Vector2 Position;

    [WriteOnly] public NativeArray<float> CraterMap;

    [BurstCompile]
    public void Execute(int index)
    {
        var x = index / Size;
        var y = index % Size;

        Vector2 center = new Vector2(Size * 0.5f, Size * 0.5f);

        if (Position != Vector2.zero)
        {
            center = Position;
        }

        //calculate the middle
        float distance = Vector2.Distance(center, new Vector2(x, y));

        //create a random and add it to the Distance
        distance += RandomizeCraterDetails;

        //calculate the lerp
        float lerp = Mathf.InverseLerp(0f, CraterSize, distance);

        //create the map
        CraterMap[index] = CraterCurve.EvaluateLerp(lerp);
    }
}
