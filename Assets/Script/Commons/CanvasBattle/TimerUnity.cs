using System;
using UnityEngine;
using UnityEngine.UI;
using UnityMugen;
using UnityMugen.Combat;

namespace UnityMugen.Interface
{

    public class TimerUnity : MonoBehaviour
    {

        public int count = 0;

        private LauncherEngine Launcher => LauncherEngine.Inst;
        private FightEngine Engine => Launcher.mugen.Engine;


        public void UpdateFE()
        {

            if (Engine == null) throw new ArgumentNullException(nameof(Engine));

            Text texto = gameObject.GetComponent<Text>();
            if (!(Launcher.engineInitialization.Mode == CombatMode.Training))
            {
                count = Engine.Clock.Time;
                texto.text = count.ToString("f0").PadLeft(2, '0');
            }
            else
            {
                texto.text = "o"; //o == Infinito
            }
        }

    }
}