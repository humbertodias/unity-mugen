using UnityEngine;
using UnityMugen;

namespace UnityMugen.CustomInput
{
    public static class GamepadState
    {
        private static bool m_hasWarningBeenDisplayed = false;

        public static IGamepadStateAdapter Adapter { get; set; }

        public static bool IsGamepadSupported
        {
            get
            {
#if ENABLE_X_INPUT
				return true;
#else
                return false;
#endif
            }
        }

        public static bool IsConnected(GamepadIndex gamepad)
        {
            PrintMissingAdapterWarningIfNecessary();
            return Adapter != null ? Adapter.IsConnected(gamepad) : false;
        }

        public static float GetAxis(GamepadAxis axis, GamepadIndex gamepad)
        {
            PrintMissingAdapterWarningIfNecessary();
            return Adapter != null ? Adapter.GetAxis(axis, gamepad) : 0;
        }

        public static float GetAxisRaw(GamepadAxis axis, GamepadIndex gamepad)
        {
            PrintMissingAdapterWarningIfNecessary();
            return Adapter != null ? Adapter.GetAxisRaw(axis, gamepad) : 0;
        }

        public static bool GetButton(GamepadButton button, GamepadIndex gamepad)
        {
            PrintMissingAdapterWarningIfNecessary();
            return Adapter != null ? Adapter.GetButton(button, gamepad) : false;
        }

        public static bool GetButtonDown(GamepadButton button, GamepadIndex gamepad)
        {
            PrintMissingAdapterWarningIfNecessary();
            return Adapter != null ? Adapter.GetButtonDown(button, gamepad) : false;
        }

        public static bool GetButtonUp(GamepadButton button, GamepadIndex gamepad)
        {
            PrintMissingAdapterWarningIfNecessary();
            return Adapter != null ? Adapter.GetButtonUp(button, gamepad) : false;
        }

        public static void SetVibration(GamepadVibration vibration, GamepadIndex gamepad)
        {
            PrintMissingAdapterWarningIfNecessary();
            if (Adapter != null) Adapter.SetVibration(vibration, gamepad);
        }

        public static GamepadVibration GetVibration(GamepadIndex gamepad)
        {
            PrintMissingAdapterWarningIfNecessary();
            return Adapter != null ? Adapter.GetVibration(gamepad) : new GamepadVibration();
        }

        private static void PrintMissingAdapterWarningIfNecessary()
        {
            if (Adapter == null && !m_hasWarningBeenDisplayed)
            {
                Debug.LogWarning("No gamepad adapter has been assigned.");
                m_hasWarningBeenDisplayed = true;
            }
        }
    }
}
