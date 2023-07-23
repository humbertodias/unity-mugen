using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityMugen.Combat;
using UnityMugen.CustomInput;

namespace UnityMugen.Screens
{

    public class SelectStageScreen : MonoBehaviour
    {
        LauncherEngine Launcher => LauncherEngine.Inst;
        List<StageProfile> stageProfiles => Launcher.profileLoader.stageProfiles;
        List<MusicProfile> musicProfiles => Launcher.profileLoader.musicProfiles;

        public GameObject selectPlayer;

        public Image spriteStage;
        public Text nameStage;
        public Image Previous, Next;

        private EngineInitialization initialization;
        private int currentSelect;


        public void Start()
        {
            Launcher.screenType = ScreenType.Select;
            initialization = LauncherEngine.Inst.engineInitialization;

            //if (Launcher.IsHostClientConnected())
            //    Instantiate(Resources.Load<GameObject>("SHOW_Sync"));

            //if (Launcher.IsHost())
            //{
            //    Launcher.hostPlayerTCP.SendStatusScreen(TipeMessageHostClient.SelectScreenLoaded);
            //    Launcher.typeCurrentScreenP1 = TypeCurrentScreen.SelectScreen;
            //}
            //else if (Launcher.IsClient())
            //{
            //    Launcher.clientPlayerTCP.SendStatusScreen(TipeMessageHostClient.SelectScreenLoaded);
            //    Launcher.typeCurrentScreenP2 = TypeCurrentScreen.SelectScreen;
            //}
        }


        private bool screenReady = false;
        private bool IsSynchronized()
        {
            if (screenReady)
                return true;

            if (Launcher.typeCurrentScreenP1 == TypeCurrentScreen.SelectScreen &&
                Launcher.typeCurrentScreenP2 == TypeCurrentScreen.SelectScreen)
            {
                screenReady = true;
                Destroy(GameObject.Find("SHOW_Sync(Clone)"));
                return true;
            }
            return false;
        }

        void Update()
        {
            // if (!Launcher.IsHostClientConnected())
            DoUpdate();
        }




        float timeEffect;
        public PlayerButton oldPlayerButton;

        public void DoUpdate()
        {
            timeEffect += Time.deltaTime;
            //if (Launcher.IsHostClientConnected())
            //{
            //    if (!IsSynchronized())
            //    {
            //        return;
            //    }


            //    PlayerButton playerButton = Launcher.inputSystem.KeyboardStateP1(true);
            //    if (playerButton != oldPlayerButton)
            //    {
            //        if (Launcher.IsHost())
            //        {
            //            Launcher.hostPlayerTCP.SendCommandButton(playerButton);
            //            Launcher.inputSystem.playerButton1Newtwork = playerButton;
            //            Launcher.typeCurrentScreenP1 = TypeCurrentScreen.SelectScreen;
            //        }
            //        else if (Launcher.IsClient())
            //        {
            //            Launcher.clientPlayerTCP.SendCommandButton(playerButton);
            //            Launcher.inputSystem.playerButton2Newtwork = playerButton;
            //            Launcher.typeCurrentScreenP2 = TypeCurrentScreen.SelectScreen;
            //        }
            //        oldPlayerButton = playerButton;
            //    }


            //    bool left = Convert.ToBoolean(CommandButton1(PlayerButton.Left));
            //    bool right = Convert.ToBoolean(CommandButton1(PlayerButton.Right));
            //    bool x = Convert.ToBoolean(CommandButton1(PlayerButton.X));
            //    bool y = Convert.ToBoolean(CommandButton1(PlayerButton.Y));
            //    oldCurrentP1 = Launcher.inputSystem.playerButton1Newtwork;


            //    if (left || right)
            //    {
            //        Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
            //        //    imageStage.sprite = dadosStage.imageStage;
            //        timeEffect = 0;
            //    }

            //    if (right)
            //    {
            //        if (currentSelect == stageDatas.Length - 1)
            //            currentSelect = 0;
            //        else
            //            currentSelect++;

            //        Next.color = Color.yellow;
            //    }
            //    else if (left)
            //    {
            //        if (currentSelect == 0)
            //            currentSelect = stageDatas.Length - 1;
            //        else
            //            currentSelect--;

            //        Previous.color = Color.yellow;
            //    }

            //    if (x)
            //    {
            //        initialization.Stage = stageDatas[currentSelect].stage;
            //        LoadStageBttle(stageDatas[currentSelect].stage.name);
            //    }
            //    else if (y)
            //    {
            //        LoadSceneCustom.LoadScene("SelectScreen");
            //    }
            //}
            //else
            //{
            if (InputCustom.PressRightPlayerIDOne() || InputCustom.PressLeftPlayerIDOne())
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
                timeEffect = 0;
            }

            if (InputCustom.PressRightPlayerIDOne())
            {
                if (currentSelect == stageProfiles.Count - 1)
                    currentSelect = 0;
                else
                    currentSelect++;

                Next.color = Color.yellow;
            }
            else if (InputCustom.PressLeftPlayerIDOne())
            {
                if (currentSelect == 0)
                    currentSelect = stageProfiles.Count - 1;
                else
                    currentSelect--;

                Previous.color = Color.yellow;
            }

            if (InputManager.GetButtonDown("X", PlayerID.One))
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);

                // Ao instancial fica indisponivel fazer alteraçoes durante o jogo
                initialization.stageID = stageProfiles[currentSelect].stageID;
                initialization.musicID = musicProfiles[stageProfiles[currentSelect].musicID].musicID;
                LoadStageBattle(stageProfiles[currentSelect].Name);
            }
            else if (InputManager.GetButtonDown("Y", PlayerID.One))
            {
                selectPlayer.SetActive(true);
                gameObject.SetActive(false);
                selectPlayer.GetComponent<ISelectPlayerScreen>().ResetSelectPlayer();
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.CANCEL);
                //LoadSceneCustom.LoadScene("SelectScreen");
            }
            //}

            if (timeEffect > 0.3f)
            {
                Previous.color = Color.green;
                Next.color = Color.green;
            }
            spriteStage.sprite = stageProfiles[currentSelect].spriteStage;
            nameStage.text = stageProfiles[currentSelect].DisplayName;

        }

        public PlayerButton StringToPlayerButton(string stringPlayerButton)
        {
            PlayerButton newPlayerButton = PlayerButton.None;
            foreach (PlayerButton playerButton in Enum.GetValues(typeof(PlayerButton)))
            {
                if (stringPlayerButton.ToString().Contains(playerButton.ToString()))
                {
                    newPlayerButton |= playerButton;
                }
            }
            return newPlayerButton;
        }

        PlayerButton oldCurrentP1;
        public int CommandButton1(PlayerButton playerButton)
        {
            PlayerButton currentP1 = Launcher.inputSystem.playerButton1Newtwork;
            if (oldCurrentP1 != currentP1 && currentP1.ToString().Contains(playerButton.ToString()))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        PlayerButton oldCurrentP2;
        public int CommandButton2(PlayerButton playerButton)
        {
            PlayerButton currentP2 = Launcher.inputSystem.playerButton1Newtwork;
            if (oldCurrentP2 != currentP2 && currentP2.ToString().Contains(playerButton.ToString()))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        private void LoadStageBattle(string stageName)
        {
            GameObject init = new GameObject();
            init.name = "LoadSceneCustom";
            init.hideFlags = HideFlags.HideInHierarchy;
            new LoadBattleScene().Iniciar(stageName, Color.black, 2.5f, true);
        }
    }

}