using System;
using System.Collections.Generic;
using UnityEngine;
using UnityMugen;
using UnityMugen.Combat;
using UnityMugen.Combat.Logic;

namespace UnityMugen.Interface
{

    [Serializable]
    public class ElementRound
    {
        public int soundTime;
        public AudioClip audio;
        //public Animation anim;
        public GameObject gameObject;
    }

    // Caso de algum problema em criar uma nova animação
    // https://www.unity3dtips.com/the-animation-state-could-not-be-played-because-it-couldnt-be-found/
    // isso é para abilidatar o [Legacy] do animation.
    public class RoundInformation : MonoBehaviour
    {
        LauncherEngine Launcher => LauncherEngine.Inst;
        FightEngine Engine => Launcher.mugen.Engine;

        public AudioSource audioSource;

        public ElementRound fight;
        public ElementRound KO;
        public ElementRound drawGame;
        public ElementRound timeOver;
        public ElementRound winner;
        public ElementRound p1Win;
        public ElementRound p2Win;

        public int soundTimeRound = 0;
        public string roundText = "Round ";
        public Animation round;
        public ElementRound[] roundNumbers;

        public List<AudioClip> powerBars;


        public void UpdateFE()
        {
            if (Engine == null) throw new ArgumentNullException(nameof(Engine));

            Base _base = Engine.m_logic;
            UpdateMessage(_base);
            UpdateAudio(_base);
        }

        private void UpdateMessage(Base _base)
        {
            if (_base is DisplayRoundNumber && _base.TickCount == 0)
            {
                round.gameObject.SetActive(true);

                DisableAllRoundNumbers();
                if (Engine.RoundNumber <= roundNumbers.Length)
                    roundNumbers[Engine.RoundNumber - 1].gameObject.SetActive(true);

                round.Play();
            }
            else if (_base is DisplayRoundNumber && _base.TickCount > 0)
            {
                if (_base.TickCount == Convert.ToInt32(round.clip.frameRate * round.clip.length))
                {
                    (_base as DisplayRoundNumber).isOver = true;
                    round.gameObject.SetActive(false);
                    DisableAllRoundNumbers();
                }
            }


            if (_base is ShowFight && _base.TickCount == 0)
            {
                fight.gameObject.SetActive(true);
                fight.gameObject.GetComponent<Animation>().Play();
            }
            else if (_base is ShowFight && _base.TickCount > 0)
            {
                var anim = fight.gameObject.GetComponent<Animation>();
                if (_base.TickCount == Convert.ToInt32(anim.clip.frameRate * anim.clip.length))
                {
                    (_base as ShowFight).isOver = true;
                    fight.gameObject.SetActive(false);
                }
            }


            if (_base is CombatOver && _base.TickCount == 0)
            {
                CombatOver combatOver = _base as CombatOver;

                if (combatOver.m_typeCombatOver == TypeCombatOver.KO_WIN_P1 ||
                    combatOver.m_typeCombatOver == TypeCombatOver.KO_WIN_P2 ||
                    combatOver.m_typeCombatOver == TypeCombatOver.KO_WIN_P1_PERFECT ||
                    combatOver.m_typeCombatOver == TypeCombatOver.KO_WIN_P2_PERFECT)
                {
                    KO.gameObject.SetActive(true);
                    var anim = KO.gameObject.GetComponent<Animation>();
                    anim.Play();
                }
                else if (combatOver.m_typeCombatOver == TypeCombatOver.TimeOver_DrawGame ||
                         combatOver.m_typeCombatOver == TypeCombatOver.TimeOver_P1WIN ||
                         combatOver.m_typeCombatOver == TypeCombatOver.TimeOver_P2WIN ||
                         combatOver.m_typeCombatOver == TypeCombatOver.TimeOver_P1WIN_PERFECT ||
                         combatOver.m_typeCombatOver == TypeCombatOver.TimeOver_P2WIN_PERFECT)
                {
                    timeOver.gameObject.SetActive(true);
                    var anim = timeOver.gameObject.GetComponent<Animation>();
                    anim.Play();
                }
                else if (combatOver.m_typeCombatOver == TypeCombatOver.Draw_Game)
                {
                    drawGame.gameObject.SetActive(true);
                    var anim = drawGame.gameObject.GetComponent<Animation>();
                    anim.Play();
                }
            }
            else if (_base is CombatOver && _base.TickCount > 0)
            {
                var anim = KO.gameObject.GetComponent<Animation>();
                if (_base.TickCount == Convert.ToInt32(anim.clip.frameRate * anim.clip.length))
                {
                    (_base as CombatOver).isOver = true;
                    KO.gameObject.SetActive(false);
                    timeOver.gameObject.SetActive(false);
                    drawGame.gameObject.SetActive(false);
                }
            }

            if (_base is ShowWinPose && _base.TickCount == 0)
            {
                ShowWinPose showWinPose = _base as ShowWinPose;
                if (_base.CurrentElement == RoundInformationType.P1Win ||
                    _base.CurrentElement == RoundInformationType.P2Win)
                {
                    if (_base.CurrentElement == RoundInformationType.P1Win)
                    {
                        p1Win.gameObject.SetActive(true);
                        var p1Anim = p1Win.gameObject.GetComponent<Animation>();
                        p1Anim.Play();
                    }
                    else if (_base.CurrentElement == RoundInformationType.P2Win)
                    {
                        p2Win.gameObject.SetActive(true);
                        var p2Anim = p2Win.gameObject.GetComponent<Animation>();
                        p2Anim.Play();
                    }
                }
            }
            else if (_base is ShowWinPose && _base.TickCount > 0)
            {
                var p1Anim = p1Win.gameObject.GetComponent<Animation>();
                var p2Anim = p2Win.gameObject.GetComponent<Animation>();
                if (_base.TickCount == Convert.ToInt32(p1Anim.clip.frameRate * p1Anim.clip.length) ||
                    _base.TickCount == Convert.ToInt32(p2Anim.clip.frameRate * p2Anim.clip.length))
                {
                    (_base as ShowWinPose).isOver = true;
                    p1Win.gameObject.SetActive(false);
                    p2Win.gameObject.SetActive(false);
                }
            }
        }

