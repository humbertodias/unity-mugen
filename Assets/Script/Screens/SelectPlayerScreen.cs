using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityMugen.Combat;
using UnityMugen.CustomInput;
using UnityMugen.Drawing;

namespace UnityMugen.Screens
{

    [Serializable]
    public class BTPress
    {
        public int pressP1X = 0;
        public int pressP1Y = 0;
        public int pressP1UP = 0;
        public int pressP1DOWN = 0;
        public int pressP1LEFT = 0;
        public int pressP1RIGHT = 0;

        public int pressP2X = 0;
        public int pressP2Y = 0;
        public int pressP2UP = 0;
        public int pressP2DOWN = 0;
        public int pressP2LEFT = 0;
        public int pressP2RIGHT = 0;

        public void New()
        {
            this.pressP1X = 0;
            this.pressP1Y = 0;
            this.pressP1UP = 0;
            this.pressP1DOWN = 0;
            this.pressP1LEFT = 0;
            this.pressP1RIGHT = 0;

            this.pressP2X = 0;
            this.pressP2Y = 0;
            this.pressP2UP = 0;
            this.pressP2DOWN = 0;
            this.pressP2LEFT = 0;
            this.pressP2RIGHT = 0;
        }
    }

    public class SelectPlayerScreen : MonoBehaviour, ISelectPlayerScreen
    {
        //public LauncherEngine Launcher => LauncherEngine._inst;
        public LauncherEngine Launcher { get => LauncherEngine.Inst; }

        private EngineInitialization initialization;
        private bool pauseUpdate;

        public GameObject selectStage;

        public Sprite imageRandom;
        public Text namePlayer1;
        public Text namePlayer2;

        public CharacterSelect currentPositionP1;
        public CharacterSelect currentPositionP2;
        CharacterSelect lastPositionP1;
        CharacterSelect lastPositionP2;

        public GameObject selectorP1;
        public GameObject selectorP2;

        public Animator p1Animator;
        public Animator p2Animator;
        PaletteManager paletteManager;

        public AudioClip music;


        public Image imageChar1;
        public Image imageChar2;


        [Header("Colors Palette")]
        public GameObject colorsP1;
        public Text paletteNumberP1;
        PaletteList paletteOverrideP1;
        int colorP1Select;
        int totalPaletteP1;

        public GameObject colorsP2;
        public Text paletteNumberP2;
        PaletteList paletteOverrideP2;
        int colorP2Select;
        int totalPaletteP2;



        private enum StateSelect { FightMode, SelectCharacter, SelectColor, Finish }
        private StateSelect stateSelectP1;
        private StateSelect stateSelectP2;

        private enum StateSelectChar { None, SelectedFirst, SelectedSecond, Finish }
        private StateSelectChar stateSelectCharP1 = StateSelectChar.None;
        private StateSelectChar stateSelectCharP2 = StateSelectChar.None;



        public Transform Select;
        CharacterSelect[] selects;
        public CharacterSelect[] randomsPosition;

        //[Header("Grid Players")]
        //public CharacterSelectPC[] grid;
        //private Dictionary<string, CharacterSelect> m_grid;


        public void Start()
        {
            Launcher.screenType = ScreenType.Select;

            InputCustom.ActiveUpdate = true;

            //if (Launcher.IsHostClientConnected())
            //    Instantiate(Resources.Load<GameObject>("SHOW_Sync"));

            //m_grid = new Dictionary<string, CharacterSelectPC>();
            //foreach(var character in grid)
            //{
            //    m_grid.Add(character.nameChar, character);
            //}

            selects = Select.GetComponentsInChildren<CharacterSelect>();

            btPress = new BTPress();
            initialization = Launcher.engineInitialization;
            initialization.SetSeed();

            if (music != null)
                Launcher.soundSystem.PlayMusic(music, true);

            paletteManager = new PaletteManager();

            stateSelectP1 = StateSelect.SelectCharacter;
            stateSelectP2 = StateSelect.SelectCharacter;

            //DadosUsuario dadosUsuarioP1 = Launcher.dadosConnection.GetPlayerOne();
            //userNameP1.text = dadosUsuarioP1.playerName;

            //DadosUsuario dadosUsuarioP2 = Launcher.dadosConnection.GetPlayerTwo();
            //userNameP2.text = dadosUsuarioP2.playerName;

            ResetSelectPlayer();

            p1Animator.gameObject.SetActive(false);
            p2Animator.gameObject.SetActive(false);
            p1Animator.GetComponent<SpriteRenderer>().material = new Material(Shader.Find("UnityMugen/Sprites/ColorSwap"));
            p2Animator.GetComponent<SpriteRenderer>().material = new Material(Shader.Find("UnityMugen/Sprites/ColorSwap"));
        }

