using UnityEngine;
using UnityMugen;
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


        public string NamefileSFF()
        {
            return Application.streamingAssetsPath + "/" + charName + "/" + (sff ?? charName) + ".sff";
        }

        public string NamefileAIR()
        {
            return Application.streamingAssetsPath + "/" + charName + "/" + (air ?? charName) + ".air";
        }

        public string NamefileSND()
        {
            if (string.IsNullOrEmpty(snd))
                return null;

            return Application.streamingAssetsPath + "/" + charName + "/" + (snd ?? charName) + ".snd";
        }

    }
}