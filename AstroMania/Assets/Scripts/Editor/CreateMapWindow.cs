using UnityEngine;
using UnityEditor;

public class CreateMapWindow : EditorWindow
{
    //Map
    public Terrain terrain;
    public int size = 513;
    public float scale = 0.04f;
    public float scaleMultiplier = 0;
    public Vector2 offset;

    public float frequencX = 0;
    public float frequencY = 0;

    public AnimationCurve craterCurve = new AnimationCurve();
    public AnimationCurve craterCurve2;
    public float craterSize = 20;
    public float craterDetails = 0.5f;
    public Vector2 craterPosition;

    public int stoneIndex;

    private float minLimitCrater = 0f;
    private float maxLimitCrater = 2000f;

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

        #region size
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        size = EditorGUILayout.IntField("Map Size (z.b.: 513):", size, GUILayout.MaxWidth(300));
        GUILayout.EndHorizontal();
        #endregion

        #region Mountain Häufigkeit (scale)
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        scale = EditorGUILayout.Slider("Map Scale:", scale, minLimit, maxLimit, GUILayout.MaxWidth(300));
        GUILayout.EndHorizontal();
        #endregion

        #region MapHeight (scaleMultiplier)
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal(); 
        scaleMultiplier = EditorGUILayout.Slider("Map Height:", scaleMultiplier, minLimit, maxLimit, GUILayout.MaxWidth(300));
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

        #region Crater Curve
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        craterCurve = EditorGUILayout.CurveField("Crater Curve:", craterCurve, GUILayout.MaxWidth(300));
        GUILayout.EndHorizontal();

        #endregion

        #region Crater Size
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        craterSize = EditorGUILayout.Slider("crater Scale:", craterSize, minLimitCrater, maxLimitCrater, GUILayout.MaxWidth(300));
        GUILayout.EndHorizontal();
        #endregion

        #region Crater Details
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        craterDetails = EditorGUILayout.Slider("Crater Details:", craterDetails, minLimit,  maxLimit, GUILayout.MaxWidth(300));
        GUILayout.EndHorizontal();
        #endregion

        #region Crater Position
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        craterPosition = EditorGUILayout.Vector2Field("Crater Position:", craterPosition, GUILayout.MaxWidth(175));
        GUILayout.EndHorizontal();
        #endregion

        #region Stone Spawning
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        stoneIndex = EditorGUILayout.IntField("Stones:", stoneIndex, GUILayout.MaxWidth(175));
        GUILayout.EndHorizontal();
        #endregion

        #region Create Map Button
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create Map", GUILayout.MaxWidth(150)))
        {
            FindObjectOfType<Map>().GenerateMap(size, scale, scaleMultiplier, frequencX, frequencY, offset, craterCurve, craterSize, craterDetails, craterPosition);
        }
        GUILayout.EndHorizontal();
        #endregion

    }

}