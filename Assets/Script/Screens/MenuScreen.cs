using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityMugen.CustomInput;

namespace UnityMugen.Screens
{

    enum CURRENT_SCREEN { MAIN, OPTIONS, EXIT, VIDEO, AUDIO, DEFINITIONS, CONTROLS, PLAYER_SELECT, KEYBOARD_CONTROLLE, JOYSTICK_CONTROLLE }
    enum MAIN_SELECTED { /*STORY = 0,*/ ARCADE = 2, VERSUS = 3, TRAINER = 4, /*NETWORK = 5,*/ OPTIONS = 7, EXIT = 8, DISABLE = 9999 }
    enum OPTIONS_SELECTED { VIDEO, AUDIO, DEFINITIONS, CONTROLS }


    [Serializable]
    class MenuMain
    {
        public MAIN_SELECTED main_SELECTED;
        public GameObject mainSelect;
    }

    [Serializable]
    class MenuOptions
    {
        public OPTIONS_SELECTED options_SELECTED;
        public GameObject optionsSelect;
    }

    [Serializable]
    public class MenuNetwork
    {
        public NETWORK_SELECTED network_SELECTED;
        public GameObject networkSelect;
    }

    public class MenuScreen : MonoBehaviour
    {
        LauncherEngine Launcher => LauncherEngine.Inst;

        static float[] Volume = {
        -80f,
        -39.6f,
        -35.2f,
        -30.8f,
        -26.4f,
        -22f,
        -17.6f,
        -13.2f,
        -8.8f,
        -4.4f,
        0f
    };
        static string[] PercentVolume = {
        "0%",
        "10%",
        "20%",
        "30%",
        "40%",
        "50%",
        "60%",
        "70%",
        "80%",
        "90%",
        "100%"
    };

        CURRENT_SCREEN currentSCREEN;
        MAIN_SELECTED currentMainSelected;
        OPTIONS_SELECTED currentOptionsSelected;


        [SerializeField] Color mainColorEnable;
        [SerializeField] Color mainColorDisable;

        [SerializeField] AudioClip music;

        private Color customColorBlue = new Color(0, 0.5946891f, 0.6981132f, 1);
        private Color customColorYellow = Color.yellow;

        [SerializeField]
        MenuMain[] menuMains;

        [SerializeField]
        MenuOptions[] menuOptions;


        [SerializeField] GameObject panelOptions;
        [SerializeField] GameObject panelDefinitions;
        [SerializeField] GameObject panelExit;
        [SerializeField] GameObject panelPlayerSelect;


        public GameObject panelVideo;
        public GameObject panelAudio;
        //public GameObject dataUser;
        //public GameObject login;
        //public GameObject logout;
        //public GameObject register;

        [Header("Panel Control")]
        [SerializeField] GameObject panelControls;
        [SerializeField] GameObject panelControlKey;
        [SerializeField] GameObject panelControlJoy;
        [SerializeField] GameObject contentP1;
        [SerializeField] GameObject contentP2;


        private bool activeControl = true;
        private InitializationSettings initialization;



        [Header("Network")]
        [SerializeField] NETWORK_SELECTED currentNetworkSelected;



        [Header("Video Settings")]
        public StyleVideo[] styleVideo;

        int positionResolution;

        float[] brightnesses = new float[] { 0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f };
        int currentBrightness = 10;

        Resolution[] resolutions;

        public OptionVideo currentOptionVideo;

        [Serializable]
        public class StyleVideo
        {
            public OptionVideo optionVideo;
            public Image configVideo;
            public Text value;

            public Image[] valuesBrigh;
        }





        [Header("Definitions Settings")]
        public StyleDefinitions[] styleDefinitions;
        public OptionDefinition currentOptionDefinition;
        int currentTimeRound = 10;

        [Serializable]
        public class StyleDefinitions
        {
            public OptionDefinition optionDefinition;
            public Image configDefinitions;
            public Text value;
        }





        [Header("Audio Settings")]
        public OptionAudio currentOptionAudio;
        public AudioMixer audioMixer;
        public AudioClip[] voicesTest;
        public StyleAudio[] styleAudio;

        //public MenuControls[] menuControls;

        [Serializable]
        public class StyleAudio
        {
            public OptionAudio optionAudio;
            public Image configAudio;
            public Text textPercentVolume;
        }




        [Header("Controls Settings")]
        public OptionControl currentOptionControls;
        public MenuControls[] menuControls;

        [Serializable]
        public class MenuControls
        {
            public OptionControl options_SELECTED;
            public GameObject controlsSelect;
        }



        [Header("Control 1")]
        public Animator animatorC1;
        public Image ArrowLeftC1;
        public Image ArrowRightC1;
        public PositionPlayerSelect positionPlayerSelectC1;
        public TypeController typeControllerC1;
        public Image joystickC1;
        public Image keyboardC1;

        [Header("Control 2")]
        public Animator animatorC2;
        public Image ArrowLeftC2;
        public Image ArrowRightC2;
        public PositionPlayerSelect positionPlayerSelectC2;
        public TypeController typeControllerC2;
        public Image joystickC2;
        public Image keyboardC2;




        [Header("Automatic Start Battle")]
        public bool automaticStartBattle;
        public CombatMode combatMode;
        public PlayerProfileManager p1, p2;
        public StageProfile stageProfile;




        void AutomaticStartBattle()
        {
            if (automaticStartBattle && (p1 == null || p2 == null))
            {
                Debug.LogWarning("when the automaticStartBattle field is active, add P1 and P2 characters.");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                return;
            }
            Launcher.engineInitialization.Mode = combatMode;
            Launcher.StartCombatScreen();

            PlayerCreation createP1 = new PlayerCreation(p1, 1, PlayerMode.Human);
            PlayerCreation createP2 = new PlayerCreation(p2, 2, PlayerMode.Human);
            Launcher.engineInitialization.Team1.Clear();
            Launcher.engineInitialization.Team2.Clear();
            Launcher.engineInitialization.Team1Mode = TeamMode.Single;
            Launcher.engineInitialization.Team2Mode = TeamMode.Single;
            Launcher.engineInitialization.Team1.Add(createP1);
            Launcher.engineInitialization.Team2.Add(createP2);
            Launcher.engineInitialization.SetSeed();
            Launcher.engineInitialization.stageID = stageProfile.stageID;
            Launcher.engineInitialization.musicID = stageProfile.musicID;

            AsyncOperation ao = SceneManager.LoadSceneAsync(stageProfile.Name);
            SceneManager.LoadScene("HudCanvasBattle", LoadSceneMode.Additive);
            SceneManager.LoadScene("HudPauseFight", LoadSceneMode.Additive);
            SceneManager.LoadScene("HudCanvasMoveLists", LoadSceneMode.Additive);
        }






