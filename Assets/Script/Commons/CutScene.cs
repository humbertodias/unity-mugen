using UnityEngine;

namespace UnityMugen.Interface
{

    public class CutScene : MonoBehaviour
    {

        private LoadScene loadScene;

        private void Awake()
        {
            loadScene = GetComponent<LoadScene>();
        }

        // Update is called once per frame
        void Update()
        {

            foreach (KeyCode key in KeyCode.GetValues(typeof(KeyCode)))
            {
                if (UnityEngine.Input.GetKeyDown(key))
                    loadScene.loadSceneCustom();
            }

        }

    }
}