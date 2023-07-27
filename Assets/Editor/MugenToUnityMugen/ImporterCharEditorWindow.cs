using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityMugen;
using UnityMugen.IO;

public class ImporterCharEditorWindow : EditorWindow
{
    static ImporterCharEditorWindow s_editorWindow;

    private TextFile m_textFile;
    private string m_basePathInput;
    private string m_basePathOutput;
    private List<Tuple<int, string>> m_listFiles;
    //private List<string> statesDirectory;
    private string m_charName;
    private List<FileDirectory> m_fileDirectories;
    private TextSection m_filesTextSection;
    private string m_constantFile;
    private string m_stCommon, m_commandPath, m_sffPath, m_airPath, m_sndPath;
    private PlayerProfileManager m_manager;

    const string OutputChars = "Assets/Chars/";
    const string OutputStreamingAssets = "Assets/StreamingAssets/";

    [MenuItem("UnityMugen/Importer Char")]
    static void Init()
    {
        s_editorWindow = EditorWindow.GetWindow<ImporterCharEditorWindow>(false, "Importer Char", true);
        s_editorWindow.Show();
    }

    void OnGUI()
    {
        SerializedObject serializedObject = new SerializedObject(this);

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Load and Converter File def"))
        {
            m_listFiles = new List<Tuple<int, string>>();
            if (LoadFile())
            {
                CreateProfileManager();

                m_fileDirectories = new List<FileDirectory>();
                BuildFileDirectory(m_filesTextSection);

                m_manager.states = StatesImport(m_filesTextSection);

                CreateFolders();
                ImportFiles();
                CreateAssets();

                AssetDatabase.Refresh();
            }
        }

        if (m_listFiles != null)
        {
            foreach (var file in m_listFiles)
            {
                EditorGUILayout.LabelField(file.Item2);
            }
        }
    }

    private bool LoadFile()
    {
        string file = EditorUtility.OpenFilePanel("Carregar o arquivo em formato .def", "", "def");
        if (file.Length != 0)
        {
            m_charName = file.Substring(file.LastIndexOf('/') + 1, file.LastIndexOf('.') - file.LastIndexOf('/') - 1);
            var text = System.IO.File.ReadAllText(file).Replace('\\','/');
            m_textFile = FileToTextFile.Build(text);
            string path = file.Substring(0, file.LastIndexOf('/') + 1);
            m_basePathInput = Path.GetDirectoryName(path);
            m_basePathOutput = Application.streamingAssetsPath + "/";
            return true;
        }
        return false;
    }

    private void CreateProfileManager()
    {
        m_manager = ScriptableObject.CreateInstance<PlayerProfileManager>();

        //INFO
        var title = m_textFile.GetSection("Info");
        m_manager.charName = m_charName;// title.GetAttribute<string>("name", String.Empty);
        m_manager.displayName = title.GetAttribute<string>("displayname", String.Empty);
        m_manager.author = title.GetAttribute<string>("author", String.Empty);
        m_manager.versionDate = title.GetAttribute<string>("versiondate");
        m_manager.mugenVersion = BuildMugenVersion(title.GetAttribute<string>("mugenversion", String.Empty));
        m_manager.palettesIndex = BuildPaletteOrder(title.GetAttribute<string>("pal.defaults", null));

        //FILES
        m_filesTextSection = m_textFile.GetSection("Files");

        m_sffPath = m_filesTextSection.GetAttribute<string>("sprite", String.Empty);
        m_manager.sff = RemoveFolders(m_sffPath.Substring(0, m_sffPath.LastIndexOf('.')));

        m_airPath = m_filesTextSection.GetAttribute<string>("anim", String.Empty);
        m_manager.air = RemoveFolders(m_airPath.Substring(0, m_airPath.LastIndexOf('.')));

        m_sndPath = m_filesTextSection.GetAttribute<string>("sound", String.Empty);
        m_manager.snd = RemoveFolders(m_sndPath.Substring(0, m_sndPath.LastIndexOf('.')));

        m_stCommon = m_filesTextSection.GetAttribute<string>("stcommon", String.Empty);
        m_constantFile = m_filesTextSection.GetAttribute<string>("cns", String.Empty);
        m_commandPath = m_filesTextSection.GetAttribute<string>("cmd", String.Empty);
    }