        public void Start()
        {
            Launcher.screenType = ScreenType.Menu;

            currentSCREEN = CURRENT_SCREEN.MAIN;
            currentMainSelected = MAIN_SELECTED.ARCADE; //MAIN_SELECTED.STORY;
            currentOptionsSelected = OPTIONS_SELECTED.VIDEO;
            currentNetworkSelected = NETWORK_SELECTED.SERVER;

            resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray<Resolution>();

            InicialValuesVideo();
            InicialValuesVolume();

            initialization = Launcher.initializationSettings;

            if (music != null)
                Launcher.soundSystem.PlayMusic(music, true);


            if (automaticStartBattle)
                AutomaticStartBattle();
        }

        void Update()
        {
            DoUpdate();
        }

        public void DoUpdate()
        {
            if (activeControl)
            {
                switch (currentSCREEN)
                {
                    case CURRENT_SCREEN.MAIN:
                        MenuMain(); break;
                    case CURRENT_SCREEN.OPTIONS:
                        MenuOption(); break;
                    case CURRENT_SCREEN.VIDEO:
                        Video(); break;
                    case CURRENT_SCREEN.AUDIO:
                        Audios(); break;
                    case CURRENT_SCREEN.DEFINITIONS:
                        Definition(); break;
                    case CURRENT_SCREEN.CONTROLS:
                        Controls(); break;
                    case CURRENT_SCREEN.EXIT:
                        Exit(); break;
                    case CURRENT_SCREEN.PLAYER_SELECT:
                        PlayerSelect(); break;
                    case CURRENT_SCREEN.KEYBOARD_CONTROLLE:
                        if (!contentP1.GetComponent<ConfigurationControl>().enable &&
                            !contentP2.GetComponent<ConfigurationControl>().enable)
                        {
                            currentSCREEN = CURRENT_SCREEN.CONTROLS;
                            panelControls.SetActive(true);
                            panelControlKey.SetActive(false);
                        }

                        if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift))
                        {
                            contentP1.GetComponent<ConfigurationControl>().SetParameters(1, 0);
                        }

                        if (UnityEngine.Input.GetKeyDown(KeyCode.RightShift))
                        {
                            contentP2.GetComponent<ConfigurationControl>().SetParameters(2, 0);
                        }
                        break;
                        //case CURRENT_SCREEN.LOGIN:
                        //    NetworkSelect();
                        //    break;
                }
            }

        }

        private void MenuMain()
        {
            for (int i = 0; i < menuMains.Length; i++)
            {
                GameObject g = menuMains[i].mainSelect;
                Image image = g.GetComponent<Image>();
                if (currentMainSelected == menuMains[i].main_SELECTED)
                {
                    image.color = mainColorEnable;
                }
                else
                {
                    image.color = mainColorDisable;
                }
            }

            if (InputCustom.PressUpPlayerIDOne() || InputCustom.PressDownPlayerIDOne())
            {
                if (currentMainSelected != MAIN_SELECTED.DISABLE)
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
            }

            //if (currentMainSelected == MAIN_SELECTED.STORY && InputCustom.PressDownPlayerIDOne())
            //{
            //    currentMainSelected = MAIN_SELECTED.ARCADE;
            //}
            //else if (currentMainSelected == MAIN_SELECTED.STORY && InputCustom.PressUpPlayerIDOne())
            //{
            //    currentMainSelected = MAIN_SELECTED.EXIT;
            //}
            //else 
            if (currentMainSelected == MAIN_SELECTED.ARCADE && InputCustom.PressDownPlayerIDOne())
            {
                currentMainSelected = MAIN_SELECTED.VERSUS;
            }
            else if (currentMainSelected == MAIN_SELECTED.ARCADE && InputCustom.PressUpPlayerIDOne())
            {
                currentMainSelected = MAIN_SELECTED.ARCADE;//MAIN_SELECTED.STORY;
            }
            else if (currentMainSelected == MAIN_SELECTED.VERSUS && InputCustom.PressDownPlayerIDOne())
            {
                currentMainSelected = MAIN_SELECTED.TRAINER;
            }
            else if (currentMainSelected == MAIN_SELECTED.VERSUS && InputCustom.PressUpPlayerIDOne())
            {
                currentMainSelected = MAIN_SELECTED.ARCADE;
            }
            //else if (currentMainSelected == MAIN_SELECTED.TEAM_ARCADE && InputCustom.PressDownPlayerIDOne())
            //{
            //    currentMainSelected = MAIN_SELECTED.TRAINER;
            //}
            //else if (currentMainSelected == MAIN_SELECTED.TEAM_ARCADE && InputCustom.PressUpPlayerIDOne())
            //{
            //    currentMainSelected = MAIN_SELECTED.VERSUS;
            //}
            else if (currentMainSelected == MAIN_SELECTED.TRAINER && InputCustom.PressDownPlayerIDOne())
            {
                currentMainSelected = MAIN_SELECTED.OPTIONS;//MAIN_SELECTED.NETWORK;
            }
            else if (currentMainSelected == MAIN_SELECTED.TRAINER && InputCustom.PressUpPlayerIDOne())
            {
                currentMainSelected = MAIN_SELECTED.VERSUS;
            }
            //else if (currentMainSelected == MAIN_SELECTED.NETWORK && InputCustom.PressDownPlayerIDOne())
            //{
            //    currentMainSelected = MAIN_SELECTED.OPTIONS;
            //}
            //else if (currentMainSelected == MAIN_SELECTED.NETWORK && InputCustom.PressUpPlayerIDOne())
            //{
            //    currentMainSelected = MAIN_SELECTED.TRAINER;
            //}
            else if (currentMainSelected == MAIN_SELECTED.OPTIONS && InputCustom.PressDownPlayerIDOne())
            {
                currentMainSelected = MAIN_SELECTED.EXIT;
            }
            else if (currentMainSelected == MAIN_SELECTED.OPTIONS && InputCustom.PressUpPlayerIDOne())
            {
                currentMainSelected = MAIN_SELECTED.TRAINER;//MAIN_SELECTED.NETWORK;
            }
            else if (currentMainSelected == MAIN_SELECTED.EXIT && InputCustom.PressDownPlayerIDOne())
            {
                currentMainSelected = MAIN_SELECTED.ARCADE;//MAIN_SELECTED.STORY;
            }
            else if (currentMainSelected == MAIN_SELECTED.EXIT && InputCustom.PressUpPlayerIDOne())
            {
                currentMainSelected = MAIN_SELECTED.OPTIONS;
            }

            if (InputManager.GetButtonDown("X"))
            {

                if (currentMainSelected != MAIN_SELECTED.DISABLE)
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);

                //if (currentMainSelected == MAIN_SELECTED.STORY)
                //{
                //    Launcher.engineInitialization.Mode = CombatMode.Story;
                //    currentMainSelected = MAIN_SELECTED.DISABLE;
                //    LoadSceneCustom.LoadScene("SelectScreen");
                //}
                //else 
                if (currentMainSelected == MAIN_SELECTED.ARCADE)
                {
                    Launcher.engineInitialization.Mode = CombatMode.Arcade;
                    Launcher.trainnerSettings.typeDrawCollider = TypeDrawCollider.None;
                    Launcher.StartCombatScreen();
                    currentMainSelected = MAIN_SELECTED.DISABLE;
                    StartCoroutine(Launcher.soundSystem.FadeOutMusic(Constant.TimeFadeOutMusic));
                    LoadSceneCustom.LoadScene("SelectScreen");
                }
                else if (currentMainSelected == MAIN_SELECTED.VERSUS)
                {
                    Launcher.engineInitialization.Mode = CombatMode.Versus;
                    Launcher.trainnerSettings.typeDrawCollider = TypeDrawCollider.None;
                    Launcher.StartCombatScreen();
                    currentMainSelected = MAIN_SELECTED.DISABLE;
                    StartCoroutine(Launcher.soundSystem.FadeOutMusic(Constant.TimeFadeOutMusic));
                    LoadSceneCustom.LoadScene("SelectScreen");
                }
                //else if (currentMainSelected == MAIN_SELECTED.TEAM_ARCADE)
                //{
                //    launcherEngine.engineInitialization.Mode = CombatMode.TeamArcade;
                //    currentMainSelected = MAIN_SELECTED.DISABLE;
                //    LoadScene("SelectScreen");
                //}
                else if (currentMainSelected == MAIN_SELECTED.TRAINER)
                {
                    Launcher.engineInitialization.Mode = CombatMode.Training;
                    Launcher.StartCombatScreen();
                    currentMainSelected = MAIN_SELECTED.DISABLE;
                    StartCoroutine(Launcher.soundSystem.FadeOutMusic(Constant.TimeFadeOutMusic));
                    LoadSceneCustom.LoadScene("SelectScreen");
                }
                //else if (currentMainSelected == MAIN_SELECTED.NETWORK)
                //{
                //if (Launcher.dadosConnection.isUsuarioLogadoServer)
                //{
                //    Launcher.engineInitialization.Mode = CombatMode.Network;
                //    currentMainSelected = MAIN_SELECTED.DISABLE;
                //    LoadSceneCustom.LoadScene("RoomScreen");
                //}
                //else
                //{
                //    Launcher.showMessage.Message(TipePopupMessage.Warning, "Faça Login para jogar partidas Online.");
                //}
                //}
                else if (currentMainSelected == MAIN_SELECTED.OPTIONS)
                {
                    currentSCREEN = CURRENT_SCREEN.OPTIONS;
                    panelOptions.SetActive(true);
                }
                else if (currentMainSelected == MAIN_SELECTED.EXIT)
                {
                    currentSCREEN = CURRENT_SCREEN.EXIT;
                    panelExit.SetActive(true);
                }
            }
            else if (InputManager.GetButtonDown("Y"))
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.CANCEL);
                currentSCREEN = CURRENT_SCREEN.EXIT;
                panelExit.SetActive(true);
            }

        }

        private void MenuOption()
        {
            for (int i = 0; i < menuOptions.Length; i++)
            {
                GameObject g = menuOptions[i].optionsSelect;
                Image image = g.GetComponent<Image>();
                if (currentOptionsSelected == menuOptions[i].options_SELECTED)
                {
                    image.color = mainColorEnable;
                }
                else
                {
                    image.color = mainColorDisable;
                }
            }

            if (InputCustom.PressUpPlayerIDOne() || InputCustom.PressDownPlayerIDOne())
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
            }

            if (currentOptionsSelected == OPTIONS_SELECTED.VIDEO && InputCustom.PressDownPlayerIDOne())
            {
                currentOptionsSelected = OPTIONS_SELECTED.AUDIO;
            }
            else if (currentOptionsSelected == OPTIONS_SELECTED.VIDEO && InputCustom.PressUpPlayerIDOne())
            {
                currentOptionsSelected = OPTIONS_SELECTED.CONTROLS;
            }
            else if (currentOptionsSelected == OPTIONS_SELECTED.AUDIO && InputCustom.PressDownPlayerIDOne())
            {
                currentOptionsSelected = OPTIONS_SELECTED.DEFINITIONS;
            }
            else if (currentOptionsSelected == OPTIONS_SELECTED.AUDIO && InputCustom.PressUpPlayerIDOne())
            {
                currentOptionsSelected = OPTIONS_SELECTED.VIDEO;
            }
            else if (currentOptionsSelected == OPTIONS_SELECTED.DEFINITIONS && InputCustom.PressDownPlayerIDOne())
            {
                currentOptionsSelected = OPTIONS_SELECTED.CONTROLS;
            }
            else if (currentOptionsSelected == OPTIONS_SELECTED.DEFINITIONS && InputCustom.PressUpPlayerIDOne())
            {
                currentOptionsSelected = OPTIONS_SELECTED.AUDIO;
            }
            else if (currentOptionsSelected == OPTIONS_SELECTED.CONTROLS && InputCustom.PressDownPlayerIDOne())
            {
                currentOptionsSelected = OPTIONS_SELECTED.VIDEO;
            }
            else if (currentOptionsSelected == OPTIONS_SELECTED.CONTROLS && InputCustom.PressUpPlayerIDOne())
            {
                currentOptionsSelected = OPTIONS_SELECTED.DEFINITIONS;
            }

            if (InputManager.GetButtonDown("X"))
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);

                if (currentOptionsSelected == OPTIONS_SELECTED.VIDEO)
                {
                    currentSCREEN = CURRENT_SCREEN.VIDEO;
                    currentOptionVideo = OptionVideo.RESOLUTION;
                    panelVideo.SetActive(true);
                    panelOptions.SetActive(false);

                    Launcher.initializationSettings.isFullScreen = PlayerPrefs.GetInt("isFullScreen") == 1 ? true : false;
                    int width = PlayerPrefs.GetInt("width");
                    int height = PlayerPrefs.GetInt("height");
                    QualitySettings.vSyncCount = PlayerPrefs.GetInt("vSyncCount");

                    currentBrightness = PlayerPrefs.GetInt("brightness");
                    Screen.brightness = brightnesses[currentBrightness];

                    if (width == 0 || height == 0)
                    {
                        width = Screen.currentResolution.width;
                        height = Screen.currentResolution.height;
                    }

                    for (int i = 0; i < resolutions.Length; i++)
                    {
                        if (resolutions[i].width == width && resolutions[i].height == height)
                            positionResolution = i;
                    }

                    string fmt = string.Format("{0}x{1} "/*{2}Hz*/, resolutions[positionResolution].width, resolutions[positionResolution].height/*, resolutions[positionResolution].refreshRate*/);
                    styleVideo[(int)OptionVideo.RESOLUTION].value.text = fmt;

                    styleVideo[(int)OptionVideo.DISPLAY].value.text = (Launcher.initializationSettings.isFullScreen ? "FullScreen" : "Windowed");

                    styleVideo[(int)OptionVideo.VSYNC].value.text = (QualitySettings.vSyncCount == 1 ? "ENABLE" : "DISABLE");

                    for (int i = 0; i < styleVideo[(int)OptionVideo.BRIGHTINESS].valuesBrigh.Length; i++)
                    {
                        Image image = styleVideo[(int)OptionVideo.BRIGHTINESS].valuesBrigh[i];
                        if (i < currentBrightness)
                            image.fillCenter = true;
                        else
                            image.fillCenter = false;
                    }
                }
                else if (currentOptionsSelected == OPTIONS_SELECTED.AUDIO)
                {
                    currentSCREEN = CURRENT_SCREEN.AUDIO;
                    currentOptionAudio = OptionAudio.MUSIC;
                    panelAudio.SetActive(true);
                    panelOptions.SetActive(false);
                }
                else if (currentOptionsSelected == OPTIONS_SELECTED.DEFINITIONS)
                {
                    currentSCREEN = CURRENT_SCREEN.DEFINITIONS;
                    currentOptionDefinition = OptionDefinition.VOICE_LANGUAGE;
                    panelDefinitions.SetActive(true);
                    panelOptions.SetActive(false);

                    Launcher.initializationSettings.SpeedTime = (SpeedTime)PlayerPrefs.GetInt("Definitions_SpeedTime");
                    styleDefinitions[(int)OptionDefinition.SPEED_TIME].value.text = Launcher.initializationSettings.SpeedTime.ToString();

                    currentTimeRound = PlayerPrefs.GetInt("Definitions_TimeRound");
                    Launcher.initializationSettings.RoundLength = Constant.roundLengths[currentTimeRound];
                    styleDefinitions[(int)OptionDefinition.TIME_ROUND].value.text = Constant.roundLengths[currentTimeRound].ToString();

                    Launcher.initializationSettings.NumberOfRounds = PlayerPrefs.GetInt("Definitions_NumberOfRounds");
                    styleDefinitions[(int)OptionDefinition.NUMBER_OF_ROUNDS].value.text = Launcher.initializationSettings.NumberOfRounds.ToString();

                    Launcher.initializationSettings.AiLevel = PlayerPrefs.GetInt("Definitions_AILevel");
                    styleDefinitions[(int)OptionDefinition.AI_LEVEL].value.text = Launcher.initializationSettings.AiLevel.ToString();
                }
                else if (currentOptionsSelected == OPTIONS_SELECTED.CONTROLS)
                {
                    currentSCREEN = CURRENT_SCREEN.CONTROLS;
                    currentOptionControls = OptionControl.CHOOSE_CONTROL;
                    panelControls.SetActive(true);
                    panelOptions.SetActive(false);
                }
            }
            else if (InputManager.GetButtonDown("Y"))
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.CANCEL);
                currentSCREEN = CURRENT_SCREEN.MAIN;
                currentOptionsSelected = OPTIONS_SELECTED.VIDEO;
                panelOptions.SetActive(false);
            }


        }












        private void Video()
        {
            for (int i = 0; i < styleVideo.Length; i++)
            {

                if (styleVideo[i].optionVideo == currentOptionVideo)
                {
                    styleVideo[i].configVideo.enabled = true;
                }
                else
                {
                    styleVideo[i].configVideo.enabled = false;
                }
            }

            if (InputCustom.PressUpPlayerIDOne() || InputCustom.PressDownPlayerIDOne())
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
            }

            if (currentOptionVideo == OptionVideo.RESOLUTION && InputCustom.PressDownPlayerIDOne())
            {
                currentOptionVideo = OptionVideo.DISPLAY;
            }
            else if (currentOptionVideo == OptionVideo.RESOLUTION && InputCustom.PressUpPlayerIDOne())
            {
                currentOptionVideo = OptionVideo.SHAKE_CAMERA;
            }
            else if (currentOptionVideo == OptionVideo.DISPLAY && InputCustom.PressDownPlayerIDOne())
            {
                currentOptionVideo = OptionVideo.VSYNC;
            }
            else if (currentOptionVideo == OptionVideo.DISPLAY && InputCustom.PressUpPlayerIDOne())
            {
                currentOptionVideo = OptionVideo.RESOLUTION;
            }
            else if (currentOptionVideo == OptionVideo.VSYNC && InputCustom.PressDownPlayerIDOne())
            {
                currentOptionVideo = OptionVideo.BRIGHTINESS;
            }
            else if (currentOptionVideo == OptionVideo.VSYNC && InputCustom.PressUpPlayerIDOne())
            {
                currentOptionVideo = OptionVideo.DISPLAY;
            }
            else if (currentOptionVideo == OptionVideo.BRIGHTINESS && InputCustom.PressDownPlayerIDOne())
            {
                currentOptionVideo = OptionVideo.ANTI_ALIAS;
            }
            else if (currentOptionVideo == OptionVideo.BRIGHTINESS && InputCustom.PressUpPlayerIDOne())
            {
                currentOptionVideo = OptionVideo.VSYNC;
            }
            else if (currentOptionVideo == OptionVideo.ANTI_ALIAS && InputCustom.PressDownPlayerIDOne())
            {
                currentOptionVideo = OptionVideo.SHAKE_CAMERA;
            }
            else if (currentOptionVideo == OptionVideo.ANTI_ALIAS && InputCustom.PressUpPlayerIDOne())
            {
                currentOptionVideo = OptionVideo.BRIGHTINESS;
            }
            else if (currentOptionVideo == OptionVideo.SHAKE_CAMERA && InputCustom.PressDownPlayerIDOne())
            {
                currentOptionVideo = OptionVideo.RESOLUTION;
            }
            else if (currentOptionVideo == OptionVideo.SHAKE_CAMERA && InputCustom.PressUpPlayerIDOne())
            {
                currentOptionVideo = OptionVideo.ANTI_ALIAS;
            }

            if (currentOptionVideo == OptionVideo.RESOLUTION)
            {
                if (InputCustom.PressLeftPlayerIDOne())
                {
                    if (positionResolution > 0)
                    {
                        Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
                        positionResolution--;
                    }
                }
                else if (InputCustom.PressRightPlayerIDOne())
                {
                    if (positionResolution < resolutions.Length - 1)
                    {
                        Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
                        positionResolution++;
                    }
                }
                if (InputCustom.PressLeftPlayerIDOne() || InputCustom.PressRightPlayerIDOne())
                {
                    string fmt = string.Format("{0}x{1} ", resolutions[positionResolution].width, resolutions[positionResolution].height);
                    styleVideo[(int)currentOptionVideo].value.text = fmt;

                    PlayerPrefs.SetInt("width", resolutions[positionResolution].width);
                    PlayerPrefs.SetInt("height", resolutions[positionResolution].height);

                    Screen.SetResolution(resolutions[positionResolution].width, resolutions[positionResolution].height, Launcher.initializationSettings.isFullScreen);
                }
            }
            else if (currentOptionVideo == OptionVideo.DISPLAY)
            {
                if (InputCustom.PressLeftPlayerIDOne() || InputCustom.PressRightPlayerIDOne())
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);

                    if (Launcher.initializationSettings.isFullScreen == false)
                        Launcher.initializationSettings.isFullScreen = true;
                    else
                        Launcher.initializationSettings.isFullScreen = false;

                    styleVideo[(int)currentOptionVideo].value.text =
                        (Launcher.initializationSettings.isFullScreen ? "FullScreen" : "Windowed");

                    PlayerPrefs.SetInt("isFullScreen", Launcher.initializationSettings.isFullScreen ? 1 : 0);
                    if (resolutions[positionResolution].width == 0 || resolutions[positionResolution].height == 0)
                        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, Launcher.initializationSettings.isFullScreen);
                    else
                        Screen.SetResolution(resolutions[positionResolution].width, resolutions[positionResolution].height, Launcher.initializationSettings.isFullScreen);
                }
            }
            else if (currentOptionVideo == OptionVideo.VSYNC)
            {
                if (InputCustom.PressLeftPlayerIDOne() || InputCustom.PressRightPlayerIDOne())
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);

                    if (QualitySettings.vSyncCount == 1)
                        QualitySettings.vSyncCount = 0;
                    else if (QualitySettings.vSyncCount == 0)
                        QualitySettings.vSyncCount = 1;

                    PlayerPrefs.SetInt("vSyncCount", QualitySettings.vSyncCount);
                    styleVideo[(int)currentOptionVideo].value.text = (QualitySettings.vSyncCount == 1 ? "ON" : "OFF");
                }
            }
            else if (currentOptionVideo == OptionVideo.BRIGHTINESS)
            {
                if (InputCustom.PressLeftPlayerIDOne())
                {
                    if (currentBrightness > 0)
                        currentBrightness--;
                }
                else if (InputCustom.PressRightPlayerIDOne())
                {
                    if (currentBrightness < brightnesses.Length - 1)
                        currentBrightness++;
                }

                if (InputCustom.PressLeftPlayerIDOne() || InputCustom.PressRightPlayerIDOne())
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);

                    Screen.brightness = brightnesses[currentBrightness];
                    PlayerPrefs.SetInt("brightness", currentBrightness);

                    foreach (Image brigh in styleVideo[(int)currentOptionVideo].valuesBrigh)
                    {
                        brigh.fillCenter = false;
                    }
                    for (int i = 0; i < currentBrightness; i++)
                    {
                        styleVideo[(int)currentOptionVideo].valuesBrigh[i].fillCenter = true;
                    }
                }
            }
            else if (currentOptionVideo == OptionVideo.SHAKE_CAMERA)
            {
                if (InputCustom.PressLeftPlayerIDOne() || InputCustom.PressRightPlayerIDOne())
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);

                    if (Launcher.initializationSettings.isShakeCamera == false)
                        Launcher.initializationSettings.isShakeCamera = true;
                    else
                        Launcher.initializationSettings.isShakeCamera = false;

                    styleVideo[(int)currentOptionVideo].value.text = (Launcher.initializationSettings.isShakeCamera ? "ENABLE" : "DISABLE");
                    PlayerPrefs.SetInt("isShakeCamera", Launcher.initializationSettings.isShakeCamera ? 1 : 0);
                }
            }

            if (InputManager.GetButtonDown("Y"))
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.CANCEL);
                currentSCREEN = CURRENT_SCREEN.OPTIONS;
                panelVideo.SetActive(false);
                panelOptions.SetActive(true);
            }

        }




        private void Definition()
        {
            for (int i = 0; i < styleDefinitions.Length; i++)
            {
                if (styleDefinitions[i].optionDefinition == currentOptionDefinition)
                {
                    styleDefinitions[i].configDefinitions.enabled = true;
                }
                else
                {
                    styleDefinitions[i].configDefinitions.enabled = false;
                }
            }

            if (InputCustom.PressUpPlayerIDOne() || InputCustom.PressDownPlayerIDOne())
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
            }

            if (currentOptionDefinition == OptionDefinition.VOICE_LANGUAGE && InputCustom.PressDownPlayerIDOne())
            {
                currentOptionDefinition = OptionDefinition.TEXT_LANGUAGE;
            }
            else if (currentOptionDefinition == OptionDefinition.VOICE_LANGUAGE && InputCustom.PressUpPlayerIDOne())
            {
                currentOptionDefinition = OptionDefinition.AI_LEVEL;
            }
            else if (currentOptionDefinition == OptionDefinition.TEXT_LANGUAGE && InputCustom.PressDownPlayerIDOne())
            {
                currentOptionDefinition = OptionDefinition.SPEED_TIME;
            }
            else if (currentOptionDefinition == OptionDefinition.TEXT_LANGUAGE && InputCustom.PressUpPlayerIDOne())
            {
                currentOptionDefinition = OptionDefinition.VOICE_LANGUAGE;
            }
            else if (currentOptionDefinition == OptionDefinition.SPEED_TIME && InputCustom.PressDownPlayerIDOne())
            {
                currentOptionDefinition = OptionDefinition.TIME_ROUND;
            }
            else if (currentOptionDefinition == OptionDefinition.SPEED_TIME && InputCustom.PressUpPlayerIDOne())
            {
                currentOptionDefinition = OptionDefinition.TEXT_LANGUAGE;
            }
            else if (currentOptionDefinition == OptionDefinition.TIME_ROUND && InputCustom.PressDownPlayerIDOne())
            {
                currentOptionDefinition = OptionDefinition.NUMBER_OF_ROUNDS;
            }
            else if (currentOptionDefinition == OptionDefinition.TIME_ROUND && InputCustom.PressUpPlayerIDOne())
            {
                currentOptionDefinition = OptionDefinition.SPEED_TIME;
            }
            else if (currentOptionDefinition == OptionDefinition.NUMBER_OF_ROUNDS && InputCustom.PressDownPlayerIDOne())
            {
                currentOptionDefinition = OptionDefinition.AI_LEVEL;
            }
            else if (currentOptionDefinition == OptionDefinition.NUMBER_OF_ROUNDS && InputCustom.PressUpPlayerIDOne())
            {
                currentOptionDefinition = OptionDefinition.TIME_ROUND;
            }
            else if (currentOptionDefinition == OptionDefinition.AI_LEVEL && InputCustom.PressDownPlayerIDOne())
            {
                currentOptionDefinition = OptionDefinition.VOICE_LANGUAGE;
            }
            else if (currentOptionDefinition == OptionDefinition.AI_LEVEL && InputCustom.PressUpPlayerIDOne())
            {
                currentOptionDefinition = OptionDefinition.NUMBER_OF_ROUNDS;
            }

            if (currentOptionDefinition == OptionDefinition.VOICE_LANGUAGE)
            {

            }
            else if (currentOptionDefinition == OptionDefinition.TEXT_LANGUAGE)
            {

            }
            else if (currentOptionDefinition == OptionDefinition.SPEED_TIME)
            {
                if (InputCustom.PressLeftPlayerIDOne())
                {
                    if (Launcher.initializationSettings.SpeedTime > 0)
                        Launcher.initializationSettings.SpeedTime--;
                }
                else if (InputCustom.PressRightPlayerIDOne())
                {
                    if ((int)Launcher.initializationSettings.SpeedTime < Enum.GetNames(typeof(SpeedTime)).Length - 1)
                        Launcher.initializationSettings.SpeedTime++;
                }

                if (InputCustom.PressLeftPlayerIDOne() || InputCustom.PressRightPlayerIDOne())
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);

                    PlayerPrefs.SetInt("Definitions_SpeedTime", (int)Launcher.initializationSettings.SpeedTime);
                    styleDefinitions[(int)currentOptionDefinition].value.text = Launcher.initializationSettings.SpeedTime.ToString();
                }

            }
            else if (currentOptionDefinition == OptionDefinition.TIME_ROUND)
            {
                if (InputCustom.PressLeftPlayerIDOne())
                {
                    if (currentTimeRound > 0)
                        currentTimeRound--;
                }
                else if (InputCustom.PressRightPlayerIDOne())
                {
                    if (currentTimeRound < Constant.roundLengths.Length - 1)
                        currentTimeRound++;
                }

                if (InputCustom.PressLeftPlayerIDOne() || InputCustom.PressRightPlayerIDOne())
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);

                    Launcher.initializationSettings.RoundLength = Constant.roundLengths[currentTimeRound];
                    PlayerPrefs.SetInt("Definitions_TimeRound", currentTimeRound);
                    styleDefinitions[(int)currentOptionDefinition].value.text = Constant.roundLengths[currentTimeRound].ToString();
                }
            }
            else if (currentOptionDefinition == OptionDefinition.NUMBER_OF_ROUNDS)
            {
                if (InputCustom.PressLeftPlayerIDOne())
                {
                    if (Launcher.initializationSettings.NumberOfRounds > 1)
                        Launcher.initializationSettings.NumberOfRounds--;
                }
                else if (InputCustom.PressRightPlayerIDOne())
                {
                    if (Launcher.initializationSettings.NumberOfRounds < 5)
                        Launcher.initializationSettings.NumberOfRounds++;
                }

                if (InputCustom.PressLeftPlayerIDOne() || InputCustom.PressRightPlayerIDOne())
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
                    PlayerPrefs.SetInt("Definitions_NumberOfRounds", Launcher.initializationSettings.NumberOfRounds);
                    styleDefinitions[(int)currentOptionDefinition].value.text = Launcher.initializationSettings.NumberOfRounds.ToString();
                }
            }
            else if (currentOptionDefinition == OptionDefinition.AI_LEVEL)
            {
                if (InputCustom.PressLeftPlayerIDOne())
                {
                    if (Launcher.initializationSettings.AiLevel > 1)
                        Launcher.initializationSettings.AiLevel--;
                }
                else if (InputCustom.PressRightPlayerIDOne())
                {
                    if (Launcher.initializationSettings.AiLevel < 8)
                        Launcher.initializationSettings.AiLevel++;
                }

                if (InputCustom.PressLeftPlayerIDOne() || InputCustom.PressRightPlayerIDOne())
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
                    PlayerPrefs.SetInt("Definitions_AILevel", Launcher.initializationSettings.AiLevel);
                    styleDefinitions[(int)currentOptionDefinition].value.text = Launcher.initializationSettings.AiLevel.ToString();
                }
            }

            if (InputManager.GetButtonDown("Y"))
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.CANCEL);
                currentSCREEN = CURRENT_SCREEN.OPTIONS;
                panelDefinitions.SetActive(false);
                panelOptions.SetActive(true);
            }

        }



        private void Audios()
        {
            for (int i = 0; i < styleAudio.Length; i++)
            {
                if (styleAudio[i].optionAudio == currentOptionAudio)
                {
                    styleAudio[i].configAudio.enabled = true;
                }
                else
                {
                    styleAudio[i].configAudio.enabled = false;
                }
            }

            if (InputCustom.PressUpPlayerIDOne() || InputCustom.PressDownPlayerIDOne())
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
            }

            if (currentOptionAudio == OptionAudio.MUSIC && InputCustom.PressDownPlayerIDOne())
            {
                currentOptionAudio = OptionAudio.SFX;
            }
            else if (currentOptionAudio == OptionAudio.MUSIC && InputCustom.PressUpPlayerIDOne())
            {
                currentOptionAudio = OptionAudio.VOICE;
            }
            else if (currentOptionAudio == OptionAudio.SFX && InputCustom.PressDownPlayerIDOne())
            {
                currentOptionAudio = OptionAudio.VOICE;
            }
            else if (currentOptionAudio == OptionAudio.SFX && InputCustom.PressUpPlayerIDOne())
            {
                currentOptionAudio = OptionAudio.MUSIC;
            }
            else if (currentOptionAudio == OptionAudio.VOICE && InputCustom.PressDownPlayerIDOne())
            {
                currentOptionAudio = OptionAudio.MUSIC;
            }
            else if (currentOptionAudio == OptionAudio.VOICE && InputCustom.PressUpPlayerIDOne())
            {
                currentOptionAudio = OptionAudio.SFX;
            }
            else if (InputCustom.PressLeftPlayerIDOne())
            {
                SetMinus();
            }
            else if (InputCustom.PressRightPlayerIDOne())
            {
                SetMax();
            }

            if (InputManager.GetButtonDown("Y"))
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.CANCEL);
                currentSCREEN = CURRENT_SCREEN.OPTIONS;
                panelAudio.SetActive(false);
                panelOptions.SetActive(true);
            }
        }

        private void SetMinus()
        {
            if (currentOptionAudio == OptionAudio.MUSIC)
            {
                int vol = PlayerPrefs.GetInt("MusicVolume");
                if (vol > 0)
                {
                    PlayerPrefs.SetInt("MusicVolume", vol - 1);
                    audioMixer.SetFloat("MusicVolume", Volume[vol - 1]);
                    styleAudio[0].textPercentVolume.text = PercentVolume[vol - 1];
                }
            }
            else if (currentOptionAudio == OptionAudio.SFX)
            {
                int vol = PlayerPrefs.GetInt("SFXVolume");
                if (vol > 0)
                {
                    PlayerPrefs.SetInt("SFXVolume", vol - 1);
                    audioMixer.SetFloat("SFXVolume", Volume[vol - 1]);
                    styleAudio[1].textPercentVolume.text = PercentVolume[vol - 1];

                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
                }
            }
            else if (currentOptionAudio == OptionAudio.VOICE)
            {
                int vol = PlayerPrefs.GetInt("VoiceVolume");
                if (vol > 0)
                {
                    PlayerPrefs.SetInt("VoiceVolume", vol - 1);
                    audioMixer.SetFloat("VoiceVolume", Volume[vol - 1]);
                    styleAudio[2].textPercentVolume.text = PercentVolume[vol - 1];

                    int voiceRandom = UnityEngine.Random.Range(0, voicesTest.Length);
                    Launcher.soundSystem.PlayVoiceTest(voicesTest[voiceRandom]);
                }
            }
        }

        private void SetMax()
        {
            if (currentOptionAudio == OptionAudio.MUSIC)
            {
                int vol = PlayerPrefs.GetInt("MusicVolume");
                if (vol < 10)
                {
                    PlayerPrefs.SetInt("MusicVolume", vol + 1);
                    audioMixer.SetFloat("MusicVolume", Volume[vol + 1]);
                    styleAudio[0].textPercentVolume.text = PercentVolume[vol + 1];
                }
            }
            else if (currentOptionAudio == OptionAudio.SFX)
            {
                int vol = PlayerPrefs.GetInt("SFXVolume");
                if (vol < 10)
                {
                    PlayerPrefs.SetInt("SFXVolume", vol + 1);
                    audioMixer.SetFloat("SFXVolume", Volume[vol + 1]);
                    styleAudio[1].textPercentVolume.text = PercentVolume[vol + 1];

                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
                }
            }
            else if (currentOptionAudio == OptionAudio.VOICE)
            {
                int vol = PlayerPrefs.GetInt("VoiceVolume");
                if (vol < 10)
                {
                    PlayerPrefs.SetInt("VoiceVolume", vol + 1);
                    audioMixer.SetFloat("VoiceVolume", Volume[vol + 1]);
                    styleAudio[2].textPercentVolume.text = PercentVolume[vol + 1];

                    int voiceRandom = UnityEngine.Random.Range(0, voicesTest.Length - 1);
                    Launcher.soundSystem.PlayVoiceTest(voicesTest[voiceRandom]);
                }
            }
        }

        void InicialValuesVideo()
        {
            Launcher.initializationSettings.VSync = PlayerPrefs.GetInt("isFullScreen") == 1;
            Launcher.initializationSettings.isFullScreen = PlayerPrefs.GetInt("isFullScreen") == 1;
            Launcher.initializationSettings.isShakeCamera = PlayerPrefs.GetInt("isShakeCamera") == 1;
        }

        void InicialValuesVolume()
        {
            int volMusic = PlayerPrefs.GetInt("MusicVolume");
            audioMixer.SetFloat("MusicVolume", Volume[volMusic]);
            styleAudio[0].textPercentVolume.text = PercentVolume[volMusic];

            int volSFX = PlayerPrefs.GetInt("SFXVolume");
            audioMixer.SetFloat("SFXVolume", Volume[volSFX]);
            styleAudio[1].textPercentVolume.text = PercentVolume[volSFX];

            int volVoice = PlayerPrefs.GetInt("VoiceVolume");
            audioMixer.SetFloat("VoiceVolume", Volume[volVoice]);
            styleAudio[2].textPercentVolume.text = PercentVolume[volVoice];
        }


        private void Controls()
        {
            for (int i = 0; i < menuControls.Length; i++)
            {
                GameObject g = menuControls[i].controlsSelect;
                Image image = g.GetComponent<Image>();
                if (currentOptionControls == menuControls[i].options_SELECTED)
                {
                    image.color = mainColorEnable;
                }
                else
                {
                    image.color = mainColorDisable;
                }
            }

            if (InputCustom.PressUpPlayerIDOne() || InputCustom.PressDownPlayerIDOne())
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
            }
            /*
            if (currentOptionControls == OptionControl.CHOOSE_CONTROL && InputCustom.PressDownPlayerIDOne())
            {
                currentOptionControls = OptionControl.KEYBOARD_CONTROLLE;
            }
            else if (currentOptionControls == OptionControl.CHOOSE_CONTROL && InputCustom.PressUpPlayerIDOne())
            {
                currentOptionControls = OptionControl.JOYSTICK_CONTROLLE;
            }

            else if (currentOptionControls == OptionControl.KEYBOARD_CONTROLLE && InputCustom.PressDownPlayerIDOne())
            {
                currentOptionControls = OptionControl.JOYSTICK_CONTROLLE;
            }
            else if (currentOptionControls == OptionControl.KEYBOARD_CONTROLLE && InputCustom.PressUpPlayerIDOne())
            {
                currentOptionControls = OptionControl.CHOOSE_CONTROL;
            }

            else if (currentOptionControls == OptionControl.JOYSTICK_CONTROLLE && InputCustom.PressDownPlayerIDOne())
            {
                currentOptionControls = OptionControl.CHOOSE_CONTROL;
            }
            else if (currentOptionControls == OptionControl.JOYSTICK_CONTROLLE && InputCustom.PressUpPlayerIDOne())
            {
                currentOptionControls = OptionControl.KEYBOARD_CONTROLLE;
            }
            */

