using System;
using System.Diagnostics;
using UnityMugen.CustomInput;

namespace UnityMugen.Combat
{
    public class ForceFeedbackJoy
    {
        public ForceFeedbackJoy(Character character)
        {
            if (character == null) throw new ArgumentNullException(nameof(character));

            m_character = character;
            m_time = 0;
            m_isactive = false;
        }

        public void ResetFE()
        {
            m_time = 0;
            m_isactive = false;
            m_gamepad = GamepadIndex.GamepadOne;
            GamepadState.SetVibration(new GamepadVibration(ZERO_ROTATION, ZERO_ROTATION), m_gamepad);
        }

        public void UpdateFE()
        {
            if (IsActive && (Time == -1 || Time > 0))
            {
                if (Time > 0) --m_time;

                Feedback();
            }
            else
            {
                ResetFE();
            }
        }

        public void Set(int time, GamepadIndex gamepad)
        {
            m_time = time;
            m_isactive = true;
            m_gamepad = gamepad;
        }

        private void Feedback()
        {
            GamepadState.SetVibration(new GamepadVibration(MAX_ROTATION, MAX_ROTATION), Gamepad);
        }

        const float ZERO_ROTATION = 0f;
        const float MAX_ROTATION = 1f;

        public bool IsActive => m_isactive;
        public Character Character => m_character;
        public int Time => m_time;
        public GamepadIndex Gamepad => m_gamepad;


        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Character m_character;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isactive;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_time;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private GamepadIndex m_gamepad;

        #endregion
    }
}