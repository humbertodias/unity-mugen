using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityMugen.Commands;
using UnityMugen.IO;

public class ConvertCmdToCommandListEditorWindow : EditorWindow
{

    bool currentPathProject = true;

    public string nameChar;
    private TextFile m_textFile;
    private string m_outputChars = "Assets/";

    [MenuItem("UnityMugen/Convert Cmd To CommandList")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(ConvertCmdToCommandListEditorWindow)).Show();
    }

    void OnGUI()
    {
        SerializedObject serializedObject = new SerializedObject(this);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("nameChar"), true);

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Load and Converter File cmd"))
        {
            LoadGameData();
            CreateCommandList();
        }

        currentPathProject = GUILayout.Toggle(currentPathProject, "Current Path Project");
    }

    private void LoadGameData()
    {
        string file = EditorUtility.OpenFilePanel("Carregar o arquivo em formato .cmd, .mfg", "", "cmd,mfg");
        if (file.Length != 0)
        {
            var text = System.IO.File.ReadAllText(file);
            m_textFile = FileToTextFile.Build(text);
        }
    }

    public CommandList BuildExternal(string path, string _nameChar, string _OutputChars)
    {
        nameChar = _nameChar;
        m_outputChars = _OutputChars;
        currentPathProject = false;
        var text = System.IO.File.ReadAllText(path);
        m_textFile = FileToTextFile.Build(text);
        return CreateCommandList(); 
    }

    private CommandList CreateCommandList()
    {
        CommandList commandList = ScriptableObject.CreateInstance<CommandList>();
        commandList.commands = new List<CommandFE>();
        commandList.nameChar = nameChar;

        //var textfile = new FileSystem().OpenTextFile(filepath);
        foreach (var textsection in m_textFile)
        {
            if (string.Equals("Command", textsection.Title, StringComparison.OrdinalIgnoreCase))
            {
                var name = textsection.GetAttribute<string>("name");
                var command = textsection.GetAttribute<string>("command");
                var time = textsection.GetAttribute("time", 15);
                var buffertime = textsection.GetAttribute("Buffer.time", 1);

                CommandFE commandFE = new CommandFE(name, command, time, buffertime);
                commandList.commands.Add(commandFE);
            }
        }

        m_textFile = null;

        if (currentPathProject)
        {
            Type projectWindowUtilType = typeof(ProjectWindowUtil);
            MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
            object obj = getActiveFolderPath.Invoke(null, new object[0]);
            string path = obj.ToString() + "/" + nameChar + "CommandList.asset";
            AssetDatabase.CreateAsset(commandList, path);
        }
        else
        {
            AssetDatabase.CreateAsset(commandList, m_outputChars + nameChar + "CommandList.asset");
        }

        AssetDatabase.SaveAssets();
        Selection.activeObject = commandList;
        EditorUtility.FocusProjectWindow();

        return commandList;
    }

}
