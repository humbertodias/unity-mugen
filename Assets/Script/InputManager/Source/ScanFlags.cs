namespace UnityMugen.CustomInput
{
    public enum ScanFlags
    {
        None = 0,
        Key = 1 << 1,
        JoystickButton = 1 << 2,
        JoystickAxis = 1 << 3,
        MouseAxis = 1 << 4
    }
}