﻿using UnityEngine;

namespace UnityMugen.CustomInput
{
    public class ScanService
    {
        private ScanHandler m_scanHandler;
        private ScanResult m_scanResult;
        private ScanFlags m_scanFlags;
        private KeyCode m_cancelScanKey;
        private float m_scanStartTime;
        private float m_scanTimeout;
        private int? m_scanJoystick;
        private object m_scanUserData;
        private KeyCode[] m_keys;
        private string[] m_rawMouseAxes;
        private string[] m_rawJoystickAxes;

        public float GameTime { get; set; }
        public bool IsScanning { get; private set; }

        public ScanService()
        {
            m_rawMouseAxes = new string[InputBinding.MAX_MOUSE_AXES];
            for (int i = 0; i < m_rawMouseAxes.Length; i++)
            {
                m_rawMouseAxes[i] = string.Concat("mouse_axis_", i);
            }

            m_rawJoystickAxes = new string[InputBinding.MAX_JOYSTICKS * InputBinding.MAX_JOYSTICK_AXES];
            for (int i = 0; i < InputBinding.MAX_JOYSTICKS; i++)
            {
                for (int j = 0; j < InputBinding.MAX_JOYSTICK_AXES; j++)
                {
                    m_rawJoystickAxes[i * InputBinding.MAX_JOYSTICK_AXES + j] = string.Concat("joy_", i, "_axis_", j);
                }
            }

            m_keys = (KeyCode[])System.Enum.GetValues(typeof(KeyCode));
            IsScanning = false;
        }

        public void Start(ScanSettings settings, ScanHandler scanHandler)
        {
            if (settings.Joystick.HasValue && (settings.Joystick < 0 || settings.Joystick >= InputBinding.MAX_JOYSTICK_AXES))
            {
                Debug.LogError("Joystick is out of range. Cannot start scan.");
                return;
            }

            if (IsScanning)
                Stop();

            m_scanTimeout = settings.Timeout;
            m_scanFlags = settings.ScanFlags;
            m_scanStartTime = GameTime;
            m_cancelScanKey = settings.CancelScanKey;
            m_scanJoystick = settings.Joystick;
            m_scanUserData = settings.UserData;
            m_scanHandler = scanHandler;
            IsScanning = true;
        }

        public void Stop()
        {
            if (IsScanning)
            {
                m_scanResult.ScanFlags = ScanFlags.None;
                m_scanResult.Key = KeyCode.None;
                m_scanResult.Joystick = -1;
                m_scanResult.JoystickAxis = -1;
                m_scanResult.JoystickAxisValue = 0.0f;
                m_scanResult.MouseAxis = -1;
                m_scanResult.UserData = m_scanUserData;

                m_scanHandler(m_scanResult);

                m_scanJoystick = null;
                m_scanHandler = null;
                m_scanResult.UserData = null;
                m_scanFlags = ScanFlags.None;
                IsScanning = false;
            }
        }

        public void Update()
        {
            float timeout = GameTime - m_scanStartTime;
            if (UnityEngine.Input.GetKeyDown(m_cancelScanKey) || timeout >= m_scanTimeout)
            {
                Stop();
                return;
            }

            bool success = false;
            if (HasFlag(ScanFlags.Key))
            {
                success = ScanKey();
            }
            if (!success && HasFlag(ScanFlags.JoystickButton))
            {
                success = ScanJoystickButton();
            }
            if (!success && HasFlag(ScanFlags.JoystickAxis))
            {
                success = ScanJoystickAxis();
            }
            if (!success && HasFlag(ScanFlags.MouseAxis))
            {
                success = ScanMouseAxis();
            }

            IsScanning = !success;
        }

        private bool ScanKey()
        {
            int length = m_keys.Length;
            for (int i = 0; i < length; i++)
            {
                if ((int)m_keys[i] >= (int)KeyCode.JoystickButton0)
                    break;

                if (UnityEngine.Input.GetKeyDown(m_keys[i]))
                {
                    m_scanResult.ScanFlags = ScanFlags.Key;
                    m_scanResult.Key = m_keys[i];
                    m_scanResult.Joystick = -1;
                    m_scanResult.JoystickAxis = -1;
                    m_scanResult.JoystickAxisValue = 0.0f;
                    m_scanResult.MouseAxis = -1;
                    m_scanResult.UserData = m_scanUserData;
                    if (m_scanHandler(m_scanResult))
                    {
                        m_scanHandler = null;
                        m_scanResult.UserData = null;
                        m_scanFlags = ScanFlags.None;
                        return true;
                    }
                }
            }

            return false;
        }

