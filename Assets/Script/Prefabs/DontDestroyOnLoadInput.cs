using UnityEngine;

namespace UnityMugen.Prefabs
{

    public class DontDestroyOnLoadInput : MonoBehaviour
    {

        private static DontDestroyOnLoadInput _instance;

        public GameObject disableMouse;
        public GameObject loadInput;

        public GameObject pcStandaloneInputModule;


        void Awake()
        {
            if (!_instance)
                _instance = this;
            else
                Destroy(this.gameObject);

            DontDestroyOnLoad(this.gameObject);

            disableMouse.SetActive(false);
            loadInput.SetActive(true);

        }
    }
}