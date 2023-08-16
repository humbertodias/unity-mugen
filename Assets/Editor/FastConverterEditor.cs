using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityMugen.IO;
using File = UnityMugen.IO.File;

/// <summary>
/// Use this script in the future to update the classes
/// </summary>
public class FastConverter : EditorWindow
{
    [SerializeField] string _filepath;
    [TextArea(10, 15)] public string stringConverter;
    [TextArea(15, 20)] public string resultConvert;

    private UnityMugen.Collections.KeyedCollection<string, TextFile> m_textcache;
    private Regex m_titleregex;
    private Regex m_parsedlineregex;

    //[MenuItem("UnityMugen/FastConverter")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(FastConverter)).Show();
    }

    void OnGUI()
    {
        SerializedObject serializedObject = new SerializedObject(this);

        if (GUILayout.Button("Converter State File"))
        {
            if (string.IsNullOrEmpty(stringConverter))
            {
                string file = EditorUtility.OpenFilePanel("Load .cns file", "", "cns,cmd,st,txt,mfg");
                if (file.Length != 0)
                {
                    OpenTextFile(file);

                    this.ShowNotification(new GUIContent("Finish."));
                }
            }
            else
            {
                UpdateScript();
            }
        }

        EditorGUILayout.LabelField("Se [Debug Active] estiver selecionado, ");
        EditorGUILayout.LabelField("não sera gerado codigo .cs.");
        EditorGUILayout.LabelField("Usar isso para quando quiser fazer Debugs no codigo.");

        EditorGUI.BeginChangeCheck();
        stringConverter = EditorGUILayout.TextArea(stringConverter, GUILayout.Height(120));
        if (EditorGUI.EndChangeCheck())
        {
            UpdateScript();
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("resultConvert"), true);

        serializedObject.ApplyModifiedProperties();
    }

    private void UpdateScript()
    {
        lista = new List<Tuple<string, string, string, string, bool>>();
        string[] lines = stringConverter.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        foreach (string line in lines)
        {
            TrataLinha(line);
        }
        resultConvert = CriarMetodoSetAttributes();
    }

    public TextFile OpenTextFile(string filepathR)
    {
        m_textcache = new UnityMugen.Collections.KeyedCollection<string, TextFile>(x => x.Filepath, StringComparer.OrdinalIgnoreCase);
        m_titleregex = new Regex(@"^\s*\[(.+?)\]\s*$", RegexOptions.IgnoreCase);
        m_parsedlineregex = new Regex(@"^\s*(.+?)\s*=\s*(.+?)\s*$", RegexOptions.IgnoreCase);

        if (filepathR == null) throw new ArgumentNullException(nameof(filepathR));

        if (m_textcache.Contains(filepathR)) return m_textcache[filepathR];

        return BuildTextFile(OpenFile(filepathR));
    }

    File OpenFile(string filepath)
    {
        try
        {
            if (filepath == null) throw new ArgumentNullException(nameof(filepath));

            if (Environment.OSVersion.Platform == PlatformID.MacOSX || Environment.OSVersion.Platform == PlatformID.Unix)
            {
                filepath = filepath.Replace('\\', '/');
            }
            UnityEngine.Debug.LogFormat("Opening file: {0}", filepath);

            if (string.Compare(filepath, 0, "UnityMugen.", 0, 9, StringComparison.Ordinal) == 0)
            {
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(filepath);
                if (stream != null) return new File(filepath, stream);
            }

            return new File(filepath, new FileStream(filepath, FileMode.Open, FileAccess.Read));
        }
        catch (FileNotFoundException)
        {
            UnityEngine.Debug.LogErrorFormat("File not found: {0}", filepath);
            throw;
        }
    }

    TextFile BuildTextFile(File file)
    {
        if (file == null) throw new ArgumentNullException(nameof(file));

        if (m_textcache.Contains(file.Filepath)) return m_textcache[file.Filepath];

        lista = new List<Tuple<string, string, string, string, bool>>();
        var textfile = Build(file);
        m_textcache.Add(textfile);

        return textfile;
    }

    private TextFile Build(File file)
    {
        if (file == null) throw new ArgumentNullException(nameof(file));

        var sections = new List<TextSection>();

        for (var line = file.ReadLine(); line != null; line = file.ReadLine())
            TrataLinha(line);

        resultConvert = CriarMetodoSetAttributes();

        return new TextFile(file.Filepath, sections);
    }


    private string CriarMetodoSetAttributes()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("    public override void SetAttributes(string idAttribute, string expression)");
        sb.AppendLine("    {");
        sb.AppendLine("        base.SetAttributes(idAttribute, expression);");
        sb.AppendLine("        switch (idAttribute)");
        sb.AppendLine("        {");

        foreach (var data in lista)
        {
            sb.AppendLine($"            case \"{data.Item1.ToLower()}\":");
            if (data.Item5 == true)
                sb.AppendLine($"                {data.Item2} = GetAttribute<{data.Item4}>(expression, {data.Item3});");
            else
                sb.AppendLine($"                {data.Item2} = GetAttribute(expression, {data.Item3});");
            sb.AppendLine($"                break;");
        }

        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine("");

        return sb.ToString();
    }


    List<Tuple<string, string, string, string, bool>> lista = new List<Tuple<string, string, string, string, bool>>();
    void TrataLinha(string linha)
    {
        if (linha.Contains("\"") &&
            linha.Contains(" ") &&
            linha.Contains(",") &&
            linha.Contains(");"))
        {

            string[] nomeAtributo = linha.Split('"');
            string nomeVariavel = linha.TrimStart().Substring(0, linha.TrimStart().IndexOf('=') - 1);
            if (nomeVariavel.Contains(" ") || nomeVariavel.Contains("//"))
                return;

            string tipoRetorno = linha.Substring(linha.IndexOf(',') + 1, linha.IndexOf(");") - linha.IndexOf(',') - 1).Trim();

            string tipoExpression = null;
            if (linha.Contains("<") && linha.Contains(">"))
            {
                tipoExpression = linha.Substring(linha.IndexOf('<') + 1, linha.IndexOf(">") - linha.IndexOf('<') - 1);
            }

            if (nomeAtributo.Length >= 1 && nomeVariavel != null && tipoRetorno != null)
            {
                if (tipoExpression != null)
                    lista.Add(new Tuple<string, string, string, string, bool>(nomeAtributo[1], nomeVariavel, tipoRetorno, tipoExpression, true));
                else
                    lista.Add(new Tuple<string, string, string, string, bool>(nomeAtributo[1], nomeVariavel, tipoRetorno, tipoExpression, false));
            }

        }
    }

}
