using System;
using System.IO;
using UnityEngine;

namespace UnityMugen.Interface
{

    public class LogMessage
    {
        static FileInfo fi;

        public static void Create()
        {
            try
            {

                if (fi == null)
                {
                    fi = new FileInfo(Application.dataPath + "/Log.txt");
                    if (fi.Exists)
                        fi.Delete();

                    TextWriter writer = new StreamWriter(Application.dataPath + "/Log.txt", true);
                    string allMessage = DateTime.Now.ToString("dd/MM/yyyy");
                    writer.WriteLine(allMessage);
                    writer.Flush();
                    writer.Close();
                }
            }
            catch (Exception Ex)
            {
                Debug.Log(Ex.ToString());
            }
        }

        public static void Log(object message)
        {
            try
            {
                string allMessage = "OK - " + DateTime.Now.ToString() + " - " + (string)message;
#if UNITY_EDITOR
                if (fi == null)
                    Create();

                TextWriter writer = new StreamWriter(Application.dataPath + "/Log.txt", true);
                writer.WriteLine(allMessage);
                writer.Flush();
                writer.Close();
#endif
                Debug.Log(allMessage);
            }
            catch (Exception Ex)
            {
                Debug.Log(Ex.ToString());
            }
        }

        public static void LogWarning(object message)
        {
            try
            {
                string allMessage = "WARNING - " + DateTime.Now.ToString() + " - " + (string)message;
#if UNITY_EDITOR
                if (fi == null)
                    Create();

                TextWriter writer = new StreamWriter(Application.dataPath + "/Log.txt", true);
                writer.WriteLine(allMessage);
                writer.Flush();
                writer.Close();
#endif
                Debug.LogWarning(allMessage);
            }
            catch (Exception Ex)
            {
                Debug.LogWarning(Ex.ToString());
            }
        }

        public static void LogError(object message)
        {
            try
            {
                string allMessage = "ERROR - " + DateTime.Now.ToString() + " - " + (string)message;
#if UNITY_EDITOR
                if (fi == null)
                    Create();

                TextWriter writer = new StreamWriter(Application.dataPath + "/Log.txt", true);
                writer.WriteLine(allMessage);
                writer.Flush();
                writer.Close();
#endif
                Debug.LogError(allMessage);
            }
            catch (Exception Ex)
            {
                Debug.LogError(Ex.ToString());
            }
        }

    }
}