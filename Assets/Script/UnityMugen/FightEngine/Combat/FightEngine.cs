using System;
using UnityEngine;
using UnityMugen.Animations;
using UnityMugen.Audio;
using UnityMugen.Combat.Logic;
using UnityMugen.Drawing;
using UnityMugen.Interface;
using UnityMugen.Screens;

namespace UnityMugen.Combat
{

    public class FightEngine
    {
        public static LauncherEngine Launcher => LauncherEngine.Inst;

        public EntityCollection Entities;
        public Pause Pause;
        public Pause SuperPause;
        public CombatChecker m_combatcheck;
        public RoundInformation RoundInformation;
        public Team Team1;
        public Team Team2;
        public Clock Clock;
        public CameraFE CameraFE;
        //    private fightEngine.Audio.SoundManager FightSounds;
        public SoundManager CommonSounds;
        public int MatchNumber { get; set; }
        public int RoundNumber;
        public int DrawGames { get; set; }
        public int TickCount { get; set; }

        public IStageScreen stageScreen;
        public EngineInitialization Initialization { get; private set; }
        public EngineAssertions Assertions;
        public EnvironmentColor EnvironmentColor;
        public EnvironmentShake EnvironmentShake;
        public GameSpeed Speed;
        //    public fightEngine.Elements.Collection Elements;
        public SpriteManager FxSprites;
        public AnimationManager FxAnimations;
        //public List<Texture2D> FxPalettes;
        public PaletteList PaletteListFX;
        private AnimationManager FightAnimations;

        public RoundState RoundState => m_logic == null ? RoundState.None : m_logic.State;
        private int m_slowspeedbuffer;
        public Base m_logic;
        private int m_idcounter;

        public FightEngine()
        {
            Entities = new EntityCollection();
            m_combatcheck = new CombatChecker();
            Pause = new Pause(false);
            SuperPause = new Pause(true);
            Assertions = new EngineAssertions();
            EnvironmentColor = new EnvironmentColor();
            EnvironmentShake = new EnvironmentShake();
            Speed = GameSpeed.Normal;

            //   FightAnimations = launcherEngine.animationSystem.CreateManager(textfile.Filepath);
            //    FE.Elements = new fightEngine.Elements.Collection(/*FightSprites, FightAnimations,*/ FE.FightSounds/*, Fonts*/);
            // new RoundInformation(textfile);
            //    FE.FightSounds = FE.RoundInformation.FightSound();
            CommonSounds = Launcher.soundSystem.CreateManager(Application.streamingAssetsPath + "/Data/common.snd");

            Team1 = new Team(TeamSide.Left);
            Team2 = new Team(TeamSide.Right);

            m_logic = new PreIntro();
            Clock = new Clock();
            //    FE.CameraFE = new CameraFE(FE);

            FxSprites = Launcher.spriteSystem.CreateManager(Application.streamingAssetsPath + "/Data/FxPalette.sff"/*Launcher.fxSprites*/);
            FxAnimations = Launcher.animationSystem.CreateManager(Application.streamingAssetsPath + "/Data/fightfx.air"/*Launcher.fxAnimations*/, Vector2.one);
            //FxPalettes = Launcher.paletteSystem.BuildPalettes(Launcher.fxPalette);
            /*FxPalettes = */
            Launcher.paletteSystem.LoadPalette(Application.streamingAssetsPath + "/Data/FxPalette.sff");
            PaletteListFX = Launcher.paletteSystem.LoadPalette(Application.streamingAssetsPath + "/Data/FxPalette.sff");
        }


        public void UpdateFE()
        {

            if (SpeedSkip() == false) return;

            UpdatePauses();

            //    Elements.UpdateFE();

            if(Initialization.Mode != CombatMode.Training)
                UpdateLogic();
            else
                UpdateLogicTraining();

            Assertions.UpdateFE();

            stageScreen.Stage.PaletteFx.UpdateFE();
            stageScreen.CanvasFull.UpdateFE();
            //if (Pause.IsPaused(Stage) == false && SuperPause.IsPaused(Stage) == false) 
            //    Stage.UpdateFE();

            EnvironmentColor.Update();

            Entities.UpdateFE();
            m_combatcheck.Run();

            Team1.ComboCounter.UpdateFE();
            Team2.ComboCounter.UpdateFE();


            CameraFE.UpdateFE();
            EnvironmentShake.UpdateFE();

            ++TickCount;
        }

        public void ResetFE(bool showIntro = true)
        {
            if(showIntro)
                m_logic = new PreIntro();
    
            m_slowspeedbuffer = 0;
            TickCount = 0;
            RoundNumber = 1;

            Speed = GameSpeed.Normal;


            var init = Launcher.engineInitialization;
            //    Team1.CreatePlayers(init.Team1Mode, init.Team1, PlayerID.One);
            //    Team2.CreatePlayers(init.Team2Mode, init.Team2, PlayerID.Two);
            //Team1.MainPlayer.SetLocalAnimation(0, 0);
            //Team2.MainPlayer.SetLocalAnimation(0, 0);
            Team1.ResetPlayers();
            Team2.ResetPlayers();

            //    FightSounds.Stop();
            CommonSounds.Stop();

            //    Stage.ResetFE();
            CameraFE.ResetFE();
            Pause.ResetFE();
            SuperPause.ResetFE();
            Assertions.ResetFE();
            EnvironmentColor.Reset();
            EnvironmentShake.ResetFE();
            //    Elements.ResetFE();
        }

