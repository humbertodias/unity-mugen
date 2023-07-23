using System;
using UnityEngine;
using UnityEngine.UI;
using UnityMugen.Combat;
using UnityMugen.Combat.Logic;
using UnityMugen.CustomInput;

namespace UnityMugen.Screens
{

    public class Mugen : MonoBehaviour
    {
        public static LauncherEngine Launcher => LauncherEngine.Inst;
        public FightEngine Engine;

        public bool BattleActive { get; set; }

        [NonSerialized] public PauseState Pause = PauseState.Paused;

        public GameObject testeButtomConect;
        public Text testeFrames;


        void Start()
        {
            Launcher.screenType = ScreenType.Combat;

            if (Engine == null)
                Engine = new FightEngine();
        }

        public void FadeOutMusic()
        {
            StartCoroutine(Launcher.soundSystem.FadeOutMusic(Constant.TimeFadeOutMusic));
        }

        public void FadeOutComplete()
        {
            //	Recorder.EndRecording();
            Launcher.soundSystem.StopAllSounds();
        }

        void Update()
        {
            DoUpdate();

            //DoUpdateGGPO();
        }



        public void DoUpdate()
        {
            if (Pause == PauseState.Unpaused || Pause == PauseState.PauseStep)
            {

                //Engine.Team1.MainPlayer.m_currentinput = Launcher.inputSystem.playerButton1Newtwork;
                //Engine.Team2.MainPlayer.m_currentinput = Launcher.inputSystem.playerButton2Newtwork;

                Engine.Team1.MainPlayer.CurrentInput = Launcher.inputSystem.KeyboardStateP1();
                Engine.Team2.MainPlayer.CurrentInput = Launcher.inputSystem.KeyboardStateP2();

                Engine.stageScreen.HCBU.RecordUpdate();

                Engine.UpdateFE();
                Engine.Draw(true);

                DeleteEntityInused();

                Engine.stageScreen.HCBU.UpdateFE();

                if (Launcher.inputSystem.isControllersActives && UnityEngine.Input.GetKeyDown(KeyCode.L))
                {
                    Engine.stageScreen.HCBU.teamLifeBarUnity.ResetDamage();
                }
                else if (Launcher.inputSystem.isControllersActives && UnityEngine.Input.GetKeyDown(KeyCode.O))
                {
                    Engine.stageScreen.HCBU.teamLifeBarUnity.LifeZero();
                }
                else if (Launcher.inputSystem.isControllersActives && UnityEngine.Input.GetKeyDown(KeyCode.P))
                {
                    Engine.stageScreen.HCBU.powerBarUnity.FullPowerBar();
                }
                else if (Launcher.inputSystem.isControllersActives && 
                    UnityEngine.Input.GetKeyDown(KeyCode.Return) &&
                    Engine.Initialization.Mode == CombatMode.Training)
                {
                    Engine.ResetTrainner();
                    //Engine.m_logic = new ShowWinPose();
                    //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    //LoadStageBttle(SceneManager.GetActiveScene().name);
                    //Pause = PauseState.Paused;
                }
                else if (Launcher.inputSystem.isControllersActives && UnityEngine.Input.GetKeyDown(KeyCode.I))
                {
                    if (Engine.Team2.MainPlayer != null)
                        Engine.Team2.MainPlayer.Life = Engine.Team2.MainPlayer.playerConstants.MaximumLife;
                    if (Engine.Team2.TeamMate != null)
                        Engine.Team2.TeamMate.Life = Engine.Team2.TeamMate.playerConstants.MaximumLife;
                }
            }

            if (InputManager.GetButtonDown("Start") &&
                (Engine.m_logic is UnityMugen.Combat.Logic.Fighting) &&
                (Pause != UnityMugen.PauseState.Paused))
            {
                InputCustom.ActiveUpdate = true;
                Pause = UnityMugen.PauseState.Paused;
                //SceneManager.LoadSceneAsync("HudPauseFight", LoadSceneMode.Additive);
                Engine.stageScreen.PauseFight.enabled = true;

                Engine.stageScreen.MoveLists.namesChar.Clear();
                Engine.stageScreen.MoveLists.namesChar.Add(Engine.Team1.MainPlayer.profile.charName);
                Engine.stageScreen.MoveLists.namesChar.Add(Engine.Team2.MainPlayer.profile.charName);

                Launcher.soundSystem.ControlVolumeMusic(.25f);
                Launcher.soundSystem.PauseSounds();
            }


        }

        private void LoadStageBttle(string stageName)
        {
            GameObject init = new GameObject();
            init.name = "LoadSceneCustom";
            init.hideFlags = HideFlags.HideInHierarchy;
            new LoadBattleScene().Iniciar(stageName, Color.black, 2.5f, true);
        }

        private void DeleteEntityInused()
        {
            GameObject[] entitys = GameObject.FindGameObjectsWithTag("Entity");
            foreach (GameObject go in entitys)
            {
                Entity entity = go.GetComponent<Entity>();

                bool notContains = false;
                try
                {
                    notContains = !Engine.Entities.Contains(entity);
                }
                catch (ArgumentNullException)
                {
                    notContains = true;
                }

                if (notContains)
                {
                    if (entity != null && entity is Player)
                    {
                        Player p = (entity as Player);
                        if (p.iniciado)
                        {
                            if (p.reflection != null)
                                Destroy(p.reflection.gameObject);

                            if (p.shadown != null)
                                Destroy(p.shadown.gameObject);

                            Destroy(p.gameObject);
                        }
                    }
                    else if (entity != null)
                    {
                        if (entity.reflection != null)
                            Destroy(entity.reflection.gameObject);

                        if (entity.shadown != null)
                            Destroy(entity.shadown.gameObject);

                        Destroy(entity.gameObject);
                    }
                    Destroy(go);
                }
            }
        }

    }
}