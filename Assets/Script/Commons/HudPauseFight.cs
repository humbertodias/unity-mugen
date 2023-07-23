using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityMugen.Combat;
using UnityMugen.CustomInput;
using UnityMugen.Screens;

namespace UnityMugen.Interface
{

    public enum ItemPanelPause
    {
        Continue,
        CommandsList,
        KeyboardSettings,
        MainMenu
    }

    public enum ItemPanelPauseTrainner
    {
        Continue,
        DisplayInformations,
        GaugeOptions,
        ActionEnemy,
        CommandsList,
        KeyboardSettings,
        QuickSelect,
        SelectPlayersScreen,
        MainMenu
    }

    public enum ItemPanelDisplayInformation
    {
        Commands,
        InputHistory,
        BoxColliders,
        InformationP1,
        InformationP2,
        InfoGeral,
        DisplayFPS
    }

    public enum ItemPanelGaugeOption
    {
        HPRecovery,
        P1HP,
        P2HP,
        PowerRecovery
    }

    public enum ItemPanelActionEnemy
    {
        StateType,
        Guard,
        GuardTime,
        Teching,
        COMLevel
    }

    public enum ItemPanelQuickCharChange
    {
        P1Character,
        P2Character,
        Stage,
        BMG,
        ApplyChange
    }

    public enum PanelPauseFight
    {
        Pause,
        DisplayInformation,
        GaugeOption,
        ActionEnemy,
        CommandsList,
        KeyboardSettings,
        QuickCharChange,

        SelectConfirm
    }



    [Serializable]
    public class SelectItemPanelPause
    {
        public ItemPanelPauseTrainner itemPanelPause;
        public GameObject item;
    }

    [Serializable]
    public class SelectItemPanelDisplayInformation
    {
        public ItemPanelDisplayInformation panelDisplayInformation;
        public GameObject item;
        public Text description;
        public Image[] selecters;
    }

    [Serializable]
    public class SelectItemPanelGaugeOption
    {
        public ItemPanelGaugeOption panelGaugeOption;
        public GameObject item;
        public Text description;
        public Image[] selecters;
    }

    //
    [Serializable]
    public class SelectItemPanelActionEnemy
    {
        public ItemPanelActionEnemy panelActionEnemy;
        public GameObject item;
        public Text description;
        public Image[] selecters;
    }

    [Serializable]
    public class SelectItemPanelQuickCharChange
    {
        public ItemPanelQuickCharChange panelQuickCharChange;
        public GameObject item;
        public Text description;
        public Image[] selecters;
    }

    public class HudPauseFight : MonoBehaviour
    {
        LauncherEngine Launcher => LauncherEngine.Inst;
        FightEngine Engine => Launcher.mugen.Engine;
        TrainnerSettings settings => Launcher.trainnerSettings;

        public PanelPauseFight currentPanelPauseFight;
        public ItemPanelPauseTrainner currentItemPanelPause;
        public ItemPanelDisplayInformation currentItemPanelDisplayInformation;
        public ItemPanelGaugeOption currentItemPanelGaugeOption;
        public ItemPanelActionEnemy currentItemPanelActionEnemy;
        public ItemPanelQuickCharChange currentItemPanelQuickCharChange;

        [Header("Panels")]
        public GameObject panelPause;
        public GameObject panelPauseTrainner;
        public GameObject panelDisplayInformation;
        public GameObject panelGaugeOption;
        public GameObject panelActionEnemy;
        public GameObject panelQuickCharChange;

        [Header("Controllers")]
        public GameObject panelControllerKey;
        public GameObject contentP1;
        public GameObject contentP2;


        public GameObject panelSelectConfirm;
        public Text message;

        public SelectItemPanelPause[] itensPanelPause;
        public SelectItemPanelPause[] itensPanelPauseTrainner;
        public SelectItemPanelDisplayInformation[] itemPanelDisplayInformation;
        public SelectItemPanelGaugeOption[] itemPanelGaugeOptions;
        public SelectItemPanelActionEnemy[] itemPanelActionEnemy;
        public SelectItemPanelQuickCharChange[] itemPanelQuickCharChange;

        void OnEnable()
        {
            GetComponent<Image>().enabled = true;
            currentPanelPauseFight = PanelPauseFight.Pause;
            currentItemPanelPause = ItemPanelPauseTrainner.Continue;

            if (Engine.Initialization.Mode != CombatMode.Training)
                panelPause.SetActive(true);
            else
                panelPauseTrainner.SetActive(true);
        }

        void UpdateData()
        {
            // DisplayInformation
            itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.Commands].description.text =
                settings.showCommands ? "ON" : "OFF";
            itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.Commands].
                selecters[Convert.ToInt32(settings.showCommands)].color = Color.yellow;

            itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.InputHistory].description.text =
                settings.showInputHistory ? "ON" : "OFF";
            itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.InputHistory].
                selecters[Convert.ToInt32(settings.showInputHistory)].color = Color.yellow;

            itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.InformationP1].description.text =
                settings.showInfoP1 ? "ON" : "OFF";
            itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.InformationP1].
                selecters[Convert.ToInt32(settings.showInfoP1)].color = Color.yellow;

            itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.InformationP2].description.text =
                settings.showInfoP2 ? "ON" : "OFF";
            itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.InformationP2].
                selecters[Convert.ToInt32(settings.showInfoP2)].color = Color.yellow;

            itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.DisplayFPS].description.text =
                settings.showFPS ? "ON" : "OFF";
            itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.DisplayFPS].
                selecters[Convert.ToInt32(settings.showFPS)].color = Color.yellow;

            itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.InfoGeral].description.text =
                settings.showInfoGeral ? "ON" : "OFF";
            itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.InfoGeral].
                selecters[Convert.ToInt32(settings.showInfoGeral)].color = Color.yellow;

            itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.BoxColliders].description.text =
                settings.typeDrawCollider.ToString();
            itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.BoxColliders].
                selecters[(int)settings.typeDrawCollider].color = Color.yellow;
            ///////////////////////////////


            // ActionEnemy
            itemPanelActionEnemy[(int)ItemPanelActionEnemy.StateType].description.text = settings.stanceType.ToString();
            itemPanelActionEnemy[(int)ItemPanelActionEnemy.StateType].selecters[(int)settings.stanceType].color = Color.yellow;

            itemPanelActionEnemy[(int)ItemPanelActionEnemy.Guard].description.text = settings.guard.ToName();
            itemPanelActionEnemy[(int)ItemPanelActionEnemy.Guard].selecters[(int)settings.guard].color = Color.yellow;

            itemPanelActionEnemy[(int)ItemPanelActionEnemy.GuardTime].description.text = settings.guardTime.ToName();
            itemPanelActionEnemy[(int)ItemPanelActionEnemy.GuardTime].selecters[(int)settings.guardTime].color = Color.yellow;

            itemPanelActionEnemy[(int)ItemPanelActionEnemy.Teching].description.text = settings.teching.ToString();
            itemPanelActionEnemy[(int)ItemPanelActionEnemy.Teching].selecters[(int)settings.teching].color = Color.yellow;

            itemPanelActionEnemy[(int)ItemPanelActionEnemy.COMLevel].description.text = settings.COMLevel.ToString();
            itemPanelActionEnemy[(int)ItemPanelActionEnemy.COMLevel].selecters[settings.COMLevel - 1].color = Color.yellow;
            ///////////////////////////////

            // QuickCharacterChange
            currentItemPanelQuickCharChangeP1Character = Engine.Team1.MainPlayer.profile.charID;
            charNames.TryGetValue(currentItemPanelQuickCharChangeP1Character, out string option);
            itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.P1Character].description.text = option;
            itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.P1Character].
                selecters[currentItemPanelQuickCharChangeP1Character].color = Color.yellow;

            currentItemPanelQuickCharChangeP2Character = Engine.Team2.MainPlayer.profile.charID;
            charNames.TryGetValue(currentItemPanelQuickCharChangeP2Character, out string option2);
            itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.P2Character].description.text = option2;
            itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.P2Character].
                selecters[currentItemPanelQuickCharChangeP2Character].color = Color.yellow;

            currentItemPanelQuickCharChangeStage = Engine.stageScreen.Stage.stageID;
            stageNames.TryGetValue(currentItemPanelQuickCharChangeStage, out string option3);
            itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.Stage].description.text = option3;
            itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.Stage].
                selecters[currentItemPanelQuickCharChangeStage].color = Color.yellow;

            currentItemPanelQuickCharChangeBMG = Launcher.engineInitialization.musicID;
            BMGNames.TryGetValue(currentItemPanelQuickCharChangeBMG, out string option4);
            itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.BMG].description.text = option4;
            itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.BMG].
                selecters[currentItemPanelQuickCharChangeBMG].color = Color.yellow;
            ///////////////////////////////

            // GaugeOptions
            itemPanelGaugeOptions[(int)ItemPanelGaugeOption.HPRecovery].description.text = settings.hpRecovery.ToString();
            itemPanelGaugeOptions[(int)ItemPanelGaugeOption.HPRecovery].selecters[(int)settings.hpRecovery].color = Color.yellow;

            itemPanelGaugeOptions[(int)ItemPanelGaugeOption.P1HP].description.text = settings.percentP1HPMax * 10 + "%";
            itemPanelGaugeOptions[(int)ItemPanelGaugeOption.P1HP].selecters[settings.percentP1HPMax - 1].color = Color.yellow;

            itemPanelGaugeOptions[(int)ItemPanelGaugeOption.P2HP].description.text = settings.percentP2HPMax * 10 + "%";
            itemPanelGaugeOptions[(int)ItemPanelGaugeOption.P2HP].selecters[settings.percentP2HPMax - 1].color = Color.yellow;

            itemPanelGaugeOptions[(int)ItemPanelGaugeOption.PowerRecovery].description.text = settings.powerRecovery.ToString();
            itemPanelGaugeOptions[(int)ItemPanelGaugeOption.PowerRecovery].selecters[(int)settings.powerRecovery].color = Color.yellow;
            ///////////////////////////////


        }

        void OnDisable()
        {
            GetComponent<Image>().enabled = false;
            DisableAllPanel();
        }

        void Start()
        {
            charNames = Launcher.profileLoader.NamesCharacters();
            stageNames = Launcher.profileLoader.NamesStages();
            BMGNames = Launcher.profileLoader.NamesBMG();

            CreateSelectorP1Character();
            CreateSelectorP2Character();
            CreateSelectorStage();
            CreateSelectorBMG();

            UpdateData();
        }


        public void DisableAllPanel()
        {
            panelPause.SetActive(false);
            panelPauseTrainner.SetActive(false);
            panelDisplayInformation.SetActive(false);
            panelGaugeOption.SetActive(false);
            panelActionEnemy.SetActive(false);
            panelControllerKey.SetActive(false);
            panelQuickCharChange.SetActive(false);
        }

        void Update()
        {
            if (currentPanelPauseFight == PanelPauseFight.Pause)
            {
                UpdatePanelPause();
            }
            else if (currentPanelPauseFight == PanelPauseFight.DisplayInformation)
            {
                UpdateItemPanelDisplayInformation();
            }
            else if (currentPanelPauseFight == PanelPauseFight.GaugeOption)
            {
                UpdateItemPanelGaugeOption();
            }
            else if (currentPanelPauseFight == PanelPauseFight.ActionEnemy)
            {
                UpdateItemPanelActionEnemy();
            }
            else if (currentPanelPauseFight == PanelPauseFight.KeyboardSettings)
            {
                if (!contentP1.GetComponent<ConfigurationControl>().enable &&
                    !contentP2.GetComponent<ConfigurationControl>().enable)
                {
                    currentPanelPauseFight = PanelPauseFight.Pause;
                    if (Engine.Initialization.Mode != CombatMode.Training)
                        panelPause.SetActive(true);
                    else
                        panelPauseTrainner.SetActive(true);
                    panelControllerKey.SetActive(false);
                }

                if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift))
                {
                    contentP1.GetComponent<ConfigurationControl>().SetParameters(1, 0);
                }

                if (UnityEngine.Input.GetKeyDown(KeyCode.RightShift))
                {
                    contentP2.GetComponent<ConfigurationControl>().SetParameters(2, 0);
                }
            }
            else if (currentPanelPauseFight == PanelPauseFight.QuickCharChange)
            {
                UpdateItemPanelQuickCharChange();
            }

            else if (currentPanelPauseFight == PanelPauseFight.SelectConfirm)
            {
                UpdatePanelSelectConfirm();
            }
        }

        private void UpdatePanelPause()
        {
            if (Engine.Initialization.Mode != CombatMode.Training)
            {
                foreach (SelectItemPanelPause item in itensPanelPause)
                {
                    if (currentItemPanelPause == item.itemPanelPause)
                        item.item.GetComponent<Image>().enabled = true;
                    else
                        item.item.GetComponent<Image>().enabled = false;
                }
            }
            else
            {
                foreach (SelectItemPanelPause item in itensPanelPauseTrainner)
                {
                    if (currentItemPanelPause == item.itemPanelPause)
                        item.item.GetComponent<Image>().enabled = true;
                    else
                        item.item.GetComponent<Image>().enabled = false;
                }
            }
            if (InputCustom.PressUpPlayerIDOne() || InputCustom.PressDownPlayerIDOne())
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
            }

            if (Engine.Initialization.Mode != CombatMode.Training)
                UpdateItemPanelPause();
            else
                UpdateItemPanelPauseTrainner();

            if (InputManager.GetButtonDown("X"))
            {
                if (currentItemPanelPause == ItemPanelPauseTrainner.Continue)
                {
                    Back();
                }
                else if (currentItemPanelPause == ItemPanelPauseTrainner.DisplayInformations)
                {
                    currentPanelPauseFight = PanelPauseFight.DisplayInformation;
                    currentItemPanelDisplayInformation = ItemPanelDisplayInformation.Commands;
                    panelDisplayInformation.SetActive(true);
                    panelPause.SetActive(false);
                    panelPauseTrainner.SetActive(false);
                }
                else if (currentItemPanelPause == ItemPanelPauseTrainner.GaugeOptions)
                {
                    currentPanelPauseFight = PanelPauseFight.GaugeOption;
                    currentItemPanelGaugeOption = ItemPanelGaugeOption.HPRecovery;
                    panelGaugeOption.SetActive(true);
                    panelPauseTrainner.SetActive(false);
                }
                else if (currentItemPanelPause == ItemPanelPauseTrainner.ActionEnemy)
                {
                    currentPanelPauseFight = PanelPauseFight.ActionEnemy;
                    panelActionEnemy.SetActive(true);
                    panelPause.SetActive(false);
                    panelPauseTrainner.SetActive(false);
                }
                else if (currentItemPanelPause == ItemPanelPauseTrainner.CommandsList)
                {
                    currentPanelPauseFight = PanelPauseFight.CommandsList;
                    Launcher.mugen.Engine.stageScreen.MoveLists.Active();
                    panelPause.SetActive(false);
                    panelPauseTrainner.SetActive(false);
                }
                else if (currentItemPanelPause == ItemPanelPauseTrainner.KeyboardSettings)
                {
                    currentPanelPauseFight = PanelPauseFight.KeyboardSettings;
                    panelControllerKey.SetActive(true);
                    panelPause.SetActive(false);
                    panelPauseTrainner.SetActive(false);
                    contentP1.GetComponent<ConfigurationControl>().SetParameters(1, 0);
                }
                else if (currentItemPanelPause == ItemPanelPauseTrainner.QuickSelect)
                {
                    currentPanelPauseFight = PanelPauseFight.QuickCharChange;
                    message.text = "Do you want to confirm changes?";
                    panelQuickCharChange.SetActive(true);
                    panelPause.SetActive(false);
                    panelPauseTrainner.SetActive(false);
                }
                else if (currentItemPanelPause == ItemPanelPauseTrainner.SelectPlayersScreen)
                {
                    currentPanelPauseFight = PanelPauseFight.SelectConfirm;
                    message.text = "Go to the Player select screen?";
                    panelSelectConfirm.SetActive(true);
                    panelPause.SetActive(false);
                    panelPauseTrainner.SetActive(false);
                }
                else if (currentItemPanelPause == ItemPanelPauseTrainner.MainMenu)
                {
                    currentPanelPauseFight = PanelPauseFight.SelectConfirm;
                    message.text = "Go to the Main screen?";
                    panelSelectConfirm.SetActive(true);
                    panelPause.SetActive(false);
                    panelPauseTrainner.SetActive(false);
                }
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
            }

            else if (InputManager.GetButtonDown("Y") || InputManager.GetButtonDown("Start"))
            {
                Back();
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.CANCEL);
            }
        }

        void UpdateItemPanelPause()
        {
            if (currentItemPanelPause == ItemPanelPauseTrainner.Continue && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.MainMenu;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.Continue && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.CommandsList;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.CommandsList && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.Continue;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.CommandsList && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.KeyboardSettings;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.KeyboardSettings && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.CommandsList;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.KeyboardSettings && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.MainMenu;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.MainMenu && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.KeyboardSettings;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.MainMenu && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.Continue;
            }
        }

        void UpdateItemPanelPauseTrainner()
        {
            if (currentItemPanelPause == ItemPanelPauseTrainner.Continue && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.MainMenu;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.Continue && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.DisplayInformations;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.DisplayInformations && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.Continue;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.DisplayInformations && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.GaugeOptions;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.GaugeOptions && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.DisplayInformations;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.GaugeOptions && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.ActionEnemy;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.ActionEnemy && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.GaugeOptions;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.ActionEnemy && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.CommandsList;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.CommandsList && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.ActionEnemy;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.CommandsList && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.KeyboardSettings;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.KeyboardSettings && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.CommandsList;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.KeyboardSettings && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.QuickSelect;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.QuickSelect && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.KeyboardSettings;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.QuickSelect && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.SelectPlayersScreen;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.SelectPlayersScreen && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.QuickSelect;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.SelectPlayersScreen && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.MainMenu;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.MainMenu && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.SelectPlayersScreen;
            }
            else if (currentItemPanelPause == ItemPanelPauseTrainner.MainMenu && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelPause = ItemPanelPauseTrainner.Continue;
            }
        }





        void UpdateItemPanelDisplayInformation()
        {
            foreach (SelectItemPanelDisplayInformation item in itemPanelDisplayInformation)
            {
                if (currentItemPanelDisplayInformation == item.panelDisplayInformation)
                    item.item.GetComponent<Image>().enabled = true;
                else
                    item.item.GetComponent<Image>().enabled = false;
            }

            if (InputCustom.PressUpPlayerIDOne() || InputCustom.PressDownPlayerIDOne() ||
                InputCustom.PressLeftPlayerIDOne() || InputCustom.PressRightPlayerIDOne())
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
            }

            if (currentItemPanelDisplayInformation == ItemPanelDisplayInformation.Commands && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelDisplayInformation = ItemPanelDisplayInformation.DisplayFPS;
            }
            else if (currentItemPanelDisplayInformation == ItemPanelDisplayInformation.Commands && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelDisplayInformation = ItemPanelDisplayInformation.InputHistory;
            }
            else if (currentItemPanelDisplayInformation == ItemPanelDisplayInformation.InputHistory && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelDisplayInformation = ItemPanelDisplayInformation.Commands;
            }
            else if (currentItemPanelDisplayInformation == ItemPanelDisplayInformation.InputHistory && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelDisplayInformation = ItemPanelDisplayInformation.BoxColliders;
            }
            else if (currentItemPanelDisplayInformation == ItemPanelDisplayInformation.BoxColliders && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelDisplayInformation = ItemPanelDisplayInformation.InputHistory;
            }
            else if (currentItemPanelDisplayInformation == ItemPanelDisplayInformation.BoxColliders && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelDisplayInformation = ItemPanelDisplayInformation.InformationP1;
            }
            else if (currentItemPanelDisplayInformation == ItemPanelDisplayInformation.InformationP1 && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelDisplayInformation = ItemPanelDisplayInformation.BoxColliders;
            }
            else if (currentItemPanelDisplayInformation == ItemPanelDisplayInformation.InformationP1 && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelDisplayInformation = ItemPanelDisplayInformation.InformationP2;
            }
            else if (currentItemPanelDisplayInformation == ItemPanelDisplayInformation.InformationP2 && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelDisplayInformation = ItemPanelDisplayInformation.InformationP1;
            }
            else if (currentItemPanelDisplayInformation == ItemPanelDisplayInformation.InformationP2 && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelDisplayInformation = ItemPanelDisplayInformation.InfoGeral;
            }
            else if (currentItemPanelDisplayInformation == ItemPanelDisplayInformation.InfoGeral && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelDisplayInformation = ItemPanelDisplayInformation.InformationP2;
            }
            else if (currentItemPanelDisplayInformation == ItemPanelDisplayInformation.InfoGeral && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelDisplayInformation = ItemPanelDisplayInformation.DisplayFPS;
            }
            else if (currentItemPanelDisplayInformation == ItemPanelDisplayInformation.DisplayFPS && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelDisplayInformation = ItemPanelDisplayInformation.InfoGeral;
            }
            else if (currentItemPanelDisplayInformation == ItemPanelDisplayInformation.DisplayFPS && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelDisplayInformation = ItemPanelDisplayInformation.Commands;
            }


            if (InputCustom.PressLeftPlayerIDOne() || InputCustom.PressRightPlayerIDOne())
            {
                if (currentItemPanelDisplayInformation == ItemPanelDisplayInformation.Commands)
                {
                    settings.showCommands = !settings.showCommands;

                    Image[] imgs = itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.Commands].selecters;
                    foreach (Image img in imgs)
                        img.color = Color.white;

                    string option = settings.showCommands ? "ON" : "OFF";
                    itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.Commands].description.text = option;
                    imgs[Convert.ToInt32(settings.showCommands)].color = Color.yellow;
                }
                else if (currentItemPanelDisplayInformation == ItemPanelDisplayInformation.InputHistory)
                {
                    settings.showInputHistory = !settings.showInputHistory;

                    Image[] imgs = itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.InputHistory].selecters;
                    foreach (Image img in imgs)
                        img.color = Color.white;

                    string option = settings.showInputHistory ? "ON" : "OFF";
                    itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.InputHistory].description.text = option;
                    imgs[Convert.ToInt32(settings.showInputHistory)].color = Color.yellow;
                }
                else if (currentItemPanelDisplayInformation == ItemPanelDisplayInformation.InformationP1)
                {
                    settings.showInfoP1 = !settings.showInfoP1;

                    Image[] imgs = itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.InformationP1].selecters;
                    foreach (Image img in imgs)
                        img.color = Color.white;

                    string option = settings.showInfoP1 ? "ON" : "OFF";
                    itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.InformationP1].description.text = option;
                    imgs[Convert.ToInt32(settings.showInfoP1)].color = Color.yellow;
                }
                else if (currentItemPanelDisplayInformation == ItemPanelDisplayInformation.InformationP2)
                {
                    settings.showInfoP2 = !settings.showInfoP2;

                    Image[] imgs = itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.InformationP2].selecters;
                    foreach (Image img in imgs)
                        img.color = Color.white;

                    string option = settings.showInfoP2 ? "ON" : "OFF";
                    itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.InformationP2].description.text = option;
                    imgs[Convert.ToInt32(settings.showInfoP2)].color = Color.yellow;
                }
                else if (currentItemPanelDisplayInformation == ItemPanelDisplayInformation.DisplayFPS)
                {
                    settings.showFPS = !settings.showFPS;

                    Image[] imgs = itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.DisplayFPS].selecters;
                    foreach (Image img in imgs)
                        img.color = Color.white;

                    string option = settings.showFPS ? "ON" : "OFF";
                    itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.DisplayFPS].description.text = option;
                    imgs[Convert.ToInt32(settings.showFPS)].color = Color.yellow;
                }
                else if (currentItemPanelDisplayInformation == ItemPanelDisplayInformation.InfoGeral)
                {
                    settings.showInfoGeral = !settings.showInfoGeral;

                    Image[] imgs = itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.InfoGeral].selecters;
                    foreach (Image img in imgs)
                        img.color = Color.white;

                    string option = settings.showInfoGeral ? "ON" : "OFF";
                    itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.InfoGeral].description.text = option;
                    imgs[Convert.ToInt32(settings.showInfoGeral)].color = Color.yellow;
                }


                else if (currentItemPanelDisplayInformation == ItemPanelDisplayInformation.BoxColliders)
                {
                    if (InputCustom.PressLeftPlayerIDOne())
                    {
                        if ((int)settings.typeDrawCollider > 0)
                        {
                            Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
                            settings.typeDrawCollider--;
                        }
                    }
                    else if (InputCustom.PressRightPlayerIDOne())
                    {
                        if ((int)settings.typeDrawCollider < 3)
                        {
                            Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
                            settings.typeDrawCollider++;
                        }
                    }

                    Image[] imgs = itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.BoxColliders].selecters;
                    foreach (Image img in imgs)
                        img.color = Color.white;

                    string option = settings.typeDrawCollider.ToString();
                    itemPanelDisplayInformation[(int)ItemPanelDisplayInformation.BoxColliders].description.text = option;
                    imgs[(int)settings.typeDrawCollider].color = Color.yellow;
                }
            }


            if (InputManager.GetButtonDown("Y") || InputManager.GetButtonDown("Start"))
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.CANCEL);
                currentPanelPauseFight = PanelPauseFight.Pause;
                panelDisplayInformation.SetActive(false);

                if (Engine.Initialization.Mode != CombatMode.Training)
                    panelPause.SetActive(true);
                else
                    panelPauseTrainner.SetActive(true);
            }
        }






        void UpdateItemPanelGaugeOption()
        {
            foreach (SelectItemPanelGaugeOption item in itemPanelGaugeOptions)
            {
                if (currentItemPanelGaugeOption == item.panelGaugeOption)
                    item.item.GetComponent<Image>().enabled = true;
                else
                    item.item.GetComponent<Image>().enabled = false;
            }

            if (InputCustom.PressUpPlayerIDOne() || InputCustom.PressDownPlayerIDOne() ||
                InputCustom.PressLeftPlayerIDOne() || InputCustom.PressRightPlayerIDOne())
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
            }

            if (currentItemPanelGaugeOption == ItemPanelGaugeOption.HPRecovery && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelGaugeOption = ItemPanelGaugeOption.PowerRecovery;
            }
            else if (currentItemPanelGaugeOption == ItemPanelGaugeOption.HPRecovery && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelGaugeOption = ItemPanelGaugeOption.P1HP;
            }
            else if (currentItemPanelGaugeOption == ItemPanelGaugeOption.P1HP && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelGaugeOption = ItemPanelGaugeOption.HPRecovery;
            }
            else if (currentItemPanelGaugeOption == ItemPanelGaugeOption.P1HP && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelGaugeOption = ItemPanelGaugeOption.P2HP;
            }
            else if (currentItemPanelGaugeOption == ItemPanelGaugeOption.P2HP && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelGaugeOption = ItemPanelGaugeOption.P1HP;
            }
            else if (currentItemPanelGaugeOption == ItemPanelGaugeOption.P2HP && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelGaugeOption = ItemPanelGaugeOption.PowerRecovery;
            }
            else if (currentItemPanelGaugeOption == ItemPanelGaugeOption.PowerRecovery && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelGaugeOption = ItemPanelGaugeOption.P2HP;
            }
            else if (currentItemPanelGaugeOption == ItemPanelGaugeOption.PowerRecovery && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelGaugeOption = ItemPanelGaugeOption.HPRecovery;
            }


            if (currentItemPanelGaugeOption == ItemPanelGaugeOption.HPRecovery)
            {
                if (InputCustom.PressLeftPlayerIDOne())
                {
                    if ((int)settings.hpRecovery > 0)
                    {
                        Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
                        settings.hpRecovery--;
                    }
                }
                else if (InputCustom.PressRightPlayerIDOne())
                {
                    if ((int)settings.hpRecovery < 2)
                    {
                        Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
                        settings.hpRecovery++;
                    }
                }

                Image[] imgs = itemPanelGaugeOptions[(int)ItemPanelGaugeOption.HPRecovery].selecters;
                foreach (Image img in imgs)
                    img.color = Color.white;

                string option = settings.hpRecovery.ToString();
                itemPanelGaugeOptions[(int)ItemPanelGaugeOption.HPRecovery].description.text = option;
                imgs[(int)settings.hpRecovery].color = Color.yellow;
            }
            else if (currentItemPanelGaugeOption == ItemPanelGaugeOption.P1HP)
            {
                if (InputCustom.PressLeftPlayerIDOne())
                {
                    if (settings.percentP1HPMax > 1)
                        settings.percentP1HPMax--;
                }
                else if (InputCustom.PressRightPlayerIDOne())
                {
                    if (settings.percentP1HPMax < 10)
                        settings.percentP1HPMax++;
                }

                Image[] imgs = itemPanelGaugeOptions[(int)ItemPanelGaugeOption.P1HP].selecters;
                foreach (Image img in imgs)
                    img.color = Color.white;

                string option = settings.percentP1HPMax * 10 + "%";
                itemPanelGaugeOptions[(int)ItemPanelGaugeOption.P1HP].description.text = option;
                imgs[settings.percentP1HPMax - 1].color = Color.yellow;
            }
            else if (currentItemPanelGaugeOption == ItemPanelGaugeOption.P2HP)
            {
                if (InputCustom.PressLeftPlayerIDOne())
                {
                    if (settings.percentP2HPMax > 1)
                        settings.percentP2HPMax--;
                }
                else if (InputCustom.PressRightPlayerIDOne())
                {
                    if (settings.percentP2HPMax < 10)
                        settings.percentP2HPMax++;
                }

                Image[] imgs = itemPanelGaugeOptions[(int)ItemPanelGaugeOption.P2HP].selecters;
                foreach (Image img in imgs)
                    img.color = Color.white;

                string option = settings.percentP2HPMax * 10 + "%";
                itemPanelGaugeOptions[(int)ItemPanelGaugeOption.P2HP].description.text = option;
                imgs[settings.percentP2HPMax - 1].color = Color.yellow;
            }
            if (currentItemPanelGaugeOption == ItemPanelGaugeOption.PowerRecovery)
            {
                if (InputCustom.PressLeftPlayerIDOne())
                {
                    if ((int)settings.powerRecovery > 0)
                        settings.powerRecovery--;
                }
                else if (InputCustom.PressRightPlayerIDOne())
                {
                    if ((int)settings.powerRecovery < 2)
                        settings.powerRecovery++;
                }

                Image[] imgs = itemPanelGaugeOptions[(int)ItemPanelGaugeOption.PowerRecovery].selecters;
                foreach (Image img in imgs)
                    img.color = Color.white;

                string option = settings.powerRecovery.ToString();
                itemPanelGaugeOptions[(int)ItemPanelGaugeOption.PowerRecovery].description.text = option;
                imgs[(int)settings.powerRecovery].color = Color.yellow;
            }


            if (InputManager.GetButtonDown("Y") || InputManager.GetButtonDown("Start"))
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.CANCEL);
                currentPanelPauseFight = PanelPauseFight.Pause;
                panelGaugeOption.SetActive(false);

                if (Engine.Initialization.Mode != CombatMode.Training)
                    panelPause.SetActive(true);
                else
                    panelPauseTrainner.SetActive(true);
            }

        }







        private void UpdateItemPanelActionEnemy()
        {
            foreach (SelectItemPanelActionEnemy item in itemPanelActionEnemy)
            {
                if (currentItemPanelActionEnemy == item.panelActionEnemy)
                    item.item.GetComponent<Image>().enabled = true;
                else
                    item.item.GetComponent<Image>().enabled = false;
            }

            if (InputCustom.PressUpPlayerIDOne() || InputCustom.PressDownPlayerIDOne() ||
                InputCustom.PressLeftPlayerIDOne() || InputCustom.PressRightPlayerIDOne())
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
            }

            if (currentItemPanelActionEnemy == ItemPanelActionEnemy.StateType && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelActionEnemy = ItemPanelActionEnemy.COMLevel;
            }
            else if (currentItemPanelActionEnemy == ItemPanelActionEnemy.StateType && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelActionEnemy = ItemPanelActionEnemy.Guard;
            }
            else if (currentItemPanelActionEnemy == ItemPanelActionEnemy.Guard && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelActionEnemy = ItemPanelActionEnemy.StateType;
            }
            else if (currentItemPanelActionEnemy == ItemPanelActionEnemy.Guard && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelActionEnemy = ItemPanelActionEnemy.GuardTime;
            }
            else if (currentItemPanelActionEnemy == ItemPanelActionEnemy.GuardTime && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelActionEnemy = ItemPanelActionEnemy.Guard;
            }
            else if (currentItemPanelActionEnemy == ItemPanelActionEnemy.GuardTime && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelActionEnemy = ItemPanelActionEnemy.Teching;
            }
            else if (currentItemPanelActionEnemy == ItemPanelActionEnemy.Teching && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelActionEnemy = ItemPanelActionEnemy.GuardTime;
            }
            else if (currentItemPanelActionEnemy == ItemPanelActionEnemy.Teching && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelActionEnemy = ItemPanelActionEnemy.COMLevel;
            }
            else if (currentItemPanelActionEnemy == ItemPanelActionEnemy.COMLevel && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelActionEnemy = ItemPanelActionEnemy.Teching;
            }
            else if (currentItemPanelActionEnemy == ItemPanelActionEnemy.COMLevel && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelActionEnemy = ItemPanelActionEnemy.StateType;
            }


            if (currentItemPanelActionEnemy == ItemPanelActionEnemy.StateType)
            {
                if (InputCustom.PressLeftPlayerIDOne())
                {
                    if ((int)settings.stanceType > 0)
                        settings.stanceType--;
                }
                else if (InputCustom.PressRightPlayerIDOne())
                {
                    if ((int)settings.stanceType < 4)
                        settings.stanceType++;
                }

                if (settings.stanceType == StanceType.COM)
                    Engine.Team2.MainPlayer.PlayerMode = PlayerMode.Ai;
                else
                    Engine.Team2.MainPlayer.PlayerMode = PlayerMode.Human;

                Image[] imgs = itemPanelActionEnemy[(int)ItemPanelActionEnemy.StateType].selecters;
                foreach (Image img in imgs)
                    img.color = Color.white;

                itemPanelActionEnemy[(int)ItemPanelActionEnemy.StateType].description.text = settings.stanceType.ToString();
                imgs[(int)settings.stanceType].color = Color.yellow;
            }
            else if (currentItemPanelActionEnemy == ItemPanelActionEnemy.Guard)
            {
                if (InputCustom.PressLeftPlayerIDOne())
                {
                    if ((int)settings.guard > 0)
                        settings.guard--;
                }
                else if (InputCustom.PressRightPlayerIDOne())
                {
                    if ((int)settings.guard < 2)
                        settings.guard++;
                }

                Image[] imgs = itemPanelActionEnemy[(int)ItemPanelActionEnemy.Guard].selecters;
                foreach (Image img in imgs)
                    img.color = Color.white;

                itemPanelActionEnemy[(int)ItemPanelActionEnemy.Guard].description.text = settings.guard.ToName();
                imgs[(int)settings.guard].color = Color.yellow;
            }
            else if (currentItemPanelActionEnemy == ItemPanelActionEnemy.GuardTime)
            {
                if (InputCustom.PressLeftPlayerIDOne())
                {
                    if ((int)settings.guardTime > 0)
                        settings.guardTime--;
                }
                else if (InputCustom.PressRightPlayerIDOne())
                {
                    if ((int)settings.guardTime < 3)
                        settings.guardTime++;
                }

                Image[] imgs = itemPanelActionEnemy[(int)ItemPanelActionEnemy.GuardTime].selecters;
                foreach (Image img in imgs)
                    img.color = Color.white;

                itemPanelActionEnemy[(int)ItemPanelActionEnemy.GuardTime].description.text = settings.guardTime.ToName();
                imgs[(int)settings.guardTime].color = Color.yellow;
            }
            else if (currentItemPanelActionEnemy == ItemPanelActionEnemy.Teching)
            {
                if (InputCustom.PressLeftPlayerIDOne())
                {
                    if ((int)settings.teching > 0)
                        settings.teching--;
                }
                else if (InputCustom.PressRightPlayerIDOne())
                {
                    if ((int)settings.teching < 2)
                        settings.teching++;
                }

                Image[] imgs = itemPanelActionEnemy[(int)ItemPanelActionEnemy.Teching].selecters;
                foreach (Image img in imgs)
                    img.color = Color.white;

                itemPanelActionEnemy[(int)ItemPanelActionEnemy.Teching].description.text = settings.teching.ToString();
                imgs[(int)settings.teching].color = Color.yellow;
            }
            else if (currentItemPanelActionEnemy == ItemPanelActionEnemy.COMLevel)
            {
                if (InputCustom.PressLeftPlayerIDOne())
                {
                    if (settings.COMLevel > 1)
                        settings.COMLevel--;
                }
                else if (InputCustom.PressRightPlayerIDOne())
                {
                    if (settings.COMLevel < 8)
                        settings.COMLevel++;
                }

                Image[] imgs = itemPanelActionEnemy[(int)ItemPanelActionEnemy.COMLevel].selecters;
                foreach (Image img in imgs)
                    img.color = Color.white;

                itemPanelActionEnemy[(int)ItemPanelActionEnemy.COMLevel].description.text = settings.COMLevel.ToString();
                imgs[settings.COMLevel - 1].color = Color.yellow;
            }


            if (InputManager.GetButtonDown("Y") || InputManager.GetButtonDown("Start"))
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.CANCEL);
                currentPanelPauseFight = PanelPauseFight.Pause;
                panelActionEnemy.SetActive(false);

                if (Engine.Initialization.Mode != CombatMode.Training)
                    panelPause.SetActive(true);
                else
                    panelPauseTrainner.SetActive(true);
            }
        }








        Dictionary<int, string> charNames;
        Dictionary<int, string> stageNames;
        Dictionary<int, string> BMGNames;

        int currentItemPanelQuickCharChangeP1Character;
        int currentItemPanelQuickCharChangeP2Character;
        int currentItemPanelQuickCharChangeStage;
        int currentItemPanelQuickCharChangeBMG;

        void CreateSelectorP1Character()
        {
            Image[] selecters = itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.P1Character].selecters;

            for (int i = 0; i < charNames.Count - 1; i++)
            {
                GameObject go = Instantiate(selecters[0].gameObject);
                go.transform.SetParent(selecters[0].gameObject.transform.parent, false);
                itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.P1Character].selecters
                    = itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.P1Character].selecters
                    .Add(go.GetComponent<Image>());
            }
        }

        void CreateSelectorP2Character()
        {
            Image[] selecters = itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.P2Character].selecters;

            for (int i = 0; i < charNames.Count - 1; i++)
            {
                GameObject go = Instantiate(selecters[0].gameObject);
                go.transform.SetParent(selecters[0].gameObject.transform.parent, false);
                itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.P2Character].selecters
                    = itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.P2Character].selecters
                    .Add(go.GetComponent<Image>());
            }
        }

        void CreateSelectorStage()
        {
            Image[] selecters = itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.Stage].selecters;

            for (int i = 0; i < stageNames.Count - 1; i++)
            {
                GameObject go = Instantiate(selecters[0].gameObject);
                go.transform.SetParent(selecters[0].gameObject.transform.parent, false);
                itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.Stage].selecters
                    = itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.Stage].selecters
                    .Add(go.GetComponent<Image>());
            }
        }

        void CreateSelectorBMG()
        {
            Image[] selecters = itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.BMG].selecters;

            for (int i = 0; i < BMGNames.Count - 1; i++)
            {
                GameObject go = Instantiate(selecters[0].gameObject);
                go.transform.SetParent(selecters[0].gameObject.transform.parent, false);
                itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.BMG].selecters
                    = itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.BMG].selecters
                    .Add(go.GetComponent<Image>());
            }
        }

        void QuickChangeButton()
        {
            EngineInitialization initialization = Launcher.engineInitialization;

            initialization.Team1Mode = TeamMode.Single;
            initialization.Team2Mode = TeamMode.Single;

            initialization.Team1.Clear();
            initialization.Team2.Clear();

            PlayerProfileManager charP1 = Launcher.profileLoader.profiles[currentItemPanelQuickCharChangeP1Character];
            PlayerCreation create1 = new PlayerCreation(charP1, charP1.palettesIndex[0], PlayerMode.Human);
            initialization.Team1.Add(create1);

            PlayerProfileManager charP2 = Launcher.profileLoader.profiles[currentItemPanelQuickCharChangeP2Character];
            PlayerCreation create2 = new PlayerCreation(charP2, charP2.palettesIndex[1], PlayerMode.Human);
            initialization.Team2.Add(create2);

            initialization.musicID = currentItemPanelQuickCharChangeBMG;
            initialization.stageID = currentItemPanelQuickCharChangeStage;

            Launcher.engineInitialization.SetSeed();

            ////LoadScene
            GameObject init = new GameObject();
            init.name = "LoadSceneCustom";
            init.hideFlags = HideFlags.HideInHierarchy;
            string stageName = Launcher.profileLoader.stageProfiles[currentItemPanelQuickCharChangeStage].Name;
            new LoadBattleScene().Iniciar(stageName, Color.black, 2.5f, true);
        }

        void UpdateItemPanelQuickCharChange()
        {
            foreach (SelectItemPanelQuickCharChange item in itemPanelQuickCharChange)
            {
                if (currentItemPanelQuickCharChange == item.panelQuickCharChange)
                    item.item.GetComponent<Image>().enabled = true;
                else
                    item.item.GetComponent<Image>().enabled = false;
            }

            if (InputCustom.PressUpPlayerIDOne() || InputCustom.PressDownPlayerIDOne() ||
                InputCustom.PressLeftPlayerIDOne() || InputCustom.PressRightPlayerIDOne())
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
            }

            if (currentItemPanelQuickCharChange == ItemPanelQuickCharChange.P1Character && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelQuickCharChange = ItemPanelQuickCharChange.ApplyChange;
            }
            else if (currentItemPanelQuickCharChange == ItemPanelQuickCharChange.P1Character && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelQuickCharChange = ItemPanelQuickCharChange.P2Character;
            }
            else if (currentItemPanelQuickCharChange == ItemPanelQuickCharChange.P2Character && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelQuickCharChange = ItemPanelQuickCharChange.P1Character;
            }
            else if (currentItemPanelQuickCharChange == ItemPanelQuickCharChange.P2Character && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelQuickCharChange = ItemPanelQuickCharChange.Stage;
            }
            else if (currentItemPanelQuickCharChange == ItemPanelQuickCharChange.Stage && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelQuickCharChange = ItemPanelQuickCharChange.P2Character;
            }
            else if (currentItemPanelQuickCharChange == ItemPanelQuickCharChange.Stage && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelQuickCharChange = ItemPanelQuickCharChange.BMG;
            }
            else if (currentItemPanelQuickCharChange == ItemPanelQuickCharChange.BMG && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelQuickCharChange = ItemPanelQuickCharChange.Stage;
            }
            else if (currentItemPanelQuickCharChange == ItemPanelQuickCharChange.BMG && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelQuickCharChange = ItemPanelQuickCharChange.ApplyChange;
            }
            else if (currentItemPanelQuickCharChange == ItemPanelQuickCharChange.ApplyChange && InputCustom.PressUpPlayerIDOne())
            {
                currentItemPanelQuickCharChange = ItemPanelQuickCharChange.BMG;
            }
            else if (currentItemPanelQuickCharChange == ItemPanelQuickCharChange.ApplyChange && InputCustom.PressDownPlayerIDOne())
            {
                currentItemPanelQuickCharChange = ItemPanelQuickCharChange.P1Character;
            }


            if (currentItemPanelQuickCharChange == ItemPanelQuickCharChange.P1Character)
            {
                if (InputCustom.PressLeftPlayerIDOne())
                {
                    if (currentItemPanelQuickCharChangeP1Character > 0)
                        currentItemPanelQuickCharChangeP1Character--;
                }
                else if (InputCustom.PressRightPlayerIDOne())
                {
                    if (currentItemPanelQuickCharChangeP1Character < charNames.Count - 1)
                        currentItemPanelQuickCharChangeP1Character++;
                }

                Image[] imgs = itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.P1Character].selecters;
                foreach (Image img in imgs)
                    img.color = Color.white;

                charNames.TryGetValue(currentItemPanelQuickCharChangeP1Character, out string option);
                itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.P1Character].description.text = option;
                imgs[currentItemPanelQuickCharChangeP1Character].color = Color.yellow;
            }
            else if (currentItemPanelQuickCharChange == ItemPanelQuickCharChange.P2Character)
            {
                if (InputCustom.PressLeftPlayerIDOne())
                {
                    if (currentItemPanelQuickCharChangeP2Character > 0)
                        currentItemPanelQuickCharChangeP2Character--;
                }
                else if (InputCustom.PressRightPlayerIDOne())
                {
                    if (currentItemPanelQuickCharChangeP2Character < charNames.Count - 1)
                        currentItemPanelQuickCharChangeP2Character++;
                }

                Image[] imgs = itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.P2Character].selecters;
                foreach (Image img in imgs)
                    img.color = Color.white;

                charNames.TryGetValue(currentItemPanelQuickCharChangeP2Character, out string option);
                itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.P2Character].description.text = option;
                imgs[currentItemPanelQuickCharChangeP2Character].color = Color.yellow;
            }
            else if (currentItemPanelQuickCharChange == ItemPanelQuickCharChange.Stage)
            {
                if (InputCustom.PressLeftPlayerIDOne())
                {
                    if (currentItemPanelQuickCharChangeStage > 0)
                        currentItemPanelQuickCharChangeStage--;
                }
                else if (InputCustom.PressRightPlayerIDOne())
                {
                    if (currentItemPanelQuickCharChangeStage < stageNames.Count - 1)
                        currentItemPanelQuickCharChangeStage++;
                }

                Image[] imgs = itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.Stage].selecters;
                foreach (Image img in imgs)
                    img.color = Color.white;

                stageNames.TryGetValue(currentItemPanelQuickCharChangeStage, out string option);
                itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.Stage].description.text = option;
                imgs[currentItemPanelQuickCharChangeStage].color = Color.yellow;
            }
            else if (currentItemPanelQuickCharChange == ItemPanelQuickCharChange.BMG)
            {
                if (InputCustom.PressLeftPlayerIDOne())
                {
                    if (currentItemPanelQuickCharChangeBMG > 0)
                        currentItemPanelQuickCharChangeBMG--;
                }
                else if (InputCustom.PressRightPlayerIDOne())
                {
                    if (currentItemPanelQuickCharChangeBMG < BMGNames.Count - 1)
                        currentItemPanelQuickCharChangeBMG++;
                }

                Image[] imgs = itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.BMG].selecters;
                foreach (Image img in imgs)
                    img.color = Color.white;

                BMGNames.TryGetValue(currentItemPanelQuickCharChangeBMG, out string option);
                itemPanelQuickCharChange[(int)ItemPanelQuickCharChange.BMG].description.text = option;
                imgs[currentItemPanelQuickCharChangeBMG].color = Color.yellow;
            }
            else if (currentItemPanelQuickCharChange == ItemPanelQuickCharChange.ApplyChange)
            {
                if (InputManager.GetButtonDown("X"))
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
                    currentPanelPauseFight = PanelPauseFight.SelectConfirm;
                    panelQuickCharChange.SetActive(false);
                    panelSelectConfirm.SetActive(true);
                }
            }

            if (InputManager.GetButtonDown("Y") || InputManager.GetButtonDown("Start"))
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.CANCEL);
                currentPanelPauseFight = PanelPauseFight.Pause;
                panelQuickCharChange.SetActive(false);

                if (Engine.Initialization.Mode != CombatMode.Training)
                    panelPause.SetActive(true);
                else
                    panelPauseTrainner.SetActive(true);
            }
        }


        public void Back()
        {
            InputCustom.ActiveUpdate = false;
            Launcher.mugen.Pause = UnityMugen.PauseState.Unpaused;
            currentItemPanelPause = ItemPanelPauseTrainner.Continue;
            //UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("HudPauseFight");
            enabled = false;

            Launcher.soundSystem.ControlVolumeMusic(1);
            LauncherEngine.Inst.soundSystem.UnPauseSounds();
        }


        private void UpdatePanelSelectConfirm()
        {
            if (InputManager.GetButtonDown("X"))
            {
                if (currentItemPanelPause == ItemPanelPauseTrainner.SelectPlayersScreen)
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
                    StartCoroutine(Launcher.soundSystem.FadeOutMusic(Constant.TimeFadeOutMusic));
                    LoadSceneCustom.LoadScene("SelectScreen");
                }
                else if (currentItemPanelPause == ItemPanelPauseTrainner.MainMenu)
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
                    StartCoroutine(Launcher.soundSystem.FadeOutMusic(Constant.TimeFadeOutMusic));
                    LoadSceneCustom.LoadScene("MenuScreen");
                }
                else if (currentItemPanelPause == ItemPanelPauseTrainner.QuickSelect)
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
                    QuickChangeButton();
                }
            }
            if (InputManager.GetButtonDown("Y") || InputManager.GetButtonDown("Start"))
            {
                if (currentItemPanelPause != ItemPanelPauseTrainner.QuickSelect)
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.CANCEL);
                    currentPanelPauseFight = PanelPauseFight.Pause;

                    panelSelectConfirm.SetActive(false);

                    if (Engine.Initialization.Mode != CombatMode.Training)
                        panelPause.SetActive(true);
                    else
                        panelPauseTrainner.SetActive(true);
                }
                else
                {
                    currentPanelPauseFight = PanelPauseFight.QuickCharChange;
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.CANCEL);
                    panelQuickCharChange.SetActive(true);
                    panelSelectConfirm.SetActive(false);
                }
            }
        }

    }
}