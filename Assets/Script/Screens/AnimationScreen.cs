using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityMugen.Combat;

namespace UnityMugen.Screens
{

    public class AnimationScreen : MonoBehaviour
    {

        FightEngine Engine => LauncherEngine.Inst.mugen.Engine;

        public PlayableDirector playableDirector;
        public Image skip;

        float fillAmount = 0;
        bool skipActived;

        private bool isFinish = false;

        public void Start()
        {
            //    Launcher.currentScreen = this;

            playableDirector.Play();
        }


        public void Update()
        {
            UpdateFE();
        }

        public void UpdateFE()
        {
            bool state = playableDirector.state == PlayState.Paused;
            if (state && isFinish == false)
            {
                isFinish = true;
                //LoadStageBattle("StageName");
            }

            if (skipActived)
            {
                fillAmount += (0.5f * Time.deltaTime);
                skip.fillAmount = fillAmount;
                //if (fillAmount == 1)
                //LoadStageBattle("StageName");
            }
        }

        private void LoadStageBattle(string stageName)
        {
            GameObject init = new GameObject();
            init.name = "LoadSceneCustom";
            init.hideFlags = HideFlags.HideInHierarchy;
            //   LoadBattleScene.Iniciar(stageName, Color.black, 2.5f, true, nameHudCanvasBattle);
        }

        public void Down()
        {
            skipActived = true;
            skip.gameObject.SetActive(true);
        }

        public void Up()
        {
            skipActived = false;
            fillAmount = 0;
            skip.gameObject.SetActive(false);
        }

    }
}