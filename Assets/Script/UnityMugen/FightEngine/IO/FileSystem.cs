using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityMugen.Collections;


namespace UnityMugen.IO
{
    /// <summary>
    /// Interfaces between game code underlying OS filesystem.
    /// </summary>
    public class FileSystem
    {

        public FileSystem()
        {
            m_textcache = new KeyedCollection<string, TextFile>(x => x.Filepath, StringComparer.OrdinalIgnoreCase);
            m_titleregex = new Regex(@"^\s*\[(.+?)\]\s*$", RegexOptions.IgnoreCase);
            m_parsedlineregex = new Regex(@"^\s*(.+?)\s*=\s*(.+?)\s*$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <param name="filepath">The file to look for.</param>
        /// <returns>true is the file exists; false otherwise.</returns>
        public bool DoesFileExist(string filepath)
        {
            if (filepath == null) throw new ArgumentNullException(nameof(filepath));
            TextAsset ta = Resources.Load<TextAsset>(filepath);
            return ta != null;
        }

        /// <summary>
        /// Combines two paths strings.
        /// </summary>
        /// <param name="lhs">The first path to combine.</param>
        /// <param name="rhs">The second path to combine.</param>
        /// <returns>The combined path string.</returns>
        public string CombinePaths(string lhs, string rhs)
        {
            if (lhs == null) throw new ArgumentNullException(nameof(lhs));
            if (rhs == null) throw new ArgumentNullException(nameof(rhs));

            return Path.Combine(lhs, rhs);
        }

        /// <summary>
        /// Returns the directory path of the supplied path string.
        /// </summary>
        /// <param name="filepath">The path of a file or directory.</param>
        /// <returns>A string containing the filepath to the containing directory.</returns>
        public string GetDirectory(string filepath)
        {
            if (filepath == null) throw new ArgumentNullException(nameof(filepath));

            return Path.GetDirectoryName(filepath);
        }


        /// <summary>
        /// Opens a file with the given path and parses it as text.
        /// </summary>
        /// <param name="filepath">The path to the file to be opened.</param>
        /// <returns>A fightEngine.IO.TextFile for the given path.</returns>
        public TextFile OpenTextFile(string filepath)
        {
            if (filepath == null) throw new ArgumentNullException(nameof(filepath));

            if (m_textcache.Contains(filepath)) return m_textcache[filepath];

            return BuildTextFile(OpenFile(filepath));
        }


        /// <summary>
        /// Opens a file with the given path.
        /// </summary>
        /// <param name="filepath">The path to the file to be opened.</param>
        /// <returns>A UnityMugen.IO.File for the given path.</returns>
        public File OpenFile(string filepath)
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


        public TextFile OpenTextFile(TextAsset textAsset)
        {
            if (textAsset == null) throw new ArgumentNullException(nameof(textAsset));

            if (m_textcache.Contains(textAsset.name)) return m_textcache[textAsset.name];

            return BuildTextFile(textAsset);
        }

        /// <summary>
        /// Parses a fightEngine.IO.File as a text file.
        /// </summary>
        /// <param name="file">The file to be parsed. The read position must be set to the beginning of the text location.</param>
        /// <returns>The fightEngine.IO.TextFile parsed out of the given file.</returns>
        public TextFile BuildTextFile(string file)
        {
            if (m_textcache.Contains(file)) return m_textcache[file];

            var textfile = Build(file);
            m_textcache.Add(textfile);

            return textfile;
        }

        public TextFile BuildTextFile(TextAsset textAsset)
        {
            if (m_textcache.Contains(textAsset.name)) return m_textcache[textAsset.name];

            var textfile = Build(textAsset);
            m_textcache.Add(textfile);

            return textfile;
        }

        private TextFile Build(string file)
        {
            // Formatos Suportados  por Resources.Load<TextAsset>: (.txt .html .htm .xml .bytes .json .csv .amil .fnt
            TextAsset text = Resources.Load<TextAsset>(file); // Não aceita caracters Chines - Japones

            if (text == null)
            {
                UnityEngine.Debug.LogError("Diretorio: " + file + " não encontrado.");
                throw new ArgumentNullException(nameof(file));
            }
            string[] lines = Regex.Split(text.text, "\n|\r|\r\n");



            var sections = new List<TextSection>();

            string sectiontitle = null;
            List<string> sectionlines = null;
            List<KeyValuePair<string, string>> sectionparsedlines = null;

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim();

                var commentindex = lines[i].IndexOf(';');
                if (commentindex != -1) lines[i] = lines[i].Substring(0, commentindex);

                if (lines[i] == string.Empty) continue;

                var titlematch = m_titleregex.Match(lines[i]);
                if (titlematch.Success)
                {
                    if (sectiontitle != null) sections.Add(new TextSection(sectiontitle, sectionlines, sectionparsedlines));

                    sectiontitle = titlematch.Groups[1].Value;
                    sectionlines = new List<string>();
                    sectionparsedlines = new List<KeyValuePair<string, string>>();
                }
                else if (sectiontitle != null)
                {
                    sectionlines.Add(lines[i]);

                    var parsedmatch = m_parsedlineregex.Match(lines[i]);
                    if (parsedmatch.Success)
                    {
                        var key = parsedmatch.Groups[1].Value;
                        var value = parsedmatch.Groups[2].Value;

                        if (value.Length >= 2 && value[0] == '"' && value[value.Length - 1] == '"') value = value.Substring(1, value.Length - 2);

                        sectionparsedlines.Add(new KeyValuePair<string, string>(key, value));
                    }
                    else
                    {
                        UnityEngine.Debug.LogError("never create an empty attribute: " + sectiontitle + " - " + lines[i]);
                    }
                }
            }

            if (sectiontitle != null) sections.Add(new TextSection(sectiontitle, sectionlines, sectionparsedlines));

            return new TextFile(file, sections);
        }

        private TextFile Build(TextAsset text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            string[] lines = Regex.Split(text.text, "\n|\r|\r\n");

            var sections = new List<TextSection>();

            string sectiontitle = null;
            List<string> sectionlines = null;
            List<KeyValuePair<string, string>> sectionparsedlines = null;

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim();

                var commentindex = lines[i].IndexOf(';');
                if (commentindex != -1) lines[i] = lines[i].Substring(0, commentindex);

                if (lines[i] == string.Empty) continue;

                var titlematch = m_titleregex.Match(lines[i]);
                if (titlematch.Success)
                {
                    if (sectiontitle != null) sections.Add(new TextSection(sectiontitle, sectionlines, sectionparsedlines));

                    sectiontitle = titlematch.Groups[1].Value;
                    sectionlines = new List<string>();
                    sectionparsedlines = new List<KeyValuePair<string, string>>();
                }
                else if (sectiontitle != null)
                {
                    sectionlines.Add(lines[i]);

                    var parsedmatch = m_parsedlineregex.Match(lines[i]);
                    if (parsedmatch.Success)
                    {
                        var key = parsedmatch.Groups[1].Value;
                        var value = parsedmatch.Groups[2].Value;

                        if (value.Length >= 2 && value[0] == '"' && value[value.Length - 1] == '"') value = value.Substring(1, value.Length - 2);

                        sectionparsedlines.Add(new KeyValuePair<string, string>(key, value));
                    }
                    else
                    {
                        UnityEngine.Debug.LogError("never create an empty attribute: " + sectiontitle + " - " + lines[i]);
                    }
                }
            }

            if (sectiontitle != null) sections.Add(new TextSection(sectiontitle, sectionlines, sectionparsedlines));

            return new TextFile(text.name, sections);
        }

