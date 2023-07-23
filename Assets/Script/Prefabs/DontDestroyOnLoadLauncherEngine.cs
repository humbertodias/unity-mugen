using UnityEngine;

namespace UnityMugen.Prefabs
{

    public class DontDestroyOnLoadLauncherEngine : MonoBehaviour
    {

        private static DontDestroyOnLoadLauncherEngine _instance;

        void Awake()
        {
            if (!_instance)
                _instance = this;
            else
                Destroy(this.gameObject);


            DontDestroyOnLoad(this.gameObject);
        }
    }
}