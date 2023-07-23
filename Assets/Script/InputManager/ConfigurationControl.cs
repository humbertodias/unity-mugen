using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace UnityMugen.CustomInput
{
    public class ConfigurationControl : MonoBehaviour
    {
        private LauncherEngine Launcher => LauncherEngine.Inst;

        public TypeController typeController;
        public Text titleController;
        public TextAsset m_defaultInputProfile;

        private int numberController, numberAction;
        [SerializeField] GameObject panelPress;
        [SerializeField] Text messageText;
        [SerializeField] Text timeText;

        public Color selected;
        public Color noSelected;

        public GameObject button;
        public GameObject message;

        public int indexBack;
        public int indexReset;
        public Scrollbar scrollbar;

        public int totalItensShow;
        public RectTransform interno;
        public GameObject[] contents;

        int currentIndex;
        int positionGrid = 1;
        float sizeHeightContent;
        float spacing;

        [NonSerialized] public bool enable = false;

        void Start()
        {
            sizeHeightContent = contents[0].GetComponent<RectTransform>().sizeDelta.y;
            interno.sizeDelta = new Vector2(interno.sizeDelta.x, contents.Length * sizeHeightContent);
            spacing = interno.GetComponent<VerticalLayoutGroup>().spacing;
            enable = true;
        }

        void ResetGrid()
        {
            currentIndex = 0;
            positionGrid = 1;

            button = contents[currentIndex];
            scrollbar.value = 0;
            interno.anchoredPosition = new Vector2(interno.anchoredPosition.x, 0);
        }

        public void SetParameters(int m_numberController, int m_numberAction)
        {
            this.numberController = m_numberController;
            this.numberAction = m_numberAction;

            if (enable == false)
                ResetGrid();

            enable = true;

            titleController.text = string.Format("Controller {0} - {1}", m_numberController, typeController.ToString());
            SetControlSchemeName("Player" + numberController, numberAction);

            gameObject.SetActive(true);
            message.SetActive(false);
        }

        float time = 0f;
        float timeout = 0f;
        void Update()
        {

            if (InputManager.IsScanning)
            {
                time = 0f;
                timeout -= Time.deltaTime;
                timeText.text = string.Format("time to cancel {0} ...", timeout.ToString("f0").PadLeft(2, '0'));
            }
            else
            {
                time += Time.deltaTime;
                panelPress.SetActive(false);
            }


            if ((numberController == 1 && InputManager.GetButtonDown("X", PlayerID.One)) ||
                (numberController == 2 && InputManager.GetButtonDown("X", PlayerID.Two)))
            {
                RebindInput rebindInput = button.GetComponentInChildren<RebindInput>();
                if (rebindInput && time > 0.5f)
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
                    rebindInput.StartInputScanDelayed();
                    timeout = rebindInput.m_timeout;
                    panelPress.SetActive(true);
                    messageText.text = string.Format("press the button to use [ {0} ]", button.GetComponentInChildren<Text>().text);

                }
                if (currentIndex == indexBack)
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
                    Save();
                    enable = false;
                    gameObject.SetActive(false);
                    message.SetActive(true);
                }
                else if (currentIndex == indexReset)
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
                    ResetInputs(numberController, numberAction);
                }
            }
            else if (InputManager.GetKeyDown(KeyCode.Delete))
            {
                RebindInput rebindInput = button.GetComponentInChildren<RebindInput>();
                if (rebindInput)
                {
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECTED);
                    rebindInput.SetNoneBinding();
                }
            }


            if ((numberController == 1 && InputCustom.PressUpPlayerIDOne()) || (numberController == 2 && InputCustom.PressUpPlayerIDTwo()))
            {
                if (currentIndex == 0)
                {
                    currentIndex = contents.Length - 1;
                    positionGrid = totalItensShow;

                    button = contents[currentIndex];
                    scrollbar.value = 1;
                    interno.anchoredPosition = new Vector2(interno.anchoredPosition.x, (sizeHeightContent + spacing) * (contents.Length - totalItensShow));
                }
                else if (currentIndex > 0 && currentIndex > contents.Length - 1 - 2)
                {
                    if (currentIndex == contents.Length - 2)
                    {
                        currentIndex--;
                        positionGrid--;
                    }

                    currentIndex--;
                    if (positionGrid > 1)
                        positionGrid--;

                    button = contents[currentIndex];
                    scrollbar.value = (((float)currentIndex / ((float)contents.Length - 1f)) * 100f) * 0.01f;
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
                }
                else if (currentIndex > 0)
                {
                    currentIndex--;
                    if (positionGrid == 1)
                        interno.anchoredPosition = new Vector2(interno.anchoredPosition.x, interno.anchoredPosition.y - sizeHeightContent - spacing);

                    if (positionGrid > 1)
                        positionGrid--;

                    button = contents[currentIndex];
                    scrollbar.value = (((float)currentIndex / ((float)contents.Length - 1f)) * 100f) * 0.01f;
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
                }
            }
            else if ((numberController == 1 && InputCustom.PressDownPlayerIDOne()) || (numberController == 2 && InputCustom.PressDownPlayerIDTwo()))
            {
                if (currentIndex == contents.Length - 1)
                {
                    ResetGrid();
                }
                else if (currentIndex < contents.Length - 1 - 2)
                {
                    currentIndex++;
                    if (positionGrid == totalItensShow - 2)
                        interno.anchoredPosition = new Vector2(interno.anchoredPosition.x, interno.anchoredPosition.y + sizeHeightContent + spacing);

                    if (positionGrid < totalItensShow - 2)
                        positionGrid++;

                    if (currentIndex == contents.Length - 3)
                    {
                        positionGrid++;
                        currentIndex++;
                    }

                    button = contents[currentIndex];
                    scrollbar.value = (((float)currentIndex / ((float)contents.Length - 1f)) * 100f) * 0.01f;
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
                }
                else if (currentIndex < contents.Length - 1)
                {
                    positionGrid++;
                    currentIndex++;
                    button = contents[currentIndex];
                    scrollbar.value = (((float)currentIndex / ((float)contents.Length - 1f)) * 100f) * 0.01f;
                    Launcher.soundSystem.PlayChannelPrimary(TypeSound.SELECT);
                }
            }

            UpdateColorSelected();
        }



        private void SetControlSchemeName(string controlSchemeName, int numberAction)
        {
            for (int i = 0; i < contents.Length; i++)
            {
                RebindInput rebindInput = contents[i].GetComponentInChildren<RebindInput>();
                if (rebindInput)
                {
                    rebindInput.m_controlSchemeName = controlSchemeName;
                    rebindInput.m_joystick = (numberController - 1);
                    rebindInput.Inicialize();
                }
            }
        }


        private void Save()
        {
            string saveFolder = PathUtility.GetInputSaveFolder(1);
            if (!System.IO.Directory.Exists(saveFolder))
                System.IO.Directory.CreateDirectory(saveFolder);

            InputSaverXML saver = new InputSaverXML(saveFolder + "/input_config.xml");
            InputManager.Save(saver);
        }

        private void ResetInputs(int numberController, int numberAction)
        {
            ControlScheme controlScheme = InputManager.GetControlScheme("Player" + numberController);
            ControlScheme defControlScheme = null;

            using (StringReader reader = new StringReader(m_defaultInputProfile.text))
            {
                InputLoaderXML loader = new InputLoaderXML(reader);
                defControlScheme = loader.Load("Player" + numberController);
            }

            if (defControlScheme != null)
            {
                if (defControlScheme.Actions.Count == controlScheme.Actions.Count)
                {
                    for (int i = 0; i < defControlScheme.Actions.Count; i++)
                    {
                        controlScheme.Actions[i].CopyBinding(defControlScheme.Actions[i], numberAction);
                    }

                    InputManager.Reinitialize();
                }
                else
                {
                    Debug.LogError("Current and default control scheme don't have the same number of actions");
                }
            }
        }


        public void UpdateColorSelected()
        {
            for (int i = 0; i < contents.Length; i++)
            {
                if (currentIndex == i)
                {
                    Image o = contents[i].GetComponent<Image>();
                    if (o != null)
                        o.color = selected;
                }
                else
                {
                    Image o = contents[i].GetComponent<Image>();
                    if (o != null)
                        o.color = noSelected;
                }
            }
        }

    }
}