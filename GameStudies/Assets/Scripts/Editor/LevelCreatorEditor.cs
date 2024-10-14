using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Buffers.Text;

[CustomEditor(typeof(LevelCreator))]
public class LevelCreatorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        LevelCreator levelCreator = (LevelCreator)target;
        if (GUILayout.Button("Create New Level"))
        {
            levelCreator.CreateLevel();
        }
        if (GUILayout.Button("Reset"))
        {
            levelCreator.DestroyAllChildren();
        }
    }


}
