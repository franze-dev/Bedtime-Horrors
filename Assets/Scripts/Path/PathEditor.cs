using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(PathPiece))]
public class PathEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PathPiece pathGenerator = (PathPiece)target;
        if (GUILayout.Button("Generate Path"))
        {
            pathGenerator.GeneratePath();
        }
    }
}

#endif