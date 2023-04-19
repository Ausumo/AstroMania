using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraterGenerator
{
    public static float[,] CreateCraterMap(int size, float craterSize, AnimationCurve craterCurve)
    {

        float[,] craterMap = new float[size, size];

        Vector2 center = new Vector2(size / 2, size / 2);

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                //Berechne die Mitte
                float distance = Vector2.Distance(center, new Vector2(x, y));
               
                float lerp = Mathf.InverseLerp(0f, craterSize, distance);

                craterMap[x, y] = craterCurve.Evaluate(lerp);
            }
        }

        return craterMap;
    }
}
