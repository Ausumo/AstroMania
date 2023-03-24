using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class CreateEnemyWindow : EditorWindow
{

    public string enemyName = "Enter Enemy Name";
    public int enemyAtk = 0;
    public int enemyHealth = 0;

    [MenuItem("Window/Enemy Creator")]
    public static void ShowCreateEnemyWindow()
    {
        GetWindow<CreateEnemyWindow>("Create Enemy");
    }

    private void OnGUI()
    {
        GUILayout.Label("Enemy Settings", EditorStyles.label);

        #region EnemyName
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Enemy Name", GUILayout.MaxWidth(125));
        enemyName = EditorGUILayout.TextField(enemyName).ToString();
        GUILayout.EndHorizontal();
        #endregion

        #region EnemyAtk
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Enemy Atk", GUILayout.MaxWidth(125));
        if(!int.TryParse(EditorGUILayout.TextField(enemyAtk.ToString()), out enemyAtk))
        {
            Debug.LogError("Only Numbers!");
        } 
        GUILayout.EndHorizontal();
        #endregion

        #region EnemyHealth
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Enemy Health", GUILayout.MaxWidth(125));
        if (!int.TryParse(EditorGUILayout.TextField(enemyHealth.ToString()), out enemyHealth))
        {
            Debug.LogError("Only Numbers!");
        }
        GUILayout.EndHorizontal();
        #endregion

        #region Create Enemy Button
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Create Enemey", GUILayout.MaxWidth(125));
        if (GUILayout.Button("Create Enemy"))
        {
            Debug.Log("Name: " + enemyName + " | Atk: " + enemyAtk + " | Health: " + enemyHealth);
        }
        GUILayout.EndHorizontal();
        #endregion

    }
}
