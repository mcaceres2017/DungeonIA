using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DungeonGeneratorAgent),true)]

public class DungeonGeneratorEditorAgent : Editor
{
    DungeonGeneratorAgent generator;

    private void Awake()
    {
 		generator = (DungeonGeneratorAgent)target;       
    }

    // Update is called once per frame
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Create Dungeon"))
        {
        	generator.GenerateDungeon();
        }
    }
}