#warning temporario
            currentOptionControls = OptionControl.KEYBOARD_CONTROLLE;

            if (InputManager.GetButtonDown("X"))
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);

                if (currentOptionControls == OptionControl.CHOOSE_CONTROL)
                {
                    currentSCREEN = CURRENT_SCREEN.PLAYER_SELECT;
                    panelControls.SetActive(false);
                    panelPlayerSelect.SetActive(true);
                    if (PlayerPrefs.GetInt("controller1") == 1)
                    {
                        positionPlayerSelectC1 = PositionPlayerSelect.Left;
                        animatorC1.SetTrigger("CenterLeft");
                    }
                    else if (PlayerPrefs.GetInt("controller1") == 2)
                    {
                        positionPlayerSelectC1 = PositionPlayerSelect.Right;
                        animatorC1.SetTrigger("CenterRight");
                    }

                    if (PlayerPrefs.GetInt("controller2") == 1)
                    {
                        positionPlayerSelectC2 = PositionPlayerSelect.Left;
                        animatorC2.SetTrigger("CenterLeft");
                    }
                    else if (PlayerPrefs.GetInt("controller2") == 2)
                    {
                        positionPlayerSelectC2 = PositionPlayerSelect.Right;
                        animatorC2.SetTrigger("CenterRight");
                    }
                }
                else if (currentOptionControls == OptionControl.KEYBOARD_CONTROLLE)
                {
                    currentSCREEN = CURRENT_SCREEN.KEYBOARD_CONTROLLE;
                    panelControls.SetActive(false);
                    panelControlKey.SetActive(true);
                    contentP1.GetComponent<ConfigurationControl>().SetParameters(1, 0);
                    panelOptions.SetActive(false);
                }
                else if (currentOptionControls == OptionControl.JOYSTICK_CONTROLLE)
                {
                    currentSCREEN = CURRENT_SCREEN.JOYSTICK_CONTROLLE;
                    panelControls.SetActive(false);
                    panelControlJoy.SetActive(true);
                    panelControlJoy.GetComponent<ConfigurationControl>().SetParameters(1, 1);
                }
                //else if (currentOptionControls == OptionControl.KEYBOARD_CONTROLLE_2)
                //{
                //    currentSCREEN = CURRENT_SCREEN.KEYBOARD_CONTROLLE;
                //    panelControls.SetActive(false);
                //    panelControlKey.SetActive(true);
                //    panelControlKey.GetComponent<ConfigurationControl>().SetParameters(2, 0);
                //}
                //else if (currentOptionControls == OptionControl.JOYSTICK_CONTROLLE_2)
                //{
                //    currentSCREEN = CURRENT_SCREEN.JOYSTICK_CONTROLLE;
                //    panelControls.SetActive(false);
                //    panelControlJoy.SetActive(true);
                //    panelControlJoy.GetComponent<ConfigurationControl>().SetParameters(2, 1);
                //}
            }
            else if (InputManager.GetButtonDown("Y"))
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.CANCEL);
                currentSCREEN = CURRENT_SCREEN.OPTIONS;
                panelControls.SetActive(false);
                panelOptions.SetActive(true);
            }

        }


        private void Exit()
        {
            if (InputManager.GetButtonDown("X"))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            }
            else if (InputManager.GetButtonDown("Y"))
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.CANCEL);
                currentSCREEN = CURRENT_SCREEN.MAIN;
                panelExit.SetActive(false);
            }

        }







        private void PlayerSelect()
        {
            if (InputCustom.LastBindingAcitivPlayerIDOne == 0)
            {
                keyboardC1.gameObject.SetActive(true);
                joystickC1.gameObject.SetActive(false);
            }
            else if (InputCustom.LastBindingAcitivPlayerIDOne == 1)
            {
                keyboardC1.gameObject.SetActive(false);
                joystickC1.gameObject.SetActive(true);
            }

            if (InputCustom.LastBindingAcitivPlayerIDTwo == 0)
            {
                keyboardC2.gameObject.SetActive(true);
                joystickC2.gameObject.SetActive(false);
            }
            else if (InputCustom.LastBindingAcitivPlayerIDTwo == 1)
            {
                keyboardC2.gameObject.SetActive(false);
                joystickC2.gameObject.SetActive(true);
            }

            if (InputManager.GetButtonDown("Y", PlayerID.One))
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.CANCEL);
                currentSCREEN = CURRENT_SCREEN.CONTROLS;
                panelControls.SetActive(true);
                panelPlayerSelect.SetActive(false);
            }

            if (positionPlayerSelectC1 == PositionPlayerSelect.Center)
            {
                if (InputCustom.PressLeftPlayerIDOne() && positionPlayerSelectC2 != PositionPlayerSelect.Left)
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
                    positionPlayerSelectC1 = PositionPlayerSelect.Left;
                    animatorC1.SetTrigger("CenterLeft");
                    ArrowLeftC1.enabled = false;
                    ArrowRightC1.enabled = true;
                    PlayerPrefs.SetInt("controller1", 1);
                    initialization.controller1 = PlayerID.One;
                }
                else if (InputCustom.PressRightPlayerIDOne() && positionPlayerSelectC2 != PositionPlayerSelect.Right)
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
                    positionPlayerSelectC1 = PositionPlayerSelect.Right;
                    animatorC1.SetTrigger("CenterRight");
                    ArrowLeftC1.enabled = true;
                    ArrowRightC1.enabled = false;
                    PlayerPrefs.SetInt("controller1", 2);
                    initialization.controller1 = PlayerID.Two;
                }
                else if (InputCustom.PressRightPlayerIDOne() && positionPlayerSelectC2 == PositionPlayerSelect.Right ||
                    InputCustom.PressLeftPlayerIDOne() && positionPlayerSelectC2 == PositionPlayerSelect.Left)
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.CANCEL);
                }
            }
            else if (positionPlayerSelectC1 == PositionPlayerSelect.Left)
            {
                if (InputCustom.PressRightPlayerIDOne())
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
                    positionPlayerSelectC1 = PositionPlayerSelect.Center;
                    animatorC1.SetTrigger("LeftCenter");
                    ArrowLeftC1.enabled = true;
                    ArrowRightC1.enabled = true;
                    PlayerPrefs.SetInt("controller1", 0);
                    initialization.controller1 = PlayerID.None;
                }
            }
            else if (positionPlayerSelectC1 == PositionPlayerSelect.Right)
            {
                if (InputCustom.PressLeftPlayerIDOne())
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
                    positionPlayerSelectC1 = PositionPlayerSelect.Center;
                    animatorC1.SetTrigger("RightCenter");
                    ArrowLeftC1.enabled = true;
                    ArrowRightC1.enabled = true;
                    PlayerPrefs.SetInt("controller1", 0);
                    initialization.controller1 = PlayerID.None;
                }
            }


            if (positionPlayerSelectC2 == PositionPlayerSelect.Center)
            {
                if (InputCustom.PressLeftPlayerIDTwo() && positionPlayerSelectC1 != PositionPlayerSelect.Left)
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
                    positionPlayerSelectC2 = PositionPlayerSelect.Left;
                    animatorC2.SetTrigger("CenterLeft");
                    ArrowLeftC2.enabled = false;
                    ArrowRightC2.enabled = true;
                    PlayerPrefs.SetInt("controller2", 1);
                    initialization.controller2 = PlayerID.One;
                }
                else if (InputCustom.PressRightPlayerIDTwo() && positionPlayerSelectC1 != PositionPlayerSelect.Right)
                {

                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
                    positionPlayerSelectC2 = PositionPlayerSelect.Right;
                    animatorC2.SetTrigger("CenterRight");
                    ArrowLeftC2.enabled = true;
                    ArrowRightC2.enabled = false;
                    PlayerPrefs.SetInt("controller2", 2);
                    initialization.controller2 = PlayerID.Two;
                }
                else if (InputCustom.PressRightPlayerIDTwo() && positionPlayerSelectC1 == PositionPlayerSelect.Right ||
                    InputCustom.PressLeftPlayerIDTwo() && positionPlayerSelectC1 == PositionPlayerSelect.Left)
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.CANCEL);
                }
            }
            else if (positionPlayerSelectC2 == PositionPlayerSelect.Left)
            {
                if (InputCustom.PressRightPlayerIDTwo())
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
                    positionPlayerSelectC2 = PositionPlayerSelect.Center;
                    animatorC2.SetTrigger("LeftCenter");
                    ArrowLeftC2.enabled = true;
                    ArrowRightC2.enabled = true;
                    PlayerPrefs.SetInt("controller2", 0);
                    initialization.controller2 = PlayerID.None;
                }
            }
            else if (positionPlayerSelectC2 == PositionPlayerSelect.Right)
            {
                if (InputCustom.PressLeftPlayerIDTwo())
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
                    positionPlayerSelectC2 = PositionPlayerSelect.Center;
                    animatorC2.SetTrigger("RightCenter");
                    ArrowLeftC2.enabled = true;
                    ArrowRightC2.enabled = true;
                    PlayerPrefs.SetInt("controller2", 0);
                    initialization.controller2 = PlayerID.None;
                }
            }
        }

    }

    public enum OptionVideo { RESOLUTION = 0, DISPLAY = 1, VSYNC = 2, BRIGHTINESS = 3, ANTI_ALIAS = 4, SHAKE_CAMERA = 5 }
    public enum OptionAudio { MUSIC = 0, SFX = 1, VOICE = 2 }
    public enum OptionDefinition { VOICE_LANGUAGE = 0, TEXT_LANGUAGE = 1, SPEED_TIME = 2, TIME_ROUND = 3, NUMBER_OF_ROUNDS = 4, AI_LEVEL = 5 }
    public enum OptionControl { CHOOSE_CONTROL, KEYBOARD_CONTROLLE, JOYSTICK_CONTROLLE }
    public enum TypeDisplay { FULLSCREEL, WINDOWS }
    public enum PositionPlayerSelect { Center, Left, Right }
    public enum NETWORK_SELECTED { SERVER, CLIENT, REPLAY }
}