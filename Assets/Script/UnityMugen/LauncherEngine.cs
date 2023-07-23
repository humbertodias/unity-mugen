using System;
using UnityEngine;
using UnityMugen.Animations;
using UnityMugen.Audio;
using UnityMugen.Combat;
using UnityMugen.Commands;
using UnityMugen.Drawing;
using UnityMugen.Evaluation;
using UnityMugen.Input;
using UnityMugen.IO;
using UnityMugen.NetcodeUM;
using UnityMugen.Screens;
using UnityMugen.StateMachine;

namespace UnityMugen
{

    public enum TypeCurrentScreen
    {
        None,
        MenuScreen,
        RoomScreen,
        SelectScreen,
        VersusScreen,
        CombatScreen
    }

    public class LauncherEngine : MonoBehaviour
    {

        public static LauncherEngine Inst { private set; get; }

        //public AnimationManager fxAnimations;
        //public SpriteFEManager fxSprites;
        //public Palette fxPalette;
        //public ShowMessage showMessage;

        public ScreenType screenType;
        public ProfileLoader profileLoader;

        public UnityMugen.Random random;

        [NonSerialized] public InitializationSettings initializationSettings;
        [NonSerialized] public TrainnerSettings trainnerSettings;
        [NonSerialized] public NetworkSettings networkSettings;
        [NonSerialized] public EngineInitialization engineInitialization;
        [NonSerialized] public InputSystem inputSystem;
        [NonSerialized] public SoundSystem soundSystem;
        [NonSerialized] public SpriteSystem spriteSystem;
        [NonSerialized] public PaletteSystem paletteSystem;
        [NonSerialized] public AnimationSystem animationSystem;
        public CommandSystem commandSystem;
        [NonSerialized] public StringConverter stringConverter;
        [NonSerialized] public FileSystem fileSystem;
        [NonSerialized] public EvaluationSystem evaluationSystem;
        [NonSerialized] public StateSystem stateSystem;

        [NonSerialized] public TypeCurrentScreen typeCurrentScreenP1;
        [NonSerialized] public TypeCurrentScreen typeCurrentScreenP2;
        [NonSerialized] public Mugen mugen;

        public void Awake()
        {
            if (Inst == null)
            {
                if(profileLoader == null)
                    throw new Exception("Profile loader is essential to start the project.");

                Inst = this;
                initializationSettings = GetComponent<InitializationSettings>();
                Application.runInBackground = true;
                Application.targetFrameRate = initializationSettings.GameSpeed; ;

                //PlayerPrefs.DeleteAll();
                if (!PlayerPrefs.HasKey("isFirstAccess"))
                    FirstAccess();

                initializationSettings = initializationSettings.Initialize();
                trainnerSettings = GetComponent<TrainnerSettings>();
                networkSettings = GetComponent<NetworkSettings>();
                
                soundSystem = transform.Find("SoundSystem").gameObject.GetComponent<SoundSystem>().Initialize();

                engineInitialization = new EngineInitialization();
                spriteSystem = new SpriteSystem();
                paletteSystem = new PaletteSystem();
                animationSystem = new AnimationSystem();

                commandSystem = new CommandSystem();
                random = new UnityMugen.Random();
                stringConverter = new StringConverter();
                fileSystem = new FileSystem();
                evaluationSystem = new EvaluationSystem();
                stateSystem = new StateSystem();
                inputSystem = new InputSystem().Inicialize();

                profileLoader.SetIDs();
                profileLoader.PreLoadStates();
                profileLoader.PreLoadStatesCNS();
                profileLoader.PreLoadPalettes();
            }
        }

        public void StartCombatScreen()
        {
            GameObject CB = new GameObject();
            mugen = CB.AddComponent<Mugen>();
            mugen.name = "CombatScreen";
            mugen.transform.parent = LauncherEngine.Inst.gameObject.transform;
        }

        public void DestroyCombatScreen()
        {
            Destroy(mugen.gameObject);
        }

        private void OnApplicationQuit()
        {
            profileLoader.ClearThreads();
        }

        public GameObject Instance(GameObject go, Vector3 vector3, Quaternion quaternion)
        {
            return Instantiate(go, vector3, quaternion);
        }

        void FirstAccess()
        {
            PlayerPrefs.SetInt("isFirstAccess", 1);
            PlayerPrefs.SetInt("Definitions_SpeedTime", 2);// Normal
            PlayerPrefs.SetInt("Definitions_TimeRound", 1);
            PlayerPrefs.SetInt("Definitions_NumberOfRounds", 2);
            PlayerPrefs.SetInt("Definitions_AILevel", 4);

            //AUDIO
            PlayerPrefs.SetInt("MusicVolume", 8);
            PlayerPrefs.SetInt("SFXVolume", 8);
            PlayerPrefs.SetInt("VoiceVolume", 8);

            //VIDEO
            PlayerPrefs.SetInt("VSync", 0);
            PlayerPrefs.SetInt("isFullScreen", 1);
            PlayerPrefs.SetInt("isShakeCamera", 1);
        }

    }
}