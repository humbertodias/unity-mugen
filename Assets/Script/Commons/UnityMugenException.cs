using System;
using UnityEngine;

public class UnityMugenException : Exception
{

    public UnityMugenException(string message) : base(message)
    {
        ShowLog(message);
        StopApplication();
    }

    public UnityMugenException(string message, Exception ex) : base(message)
    {
        ShowLog(message+" - "+ ex.ToString());
        StopApplication();
    }


    private void ShowLog(string message)
    {
#if UNITY_EDITOR
        System.Diagnostics.Debug.WriteLine(message);
        Debug.LogError(message);
#else

#endif
    }

    private void StopApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        //Application.Quit();
#endif
    }
}