    void CreateFolders()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Chars/" + m_charName))
        {
            string guid1 = AssetDatabase.CreateFolder("Assets/Chars", m_charName);
            string newFolderPath = AssetDatabase.GUIDToAssetPath(guid1);
        }

        if (!AssetDatabase.IsValidFolder("Assets/StreamingAssets/" + m_charName))
            AssetDatabase.CreateFolder("Assets/StreamingAssets", m_charName);

        if (!AssetDatabase.IsValidFolder("Assets/StreamingAssets/" + m_charName + "/States"))
            AssetDatabase.CreateFolder("Assets/StreamingAssets/" + m_charName, "States");


    }

    void ImportFiles()
    {
        foreach (FileDirectory fileDirectory in m_fileDirectories)
        {
            if (!System.IO.File.Exists(fileDirectory.directoryOutput))
                System.IO.File.Copy(fileDirectory.directoryInput, fileDirectory.directoryOutput);
        }
    }

    void CreateAssets()
    {
        m_manager.commandsList = new ConvertCmdToCommandListEditorWindow().BuildExternal(FilterPathInput(m_commandPath), m_charName, OutputChars + m_charName + "/");
        m_manager.playerConstants = new ConverterPlayerConstantEditorWindow().BuildExternal(FilterPathInput(m_constantFile), m_charName, OutputChars + m_charName + "/");
        AssetDatabase.CreateAsset(m_manager, OutputChars + m_charName + "/" + m_charName + "PlayerProfileManager.asset");

        AssetDatabase.SaveAssets();
        Selection.activeObject = m_manager;
        EditorUtility.FocusProjectWindow();
    }

    private MugenVersion BuildMugenVersion(string input)
    {
        switch (input)
        {
            case "04,14,2002":
                return MugenVersion.V_2002;
            case "1.0":
                return MugenVersion.V_1_0;
            case "1.1":
                return MugenVersion.V_1_1;
        }
        return MugenVersion.V_1_1;
    }

    private int[] BuildPaletteOrder(string input)
    {
        if (input == null)
            return new int[0];

        string[] palinfo = input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        List<int> order = new List<int>();
        foreach (var palnumber in palinfo)
        {
            if (int.TryParse(palnumber, out var palvalue)) order.Add(palvalue);
        }

        return order.ToArray();
    }

    private string[] BuildStateFiles(TextSection filesection)
    {
        if (filesection == null) throw new ArgumentNullException(nameof(filesection));

        var files = new SortedList<int, string>();
        files.Add(-1, FilterPathInput(m_commandPath));

        foreach (var kvp in filesection.ParsedLines)
        {
            if (string.Compare(kvp.Key, 0, "st", 0, 2, StringComparison.OrdinalIgnoreCase) != 0) continue;

            if (string.Equals(kvp.Key, "st", StringComparison.OrdinalIgnoreCase))
            {
                var path = FilterPathInput(kvp.Value);
                if (path != string.Empty) files[0] = path;
            }
            else
            {
                if (int.TryParse(kvp.Key.Substring(2), out var index))
                {
                    var path = FilterPathInput(kvp.Value);
                    if (path != string.Empty) files[index + 1] = path;
                }
            }
        }

        return new List<string>(files.Values).ToArray();
    }

    private string[] StatesImport(TextSection filesection)
    {
        if (filesection == null) throw new ArgumentNullException(nameof(filesection));

        var files = new List<string>();

        files.Add("/Data/TrainnerState.cns");
        files.Add("/Data/ScoreState.cns");
        files.Add("/" + m_charName + "/States/" + RemoveFolders(m_stCommon));
        files.Add("/" + m_charName + "/States/" + RemoveFolders(m_commandPath));

        foreach (var kvp in filesection.ParsedLines)
        {
            string pathCorrect = kvp.Value;
            if (kvp.Value.LastIndexOf("/") > -1)
                pathCorrect = kvp.Value.Substring(kvp.Value.LastIndexOf("/") + 1);
            
            if (string.Compare(kvp.Key, 0, "st", 0, 2, StringComparison.OrdinalIgnoreCase) != 0) continue;

            if (string.Equals(kvp.Key, "st", StringComparison.OrdinalIgnoreCase))
            {
                var path = FilterPathInput(pathCorrect);
                if (path != string.Empty)
                {
                    files.Add("/" + m_charName + "/States/" + pathCorrect);
                }
            }
            else
            {
                if (int.TryParse(kvp.Key.Substring(2), out var index))
                {
                    var path = FilterPathInput(pathCorrect);
                    if (path != string.Empty)
                    {
                        files.Add("/" + m_charName + "/States/" + pathCorrect);
                    }
                }
            }
        }

        return files.ToArray();
    }


    private void BuildFileDirectory(TextSection filesection)
    {
        if (filesection == null) throw new ArgumentNullException(nameof(filesection));

        if (!string.IsNullOrEmpty(m_stCommon))
        {
            m_fileDirectories.Add(new FileDirectory()
            {
                fileName = m_stCommon,
                directoryInput = FilterPathInput(m_stCommon),
                directoryOutput = FilterPathOutput(m_charName + "/States/" + m_stCommon),
                directoryUnity = "/" + m_charName + "/" + m_stCommon
            });
        }
        m_fileDirectories.Add(new FileDirectory()
        {
            fileName = m_sffPath,
            directoryInput = FilterPathInput(m_sffPath),
            directoryOutput = FilterPathOutput(m_charName + "/" + m_sffPath),
            directoryUnity = "/" + m_charName + "/" + m_sffPath
        });

        m_fileDirectories.Add(new FileDirectory()
        {
            fileName = m_airPath,
            directoryInput = FilterPathInput(m_airPath),
            directoryOutput = FilterPathOutput(m_charName + "/" + m_airPath),
            directoryUnity = "/" + m_charName + "/" + m_airPath
        });

        m_fileDirectories.Add(new FileDirectory()
        {
            fileName = m_sndPath,
            directoryInput = FilterPathInput(m_sndPath),
            directoryOutput = FilterPathOutput(m_charName + "/" + m_sndPath),
            directoryUnity = "/" + m_charName + "/" + m_airPath
        });

        m_fileDirectories.Add(new FileDirectory()
        {
            fileName = m_commandPath,
            directoryInput = FilterPathInput(m_commandPath),
            directoryOutput = FilterPathOutput(m_charName + "/States/" + m_commandPath),
            directoryUnity = "/" + m_charName + "/" + m_commandPath
        });

        foreach (var kvp in filesection.ParsedLines)
        {
            if (string.Compare(kvp.Key, 0, "st", 0, 2, StringComparison.OrdinalIgnoreCase) != 0) continue;

            if (string.Equals(kvp.Key, "st", StringComparison.OrdinalIgnoreCase))
            {
                var path = FilterPathInput(kvp.Value);
                if (path != string.Empty)
                {
                    m_fileDirectories.Add(new FileDirectory()
                    {
                        fileName = kvp.Value,
                        directoryInput = path,
                        directoryOutput = FilterPathOutput(m_charName + "/States/" + kvp.Value),
                        directoryUnity = "/" + m_charName + "/" + kvp.Value
                    });
                }
            }
            else
            {
                if (int.TryParse(kvp.Key.Substring(2), out var index))
                {
                    var path = FilterPathInput(kvp.Value);
                    if (path != string.Empty)
                    {
                        m_fileDirectories.Add(new FileDirectory()
                        {
                            fileName = kvp.Value,
                            directoryInput = path,
                            directoryOutput = FilterPathOutput(m_charName + "/States/" + kvp.Value),
                            directoryUnity = "/" + m_charName + "/" + kvp.Value
                        });
                    }
                }
            }
        }
    }

    string RemoveFolders(string path)
    {
        if (path.Contains("/"))
            return path.Substring(path.LastIndexOf("/") + 1);
        return path;
    }

    private string FilterPathInput(string filepath)
    {
        string path = m_basePathInput;
        if (string.IsNullOrEmpty(filepath)) return string.Empty;
        if (filepath == "common1.cns")
        {
            path = path.Remove(path.IndexOf("chars"));
            path = path + "data";
        }
        return Path.Combine(path, filepath);
    }

    private string FilterPathOutput(string filepath)
    {
        if (string.IsNullOrEmpty(filepath)) return string.Empty;

        if (filepath.Contains("/"))
        {
            if(filepath.Contains("/States/"))
                filepath = m_charName + "/States/" + filepath.Substring(filepath.LastIndexOf("/") + 1);
            else
                filepath = m_charName + "/" + filepath.Substring(filepath.LastIndexOf("/") + 1);
        }
        return Path.Combine(m_basePathOutput, filepath);
    }

    private struct FileDirectory
    {
        public string fileName;
        public string directoryInput;
        public string directoryOutput;
        public string directoryUnity;
    }

}