        public void ResetSelectPlayer()
        {
            pauseUpdate = false;

            //PARA TESTE
            initialization.Team1.Clear();
            initialization.Team2.Clear();
            ////////////

            colorP1Select = 0;
            colorP2Select = 0;

            p1Animator.gameObject.SetActive(false);
            p2Animator.gameObject.SetActive(false);

            stateSelectP1 = StateSelect.SelectCharacter;
            stateSelectCharP1 = StateSelectChar.None;
            selectorP1.SetActive(true);

            stateSelectP2 = StateSelect.SelectCharacter;
            stateSelectCharP2 = StateSelectChar.None;
            selectorP2.SetActive(true);

            if (Launcher.engineInitialization.Mode == CombatMode.Arcade)
            {
                stateSelectP2 = StateSelect.Finish;
                stateSelectCharP2 = StateSelectChar.Finish;
                selectorP2.SetActive(false);
            }
        }

        PlayerButton oldCurrentA;
        public int CommandButton1(PlayerButton playerButton)
        {
            PlayerButton currentA = Launcher.inputSystem.playerButton1Newtwork;
            if (oldCurrentA != currentA && currentA.ToString().Contains(playerButton.ToString()))
            {
                return 1;
            }
            else
            {
                return 0;
            }
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

        PlayerButton oldCurrentB;
        public int CommandButton2(PlayerButton playerButton)
        {
            PlayerButton currentB = Launcher.inputSystem.playerButton2Newtwork;
            if (oldCurrentB != currentB && currentB.ToString().Contains(playerButton.ToString()))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }


        public void teste()
        {

        }


        private BTPress btPress;

        public PlayerButton oldPlayerButton;

        void UpdatePress()
        {
            btPress.New();

            if (InputManager.GetButtonDown("X", PlayerID.One))
                btPress.pressP1X = 1;
            if (InputManager.GetButtonDown("Y", PlayerID.One))
                btPress.pressP1Y = 1;
            if (InputCustom.PressUpPlayerIDOne())
                btPress.pressP1UP = 1;
            if (InputCustom.PressDownPlayerIDOne())
                btPress.pressP1DOWN = 1;
            if (InputCustom.PressLeftPlayerIDOne())
                btPress.pressP1LEFT = 1;
            if (InputCustom.PressRightPlayerIDOne())
                btPress.pressP1RIGHT = 1;

            if (InputManager.GetButtonDown("X", PlayerID.Two))
                btPress.pressP2X = 1;
            if (InputManager.GetButtonDown("Y", PlayerID.Two))
                btPress.pressP2Y = 1;
            if (InputCustom.PressUpPlayerIDTwo())
                btPress.pressP2UP = 1;
            if (InputCustom.PressDownPlayerIDTwo())
                btPress.pressP2DOWN = 1;
            if (InputCustom.PressLeftPlayerIDTwo())
                btPress.pressP2LEFT = 1;
            if (InputCustom.PressRightPlayerIDTwo())
                btPress.pressP2RIGHT = 1;
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
            DoUpdate();
        }

        public void DoUpdate()
        {
            if (pauseUpdate)
                return;

            UpdatePress();

            if (stateSelectP1 == StateSelect.SelectColor)
                SelectColorP1();

            if (stateSelectP2 == StateSelect.SelectColor)
                SelectColorP2();

            if (stateSelectP1 == StateSelect.SelectCharacter)
            {
                if (btPress.pressP1X == 1 && stateSelectCharP1 == StateSelectChar.None)
                {
                    lastPositionP1 = currentPositionP1;
                    //  verificar como fica com a conexão TCP
                    //for (int i = 0; i < randomsPosition.Length; i++)
                    {
                        if (randomsPosition.Contains(currentPositionP1))
                        {
                        tryAgain:
                            currentPositionP1 = selects.ElementAt(UnityEngine.Random.Range(0, selects.Length - 1));
                            if (randomsPosition.Contains(currentPositionP1))
                                goto tryAgain;
                        }
                    }

                    PlayerCreation create = new PlayerCreation(currentPositionP1.profile, 0, PlayerMode.Human);
                    initialization.Team1.Add(create);

                    //imageChar1.sprite = characterSelect1.selectedPlayerImage;
                    namePlayer1.text = currentPositionP1.profile.displayName;

                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
                    stateSelectCharP1 = StateSelectChar.Finish;
                    stateSelectP1 = StateSelect.SelectColor;
                    //selectorPositionP1 = 99;
                    colorsP1.SetActive(true);

                    p1Animator.gameObject.SetActive(true);
                    p1Animator.GetComponent<Animator>().runtimeAnimatorController = currentPositionP1.animator;

                    string file = currentPositionP1.profile.NamefileSFF();
                    paletteOverrideP1 = Launcher.paletteSystem.LoadPalette(file);
                    totalPaletteP1 = UpdatePalette(p1Animator.GetComponent<SpriteRenderer>(), currentPositionP1, currentPositionP1.profile.palettesIndex[0], paletteOverrideP1);
                }
                else if (btPress.pressP1Y == 1)
                {
                    Launcher.DestroyCombatScreen();
                    StartCoroutine(Launcher.soundSystem.FadeOutMusic(Constant.TimeFadeOutMusic));
                    LoadSceneCustom.LoadScene("MenuScreen");
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.CANCEL);
                }
                CharacterSelectP1();
            }

            if (stateSelectP2 == StateSelect.SelectCharacter)
            {
                if (btPress.pressP2X == 1 && stateSelectCharP2 == StateSelectChar.None)
                {
                    lastPositionP2 = currentPositionP2;

                    if (randomsPosition.Contains(currentPositionP2))
                    {
                    tryAgain:
                        currentPositionP2 = selects.ElementAt(UnityEngine.Random.Range(0, selects.Length - 1));
                        if (randomsPosition.Contains(currentPositionP2))
                            goto tryAgain;
                    }

                    PlayerCreation create;
                    if (Launcher.engineInitialization.Mode == CombatMode.Arcade)
                        create = new PlayerCreation(currentPositionP2.profile, 1, PlayerMode.Ai);
                    else
                        create = new PlayerCreation(currentPositionP2.profile, 1, PlayerMode.Human);

                    initialization.Team2.Add(create);

                    //imageChar2.sprite = characterSelect2.selectedPlayerImage;
                    namePlayer2.text = currentPositionP2.profile.displayName;

                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
                    stateSelectCharP2 = StateSelectChar.Finish;
                    stateSelectP2 = StateSelect.SelectColor;
                    //selectorPositionP2 = 99;
                    colorsP2.SetActive(true);

                    p2Animator.gameObject.SetActive(true);
                    p2Animator.GetComponent<Animator>().runtimeAnimatorController = currentPositionP2.animator;

                    string file = currentPositionP2.profile.NamefileSFF();
                    paletteOverrideP2 = Launcher.paletteSystem.LoadPalette(file);
                    totalPaletteP2 = UpdatePalette(p2Animator.GetComponent<SpriteRenderer>(), currentPositionP2, currentPositionP2.profile.palettesIndex[0], paletteOverrideP2);
                }
                else if (btPress.pressP2Y == 1)
                {
                    LoadSceneCustom.LoadScene("MenuScreen");
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.CANCEL);
                }
                CharacterSelectP2();
            }

