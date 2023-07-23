using UnityEngine;

namespace UnityMugen.CustomInput
{
    public struct ScanResult
    {
        public ScanFlags ScanFlags;
        public KeyCode Key;
        public int Joystick;
        public int JoystickAxis;
        public float JoystickAxisValue;
        public int MouseAxis;
        public object UserData;
    }
}