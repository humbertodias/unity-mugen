using UnityEditor;
using UnityEngine;

namespace UnityMugen.Editors
{

    public class ExtendEditorWindow : EditorWindow
    {

        protected void HelpButton(string page)
        {
            if (GUILayout.Button("?", GUILayout.Width(40), GUILayout.Height(18)))
                Application.OpenURL(page);
        }

    }

}