        public void ResetTrainner()
        {
            

            m_logic = new ShowTransition(TypeShowTransition.IN);

            Team1.MainPlayer.PlayerControl = PlayerControl.InControl;
            Team2.MainPlayer.PlayerControl = PlayerControl.InControl;
        }

        public void Set(EngineInitialization init)
        {
            if (init == null) throw new ArgumentNullException(nameof(init));

            Initialization = init;
            m_idcounter = 0;

            Team1.CreatePlayers(init.Team1Mode, init.Team1, PlayerID.One, stageScreen.Entities);
            Team2.CreatePlayers(init.Team2Mode, init.Team2, PlayerID.Two, stageScreen.Entities);

            Launcher.random.Seed(init.Seed);

            ResetFE();
        }

        private void PlayerRestore(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));

            player.Life = player.playerConstants.MaximumLife;
            player.Power = player.playerConstants.MaximumPower;
        }

        private void RestoreLifeAndPower(bool pressed)
        {
            if (pressed)
            {
                if (RoundState == RoundState.Fight)
                {
                    Clock.Time = Launcher.initializationSettings.RoundLength;

                    Team1.DoAction(PlayerRestore);
                    Team2.DoAction(PlayerRestore);
                }
            }
        }

        private void SetPlayerLifeTo(bool pressed, Character character, int life)
        {
            if (!pressed) return;
            if (RoundState != RoundState.Fight) return;
            character.Life = life;
        }

        private void TimeOver(bool pressed)
        {
            if (!pressed) return;
            if (RoundState != RoundState.Fight) return;
            Clock.Time = 0;
        }

        public int GenerateCharacterId()
        {
            return m_idcounter++;
        }

        //public void SetInput(InputState inputstate)
        //{
        //if (inputstate == null) throw new ArgumentNullException(nameof(inputstate));

        //foreach (PlayerButton button in Enum.GetValues(typeof(PlayerButton)))
        //{
        //    var b = button;

        //    inputstate[1].Add(b, x => Team1.MainPlayer.RecieveInput(b, x));
        //    inputstate[2].Add(b, x => Team2.MainPlayer.RecieveInput(b, x));
        //}

        //inputstate[0].Add(SystemButton.FullLifeAndPower, RestoreLifeAndPower);
        //inputstate[0].Add(SystemButton.SetPlayer1LifeToZero, p => SetPlayerLifeTo(p, Team1.MainPlayer, 0));
        //inputstate[0].Add(SystemButton.SetPlayer2LifeToZero, p => SetPlayerLifeTo(p, Team2.MainPlayer, 0));
        //inputstate[0].Add(SystemButton.SetBothPlayersLifeToOne, p =>
        //{
        //    SetPlayerLifeTo(p, Team1.MainPlayer, 1);
        //    SetPlayerLifeTo(p, Team2.MainPlayer, 1);
        //});
        //inputstate[0].Add(SystemButton.TimeOver, TimeOver);
        //inputstate[0].Add(SystemButton.TestCheat, Tester);
        //}

        private bool SpeedSkip()
        {
            if (Speed == GameSpeed.Slow)
            {
                ++m_slowspeedbuffer;

                if (m_slowspeedbuffer < 3) return false;
            }

            m_slowspeedbuffer = 0;
            return true;
        }


        private void UpdatePauses()
        {
            if (SuperPause.IsActive)
            {
                SuperPause.UpdateFE();
            }
            else
            {
                Pause.UpdateFE();
            }
        }

        private void UpdateLogic()
        {
            m_logic.UpdateFE();

            if (m_logic.IsFinished())
            {
                if (m_logic is PreIntro)
                {
                    m_logic = new ShowTransition(TypeShowTransition.OUT);
                }
                else if (m_logic is ShowTransition && (m_logic as ShowTransition).typeShowTransition == TypeShowTransition.OUT)
                {
                    m_logic = new ShowCharacterIntro();
                }
                else if (m_logic is ShowCharacterIntro)
                {
                    m_logic = new DisplayRoundNumber();
                }
                else if (m_logic is DisplayRoundNumber)
                {
                    m_logic = new ShowFight();
                }
                else if (m_logic is ShowFight)
                {
                    m_logic = new Fighting();
                }
                else if (m_logic is Fighting)
                {
                    m_logic = new CombatOver();
                }
                else if (m_logic is CombatOver)
                {
                    m_logic = new ShowWinPose();
                }
                else if (m_logic is ShowWinPose)
                {

                    if (Team1.Wins.Count >= Launcher.initializationSettings.NumberOfRounds ||
                        Team2.Wins.Count >= Launcher.initializationSettings.NumberOfRounds)
                    {
                        if (Initialization.Mode == CombatMode.Story)
                        {

                        }
                        else if (Initialization.Mode == CombatMode.Survival)
                        {

                        }
                        else if (Initialization.Mode == CombatMode.Training)
                        {
                            Launcher.soundSystem.StopAllSounds();
                            RoundInformation.StopAllSounds();

                            GameObject init = new GameObject();
                            init.name = "LoadSceneCustom";
                            init.hideFlags = HideFlags.HideInHierarchy;
                            init.AddComponent<LoadSceneCustom>();
                            LoadSceneCustom scr = init.GetComponent<LoadSceneCustom>();
                            scr.Inicialize("VersusScreen", Color.black, 2.5f, true);
                            Launcher.mugen.FadeOutMusic();
                            m_logic = new NoMoreFighting();
                            return;
                        }
                        else if (Initialization.Mode == CombatMode.Arcade)
                        {
                            RoundNumber = 1;
                            MatchNumber++;
                            Launcher.mugen.Pause = UnityMugen.PauseState.Paused;
                            Launcher.soundSystem.StopAllSounds();
                            RoundInformation.StopAllSounds();

                            GameObject init = new GameObject();
                            init.name = "LoadSceneCustom";
                            init.hideFlags = HideFlags.HideInHierarchy;
                            init.AddComponent<LoadSceneCustom>();
                            LoadSceneCustom scr = init.GetComponent<LoadSceneCustom>();

                            if (MatchNumber == Launcher.profileLoader.totalBattlesArcade)
                            {
                                scr.Inicialize("SelectScreen", Color.black, 2.5f, true);
                                Launcher.mugen.FadeOutMusic();
                            }
                            else
                            {
                                if ((Team1.Wins.Count > Team2.Wins.Count && Team1.MainPlayer.PlayerMode == PlayerMode.Ai) ||
                                    (Team2.Wins.Count > Team1.Wins.Count && Team2.MainPlayer.PlayerMode == PlayerMode.Ai))
                                    scr.Inicialize("SelectScreen", Color.black, 2.5f, true);
                                else
                                    scr.Inicialize("VersusScreen", Color.black, 2.5f, true);

                                Launcher.mugen.FadeOutMusic();
                            }

                            m_logic = new NoMoreFighting();
                            return;

                        }
                        else if (Initialization.Mode == CombatMode.Versus)
                        {
                            Launcher.mugen.Pause = UnityMugen.PauseState.Paused;
                            Launcher.soundSystem.StopAllSounds();
                            RoundInformation.StopAllSounds();

                            GameObject init = new GameObject();
                            init.name = "LoadSceneCustom";
                            init.hideFlags = HideFlags.HideInHierarchy;
                            init.AddComponent<LoadSceneCustom>();
                            LoadSceneCustom scr = init.GetComponent<LoadSceneCustom>();
                            scr.Inicialize("SelectScreen", Color.black, 2.5f, true);

                            m_logic = new NoMoreFighting();
                            return;
                        }
                        //  launcherEngine.menuSystem.PostEvent(new fightEngine.Events.SwitchScreen(ScreenType.Menu));
                        m_logic = new NoMoreFighting();
                        return;

                    }


                    RoundNumber++;
                    //m_logic = new PreIntro();
                    m_logic = new ShowTransition(TypeShowTransition.IN);
                }
                else if (m_logic is ShowTransition && (m_logic as ShowTransition).typeShowTransition == TypeShowTransition.IN)
                {
                    m_logic = new PreIntro();
                }
            }

            m_logic.LateUpdate();
        }


        private void UpdateLogicTraining()
        {
            m_logic.UpdateFE();

            if (m_logic.IsFinished())
            {
                if (m_logic is PreIntro)
                {
                    m_logic = new ShowTransition(TypeShowTransition.OUT, true);
                }
                else if (m_logic is ShowTransition && (m_logic as ShowTransition).typeShowTransition == TypeShowTransition.OUT)
                {
                    m_logic = new Fighting();
                }
                else if (m_logic is ShowTransition && (m_logic as ShowTransition).typeShowTransition == TypeShowTransition.IN)
                {
                    m_logic = new PreIntro();
                }
            }

            m_logic.LateUpdate();
        }


        public void Draw(bool debug)
        {
            if (EnvironmentColor.IsActive == false && Assertions.NoBackLayer == false)
                stageScreen.Stage.Draw(Layer.Back);

            if (EnvironmentColor.IsActive && EnvironmentColor.UnderFlag)
                EnvironmentColor.Draw();

            Entities.Draw(debug);

            if (EnvironmentColor.IsActive && EnvironmentColor.UnderFlag == false)
                EnvironmentColor.Draw();

            if (EnvironmentColor.IsActive == false && Assertions.NoFrontLayer == false)
                stageScreen.Stage.Draw(Layer.Front);

            //   m_logic?.Draw();

            //   Team1.Display.ComboCounter.Draw();
            //   Team2.Display.ComboCounter.Draw();
        }

        public bool IsMatchOver()
        {
            return m_logic is ShowWinPose && m_logic.TickCount >= 0 &&
                (Team1.Wins.Count >= Launcher.initializationSettings.NumberOfRounds ||
                Team2.Wins.Count >= Launcher.initializationSettings.NumberOfRounds);
        }

    }
}