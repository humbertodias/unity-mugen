using System;
using UnityEngine;

namespace UnityMugen
{
    [Serializable]
    public class StageProfile
    {
        public int stageID;
        public int musicID;

        [Header("Info")]
        public string Name;

        [Tooltip("Name to display")]
        public string DisplayName;

        [Tooltip("Stage author name")]
        public string Author;

        public Sprite spriteStage;
    }
}