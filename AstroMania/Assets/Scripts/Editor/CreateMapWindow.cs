using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting.ReorderableList;

public class CreateMapWindow : EditorWindow
{
    //Map
    public Vector2 size;
    public float scale = 0.04f;
    public float scaleMultiplier = 0;
    public Vector2 offset;

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

        EditorGUILayout.Space();

        #region Map Size

        GUILayout.BeginHorizontal();
        size = EditorGUILayout.Vector2Field("Map Size:", size, GUILayout.MaxWidth(175));
        GUILayout.EndHorizontal();
        #endregion

        #region MapScale
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        scale = EditorGUILayout.Slider("Map Scale:", scale, minLimit, maxLimit, GUILayout.MaxWidth(300));
        GUILayout.EndHorizontal();
        #endregion

        #region MapScaleMultiplier
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        scaleMultiplier = EditorGUILayout.Slider("Scale sMultiplier:", scaleMultiplier, minLimit, maxLimit, GUILayout.MaxWidth(300));
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
            FindObjectOfType<Map>().GenerateMap(size, scale, scaleMultiplier, offset);
        }
        GUILayout.EndHorizontal();
        #endregion

    }

}