        private bool ScanJoystickButton()
        {
            int scanStart = (int)KeyCode.Joystick1Button0;
            int scanEnd = (int)KeyCode.Joystick8Button19;

            if (m_scanJoystick.HasValue)
            {
                scanStart = (int)KeyCode.Joystick1Button0 + m_scanJoystick.Value * 20;
                scanEnd = scanStart + 20;
            }

            for (int key = scanStart; key < scanEnd; key++)
            {
                if (UnityEngine.Input.GetKeyDown((KeyCode)key))
                {
                    if (m_scanJoystick.HasValue)
                    {
                        m_scanResult.Key = (KeyCode)key;
                        m_scanResult.Joystick = m_scanJoystick.Value;
                    }
                    else
                    {
                        m_scanResult.Key = (KeyCode)((int)KeyCode.JoystickButton0 + (key - (int)KeyCode.Joystick1Button0) % 20);
                        m_scanResult.Joystick = ((key - (int)KeyCode.Joystick1Button0) / 20);
                    }

                    m_scanResult.JoystickAxis = -1;
                    m_scanResult.JoystickAxisValue = 0.0f;
                    m_scanResult.MouseAxis = -1;
                    m_scanResult.UserData = m_scanUserData;
                    m_scanResult.ScanFlags = ScanFlags.JoystickButton;

                    if (m_scanHandler(m_scanResult))
                    {
                        m_scanHandler = null;
                        m_scanResult.UserData = null;
                        m_scanFlags = ScanFlags.None;
                        return true;
                    }
                }
            }

            return false;
        }

        private bool ScanJoystickAxis()
        {
            int scanStart = 0, scanEnd = m_rawJoystickAxes.Length;
            float axisRaw = 0.0f;

            if (m_scanJoystick.HasValue)
            {
                scanStart = m_scanJoystick.Value * InputBinding.MAX_JOYSTICK_AXES;
                scanEnd = scanStart + InputBinding.MAX_JOYSTICK_AXES;
            }

            for (int i = scanStart; i < scanEnd; i++)
            {
                axisRaw = UnityEngine.Input.GetAxisRaw(m_rawJoystickAxes[i]);
                if (Mathf.Abs(axisRaw) >= 1.0f)
                {
                    m_scanResult.ScanFlags = ScanFlags.JoystickAxis;
                    m_scanResult.Key = KeyCode.None;
                    m_scanResult.Joystick = i / InputBinding.MAX_JOYSTICK_AXES;
                    m_scanResult.JoystickAxis = i % InputBinding.MAX_JOYSTICK_AXES;
                    m_scanResult.JoystickAxisValue = axisRaw;
                    m_scanResult.MouseAxis = -1;
                    m_scanResult.UserData = m_scanUserData;
                    if (m_scanHandler(m_scanResult))
                    {
                        m_scanHandler = null;
                        m_scanResult.UserData = null;
                        m_scanFlags = ScanFlags.None;
                        return true;
                    }
                }
            }

            return false;
        }

        private bool ScanMouseAxis()
        {
            for (int i = 0; i < m_rawMouseAxes.Length; i++)
            {
                if (Mathf.Abs(UnityEngine.Input.GetAxis(m_rawMouseAxes[i])) > 0.0f)
                {
                    m_scanResult.ScanFlags = ScanFlags.MouseAxis;
                    m_scanResult.Key = KeyCode.None;
                    m_scanResult.Joystick = -1;
                    m_scanResult.JoystickAxis = -1;
                    m_scanResult.JoystickAxisValue = 0.0f;
                    m_scanResult.MouseAxis = i;
                    m_scanResult.UserData = m_scanUserData;
                    if (m_scanHandler(m_scanResult))
                    {
                        m_scanHandler = null;
                        m_scanResult.UserData = null;
                        m_scanFlags = ScanFlags.None;
                        return true;
                    }
                }
            }

            return false;
        }

        private bool HasFlag(ScanFlags flag)
        {
            return ((int)m_scanFlags & (int)flag) != 0;
        }
    }
}
