using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;

[BurstCompile]
public struct TextureGenJob : IJobParallelFor
{
    public int AlphamapWidth;
    public int AlphamapHeight;
    //public TerrainData TerrainData;
    public float Height;

    public NativeArray<float> AlphamapD1;

    public void Execute(int index)
    {
        var x = index / AlphamapWidth;
        var y = index % AlphamapHeight;

        //float normX = x * 1.0f / (AlphamapWidth - 1);
        //float normY = y * 1.0f / (AlphamapHeight - 1);
        //var value = TerrainData.GetSteepness(normX, normY);
        float[,,] Alphamap = new float[AlphamapWidth - 1, AlphamapHeight - 1, 2];

        var frac = 90 / 90.0;
        Alphamap[x, y, 0] = (float)frac;
        Alphamap[x, y, 1] = (float)(1 - frac);

        AlphamapD1 = ConvertToNativeArray(Alphamap);
    }

    NativeArray<float> ConvertToNativeArray(float[,,] sourceArray)
    {
        int length = sourceArray.GetLength(0) * sourceArray.GetLength(1) * sourceArray.GetLength(2);
        NativeArray<float> nativeArray = new NativeArray<float>(length, Allocator.TempJob);

        // Kopiere Werte aus dem dreidimensionalen Array in das NativeArray
        int index = 0;
        for (int i = 0; i < sourceArray.GetLength(0); i++)
        {
            for (int j = 0; j < sourceArray.GetLength(1); j++)
            {
                for (int k = 0; k < sourceArray.GetLength(2); k++)
                {
                    nativeArray[index] = sourceArray[i, j, k];
                    index++;
                }
            }
        }

        return nativeArray;
    }
}
