using UnityMugen;

namespace UnityMugen.CustomInput
{
    public interface IGamepadStateAdapter
    {
        bool IsConnected(GamepadIndex gamepad);
        float GetAxis(GamepadAxis axis, GamepadIndex gamepad);
        float GetAxisRaw(GamepadAxis axis, GamepadIndex gamepad);
        bool GetButton(GamepadButton button, GamepadIndex gamepad);
        bool GetButtonDown(GamepadButton button, GamepadIndex gamepad);
        bool GetButtonUp(GamepadButton button, GamepadIndex gamepad);
        void SetVibration(GamepadVibration vibration, GamepadIndex gamepad);
        GamepadVibration GetVibration(GamepadIndex gamepad);
    }
}