            if (stateSelectP1 == StateSelect.SelectCharacter ||
                stateSelectP2 == StateSelect.SelectCharacter ||
                stateSelectP1 == StateSelect.SelectColor ||
                stateSelectP2 == StateSelect.SelectColor)
                GridPlayerUpdate();


            if (stateSelectP1 == StateSelect.Finish && stateSelectP2 == StateSelect.Finish)
            {
                initialization.Team1Mode = TeamMode.Single;
                initialization.Team2Mode = TeamMode.Single;

                if (Launcher.engineInitialization.Mode == CombatMode.Arcade)
                {
                    Launcher.profileLoader.GenerateOrderBattleArcade();
                    StartCoroutine(DelayLoadScene("VersusScreen"));
                }
                else
                {
                    StartCoroutine(DelayPanelStageSelect());
                }

                pauseUpdate = true;
            }
        }

        public IEnumerator DelayPanelStageSelect()
        {
            yield return new WaitForSeconds(2);
            gameObject.SetActive(false);
            selectStage.SetActive(true);
        }

        public IEnumerator DelayLoadScene(string nameScene)
        {
            yield return new WaitForSeconds(2);
            LoadSceneCustom.LoadScene(nameScene);
        }

        private void SelectColorP1()
        {
            paletteNumberP1.text = (colorP1Select + 1).ToString();

            if (btPress.pressP1LEFT == 1)
            {
                if (colorP1Select > 0)
                    colorP1Select--;
                else
                    colorP1Select = totalPaletteP1 - 1;
            }
            else if (btPress.pressP1RIGHT == 1)
            {
                if (colorP1Select < totalPaletteP1 - 1)
                    colorP1Select++;
                else
                    colorP1Select = 0;
            }
            if (btPress.pressP1LEFT == 1 || btPress.pressP1RIGHT == 1)
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
                totalPaletteP1 = UpdatePalette(p1Animator.GetComponent<SpriteRenderer>(), currentPositionP1, currentPositionP1.profile.palettesIndex[colorP1Select], paletteOverrideP1);
            }

