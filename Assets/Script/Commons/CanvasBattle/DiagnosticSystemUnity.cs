using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityMugen.Combat;

namespace UnityMugen.Interface
{

    public class DiagnosticSystemUnity : MonoBehaviour
    {
        LauncherEngine Launcher => LauncherEngine.Inst;
        FightEngine Engine => LauncherEngine.Inst.mugen.Engine;

        private Text text;
        private StringBuilder m_stringbuilder;

        public GeralInfo geralInfo;
        public DiagnosticPlayer diagPlayer1;
        public DiagnosticPlayer diagPlayer2;

        public GameObject diagGeral;
        public GameObject diagP1;
        public GameObject diagP2;
        public GameObject diagFPS;

        private void Awake()
        {
            text = GetComponent<Text>();
            m_stringbuilder = new StringBuilder();
        }


        public void UpdateFE(UnityMugen.CombatMode combatMode)
        {
            if (m_stringbuilder == null)
                return;

            if (Engine == null) throw new ArgumentNullException(nameof(Engine));

            m_stringbuilder.Length = 0;

            if (Launcher.trainnerSettings.showInfoGeral && combatMode == UnityMugen.CombatMode.Training)
            {
                diagGeral.SetActive(true);
                BuildEntityText(Engine.Entities, Engine.TickCount);
            }
            else
                diagGeral.SetActive(false);

            if (Launcher.trainnerSettings.showInfoP1 && combatMode == UnityMugen.CombatMode.Training)
            {
                diagP1.SetActive(true);
                BuildPlayerText(Engine.Team1.MainPlayer, diagPlayer1);
            }
            else
                diagP1.SetActive(false);

            if (Launcher.trainnerSettings.showInfoP2 && combatMode == UnityMugen.CombatMode.Training)
            {
                diagP2.SetActive(true);
                BuildPlayerText(Engine.Team2.MainPlayer, diagPlayer2);
            }
            else
                diagP2.SetActive(false);

            if (Launcher.trainnerSettings.showFPS && combatMode == UnityMugen.CombatMode.Training)
            {
                diagFPS.SetActive(true);
            }
            else
                diagFPS.SetActive(false);
        }

        private void BuildEntityText(EntityCollection collection, float ticks)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            collection.CountEntities(out int players, out int helpers, out int explods, out int projectiles, out int graphicUIs);

            geralInfo.tick.text = String.Format("{0}", ticks);
            geralInfo.players.text = String.Format("{0}", players);
            geralInfo.helpers.text = String.Format("{0}", helpers);
            geralInfo.explods.text = String.Format("{0}", explods);
            geralInfo.projectiles.text = String.Format("{0}", projectiles);
            //geralInfo.graphicUIs.text = String.Format("{0}", graphicUIs);
        }

        private void BuildPlayerText(Player player, DiagnosticPlayer diagPlayer)
        {
            if (player != null)
            {
                diagPlayer.life.text = String.Format("{0} / {1}", player.Life, player.playerConstants.MaximumLife);
                diagPlayer.power.text = String.Format("{0} / {1}", player.Power, player.playerConstants.MaximumPower);
                diagPlayer.anim.text = String.Format("{0}", player.AnimationManager.CurrentAnimation.Number);
                diagPlayer.spri.text = String.Format("{0}", player.AnimationManager.CurrentElement.SpriteId);
                diagPlayer.elem.text = String.Format("{0} / {1}", player.AnimationManager.CurrentElement.Id + 1, player.AnimationManager.CurrentAnimation.Elements.Count);
                diagPlayer.tick.text = String.Format("{0} / {1}", player.AnimationManager.TimeLoop, player.AnimationManager.CurrentAnimation.TotalTime);
                diagPlayer.time.text = String.Format("{0}", player.AnimationManager.TimeInAnimation);

                diagPlayer.state.text = String.Format("{0}", player.StateManager.CurrentState.number);
                diagPlayer.stateTime.text = String.Format("{0}", player.StateManager.StateTime);
                diagPlayer.foreignSate.text = String.Format("{0}", player.StateManager.ForeignManager != null);

                diagPlayer.stateType.text = String.Format("{0}", player.StateType);
                diagPlayer.moveType.text = String.Format("{0}", player.MoveType);
                diagPlayer.phsyics.text = String.Format("{0}", player.Physics);
                diagPlayer.control.text = String.Format("{0}", player.PlayerControl);

                diagPlayer.activeHitDef.text = String.Format("{0}", player.OffensiveInfo.ActiveHitDef);
                diagPlayer.jugglePoints.text = String.Format("{0}", player.JugglePoints);

                diagPlayer.posRefCamera.text = string.Format("{0}", Misc.ScreenPos(player));//Trigger.ScreenPos
                //diagPlayer.posRefCamera.text = string.Format("{0}, {1}", Pos.Evaluate(player, ref error, Axis.X), Pos.Evaluate(player, ref error, Axis.Y));
                diagPlayer.posRefStage.text = String.Format("{0}", player.CurrentLocation.ToString());
                //player.Clipboard.ToString()
            }
        }

    }

    [Serializable]
    public class GeralInfo
    {
        public Text tick;
        public Text players;
        public Text helpers;
        public Text explods;
        public Text projectiles;
        public Text graphicUIs;
    }

    [Serializable]
    public class DiagnosticPlayer
    {
        [Header("Data Play")]
        public Text life;
        public Text power;
        public Text anim;
        public Text spri;
        public Text elem;
        public Text tick;
        public Text time;
        public Text state;
        public Text stateTime;
        public Text foreignSate;
        public Text stateType;
        public Text moveType;
        public Text phsyics;
        public Text control;
        public Text activeHitDef;
        public Text jugglePoints;

        [Header("Data Screen")]
        public Text posRefCamera;
        public Text posRefStage;
    }
}