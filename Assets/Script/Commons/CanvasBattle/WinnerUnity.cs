using System;
using UnityEngine;
using UnityEngine.UI;
using UnityMugen;
using UnityMugen.Combat;

namespace UnityMugen.Interface
{

    public class WinnerUnity : MonoBehaviour
    {


        //public Sprite[] winnersImage;

        [Header("Winners")]
        public Sprite normal;
        public Sprite special;
        public Sprite hyper;
        public Sprite normalThrow;
        public Sprite cheese;
        public Sprite time;
        public Sprite suicude;
        public Sprite teamKill;
        public Sprite perfect;

        public Sprite transparence;

        [Header("Images Update")]
        public Image[] finishP1;
        public Image[] finishP2;

        public static LauncherEngine Launcher => LauncherEngine.Inst;
        FightEngine engine => Launcher.mugen.Engine;


        public void StartFE()
        {
            for (int i = 0; i < finishP1.Length; i++)
            {
                if (i >= Launcher.initializationSettings.NumberOfRounds)
                {
                    finishP1[i].gameObject.SetActive(false);
                    finishP2[i].gameObject.SetActive(false);
                }
            }
        }

        public void UpdateFE()
        {
            if (engine == null) throw new ArgumentNullException(nameof(engine));

            Team team1 = engine.Team1;
            Team team2 = engine.Team2;

            Draw(team1, finishP1);
            Draw(team2, finishP2);

            if (engine.m_logic.IsFinished() && engine.IsMatchOver())
            {
                ResetWinners(team1, finishP1);
                ResetWinners(team2, finishP2);
            }

        }

        private void ResetWinners(Team team, Image[] finish)
        {
            if (team.Wins.Count == 0)
            {
                for (int i = 0; i < finishP1.Length; i++)
                {
                    finish[i].sprite = transparence;
                    finish[i].transform.GetChild(0).GetComponent<Image>().sprite = transparence;
                }
            }

        }

        private void Draw(Team team, Image[] finish)
        {

            for (int i = 0; i < team.Wins.Count; i++)
            {
                switch (team.Wins[i].Victory)
                {
                    case Victory.Normal:
                        finish[i].sprite = normal;
                        break;

                    case Victory.Special:
                        finish[i].sprite = special;
                        break;

                    case Victory.Hyper:
                        finish[i].sprite = hyper;
                        break;

                    case Victory.NormalThrow:
                        finish[i].sprite = normalThrow;
                        break;

                    case Victory.Cheese:
                        finish[i].sprite = cheese;
                        break;

                    case Victory.Time:
                        finish[i].sprite = time;
                        break;

                    case Victory.Suicude:
                        finish[i].sprite = suicude;
                        break;

                    case Victory.TeamKill:
                        finish[i].sprite = teamKill;
                        break;
                }

                if (team.Wins[i].IsPerfectVictory)
                    finish[i].transform.GetChild(0).GetComponent<Image>().sprite = perfect;
                //   if (win.IsPerfectVictory) m_winiconperfect.Draw(location);

            }
        }

    }
}