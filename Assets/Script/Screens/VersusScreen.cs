using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityMugen.Combat;

namespace UnityMugen.Screens
{

    public class VersusScreen : MonoBehaviour
    {
        public LauncherEngine Launcher => LauncherEngine.Inst;
        public FightEngine Engine => Launcher.mugen.Engine;

        private EngineInitialization initialization;

        private Color battleHiden = new Color(0.05660379f, 0.05660379f, 0.05660379f);
        private Color battleShow = Color.white;

        public SpriteRenderer imageP1, imageP2;

        public Image[] battles;

        private float t = 0;

        void Start()
        {
            Launcher.screenType = ScreenType.Versus;
            initialization = Launcher.engineInitialization;

            SetImagesBattle();

            for (int i = 1; i <= battles.Length; i++)
            {
                if (Engine.MatchNumber >= i)
                    battles[i - 1].color = battleShow;
                else
                    battles[i - 1].color = battleHiden;
            }

            initialization.Team1Mode = TeamMode.Single;
            initialization.Team2Mode = TeamMode.Single;

            initialization.Team2.Clear();

            var (prof, stageId) = Launcher.profileLoader.GetNextBattleArcade();

            PlayerCreation create2 = new PlayerCreation(prof, 0, PlayerMode.Ai);
            initialization.Team2.Add(create2);

        tryAgain:
            int randomPalette = UnityEngine.Random.Range(0, prof.palettesIndex.Length - 1);
            if (initialization.Team1[0].profile.name ==
                initialization.Team2[0].profile.name &&
                initialization.Team1[0].paletteIndex == prof.palettesIndex[randomPalette])
                goto tryAgain;

            initialization.Team2[0].paletteIndex = prof.palettesIndex[randomPalette];

            initialization.stageID = stageId;

            Sprite p1Image = initialization.Team1[0].profile.largePortrait;
            Sprite p2Image = initialization.Team2[0].profile.largePortrait;

            imageP1.sprite = p1Image;
            imageP2.sprite = p2Image;

            StartCoroutine(LoadBattle());



             var thread = new Thread(
             (ThreadStart)delegate
             {
                 if (Engine.MatchNumber == 0)
                 {
                     //Launcher.spriteSystem.CreateManager(initialization.Team1[0].profile.NamefileSFF()/*, out Palette pal*/);
                     //Launcher.soundSystem.CreateManager(initialization.Team1[0].profile.NamefileSND());
                     Launcher.stateSystem.PreLoadStates(initialization.Team1[0].profile.states);
                     Launcher.animationSystem.CreateManager(initialization.Team1[0].profile.NamefileAIR(), initialization.Team1[0].profile.playerConstants.Scale);
                 }
                 //Launcher.spriteSystem.CreateManager(initialization.Team2[0].profile.NamefileSFF()/*, out Palette pal*/);
                 //Launcher.soundSystem.CreateManager(initialization.Team2[0].profile.NamefileSND());
                 Launcher.stateSystem.PreLoadStates(initialization.Team2[0].profile.states);
                 Launcher.animationSystem.CreateManager(initialization.Team2[0].profile.NamefileAIR(), initialization.Team1[0].profile.playerConstants.Scale);
                 completeLoad = true;
             });
             thread.Start();

        }
        bool completeLoad = false;

        IEnumerator LoadBattle()
        {
            while (!completeLoad)
                yield return null;

            //yield return new WaitForSeconds(0.5f);//3
            LoadStageBattle(Launcher.profileLoader.stageProfiles[initialization.stageID].Name);
        }

        void LoadStageBattle(string stageName)
        {
            GameObject init = new GameObject();
            init.name = "LoadSceneCustom";
            init.hideFlags = HideFlags.HideInHierarchy;
            new LoadBattleScene().Iniciar(stageName, Color.black, 2.5f, true);
        }

        void SetImagesBattle()
        {
            Dictionary<int, int> orderBattleArcade = Launcher.profileLoader.orderBattleArcade;
            for (int i = 0; i < orderBattleArcade.Count; i++)
            {
                int character = orderBattleArcade.Keys.ElementAt(i);
                int stage = orderBattleArcade.Values.ElementAt(i);

                battles[i].sprite = Launcher.profileLoader.profiles[character].largePortrait;

                // , StageProfiles[stage]
            }
        }

        void Update()
        {
            float duration = 2;

            battles[Engine.MatchNumber].color = Color.Lerp(battleHiden, battleShow, t);
            if (t < 1)
                t += Time.deltaTime / duration;
        }
    }
}