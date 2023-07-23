using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityMugen.CustomInput
{
    [CustomEditor(typeof(InputEventGroup))]
    public class InputEventGroupInspector : Editor
    {
        private SerializedProperty m_receiveInput;
        private SerializedProperty m_inputEventGroups;
        private SerializedProperty m_inputEventManagers;
        private ReorderableList m_inputEventGroupList;
        private ReorderableList m_inputEventManagerList;

        private void OnEnable()
        {
            m_receiveInput = serializedObject.FindProperty("m_receiveInput");
            m_inputEventGroups = serializedObject.FindProperty("m_inputEventGroups");
            m_inputEventManagers = serializedObject.FindProperty("m_inputEventManagers");

            m_inputEventGroupList = new ReorderableList(serializedObject, m_inputEventGroups, true, true, true, true);
            m_inputEventGroupList.drawHeaderCallback += rect =>
            {
                EditorGUI.LabelField(rect, "Groups");
            };
            m_inputEventGroupList.drawElementCallback += (rect, index, isActive, isFocused) =>
            {
                SerializedProperty item = m_inputEventGroups.GetArrayElementAtIndex(index);

                rect.y += 2;
                rect.height = 16;
                EditorGUI.PropertyField(rect, item, GUIContent.none);
            };

            m_inputEventManagerList = new ReorderableList(serializedObject, m_inputEventManagers, true, true, true, true);
            m_inputEventManagerList.drawHeaderCallback += rect =>
            {
                EditorGUI.LabelField(rect, "Events");
            };
            m_inputEventManagerList.drawElementCallback += (rect, index, isActive, isFocused) =>
            {
                SerializedProperty item = m_inputEventManagers.GetArrayElementAtIndex(index);

                rect.y += 2;
                rect.height = 16;
                EditorGUI.PropertyField(rect, item, GUIContent.none);
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_receiveInput);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Groups", EditorStyles.boldLabel);
            m_inputEventGroupList.DoLayoutList();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
            m_inputEventManagerList.DoLayoutList();

            EditorGUILayout.Space();
            if (GUILayout.Button("Find Children", GUILayout.Height(24)))
            {
                InputEventGroup ieg = target as InputEventGroup;
                ieg.FindChildren();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
