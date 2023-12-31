﻿using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Readme))]
[InitializeOnLoad]
public class ReadmeEditor : Editor
{
    const string k_ShowedReadmeSessionStateName = "ReadmeEditor.showedReadme";
    const float k_Space = 16f;

    bool m_Initialized;

    [SerializeField]
    GUIStyle m_LinkStyle;

    GUIStyle LinkStyle
    {
        get { return m_LinkStyle; }
    }

    [SerializeField]
    GUIStyle m_TitleStyle;

    GUIStyle TitleStyle
    {
        get { return m_TitleStyle; }
    }

    [SerializeField]
    GUIStyle m_HeadingStyle;

    GUIStyle HeadingStyle
    {
        get { return m_HeadingStyle; }
    }

    [SerializeField]
    GUIStyle m_BodyStyle;

    GUIStyle BodyStyle
    {
        get { return m_BodyStyle; }
    }

    static ReadmeEditor()
    {
        EditorApplication.delayCall += SelectReadmeAutomatically;
    }

    static void SelectReadmeAutomatically()
    {
        if (!SessionState.GetBool(k_ShowedReadmeSessionStateName, false))
        {
            var readme = SelectReadme();
            SessionState.SetBool(k_ShowedReadmeSessionStateName, true);

            if (readme && !readme.loadedLayout)
            {
                LoadLayout();
                readme.loadedLayout = true;
            }
        }
    }

    static void LoadLayout()
    {
        var assembly = typeof(EditorApplication).Assembly;
        var windowLayoutType = assembly.GetType("UnityEditor.WindowLayout", true);
        var method = windowLayoutType.GetMethod("LoadWindowLayout", BindingFlags.Public | BindingFlags.Static);
        method?.Invoke(null, new object[] { Path.Combine(Application.dataPath, "TutorialInfo/Layout.wlt"), false });
    }

    [MenuItem("UnityMugen/Show Instructions")]
    static Readme SelectReadme()
    {
        var ids = AssetDatabase.FindAssets("Readme t:Readme");
        if (ids.Length == 1)
        {
            var readmeObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(ids[0]));

            Selection.objects = new UnityEngine.Object[] { readmeObject };

            return (Readme)readmeObject;
        }
        else
        {
            Debug.Log("Couldn't find a readme");
            return null;
        }
    }

    protected override void OnHeaderGUI()
    {
        var readme = (Readme)target;

        //if (!readme.editReadme)
        {
            Init();

            var iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth, 140);

            GUILayout.BeginHorizontal("In BigTitle");
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label(readme.icon, GUILayout.Width(iconWidth), GUILayout.Height(iconWidth));
                GUILayout.FlexibleSpace();
                //GUILayout.Label(readme.title, TitleStyle);
            }
            GUILayout.EndHorizontal();
        }
    }

    public override void OnInspectorGUI()
    {
        var readme = (Readme)target;

        //if (!readme.editReadme)
        {
            Init();

            foreach (var section in readme.sections)
            {
                if (!string.IsNullOrEmpty(section.heading))
                {
                    GUILayout.Label(section.heading, HeadingStyle);
                }

                foreach (string text in section.texts)
                {
                    if (!string.IsNullOrEmpty(text))
                        GUILayout.Label(text, BodyStyle);
                }

                if (!string.IsNullOrEmpty(section.linkText))
                {
                    if (LinkLabel(new GUIContent(section.linkText)))
                    {
                        Application.OpenURL(section.url);
                    }
                }

                GUILayout.Space(k_Space);

            }

            //readme.editReadme = GUILayout.Toggle(readme.editReadme, "Edit Readme");
        }
        //else
        //{
        //    base.OnInspectorGUI();
        //}


    }



    void Init()
    {
        if (m_Initialized)
            return;
        m_BodyStyle = new GUIStyle(EditorStyles.label);
        m_BodyStyle.wordWrap = true;
        m_BodyStyle.fontSize = 14;

        m_TitleStyle = new GUIStyle(m_BodyStyle);
        m_TitleStyle.fontSize = 26;

        m_HeadingStyle = new GUIStyle(m_BodyStyle);
        m_HeadingStyle.fontSize = 18;

        m_LinkStyle = new GUIStyle(m_BodyStyle);
        m_LinkStyle.wordWrap = false;

        // Match selection color which works nicely for both light and dark skins
        m_LinkStyle.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
        m_LinkStyle.stretchWidth = false;

        m_Initialized = true;
    }

    bool LinkLabel(GUIContent label, params GUILayoutOption[] options)
    {
        var position = GUILayoutUtility.GetRect(label, LinkStyle, options);

        Handles.BeginGUI();
        Handles.color = LinkStyle.normal.textColor;
        Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
        Handles.color = Color.white;
        Handles.EndGUI();

        EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);

        return GUI.Button(position, label, LinkStyle);
    }
}
