using UnityEngine;
using UnityMugen;

namespace UnityMugen.Interface
{

    public class HudCanvasBattleUnity : MonoBehaviour
    {
        LauncherEngine Launcher => LauncherEngine.Inst;

        public TimerUnity timerUnity;
        public TeamLifeBarUnity teamLifeBarUnity;
        public PowerBarUnity powerBarUnity;
        public ComboCounterUnity comboCounterUnity;
        public ScoreUnity scoreUnity;
        public DiagnosticSystemUnity diagnosticSystemUnity;
        public Diagnostic2Unity diagnostic2;
        public TrainerButtons trainerButtons;
        public HistoryInput historyInput;
        public WinnerUnity winnerUnity;
        public RoundInformation roundInformation;
        public CanvasTransition canvasTransition;
        public RecSystem recSystem;

        public GameObject graphicUIBack;
        public GameObject graphicUIFront;

        private CombatMode combatMode = CombatMode.None;

        public void UpdateFE()
        {
            if (Launcher.mugen.Engine.Assertions.NoBarDisplay == false)
            {
                timerUnity.UpdateFE();
                teamLifeBarUnity.UpdateFE();
                powerBarUnity.UpdateFE();
                scoreUnity.UpdateFE();

                timerUnity.gameObject.SetActive(true);
                teamLifeBarUnity.gameObject.SetActive(true);
                powerBarUnity.gameObject.SetActive(true);
                graphicUIBack.SetActive(true);
                graphicUIFront.SetActive(true);
            }
            else
            {
                timerUnity.gameObject.SetActive(false);
                teamLifeBarUnity.gameObject.SetActive(false);
                powerBarUnity.gameObject.SetActive(false);
                graphicUIBack.SetActive(false);
                graphicUIFront.SetActive(false);
            }

            comboCounterUnity.UpdateFE();
            diagnosticSystemUnity.UpdateFE(combatMode);
            diagnostic2.UpdateFE(combatMode);
            trainerButtons.UpdateFE(combatMode);
            historyInput.UpdateFE(combatMode);
            winnerUnity.UpdateFE();
            roundInformation.UpdateFE();
            canvasTransition.UpdateFE();
        }

        public void RecordUpdate()
        {
            recSystem.RecordUpdateFE(combatMode);
        }

        public void StartFE(CombatMode _combatMode)
        {
            this.combatMode = _combatMode;
            winnerUnity.StartFE();
        }
    }
}