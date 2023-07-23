using System;
using UnityEngine;
using UnityEngine.UI;
using UnityMugen;
using UnityMugen.Combat;

namespace UnityMugen.Interface
{

    public class PowerBarUnity : MonoBehaviour
    {

        public float charge = 1f;

        public float totalPowerBar = 4f;
        public float startPowerBar = 0;

        public Image powerbarP1;
        public Text textPowerP1;

        public Image powerbarP2;
        public Text textPowerP2;

        LauncherEngine Launcher => LauncherEngine.Inst;
        FightEngine Engine => Launcher.mugen.Engine;


        public void UpdateFE()
        {
            if (Engine == null) throw new ArgumentNullException(nameof(Engine));

            Player player1 = Engine.Team1.MainPlayer;
            Player player2 = Engine.Team2.MainPlayer;

            if (Launcher.engineInitialization.Mode == CombatMode.Training &&
                Launcher.trainnerSettings.powerRecovery != PowerRecovery.Normal)
            {
                if (Launcher.trainnerSettings.powerRecovery == PowerRecovery.Immediate)
                {
                    if (player1.LastTickPowerSet < Engine.TickCount - 150) // 2.5 Segundos
                        player1.Power = player1.playerConstants.MaximumPower;

                    if (player2.LastTickPowerSet < Engine.TickCount - 150) // 2.5 Segundos
                        player2.Power = player2.playerConstants.MaximumPower;
                }
                else if (Launcher.trainnerSettings.powerRecovery == PowerRecovery.Infinity)
                {
                    player1.Power = player1.playerConstants.MaximumPower;
                    player2.Power = player2.playerConstants.MaximumPower;
                }
            }

            var powerPercentageP1 = Math.Max(0.0f, player1.Power / (float)player1.playerConstants.MaximumPower);
            var powerPercentageP2 = Math.Max(0.0f, player2.Power / (float)player2.playerConstants.MaximumPower);

            powerbarP1.fillAmount = powerPercentageP1;
            textPowerP1.text = NumberBars(Int32.Parse(textPowerP1.text), player1.Power);

            powerbarP2.fillAmount = powerPercentageP2;
            textPowerP2.text = NumberBars(Int32.Parse(textPowerP2.text), player2.Power);

        }

        string NumberBars(int currentPower, float newPower)
        {
            string result = "0";
            if (currentPower < 1000 && newPower >= 1000 && newPower < 2000) result = "1";
            if (currentPower < 2000 && newPower >= 2000 && newPower < 3000) result = "2";
            if (currentPower < 3000 && newPower >= 3000 && newPower < 4000) result = "3";
            if (currentPower < 4000 && newPower >= 4000 && newPower < 5000) result = "4";
            if (currentPower < 5000 && newPower >= 5000 && newPower < 6000) result = "5";
            if (currentPower < 6000 && newPower >= 6000 && newPower < 7000) result = "6";
            if (currentPower < 7000 && newPower >= 7000 && newPower < 8000) result = "7";
            if (currentPower < 8000 && newPower >= 8000 && newPower < 9000) result = "8";
            if (currentPower < 9000 && newPower >= 9000 && newPower < 10000) result = "9";
            return result;
        }

        public void FullPowerBar()
        {
            Engine.Team1.MainPlayer.Power = 10000f;
            Engine.Team2.MainPlayer.Power = 10000f;

            if (Engine.Team1.TeamMate)
                Engine.Team1.TeamMate.Power = 10000f;
            if (Engine.Team2.TeamMate)
                Engine.Team2.TeamMate.Power = 10000f;
        }

    }
}