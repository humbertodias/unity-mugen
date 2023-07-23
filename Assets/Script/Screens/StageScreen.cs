using System;
using System.Collections.Generic;
using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.CustomInput;
using UnityMugen.Interface;

namespace UnityMugen.Screens
{
    public class StageScreen : MonoBehaviour, IStageScreen
    {
        static LauncherEngine Launcher => LauncherEngine.Inst;
        FightEngine Engine => Launcher.mugen.Engine;

        HudCanvasBattleUnity m_hcbu;
        public HudCanvasBattleUnity HCBU
        {
            get { return m_hcbu; }
        }

        HudPauseFight m_pauseFight;
        public HudPauseFight PauseFight
        {
            get { return m_pauseFight; }
        }

        HudMoveLists m_moveLists;
        public HudMoveLists MoveLists
        {
            get { return m_moveLists; }
        }

        CanvasFull m_canvasFull;
        public CanvasFull CanvasFull
        {
            get { return m_canvasFull; }
        }

        public GameObject m_foreground;
        public GameObject Foreground
        {
            get { return m_foreground; }
        }

        public GameObject m_background;
        public GameObject Background
        {
            get { return m_background; }
        }

        public Stage m_stage;
        public Stage Stage
        {
            get { return m_stage; }
        }

        public StageActions m_stageActions;
        public StageActions StageActions
        {
            get { return m_stageActions; }
        }

        Transform m_entities;
        public Transform Entities
        {
            get { return m_entities; }
        }

        Transform m_shadowns;
        public Transform Shadowns
        {
            get { return m_shadowns; }
        }

        Transform m_reflections;
        public Transform Reflections
        {
            get { return m_reflections; }
        }

        Transform m_afterImages;
        public Transform AfterImages
        {
            get { return m_afterImages; }
        }

        void Start()
        {
            m_hcbu = GameObject.Find("HudCanvasBattle")?.GetComponent<HudCanvasBattleUnity>();
            m_pauseFight = GameObject.Find("CanvasPauseFight")?.GetComponent<HudPauseFight>();
            m_moveLists = GameObject.Find("HudMoveLists")?.GetComponent<HudMoveLists>();

            CameraFE camFE = GameObject.Find("Main Camera").GetComponent<CameraFE>();

            m_entities = new GameObject("Entities").transform;
            m_shadowns = new GameObject("Shadowns").transform;
            m_reflections = new GameObject("Reflections").transform;
            m_afterImages = new GameObject("AfterImages").transform;

            Engine.RoundInformation = HCBU.roundInformation;
            Engine.CameraFE = camFE;
            Engine.CameraFE.Initialize(Stage);

            Canvas canvas = GameObject.Find("CanvasBack")?.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
            canvas.planeDistance = 1;
            canvas.sortingLayerName = "CanvasTop";

            Canvas canvasfull = GameObject.Find("CanvasFull")?.GetComponent<Canvas>();
            m_canvasFull = canvasfull.gameObject.AddComponent<CanvasFull>();
            canvasfull.worldCamera = Camera.main;
            canvasfull.planeDistance = 1;
            canvasfull.sortingLayerName = "Entity";

            InputCustom.ActiveUpdate = false;

            Launcher.mugen.Engine.stageScreen = (IStageScreen)this;
            Launcher.mugen.Pause = PauseState.Unpaused;

            HCBU.StartFE(Launcher.engineInitialization.Mode);

            //Engine.stageScreen.stage = init.Stage;
            //stage.stageActions = GameObject.FindGameObjectWithTag("Stage").GetComponent<StageActions>();

            Engine.Set(Launcher.engineInitialization);
            Launcher.mugen.BattleActive = true;
        }

    }
}