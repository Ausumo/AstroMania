using System.Diagnostics;
using System.Xml.Schema;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using System;

public class Map : MonoBehaviour
{
    [Header("HeightMap")]
    private float[,] heightMap;
    private float[,] craterMap;

    [SerializeField]
    private Terrain _terrain;

    //public Texture2D texture;


    /// <summary>
    /// Set the Size, Heights and Maps to the Terrain
    /// </summary>
    /// <param name="size"></param>
    /// <param name="scale"></param>
    /// <param name="offset"></param>
    public void GenerateMap(int size, float scale, float scaleMultiplier, float frequencX, float frequencY, Vector2 offset, AnimationCurve craterCurve, float craterSize, float craterDetails)
    {

        if (!_terrain.terrainData)
            return;

        int terrainSize = _terrain.terrainData.heightmapResolution;

        var heights = new NativeArray<float>(terrainSize * terrainSize, Allocator.Persistent);

        var job = new TerrainGenJob()
        {
            Size = size,
            ScaleMutliplier = scaleMultiplier,
            FreuqencX = frequencX,
            FrequencY = frequencY,
            HeightMap = heights
        };

        JobHandle jobHandle = job.Schedule(heights.Length, 64);

        //Other Code

        jobHandle.Complete();

        float[] src = heights.ToArray();
        float[,] dest = new float[terrainSize, terrainSize];
        int byteLength = Buffer.ByteLength(src);
        Buffer.BlockCopy(src, 0, dest, 0, byteLength);

        //_terrain.terrainData.SetHeights(0, 0, dest);

        heights.Dispose();




        //Single Threading
        if (craterCurve == null)
            return;
        
        //heightMap = NoiseGenerator.CreateNoiseMap(size, scale, scaleMultiplier, frequencX, frequencY, offset);
        craterMap = CraterGenerator.CreateCraterMap(size, craterSize, craterDetails, craterCurve);

        _terrain.terrainData.heightmapResolution = size;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                dest[x, y] += craterMap[x, y];
            }
        }

        _terrain.terrainData.SetHeights(0, 0, dest);
    }
}
 