        /// <summary>
        /// Parses a xnaMugen.IO.File as a text file.
        /// </summary>
        /// <param name="file">The file to be parsed. The read position must be set to the beginning of the text location.</param>
        /// <returns>The xnaMugen.IO.TextFile parsed out of the given file.</returns>
        public TextFile BuildTextFile(File file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            if (m_textcache.Contains(file.Filepath)) return m_textcache[file.Filepath];

            var textfile = Build(file);
            m_textcache.Add(textfile);

            return textfile;
        }

        private TextFile Build(File file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            var sections = new List<TextSection>();

            string sectiontitle = null;
            List<string> sectionlines = null;
            List<KeyValuePair<string, string>> sectionparsedlines = null;
            HashSet<string> existKey = null;

            for (var line = file.ReadLine(); line != null; line = file.ReadLine())
            {
                line = line.Trim();

                var commentindex = line.IndexOf(';');
                if (commentindex != -1) line = line.Substring(0, commentindex);

                if (line == string.Empty) continue;

                var titlematch = m_titleregex.Match(line);
                if (titlematch.Success)
                {
                    if (sectiontitle != null) sections.Add(new TextSection(sectiontitle, sectionlines, sectionparsedlines));

                    sectiontitle = titlematch.Groups[1].Value;
                    sectionlines = new List<string>();
                    sectionparsedlines = new List<KeyValuePair<string, string>>();
                    existKey = new HashSet<string>();
                }
                else if (sectiontitle != null)
                {
                    sectionlines.Add(line);

                    var parsedmatch = m_parsedlineregex.Match(line);
                    if (parsedmatch.Success)
                    {
                        var key = parsedmatch.Groups[1].Value.ToLower();
                        var value = parsedmatch.Groups[2].Value;

                        if (value.Length >= 2 && value[0] == '"' && value[value.Length - 1] == '"') value = value.Substring(1, value.Length - 2);

                        if (!existKey.Contains(key))
                        {
                            if(!(key.Length >= 7 && key.Substring(0, 7) == "trigger"))
                                existKey.Add(key);
    
                            sectionparsedlines.Add(new KeyValuePair<string, string>(key, value));
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.LogError("never create an empty attribute: "+ sectiontitle + " - " + line);
                    }
                }
            }

            if (sectiontitle != null) sections.Add(new TextSection(sectiontitle, sectionlines, sectionparsedlines));

            return new TextFile(file.Filepath, sections);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private KeyedCollection<string, TextFile> m_textcache;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Regex m_titleregex;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Regex m_parsedlineregex;

    }
}