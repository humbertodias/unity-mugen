using System;
using UnityEditor;
using UnityEngine;
using UnityInputConverter;

namespace UnityMugen.CustomInput
{
    public class KeyCodeField
    {
        private string m_controlName;
        private string m_keyString;
        private bool m_isEditing;

        public KeyCodeField()
        {
            m_controlName = Guid.NewGuid().ToString("N");
            m_keyString = "";
            m_isEditing = false;
        }

        public KeyCode OnGUI(string label, KeyCode key)
        {
            GUI.SetNextControlName(m_controlName);
            bool hasFocus = (GUI.GetNameOfFocusedControl() == m_controlName);
            if (!m_isEditing && hasFocus)
            {
                m_keyString = key == KeyCode.None ? "" : KeyCodeConverter.KeyToString(key);
            }

            m_isEditing = hasFocus;
            if (m_isEditing)
            {
                m_keyString = EditorGUILayout.TextField(label, m_keyString);
            }
            else
            {
                EditorGUILayout.TextField(label, key == KeyCode.None ? "" : KeyCodeConverter.KeyToString(key));
            }

            if (m_isEditing && Event.current.type == EventType.KeyUp)
            {
                key = KeyCodeConverter.StringToKey(m_keyString);
                if (key == KeyCode.None)
                {
                    m_keyString = "";
                }
                else
                {
                    m_keyString = KeyCodeConverter.KeyToString(key);
                }
                m_isEditing = false;
            }

            return key;
        }

        public KeyCode OnGUI(Rect position, string label, KeyCode key)
        {
            GUI.SetNextControlName(m_controlName);
            bool hasFocus = (GUI.GetNameOfFocusedControl() == m_controlName);
            if (!m_isEditing && hasFocus)
            {
                m_keyString = key == KeyCode.None ? "" : KeyCodeConverter.KeyToString(key);
            }

            m_isEditing = hasFocus;
            if (m_isEditing)
            {
                m_keyString = EditorGUI.TextField(position, label, m_keyString);
            }
            else
            {
                EditorGUI.TextField(position, label, key == KeyCode.None ? "" : KeyCodeConverter.KeyToString(key));
            }

            if (m_isEditing && Event.current.type == EventType.KeyUp)
            {
                key = KeyCodeConverter.StringToKey(m_keyString);
                if (key == KeyCode.None)
                {
                    m_keyString = "";
                }
                else
                {
                    m_keyString = KeyCodeConverter.KeyToString(key);
                }
                m_isEditing = false;
            }

            return key;
        }

        public void Reset()
        {
            m_keyString = "";
            m_isEditing = false;
        }
    }
}
