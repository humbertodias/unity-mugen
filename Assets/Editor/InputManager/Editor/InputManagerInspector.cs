using UnityEditor;
using UnityEngine;

namespace UnityMugen.CustomInput
{
    [CustomEditor(typeof(InputManager))]
    public class InputManagerInspector : Editor
    {
        private const int BUTTON_HEIGHT = 35;

        private InputManager m_inputManager;
        private SerializedProperty m_playerOneDefault;
        private SerializedProperty m_playerTwoDefault;
        private SerializedProperty m_playerThreeDefault;
        private SerializedProperty m_playerFourDefault;
        private SerializedProperty m_ignoreTimescale;
        private GUIContent m_createSnapshotInfo;
        private string[] m_controlSchemeNames;

        private void OnEnable()
        {
            m_inputManager = target as InputManager;
            m_playerOneDefault = serializedObject.FindProperty("m_playerOneDefault");
            m_playerTwoDefault = serializedObject.FindProperty("m_playerTwoDefault");
            m_playerThreeDefault = serializedObject.FindProperty("m_playerThreeDefault");
            m_playerFourDefault = serializedObject.FindProperty("m_playerFourDefault");
            m_ignoreTimescale = serializedObject.FindProperty("m_ignoreTimescale");
            m_createSnapshotInfo = new GUIContent("Create\nSnapshot", "Creates a snapshot of your input configurations which can be restored at a later time(when you exit play-mode for example)");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            UpdateControlSchemeNames();

            EditorGUILayout.Space();
            DrawControlSchemeDropdown(m_playerOneDefault);
            DrawControlSchemeDropdown(m_playerTwoDefault);
            DrawControlSchemeDropdown(m_playerThreeDefault);
            DrawControlSchemeDropdown(m_playerFourDefault);
            EditorGUILayout.PropertyField(m_ignoreTimescale);

            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            GUI.enabled = !InputEditor.IsOpen;
            if (GUILayout.Button("Input\nEditor", GUILayout.Height(BUTTON_HEIGHT)))
            {
                InputEditor.OpenWindow(m_inputManager);
            }
            GUI.enabled = true;
            if (GUILayout.Button(m_createSnapshotInfo, GUILayout.Height(BUTTON_HEIGHT)))
            {
                EditorToolbox.CreateSnapshot(m_inputManager);
            }
            GUI.enabled = EditorToolbox.CanLoadSnapshot();
            if (GUILayout.Button("Restore\nSnapshot", GUILayout.Height(BUTTON_HEIGHT)))
            {
                EditorToolbox.LoadSnapshot(m_inputManager);
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        private void UpdateControlSchemeNames()
        {
            if (m_controlSchemeNames == null || (m_controlSchemeNames.Length - 1 != m_inputManager.ControlSchemes.Count))
            {
                m_controlSchemeNames = new string[m_inputManager.ControlSchemes.Count + 1];
            }

            m_controlSchemeNames[0] = "None";
            for (int i = 1; i < m_controlSchemeNames.Length; i++)
            {
                m_controlSchemeNames[i] = m_inputManager.ControlSchemes[i - 1].Name;
            }
        }

        private void DrawControlSchemeDropdown(SerializedProperty item)
        {
            int index = FindIndexOfControlScheme(item.stringValue);
            index = EditorGUILayout.Popup(item.displayName, index, m_controlSchemeNames);

            if (index > 0)
            {
                item.stringValue = m_inputManager.ControlSchemes[index - 1].UniqueID;
            }
            else
            {
                item.stringValue = null;
            }
        }

        private int FindIndexOfControlScheme(string id)
        {
            if (string.IsNullOrEmpty(id))
                return 0;

            for (int i = 0; i < m_inputManager.ControlSchemes.Count; i++)
            {
                if (m_inputManager.ControlSchemes[i].UniqueID == id)
                    return i + 1;
            }

            return 0;
        }
    }
}