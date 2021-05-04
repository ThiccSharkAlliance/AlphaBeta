using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TreeLSystem))]
public class TreeLSystemEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TreeLSystem treeLSystem = (TreeLSystem)target;

        if (GUILayout.Button("Generate Tree"))
        {
            treeLSystem.GenerateTree();
        }

        if (GUILayout.Button("Clear Tree"))
        {
            treeLSystem.ClearTree();
        }
    }
}
