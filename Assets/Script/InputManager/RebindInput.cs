﻿using UnityEngine;
using UnityEngine.UI;

namespace UnityMugen.CustomInput
{
    public class RebindInput : MonoBehaviour/*, IPointerDownHandler*/
    {
        public enum RebindType
        {
            Keyboard, GamepadButton, GamepadAxis
        }

        public string m_controlSchemeName;
        public float m_timeout;

        [Range(0, InputBinding.MAX_JOYSTICKS)]
        public int m_joystick = 0;

        [SerializeField]
        private Text m_keyDescription;
        [SerializeField]
        private string m_inputActionName;
        [SerializeField]
        private int m_bindingIndex;
        [SerializeField]
        private KeyCode m_cancelKey;

        [SerializeField]
        private bool m_changePositiveKey;
        [SerializeField]
        private bool m_allowAnalogButton;
        [SerializeField]
        private RebindType m_rebindType;

        private InputAction m_inputAction;
        private InputBinding m_inputBinding;
        private static string[] m_axisNames;

        //public void Inicialize()
        //{
        //    Awake();
        //}


        public void Inicialize()
        {
            GenerateJoystickAxisNames();
            InitializeInputAction();

            //	The axis config needs to be reinitialized because loading can invalidate
            //	the input configurations
            InputManager.Loaded += InitializeInputAction;
            InputManager.ControlSchemesChanged += InitializeInputAction;
        }

        private void OnDestroy()
        {
            InputManager.Loaded -= InitializeInputAction;
            InputManager.ControlSchemesChanged -= InitializeInputAction;
        }

        private void InitializeInputAction()
        {
            m_inputAction = InputManager.GetAction(m_controlSchemeName, m_inputActionName);
            if (m_inputAction != null)
            {
                m_inputBinding = m_inputAction.Bindings[m_bindingIndex];
                if (m_rebindType == RebindType.Keyboard || m_rebindType == RebindType.GamepadButton)
                {
                    if (m_changePositiveKey)
                    {
                        m_keyDescription.text = m_inputBinding.Positive == KeyCode.None ? "" : m_inputBinding.Positive.ToString();
                    }
                    else
                    {
                        m_keyDescription.text = m_inputBinding.Negative == KeyCode.None ? "" : m_inputBinding.Negative.ToString();
                    }
                }
                else
                {
                    m_keyDescription.text = m_axisNames[m_inputBinding.Axis];
                }
            }
            else
            {
                m_keyDescription.text = "";
                Debug.LogErrorFormat("Control scheme '{0}' does not exist or input action '{1}' does not exist", m_controlSchemeName, m_inputActionName);
            }
        }

        //public void OnPointerDown(PointerEventData data)
        //{
        //	StartCoroutine(StartInputScanDelayed());
        //}

        //public void OnPointerDown(float waitTime)
        //{
        //    StartCoroutine(StartInputScanDelayed(waitTime));
        //}

        public void StartInputScanDelayed()
        {
            //	yield return new WaitForSeconds(waitTime);

            if (!InputManager.IsScanning && m_inputAction != null)
            {
                m_keyDescription.text = "...";

                ScanSettings settings;
                settings.Joystick = m_joystick;
                settings.CancelScanKey = m_cancelKey;
                settings.Timeout = m_timeout;
                settings.UserData = null;
                if (m_rebindType == RebindType.GamepadAxis)
                {
                    settings.ScanFlags = ScanFlags.JoystickAxis;
                    InputManager.StartInputScan(settings, HandleJoystickAxisScan);
                }
                else if (m_rebindType == RebindType.GamepadButton)
                {
                    settings.ScanFlags = ScanFlags.JoystickButton;
                    if (m_allowAnalogButton)
                    {
                        settings.ScanFlags |= ScanFlags.JoystickAxis;
                    }

                    InputManager.StartInputScan(settings, HandleJoystickButtonScan);
                }
                else
                {
                    settings.ScanFlags = ScanFlags.Key;
                    InputManager.StartInputScan(settings, HandleKeyScan);
                }
            }
        }

        public void SetNoneBinding()
        {
            m_inputAction = InputManager.GetAction(m_controlSchemeName, m_inputActionName);
            if (m_inputAction != null)
            {
                m_inputBinding = m_inputAction.Bindings[m_bindingIndex];
                m_inputBinding.SetNoneBinding();
                m_keyDescription.text = "...";
            }
        }

        private bool HandleKeyScan(ScanResult result)
        {
            //	When you return false you tell the InputManager that it should keep scaning for other keys
            if (!IsKeyValid(result.Key))
                return false;

            //	The key is KeyCode.None when the timeout has been reached or the scan has been canceled
            if (result.Key != KeyCode.None)
            {
                //	If the key is KeyCode.Backspace clear the current binding
                result.Key = (result.Key == KeyCode.Backspace) ? KeyCode.None : result.Key;
                if (m_changePositiveKey)
                {
                    m_inputBinding.Positive = result.Key;
                }
                else
                {
                    m_inputBinding.Negative = result.Key;
                }
                m_keyDescription.text = (result.Key == KeyCode.None) ? "" : result.Key.ToString();
            }
            else
            {
                KeyCode currentKey = GetCurrentKeyCode();
                m_keyDescription.text = (currentKey == KeyCode.None) ? "" : currentKey.ToString();
            }

            return true;
        }