        private void UpdateAudio(Base _base)
        {
            if (_base.CurrentElement == RoundInformationType.None) return;

            if (_base.CurrentElement == RoundInformationType.Round)
            {
                if (_base.TickCount == soundTimeRound)
                {
                    PlaySoundRound(Engine.RoundNumber);
                }
            }
            else if (_base.CurrentElement == RoundInformationType.Fight)
            {
                if (_base.TickCount == fight.soundTime)
                {
                    PlaySound(_base.CurrentElement);
                }
            }
            else if (_base.CurrentElement == RoundInformationType.TimeOver)
            {
                if (_base.TickCount == timeOver.soundTime)
                {
                    PlaySound(_base.CurrentElement);
                }
            }
            else if (_base.CurrentElement == RoundInformationType.DrawGame)
            {
                if (_base.TickCount == drawGame.soundTime)
                {
                    PlaySound(_base.CurrentElement);
                }
            }
            else if (_base.CurrentElement == RoundInformationType.KO)
            {
                if (_base.TickCount == p2Win.soundTime)
                {
                    PlaySound(_base.CurrentElement);
                }
            }
            else if (_base.CurrentElement == RoundInformationType.P1Win)
            {
                if (_base.TickCount == p1Win.soundTime)
                {
                    PlaySoundWin(1);
                }
            }
            else if (_base.CurrentElement == RoundInformationType.P2Win)
            {
                if (_base.TickCount == p2Win.soundTime)
                {
                    PlaySoundWin(2);
                }
            }
        }


        public void PlaySoundBar(int value = 0)
        {
            if (value >= powerBars.Count)
                value = powerBars.Count - 1;

            audioSource.clip = powerBars[value];
            audioSource.Play();
        }

        void PlaySoundFight()
        {
            audioSource.clip = fight.audio;
            audioSource.Play();
        }

        void PlaySound(RoundInformationType element)
        {
            if (element == RoundInformationType.KO && Engine.Assertions.NoKOSound == true)
                return;

            if (element == RoundInformationType.Fight)
                audioSource.clip = fight.audio;
            else if (element == RoundInformationType.TimeOver)
                audioSource.clip = timeOver.audio;
            else if (element == RoundInformationType.DrawGame)
                audioSource.clip = drawGame.audio;
            else if (element == RoundInformationType.KO)
                audioSource.clip = KO.audio;

            audioSource.Play();
        }

        void PlaySoundWin(int numberPlayer = 0)
        {
            if (numberPlayer == 0)
                audioSource.clip = winner.audio;
            else if (numberPlayer == 1)
                audioSource.clip = p1Win.audio;
            else if (numberPlayer == 2)
                audioSource.clip = p2Win.audio;

            audioSource.Play();
        }

        void PlaySoundRound(int value = 0)
        {
            if (value <= roundNumbers.Length)
            {
                audioSource.clip = roundNumbers[value - 1].audio;
                audioSource.Play();
            }
        }

        private void DisableAllRoundNumbers()
        {
            foreach (ElementRound go in roundNumbers)
            {
                go.gameObject.SetActive(false);
            }
        }

        public void DisableAllMessage()
        {
            fight.gameObject.SetActive(false);
            KO.gameObject.SetActive(false);
            drawGame.gameObject.SetActive(false);
            timeOver.gameObject.SetActive(false);
            //winner.gameObject.SetActive(false);
            p1Win.gameObject.SetActive(false);
            p2Win.gameObject.SetActive(false);

            round.gameObject.SetActive(false);
            DisableAllRoundNumbers();
        }

        public void StopAllSounds()
        {
            audioSource.Stop();
        }

    }
}