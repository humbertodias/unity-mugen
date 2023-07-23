using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    public bool isIntro = false;
    public float waitTimeIntro = 0f;

    public string scene;
    public Color loadToColor = Color.white;
    public float tempoDeTransacao = 0.5f;

    void Start()
    {
        if (isIntro)
        {
            IEnumerator coroutine = StartIntro();
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator StartIntro()
    {
        yield return new WaitForSeconds(waitTimeIntro);
        loadSceneCustom();
    }

    public void loadSceneCustom()
    {
        GameObject init = new GameObject();
        init.name = "LoadSceneCustom";
        init.hideFlags = HideFlags.HideInHierarchy;
        init.AddComponent<LoadSceneCustom>();
        LoadSceneCustom scr = init.GetComponent<LoadSceneCustom>();
        scr.Inicialize(scene, loadToColor, tempoDeTransacao, true);
    }

}


public class LoadSceneCustom : MonoBehaviour
{

    private string fadeScene;
    private Color fadeColor;
    private float fadeDamp = 0.0f;
    private bool start = false;

    private bool isFadeIn = false;
    private float alpha = 0.0f;
    private bool jump = false;
    private float finalAlpha;

    public void Inicialize(string scene, Color loadToColor, float tempoDeTransacao, bool v)
    {
        this.fadeScene = scene;
        this.fadeColor = loadToColor;
        this.fadeDamp = tempoDeTransacao;
        this.start = v;
    }

    public static void LoadScene(string nameScene)
    {
        GameObject init = new GameObject();
        init.name = "LoadSceneCustom";
        init.hideFlags = HideFlags.HideInHierarchy;
        init.AddComponent<LoadSceneCustom>();
        LoadSceneCustom scr = init.GetComponent<LoadSceneCustom>();
        scr.Inicialize(nameScene, Color.black, 2.5f, true);
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
        Texture2D myTex;
        myTex = new Texture2D(1, 1);
        myTex.SetPixel(0, 0, fadeColor);
        myTex.Apply();
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), myTex);

    }

    private void Update()
    {
        if (isFadeIn)
            alpha = Mathf.Lerp(alpha, 0f, fadeDamp * 0.1f);
        else
            alpha = Mathf.Lerp(alpha, 1f, fadeDamp * 0.1f);

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

        SceneManager.LoadSceneAsync(name);
    }

}
