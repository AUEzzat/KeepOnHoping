﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PoolDatabase))]
public class PoolDBEditor : Editor
{

    public PoolDatabase poolableDB;
    Vector2 scrollPos = Vector2.zero;//list of prefabs scroll position
    bool removingAll = false;//remove all confirmation button

    //current editable values
    public PoolableType tileType;
    public GameObject prefab;
    public int instNum = 10;
    InteractablesDatabase interactablesDB;//tiles database to select from
    public int selectedTile = 0;

    private void OnEnable()
    {
        poolableDB = (PoolDatabase)target;

        string interactablesDBPath = "Assets/Data/Database/InteractablesDatabase.asset";
        interactablesDB = AssetDatabase.LoadAssetAtPath<InteractablesDatabase>(interactablesDBPath);

        tileType = interactablesDB[0];
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DisplayCurrentPrefabs();

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Separator();

        CreateNewPrefab();

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(poolableDB);
    }

    void DisplayCurrentPrefabs()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Current Prefabs: ", EditorStyles.boldLabel);
        GUILayout.Label(poolableDB.Count.ToString());
        GUILayout.EndHorizontal();

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.MaxHeight(250),
            GUILayout.MinHeight(160));
        //Display current included prefabs
        for (int i = 0; i < poolableDB.Count; i++)
        {
            EditorGUILayout.BeginVertical("HelpBox");
            LoadPrefabImage(poolableDB[i].Name);

            EditorGUILayout.BeginHorizontal("HelpBox");
            GUILayout.Label(i.ToString(), EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.LabelField(poolableDB[i].Name + "(" + poolableDB[i].count.ToString() + ")", EditorStyles.miniLabel);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Edit"))
            {
                selectedTile = interactablesDB.interactablesNames.IndexOf(poolableDB[i].Name);
                //tileType = poolableDB[i].type;
                prefab = poolableDB[i].prefab;
                instNum = poolableDB[i].count;
            }

            if (GUILayout.Button("Delete"))
            {
                poolableDB.RemoveAt(i);
                return;
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        if (poolableDB.poolableList.Count == 0)
        {
            EditorGUILayout.LabelField("Empty", EditorStyles.textField);
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        RemoveAllButton();
    }

    void CreateNewPrefab()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Create New Poolable Prefab: ", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        //center texture
        GUILayout.BeginHorizontal();
        GUILayout.Label("", GUILayout.ExpandWidth(true));
        LoadPrefabImage(tileType.name, 120, 120);
        GUILayout.Label("", GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();

        selectedTile = EditorGUILayout.Popup("Poolable Type", selectedTile,
            interactablesDB.interactablesNames.ToArray());
        tileType = interactablesDB[selectedTile];


        instNum = (int)EditorGUILayout.Slider("Instances Number", instNum, 1, 20);

        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

        EditorGUILayout.Separator();

        AddEditButtons();
    }

    void AddEditButtons()
    {
        bool showAddNotEdit = true;
        bool editButtonDisabled = false;

        //check if type already exists
        for (int i = 0; i < poolableDB.poolableList.Count; i++)
        {
            if (tileType && tileType.name == poolableDB.poolableList[i].Name)
            {
                showAddNotEdit = false;
            }
        }

        if (!prefab)
        {
            EditorGUILayout.HelpBox("Prefab must be set", MessageType.Warning);
            editButtonDisabled = true;
        }

        EditorGUI.BeginDisabledGroup(editButtonDisabled);

        if (showAddNotEdit && GUILayout.Button("Add"))
        {
            poolableDB.poolableList.Add(new PoolableObj(tileType, instNum, prefab));
        }

        if (!showAddNotEdit && GUILayout.Button("Save"))
        {
            poolableDB[tileType] = new PoolableObj(tileType, instNum, prefab);
        }

        EditorGUI.EndDisabledGroup();
    }

    void RemoveAllButton()
    {
        bool removeAll = false;

        if (removingAll)
        {
            removingAll = false;
            removeAll = EditorUtility.DisplayDialog("Deleting all!", "Are you Certain you want to do this crazy act!?", "Yes I'm Crazy", "I'm Crazy but not now");
        }

        if (removeAll)
        {
            poolableDB.RemoveAll();
        }

        if (poolableDB.Count > 0 && GUILayout.Button("Remove All"))
        {
            removingAll = true;
        }
    }

    void LoadPrefabImage(string name, int minHeight = 70, int minWidth = 70)
    {
        Texture2D inputTexture = (Texture2D)EditorGUIUtility.Load("PoolableAssets/" + name + ".png");
        if (!inputTexture)
            return;

        GUILayout.Label(inputTexture, GUILayout.MaxHeight(70), GUILayout.MaxWidth(70),
            GUILayout.MinHeight(minHeight), GUILayout.MinWidth(minWidth));
    }
}
