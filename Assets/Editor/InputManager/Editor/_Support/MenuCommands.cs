using UnityEditor;
using UnityEngine;
using UnityInputConverter;

namespace UnityMugen.CustomInput
{
    public static partial class MenuCommands
    {
        [MenuItem("UnityMugen/Input Manager/Create Input Manager", false, 2)]
        private static void CreateInputManager()
        {
            GameObject gameObject = new GameObject("Input Manager");
            gameObject.AddComponent<InputManager>();

            // Register Input Manager for undo, mark scene as dirty.
            Undo.RegisterCreatedObjectUndo(gameObject, "Create Input Manager");

            Selection.activeGameObject = gameObject;
        }

        [MenuItem("UnityMugen/Input Manager/Convert Unity Input", false, 5)]
        private static void ConvertInput()
        {
            string sourcePath = EditorUtility.OpenFilePanel("Select Unity input settings asset", "", "asset");
            if (!string.IsNullOrEmpty(sourcePath))
            {
                string destinationPath = EditorUtility.SaveFilePanel("Save imported input axes", "", "input_manager", "xml");
                if (!string.IsNullOrEmpty(destinationPath))
                {
                    try
                    {
                        InputConverter converter = new InputConverter();
                        converter.ConvertUnityInputManager(sourcePath, destinationPath);

                        EditorUtility.DisplayDialog("Success", "Unity input converted successfuly!", "OK");
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogException(ex);

                        string message = "Failed to convert Unity input! Please make sure 'InputManager.asset' is serialized as a YAML text file.";
                        EditorUtility.DisplayDialog("Error", message, "OK");
                    }
                }
            }
        }

        [MenuItem("UnityMugen/Input Manager/Check For Updates", false, 400)]
        public static void CheckForUpdates()
        {
            Application.OpenURL("https://github.com/daemon3000/InputManager");
        }

        [MenuItem("UnityMugen/Input Manager/Documentation", false, 401)]
        public static void OpenDocumentationPage()
        {
            Application.OpenURL("https://github.com/daemon3000/InputManager/wiki");
        }

        [MenuItem("UnityMugen/Input Manager/Report Bug", false, 402)]
        public static void OpenReportBugPage()
        {
            Application.OpenURL("https://github.com/daemon3000/InputManager/issues");
        }

        [MenuItem("UnityMugen/Input Manager/Contact", false, 403)]
        public static void OpenContactDialog()
        {
            string message = "Email: daemon3000@hotmail.com";
            EditorUtility.DisplayDialog("Contact", message, "Close");
        }

        [MenuItem("UnityMugen/Input Manager/Forum", false, 404)]
        public static void OpenForumPage()
        {
            Application.OpenURL("http://forum.unity3d.com/threads/223321-Free-Custom-Input-Manager");
        }

        [MenuItem("UnityMugen/Input Manager/About", false, 405)]
        public static void OpenAboutDialog()
        {
            string message = string.Format("Input Manager v{0}, MIT Licensed\nCopyright \u00A9 2018 Cristian Alexandru Geambasu\nhttps://github.com/daemon3000/InputManager", InputManager.VERSION);
            EditorUtility.DisplayDialog("About", message, "OK");
        }
    }
}
