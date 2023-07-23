using UnityEngine;

namespace UnityMugen.CustomInput
{
    public static class PathUtility
    {
        public static string GetInputSaveFolder(int example)
        {
            return string.Format("{0}/example_{1}", Application.persistentDataPath, example);
        }
    }
}