            if (btPress.pressP1X == 1)
            {
                stateSelectP1 = StateSelect.Finish;
                colorsP1.SetActive(false);
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);

                if (stateSelectP2 == StateSelect.Finish &&
                    currentPositionP1 == currentPositionP2 &&
                    colorP1Select == colorP2Select)
                {
                tryAgain:
                    colorP1Select = UnityEngine.Random.Range(0, totalPaletteP1 - 1);
                    if (colorP1Select == colorP2Select)
                        goto tryAgain;

                    totalPaletteP1 = UpdatePalette(p1Animator.GetComponent<SpriteRenderer>(), currentPositionP1, currentPositionP1.profile.palettesIndex[colorP1Select], paletteOverrideP1);
                }

                initialization.Team1[0].paletteIndex = currentPositionP1.profile.palettesIndex[colorP1Select];

                p1Animator.GetComponent<Animator>().SetTrigger("active");
            }
            else if (btPress.pressP1Y == 1)
            {
                p1Animator.gameObject.SetActive(false);
                StartCoroutine(BackSelectColor1());
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.CANCEL);
            }
        }

        private int UpdatePalette(SpriteRenderer spriteRenderer, CharacterSelect select/*, string selectorPosition*/, int colorSelect, PaletteList paletteOverride)
        {
            if (paletteOverride.PalTable.TryGetValue(new PaletteId(1, colorSelect), out int colorSelectResult))
                paletteManager.SetExternalPalette(colorSelectResult, paletteOverride, spriteRenderer);
            else
                Debug.LogWarningFormat("palette index does not exist. Pal[{0}, {1}]", 1, colorSelect);

            return select.profile.palettesIndex.Length;
        }

        IEnumerator BackSelectColor1()
        {
            yield return null;
            stateSelectP1 = StateSelect.SelectCharacter;
            stateSelectCharP1 = StateSelectChar.None;
            colorP1Select = 0;
            currentPositionP1 = lastPositionP1;
            colorsP1.SetActive(false);

            initialization.Team1.Clear();

            imageChar1.sprite = null;
            namePlayer1.text = null;
        }

        private void SelectColorP2()
        {
            paletteNumberP2.text = (colorP2Select + 1).ToString();

            if (btPress.pressP2RIGHT == 1)
            {
                if (colorP2Select > 0)
                    colorP2Select--;
                else
                    colorP2Select = totalPaletteP2 - 1;
            }
            else if (btPress.pressP2LEFT == 1)
            {
                if (colorP2Select < totalPaletteP2 - 1)
                    colorP2Select++;
                else
                    colorP2Select = 0;
            }
            if (btPress.pressP2RIGHT == 1 || btPress.pressP2LEFT == 1)
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
                totalPaletteP2 = UpdatePalette(p2Animator.GetComponent<SpriteRenderer>(), currentPositionP2, currentPositionP2.profile.palettesIndex[colorP2Select], paletteOverrideP2);
            }

            if (btPress.pressP2X == 1)
            {
                stateSelectP2 = StateSelect.Finish;
                colorsP2.SetActive(false);
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);

                if (stateSelectP1 == StateSelect.Finish &&
                    currentPositionP1 == currentPositionP2 &&
                    colorP1Select == colorP2Select)
                {
                tryAgain:
                    colorP2Select = UnityEngine.Random.Range(0, totalPaletteP2 - 1);
                    if (colorP1Select == colorP2Select)
                        goto tryAgain;

                    totalPaletteP2 = UpdatePalette(p2Animator.GetComponent<SpriteRenderer>(), currentPositionP2, currentPositionP2.profile.palettesIndex[colorP2Select], paletteOverrideP2);
                }
                initialization.Team2[0].paletteIndex = currentPositionP2.profile.palettesIndex[colorP2Select];

                p2Animator.GetComponent<Animator>().SetTrigger("active");
            }
            else if (btPress.pressP2Y == 1)
            {
                p2Animator.gameObject.SetActive(false);
                StartCoroutine(BackSelectColor2());
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.CANCEL);
            }
        }

        IEnumerator BackSelectColor2()
        {
            yield return null;
            stateSelectP2 = StateSelect.SelectCharacter;
            stateSelectCharP2 = StateSelectChar.None;
            colorP2Select = 0;
            currentPositionP2 = lastPositionP2;
            colorsP2.SetActive(false);

            initialization.Team2.Clear();

            imageChar2.sprite = null;
            namePlayer2.text = null;
        }

        private void CharacterSelectP1()
        {
            if (btPress.pressP1UP == 1)
            {
                currentPositionP1 = currentPositionP1.posiUp;
            }
            else if (btPress.pressP1DOWN == 1)
            {
                currentPositionP1 = currentPositionP1.posiDown;
            }
            else if (btPress.pressP1LEFT == 1)
            {
                currentPositionP1 = currentPositionP1.posiLeft;
            }
            else if (btPress.pressP1RIGHT == 1)
            {
                currentPositionP1 = currentPositionP1.posiRight;
            }

            if (btPress.pressP1UP == 1 ||
                btPress.pressP1DOWN == 1 ||
                btPress.pressP1LEFT == 1 ||
                btPress.pressP1RIGHT == 1)
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
            }

            if (currentPositionP1.profile != null)
            {
                imageChar1.sprite = currentPositionP1.profile.largePortrait;
            }
            else
            {
                imageChar1.sprite = imageRandom;
            }
        }
        private void CharacterSelectP2()
        {
            if (btPress.pressP2UP == 1)
            {
                currentPositionP2 = currentPositionP2.posiUp;
            }
            else if (btPress.pressP2DOWN == 1)
            {
                currentPositionP2 = currentPositionP2.posiDown;
            }
            else if (btPress.pressP2LEFT == 1)
            {
                currentPositionP2 = currentPositionP2.posiLeft;
            }
            else if (btPress.pressP2RIGHT == 1)
            {
                currentPositionP2 = currentPositionP2.posiRight;
            }

            if (btPress.pressP2UP == 1 ||
                btPress.pressP2DOWN == 1 ||
                btPress.pressP2LEFT == 1 ||
                btPress.pressP2RIGHT == 1)
            {
                Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
            }

            if (currentPositionP2.profile != null)
            {
                imageChar2.sprite = currentPositionP2.profile.largePortrait;
                imageChar2.rectTransform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                imageChar2.sprite = imageRandom;
                imageChar2.rectTransform.localScale = Vector3.one;
            }
        }



        private void GridPlayerUpdate()
        {
            if (stateSelectCharP1 == StateSelectChar.Finish)
                selectorP1.SetActive(false);
            else
                selectorP1.SetActive(true);

            if (stateSelectCharP2 == StateSelectChar.Finish)
                selectorP2.SetActive(false);
            else
                selectorP2.SetActive(true);

            if (stateSelectP1 == StateSelect.SelectCharacter)
            {
                RectTransform rt = selectorP1.GetComponent<RectTransform>();
                rt.localPosition = currentPositionP1.transform.localPosition + new Vector3(-28, -60, 0);
            }

            if (stateSelectP2 == StateSelect.SelectCharacter)
            {
                RectTransform rt = selectorP2.GetComponent<RectTransform>();
                rt.localPosition = currentPositionP2.transform.localPosition + new Vector3(28, -60, 0);
            }
        }

    }

}