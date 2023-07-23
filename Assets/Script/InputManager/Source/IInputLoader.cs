namespace UnityMugen.CustomInput
{
    public interface IInputLoader
    {
        SaveData Load();
        ControlScheme Load(string schemeName);
    }
}
