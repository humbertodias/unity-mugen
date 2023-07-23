using UnityEngine;

namespace UnityMugen.CustomInput
{
    public struct ScanSettings
    {
        public ScanFlags ScanFlags;
        public int? Joystick;
        public float Timeout;
        public KeyCode CancelScanKey;
        public object UserData;
    }
}