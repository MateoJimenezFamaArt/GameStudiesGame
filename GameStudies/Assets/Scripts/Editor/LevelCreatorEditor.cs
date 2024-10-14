using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(LevelCreator))]
public class LevelCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        LevelCreator dungeonCreator = (LevelCreator)target;
        if (GUILayout.Button("CreateNewDungeon"))
        {
            dungeonCreator.CreateLevel();
        }
        if (GUILayout.Button("DeleteNewLevel"))
        {
            dungeonCreator.DestroyAllChildren();
        }
    }
}