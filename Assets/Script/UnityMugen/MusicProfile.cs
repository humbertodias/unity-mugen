using System;
using UnityEngine;

namespace UnityMugen
{

    [Serializable]
    public class MusicProfile
    {
        public int musicID;
        public string nameMusic;

        public AudioClip musicStart;
        public AudioClip musicLoop;
        public AudioClip musicEnd;

        public int musicVolume = 255;
    }
}