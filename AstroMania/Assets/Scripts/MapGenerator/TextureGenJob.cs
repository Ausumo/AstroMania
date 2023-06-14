using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

[BurstCompile]
public class TextureGenJob : IJobParallelFor
{

    public NativeArray<float3> Alphamap;

    public void Execute(int index)
    {
        //var x = index / Size;
        //var y = index % Size;

        //Alphamap[x, y].x = x;
    }
}
