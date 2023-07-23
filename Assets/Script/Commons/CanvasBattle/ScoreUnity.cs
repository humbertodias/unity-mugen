using System;
using UnityEngine;
using UnityEngine.UI;
using UnityMugen.Combat;

namespace UnityMugen.Interface
{

    public class ScoreUnity : MonoBehaviour
    {
        LauncherEngine Launcher => LauncherEngine.Inst;
        FightEngine Engine => Launcher.mugen.Engine;

        public Text scoreP1;

        public void UpdateFE()
        {
            if (Engine == null) throw new ArgumentNullException(nameof(Engine));

            Player player1 = Engine.Team1.MainPlayer;

            scoreP1.text = player1.Score.ToString("f0").PadLeft(13, '0');
        }
    }
}