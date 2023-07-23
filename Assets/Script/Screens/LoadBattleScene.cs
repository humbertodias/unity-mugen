using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityMugen.Screens
{

    public class LoadBattleScene : MonoBehaviour, ILoadBattleScene
    {

        private string fadeScene;
        private Color fadeColor;
        private float fadeDamp = 0.0f;
        private bool start = false;

        private bool isFadeIn = false;
        private float alpha = 0.0f;
        private bool jump = false;
        private float finalAlpha;

        public void Iniciar(string scene, Color loadToColor, float tempoDeTransacao, bool s)
        {
            LoadBattleScene LBS = new GameObject().AddComponent<LoadBattleScene>();
            LBS.hideFlags = HideFlags.HideInInspector;
            LBS.fadeScene = scene;
            LBS.fadeColor = loadToColor;
            LBS.fadeDamp = tempoDeTransacao;
            LBS.start = s;
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        void OnGUI()
        {
            if (!start)
                return;

            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
            Texture2D myTex = new Texture2D(1, 1);
            myTex.SetPixel(0, 0, fadeColor);
            myTex.Apply();
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), myTex);

        }

        private void Update()
        {
            if (isFadeIn)
            {
                alpha -= .25f;// Mathf.Lerp(alpha, 0f, fadeDamp * 0.1f);
                if (alpha < 0)
                    alpha = 0;
            }
            else
            {
                alpha += .25f;// Mathf.Lerp(alpha, 1f, fadeDamp * 0.1f);
                if (alpha > 1)
                    alpha = 1;
            }
            if (alpha == finalAlpha)
                jump = true;

            finalAlpha = alpha;

            if (jump && !isFadeIn)
            {
                StartCoroutine(doLoadLevel(fadeScene));
                DontDestroyOnLoad(gameObject);
                jump = false;
            }
            else if (jump && isFadeIn)
            {
                Destroy(gameObject);
            }

        }

        void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            isFadeIn = true;
        }

        IEnumerator doLoadLevel(string name)
        {
            SceneManager.LoadScene("LoadScreen");

            yield return null;
            yield return null;

            AsyncOperation ao = SceneManager.LoadSceneAsync(name);
            SceneManager.LoadScene("HudCanvasBattle", LoadSceneMode.Additive);
            SceneManager.LoadScene("HudPauseFight", LoadSceneMode.Additive);
            SceneManager.LoadScene("HudCanvasMoveLists", LoadSceneMode.Additive);
        }

    }
}