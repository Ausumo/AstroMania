using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraterGenerator
{
    /// <summary>
    ///  If you give a Vector2 then the crater will be set at this position otherwise in the middle of the terrain
    /// </summary>
    /// <param name="size"></param>
    /// <param name="craterSize"></param>
    /// <param name="craterCurve"></param>
    /// <returns></returns>
    public static float[,] CreateCraterMap(int size, float craterSize, AnimationCurve craterCurve, Vector2 position = new Vector2())
    {
        float[,] craterMap = new float[size, size];

        Vector2 center = new Vector2(size / 2, size / 2);

        if (position != Vector2.zero)
        {
            center = position;
        }

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {

                //Calsculate the middle
                float distance = Vector2.Distance(center, new Vector2(x, y));
                //calsculate the lerp
                float lerp = Mathf.InverseLerp(0f, craterSize, distance);
                //Create the map
                craterMap[x, y] = craterCurve.Evaluate(lerp);
            }
        }

        return craterMap;
    }
}
