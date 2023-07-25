using UnityEngine;
using UnityMugen;

namespace UnityMugen
{

    public class InitializationSettings : MonoBehaviour
    {

        [Header("Video Settings")]
        public bool VSync = true;
        public bool isFullScreen = true;
        public bool isShakeCamera = true;

        [Header("Replay Settings")]
        public bool RecordReplay = true;
        public bool QuitAfterReplay = true;

        [Header("Debug Settings")]
        public bool KeepLog = false;
        public bool SaveSeedDebug;

        [Header("Game Settings")]
        [Range(1, 8)]
        public int AiLevel = 4;
        public int NumberOfRounds = 2; //Rounds needed to win a match
        public int RoundLength = 99;
        public SpeedTime SpeedTime = SpeedTime.Normal;
        public int idVoiceLanguage = 0;
        public int idTextLanguage = 0;

        [Header("Commons")]
        public int SoundChannels = 10;
        public string comboText = "Combo";
        public int displaytimeCombo = 90;
        public bool PreloadCharacterSprites = true;

        [Header("UnityMugen cfg")] // Arquivo: mugen.cfg
        public int GameSpeed = 60; // fps
        public float Default_Attack_LifeToPowerMul = 0.7f;
        public float Default_GetHit_LifeToPowerMul = 0.6f;

        [Header("Others")]
        [Tooltip("Sugerido usar o tempo maximo da transicao de scena 'effectTransition1In'")]
        public int IntroDelay = 45; //Time to wait before starting intro
        public int ControlTime = 30; //Time players get control after "Fight"
        public int KOSlowTime = 60; //Time for KO slowdown (in ticks)
        public int OverWaitTime = 45; //Time to wait after KO before player control is stopped
        public int OverTime = 210; //Time to wait before round ends
        public int WinTime = 60;

        [Header("Players Controllers")]
        public PlayerID controller1 = PlayerID.One;
        public PlayerID controller2 = PlayerID.Two;


        public InitializationSettings Initialize()
        {
            if (PlayerPrefs.GetInt("controller1") == 1)
                controller1 = PlayerID.One;
            else if (PlayerPrefs.GetInt("controller1") == 2)
                controller1 = PlayerID.Two;

            if (PlayerPrefs.GetInt("controller2") == 1)
                controller2 = PlayerID.One;
            else if (PlayerPrefs.GetInt("controller2") == 2)
                controller2 = PlayerID.Two;

            PreLoadData();

            return this;
        }


        private void PreLoadData()
        {
            SpeedTime = (SpeedTime)PlayerPrefs.GetInt("Definitions_SpeedTime");
            RoundLength = Constant.roundLengths[PlayerPrefs.GetInt("Definitions_TimeRound")];
            NumberOfRounds = PlayerPrefs.GetInt("Definitions_NumberOfRounds");
            AiLevel = PlayerPrefs.GetInt("Definitions_AILevel");
        }

    }
}