        private bool IsKeyValid(KeyCode key)
        {
            bool isValid = true;

            if (m_rebindType == RebindType.Keyboard)
            {
                if ((int)key >= (int)KeyCode.JoystickButton0)
                    isValid = false;
                else if (key == KeyCode.LeftApple || key == KeyCode.RightApple)
                    isValid = false;
                else if (key == KeyCode.LeftWindows || key == KeyCode.RightWindows)
                    isValid = false;
            }
            else
            {
                isValid = false;
            }

            return isValid;
        }

        private bool HandleJoystickButtonScan(ScanResult result)
        {
            if (result.ScanFlags == ScanFlags.JoystickButton)
            {
                //	When you return false you tell the InputManager that it should keep scaning for other keys
                if (!IsJoytickButtonValid(result.Key))
                    return false;

                //	The key is KeyCode.None when the timeout has been reached or the scan has been canceled
                if (result.Key != KeyCode.None)
                {
                    //	If the key is KeyCode.Backspace clear the current binding
                    result.Key = (result.Key == KeyCode.Backspace) ? KeyCode.None : result.Key;
                    m_inputBinding.Type = InputType.Button;
                    if (m_changePositiveKey)
                    {
                        m_inputBinding.Positive = result.Key;
                    }
                    else
                    {
                        m_inputBinding.Negative = result.Key;
                    }
                    m_keyDescription.text = (result.Key == KeyCode.None) ? "" : result.Key.ToString();
                }
                else
                {
                    if (m_inputBinding.Type == InputType.Button)
                    {
                        KeyCode currentKey = GetCurrentKeyCode();
                        m_keyDescription.text = (currentKey == KeyCode.None) ? "" : currentKey.ToString();
                    }
                    else
                    {
                        m_keyDescription.text = (m_inputBinding.Invert ? "-" : "+") + m_axisNames[m_inputBinding.Axis];
                    }
                }
            }
            else
            {
                //	The axis is negative when the timeout has been reached or the scan has been canceled
                if (result.JoystickAxis >= 0)
                {
                    m_inputBinding.Type = InputType.AnalogButton;
                    m_inputBinding.Invert = result.JoystickAxisValue < 0.0f;
                    m_inputBinding.Axis = result.JoystickAxis;
                    m_keyDescription.text = (m_inputBinding.Invert ? "-" : "+") + m_axisNames[m_inputBinding.Axis];
                }
                else
                {
                    if (m_inputBinding.Type == InputType.AnalogButton)
                    {
                        m_keyDescription.text = (m_inputBinding.Invert ? "-" : "+") + m_axisNames[m_inputBinding.Axis];
                    }
                    else
                    {
                        KeyCode currentKey = GetCurrentKeyCode();
                        m_keyDescription.text = (currentKey == KeyCode.None) ? "" : currentKey.ToString();
                    }
                }
            }

            return true;
        }

        private bool IsJoytickButtonValid(KeyCode key)
        {
            bool isValid = true;

            if (m_rebindType == RebindType.GamepadButton)
            {
                //	Allow KeyCode.None to pass because it means that the scan has been canceled or the timeout has been reached
                //	Allow KeyCode.Backspace to pass so it can clear the current binding
                if ((int)key < (int)KeyCode.JoystickButton0 && key != KeyCode.None && key != KeyCode.Backspace)
                    isValid = false;
            }
            else
            {
                isValid = false;
            }

            return isValid;
        }

        private bool HandleJoystickAxisScan(ScanResult result)
        {
            //	The axis is negative when the timeout has been reached or the scan has been canceled
            if (result.JoystickAxis >= 0)
                m_inputBinding.Axis = result.JoystickAxis;

            m_keyDescription.text = m_axisNames[m_inputBinding.Axis];
            return true;
        }

        private KeyCode GetCurrentKeyCode()
        {
            if (m_rebindType == RebindType.GamepadAxis)
                return KeyCode.None;

            if (m_changePositiveKey)
            {
                return m_inputBinding.Positive;
            }
            else
            {
                return m_inputBinding.Negative;
            }
        }

        public static void GenerateJoystickAxisNames()
        {
            if (m_axisNames == null || m_axisNames.Length != InputBinding.MAX_JOYSTICK_AXES)
            {
                m_axisNames = new string[InputBinding.MAX_JOYSTICK_AXES];
                for (int i = 0; i < InputBinding.MAX_JOYSTICK_AXES; i++)
                {
                    if (i == 0)
                        m_axisNames[i] = "Joystick LEFT/RIGHT";
                    else if (i == 1)
                        m_axisNames[i] = "Joystick UP/DOWN";
                    else if (i == 2)
                        m_axisNames[i] = "3rd axis (Joysticks and Scrollwheel)";
                    else if (i == 21)
                        m_axisNames[i] = "21st axis (Joysticks)";
                    else if (i == 22)
                        m_axisNames[i] = "22nd axis (Joysticks)";
                    else if (i == 23)
                        m_axisNames[i] = "23rd axis (Joysticks)";
                    else
                        m_axisNames[i] = string.Format("{0}th axis (Joysticks)", i + 1);
                }
            }
        }
    }
}