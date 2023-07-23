using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UnityMugen.IO
{
    public class FileToTextFile
    {
        public static TextFile Build(string text)
        {

            string[] lines = Regex.Split(text, "\n|\r|\r\n");
            var m_titleregex = new Regex(@"^\s*\[(.+?)\]\s*$", RegexOptions.IgnoreCase);
            var m_parsedlineregex = new Regex(@"^\s*(.+?)\s*=\s*(.+?)\s*$", RegexOptions.IgnoreCase);

            List<string> newLines = new List<string>();

            var sections = new List<TextSection>();

            string Comment = "";
            string sectiontitle = null;

            //KeyValuePair<int, string> lastComment;

            List<string> sectionlines = null;
            List<KeyValuePair<string, string>> sectionparsedlines = null;

            for (int i = 0; i < lines.Length; i++)
            {
                string text1 = lines[i].Trim();
                if (text1 != string.Empty)
                    newLines.Add(lines[i]);
            }

            for (int i = 0; i < newLines.Count; i++)
            {
                newLines[i] = newLines[i].Trim();

                var commentindex = newLines[i].IndexOf(';');

                //if (commentindex == 0 && newLines[i].Length > 2)
                //    lastComment = new KeyValuePair<int, string>(i, newLines[i]);

                string correctLine = "";
                if (commentindex != -1)
                    correctLine = newLines[i].Substring(0, commentindex);
                else
                    correctLine = newLines[i];

                if (correctLine == string.Empty)
                    continue;

                var titlematch = m_titleregex.Match(correctLine);
                if (titlematch.Success)
                {
                    if (sectiontitle != null)
                        sections.Add(new TextSection(sectiontitle, sectionlines, sectionparsedlines));

                    if (i > 0 && newLines[i - 1].IndexOf(';') == 0 &&
                            newLines[i - 1].Length > 2)
                    {
                        Comment = newLines[i - 1].Substring(1, newLines[i - 1].Length - 1).Trim();
                    }

                    sectiontitle = titlematch.Groups[1].Value;
                    sectionlines = new List<string>();
                    sectionparsedlines = new List<KeyValuePair<string, string>>();
                }
                else if (sectiontitle != null)
                {
                    sectionlines.Add(correctLine);

                    var parsedmatch = m_parsedlineregex.Match(correctLine);
                    if (parsedmatch.Success)
                    {
                        var key = parsedmatch.Groups[1].Value;
                        var value = parsedmatch.Groups[2].Value;

                        if (value.Length >= 2 && value[0] == '"' && value[value.Length - 1] == '"') value = value.Substring(1, value.Length - 2);

                        sectionparsedlines.Add(new KeyValuePair<string, string>(key, value));
                    }
                }
            }

            if (sectiontitle != null)
                sections.Add(new TextSection(sectiontitle, sectionlines, sectionparsedlines));

            return new TextFile("", sections);
        }
    }
}