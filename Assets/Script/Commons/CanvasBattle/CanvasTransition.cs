using System;
using UnityEngine;
using UnityEngine.UI;
using UnityMugen.Combat;
using UnityMugen.Combat.Logic;

namespace UnityMugen.Interface
{

    [Serializable]
    public class EffectTransition
    {
        public Animator effect;
        public AnimationClip animationClipIn;
        public AnimationClip animationClipOut;
    }

    public enum EffectSelect
    {
        Effect1,
        Effect2
    }

    public class CanvasTransition : MonoBehaviour
    {

        FightEngine Engine => LauncherEngine.Inst.mugen.Engine;

        public EffectSelect effectSelect;

        public EffectTransition effect1;
        public EffectTransition effect2;

        EffectTransition currentEffect;

        private void Awake()
        {
            if (effectSelect == EffectSelect.Effect1)
            {
                currentEffect = effect1;
            }
            else if (effectSelect == EffectSelect.Effect2)
            {
                currentEffect = effect2;
            }

            currentEffect.effect.gameObject.SetActive(true);
        }

        public void UpdateFE()
        {
            if (Engine == null) throw new ArgumentNullException(nameof(Engine));

            Base _base = Engine.m_logic;

            if (_base is ShowTransition)
            {
                ShowTransition showTransition = _base as ShowTransition;
                if (showTransition.typeShowTransition == TypeShowTransition.OUT)
                {
                    float timeAnim = currentEffect.animationClipOut.frameRate * currentEffect.animationClipIn.length;
                    float normalizedTime = (_base.TickCount % (timeAnim)) / (timeAnim);

                    if (_base is ShowTransition && _base.TickCount == -1)
                    {
                        currentEffect.effect.gameObject.SetActive(true);
                    }

                    if (_base is ShowTransition && _base.TickCount >= 0)
                    {
                        currentEffect.animationClipOut.SampleAnimation(currentEffect.effect.gameObject, normalizedTime);
                        if (_base.TickCount == Convert.ToInt32(timeAnim))
                        {
                            (_base as ShowTransition).isOver = true;
                            currentEffect.effect.gameObject.SetActive(false);
                        }
                    }
                }

                else if (showTransition.typeShowTransition == TypeShowTransition.IN)
                {
                    float timeAnim = currentEffect.animationClipIn.frameRate * currentEffect.animationClipIn.length;
                    float normalizedTime = (_base.TickCount % (timeAnim)) / (timeAnim);

                    if (_base is ShowTransition && _base.TickCount == 0)
                    {
                        currentEffect.effect.gameObject.SetActive(true);
                    }

                    if (_base is ShowTransition && _base.TickCount >= 0 && _base.TickCount <= Convert.ToInt32(timeAnim) - 1)
                    {
                        currentEffect.animationClipIn.SampleAnimation(currentEffect.effect.gameObject, normalizedTime);
                        if (_base.TickCount == Convert.ToInt32(timeAnim) - 1)
                        {
                            (_base as ShowTransition).isOver = true;
                            Engine.RoundInformation.DisableAllMessage();
                        }
                    }
                }

            }



        }
    }
}