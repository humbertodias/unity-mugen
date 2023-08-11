using System.Collections.Generic;
using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Commands;
using Object = UnityEngine.Object;

namespace UnityMugen
{

    [CreateAssetMenu(fileName = "PlayerProfileManager", menuName = "Unity Mugen/Character/Player Profile Manager")]
    public class PlayerProfileManager : ScriptableObject
    {

        public int charID { get; set; }

        [Header("Info")]
        public string charName;
        public string displayName;
        public string author;
        public MugenVersion mugenVersion = MugenVersion.V_1_0;
        public string versionDate;
        public Sprite smallPortrait;
        public Sprite largePortrait;
        public int[] palettesIndex;
        public string[] palettesName;

        [Header("Commons")]
        public PlayerConstants playerConstants;
        public CommandList commandsList;
        public MoveList moveList;

        [Header("Arcade Mode")]
        public Object intro;
        public Object ending;

        [Header("Old Mode Def")]
        public string sff;
        public string air;
        public string snd;


        [Header("State Path")]
        public string[] states;

        public string[] NamePalettes()
        {
            List<string> palettes = new List<string>();
            for (int i = 0; i < palettesName.Length; i++)
            {
                string path = Application.streamingAssetsPath + "/" + charName + "/Palettes/" + palettesName[i] + ".act";
                if (System.IO.File.Exists(path))
                    palettes.Add(path);
                else
                    Debug.LogWarning("File not exist: " + path);
            }
            return palettes.ToArray();
        }

        public string NamefileSFF()
        {
            string path = Application.streamingAssetsPath + "/" + charName + "/" + (sff ?? charName) + ".sff";
            if (System.IO.File.Exists(path))
                return path;
            else
                throw new UnityMugenException("Essential File not exist: " + path);
        }

        public string NamefileAIR()
        {
            string path = Application.streamingAssetsPath + "/" + charName + "/" + (air ?? charName) + ".air";
            if (System.IO.File.Exists(path))
                return path;
            else
                throw new UnityMugenException("Essential File not exist: " + path);
        }

        public string NamefileSND()
        {
            if (string.IsNullOrEmpty(snd))
                return null;

            string path = Application.streamingAssetsPath + "/" + charName + "/" + (snd ?? charName) + ".snd";
            if (System.IO.File.Exists(path))
                return path;
            else
                throw new UnityMugenException("Essential File not exist: " + path);
        }

    }
}