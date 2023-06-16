using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;

[BurstCompile]
public struct TextureGenJob : IJobParallelFor
{
    //public int AlphamapWidth;
    //public int AlphamapHeight;
    //public TerrainData TerrainData;
    //public float Height;

    //public NativeArray<float> AlphamapD1;

    public void Execute(int index)
    {
        //var x = index / AlphamapWidth;
        //var y = index % AlphamapHeight;

        //float normX = x * 1.0f / (AlphamapWidth - 1);
        //float normY = y * 1.0f / (AlphamapHeight - 1);
        ////var value = TerrainData.GetSteepness(normX, normY);

        //var frac = 90 / 90.0;
        ////Alphamap[x, y, 0] = (float)frac;
        ////Alphamap[x, y, 1] = (float)(1 - frac);
    }
}
