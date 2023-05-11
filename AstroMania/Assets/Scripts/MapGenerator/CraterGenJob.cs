using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct CraterGenJob : IJobParallelFor
{

    public int Size;
    public float CraterSize;
    public float RandomizeCraterDetails;
    public NativeCurve CraterCurve;
    public Vector2 position;

    public NativeArray<float> CraterMap;

    public void Execute(int index)
    {
        var x = index / Size;
        var y = index % Size;

        Vector2 center = new Vector2(Size * 0.5f, Size * 0.5f);

        if (position != Vector2.zero)
        {
            center = position;
        }

        //calculate the middle
        float distance = Vector2.Distance(center, new Vector2(x, y));

        //create a random and add it to the Distance
        var rnd = 
        distance += RandomizeCraterDetails;

        //calculate the lerp
        float lerp = Mathf.InverseLerp(0f, CraterSize, distance);

        //create the map
        CraterMap[index] = CraterCurve.EvaluateLerp(lerp);
    }
}
