using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting.ReorderableList;

public class CreateMapWindow : EditorWindow
{
    //Map
    public Terrain terrain;
    public float scale = 0.04f;
    public float scaleMultiplier = 0;
    public Vector2 offset;

    public float frequencX = 0;
    public float frequencY = 0;

    private float minLimit = 0.01f;
    private float maxLimit = 0.99f;


    [MenuItem("Window/Map Creator")]
    public static void ShowCreateMapWindow()
    {
        GetWindow<CreateMapWindow>("Map Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Map Settings", EditorStyles.label);


        #region MapScale
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        scale = EditorGUILayout.Slider("Map Scale:", scale, minLimit, maxLimit, GUILayout.MaxWidth(300));
        GUILayout.EndHorizontal();
        #endregion

        #region MapScaleMultiplier
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        scaleMultiplier = EditorGUILayout.Slider("Scale Multiplier:", scaleMultiplier, minLimit, maxLimit, GUILayout.MaxWidth(300));
        GUILayout.EndHorizontal();
        #endregion

        #region Frequenc
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        frequencX = EditorGUILayout.Slider("Frequenc X:", frequencX, minLimit, maxLimit, GUILayout.MaxWidth(300));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        frequencY = EditorGUILayout.Slider("Frequenc Y:", frequencY, minLimit, maxLimit, GUILayout.MaxWidth(300));
        GUILayout.EndHorizontal();
        #endregion

        #region MapOffset
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        offset = EditorGUILayout.Vector2Field("Map Offset:", offset, GUILayout.MaxWidth(175));
        GUILayout.EndHorizontal();
        #endregion

        #region Create Map Button
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create Map", GUILayout.MaxWidth(150)))
        {
            FindObjectOfType<Map>().GenerateMap(scale, scaleMultiplier, frequencX, frequencY, offset);
        }
        GUILayout.EndHorizontal();
        #endregion

    }

}