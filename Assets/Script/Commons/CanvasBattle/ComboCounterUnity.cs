using System;
using UnityEngine;
using UnityEngine.UI;
using UnityMugen;
using UnityMugen.Combat;

namespace UnityMugen.Interface
{

    public class ComboCounterUnity : MonoBehaviour
    {

        FightEngine Engine => LauncherEngine.Inst.mugen.Engine;

        public Animator counterP1;
        public Animator counterP2;

        // Use this for initialization
        void Start()
        {
            counterP1.gameObject.SetActive(false);
            counterP2.gameObject.SetActive(false);
        }

        bool AnimatorIsPlaying(Animator animator)
        {
            if(animator.isActiveAndEnabled)
                return animator.GetCurrentAnimatorStateInfo(0).length >
                   animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            return false;
        }

        bool AnimatorIsPlaying(Animator animator, string stateName)
        {
            return AnimatorIsPlaying(animator) && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
        }

        void UpdateCombo(ComboCounter combo, Animator counter)
        {
            switch (combo.StateCombo)
            {
                case ComboCounter.State.NotShown:
                    if (!AnimatorIsPlaying(counter, "MovingOut"))
                    {
                        counter.gameObject.SetActive(false);
                        counter.transform.Find("CounterValue").GetComponent<Text>().text = "";
                        combo.HitBonus = 0;
                        combo.HitCount = 0;
                        combo.DisplayTimeCount = 0;
                    }
                    break;

                case ComboCounter.State.MovingIn:
                    counter.gameObject.SetActive(true);
                    counter.Play("MovingIn");
                    counter.transform.Find("CounterValue").GetComponent<Text>().text = (combo.CounterText + " " + combo.DisplayText);
                    combo.DisplayTimeCount = 0;
                    combo.StateCombo = ComboCounter.State.Shown;
                    break;

                case ComboCounter.State.Shown:
                    if(!AnimatorIsPlaying(counter, "MovingIn") &&
                        counter.transform.Find("CounterValue").GetComponent<Text>().text != (combo.CounterText + " " + combo.DisplayText))
                        counter.Play("PlusCombo");

                    counter.gameObject.SetActive(true);
                    counter.transform.Find("CounterValue").GetComponent<Text>().text = (combo.CounterText + " " + combo.DisplayText);
                    combo.DisplayTimeCount = Math.Max(0, combo.DisplayTimeCount - 1);
                    if (combo.DisplayTimeCount == 0) combo.StateCombo = ComboCounter.State.MovingOut;
                    break;

                case ComboCounter.State.MovingOut:
                    counter.gameObject.SetActive(true);
                    counter.Play("MovingOut");
                    counter.transform.Find("CounterValue").GetComponent<Text>().text = (combo.CounterText + " " + combo.DisplayText);
                    combo.DisplayTimeCount = 0;
                    combo.StateCombo = ComboCounter.State.NotShown;
                    Engine.Team1.MainPlayer.Score += Misc.AddScoreComboCount(combo.HitBonus);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("m_state");
            }
        }

        public void UpdateFE()
        {

            if (Engine == null) throw new ArgumentNullException(nameof(Engine));

            UpdateCombo(Engine.Team1.ComboCounter, counterP1);
            UpdateCombo(Engine.Team2.ComboCounter, counterP2);

        }
    }
}