using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MoveList))]
public class CharacterEditor : Editor
{

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();

        var moveList = (MoveList)target;
        if (moveList != null && moveList.iconChar != null)
            GUILayout.Label(moveList.iconChar, GUILayout.Width(25), GUILayout.Height(25));

        if (GUILayout.Button("Open Move List Editor", GUILayout.Height(25)))
            MoveListEditorWindow.Init();

        EditorGUILayout.EndHorizontal();
    }

}
