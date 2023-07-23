using System;
using UnityEngine;

public class Readme : ScriptableObject
{
    public Texture2D icon;
    public Section[] sections;
    public bool loadedLayout;

    //public bool editReadme;

    [Serializable]
    public class Section
    {
        public string heading;
        public string[] texts;
        public string linkText;
        public string url;
    }
}

