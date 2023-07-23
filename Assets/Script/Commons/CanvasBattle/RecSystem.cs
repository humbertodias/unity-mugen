using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityMugen;
using UnityMugen.Combat;

namespace UnityMugen.Interface
{

    public class RecSystem : MonoBehaviour
    {

        public static LauncherEngine Launcher => LauncherEngine.Inst;
        public FightEngine Engine => Launcher.mugen.Engine;

        public Sprite play, rec, stop;
        public Image actionImage;
        public Text action, time;

        bool startRecord, playRecord;
        int countRecord;
        Dictionary<int, PlayerButton> addInputsP1 = new Dictionary<int, PlayerButton>();
        Dictionary<int, PlayerButton> addInputsP2 = new Dictionary<int, PlayerButton>();
         
        MemoryState memoryState;

        public void RecordUpdateFE(CombatMode combatMode)
        {
            if (Launcher.trainnerSettings.showInfoREC && combatMode == UnityMugen.CombatMode.Training)
            {
                gameObject.SetActive(true);

                if (Engine.RoundState == RoundState.Fight)
                    RecordUpdate();
            }
            else
            {
                gameObject.SetActive(false);

                if (addInputsP1.Count > 0 || addInputsP2.Count > 0)
                {
                    addInputsP1.Clear();
                    addInputsP2.Clear();
                }
            }
        }


        void RecordUpdate()
        {
            time.text = countRecord.ToString();

            if (UnityEngine.Input.GetKeyDown(KeyCode.F9))
            {
                memoryState = new MemoryState(Engine);

                actionImage.sprite = rec;
                action.text = "REC";
                countRecord = 0;
                addInputsP1.Clear();
                addInputsP2.Clear();
                startRecord = true;
                playRecord = false;
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.F10))
            {
                StopAction();
            }
            if (memoryState != null && UnityEngine.Input.GetKeyDown(KeyCode.F11))
            {
                memoryState.BackMemory(ref Launcher.mugen.Engine);

                actionImage.sprite = play;
                action.text = "PLAY";
                countRecord = 0;
                startRecord = false;
                playRecord = true;
            }

            if (startRecord)
            {
                int count = countRecord++;
                addInputsP1.Add(count, Engine.Team1.MainPlayer.CurrentInput);
                addInputsP2.Add(count, Engine.Team2.MainPlayer.CurrentInput);
            }

            else if (playRecord == true && startRecord == false && addInputsP1.Count > 0)
            {
                int count = countRecord++;
                if (count < addInputsP1.Count)
                {
                    Engine.Team1.MainPlayer.CurrentInput = addInputsP1[count];
                    Engine.Team2.MainPlayer.CurrentInput = addInputsP2[count];
                }
                else
                {
                    StopAction();
                }
            }
        }

        void StopAction()
        {
            actionImage.sprite = stop;
            action.text = "STOP";
            countRecord = 0;
            startRecord = false;
            playRecord = false;
        }
    }
}