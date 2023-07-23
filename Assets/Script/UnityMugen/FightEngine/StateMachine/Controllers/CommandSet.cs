using UnityMugen.Combat;
using Debug = UnityEngine.Debug;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("CommandSet")]
    public class CommandSet : StateController
    {
        private PlayerButton m_value;
        private Expression m_clearOld;

        public CommandSet(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection)
        {
            m_value = textSection.GetAttribute("value", PlayerButton.None);
        }

#warning olhar novamente
        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                
                m_clearOld = textSection.GetAttribute<Expression>("clear", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            if (m_value == PlayerButton.None)
            {
                Debug.Log("CommandSet : value Required");
                return;
            }

            //    var Value = EvaluationHelper.AsInt32(character, m_value, 0);
            var ClearOld = EvaluationHelper.AsInt32(character, m_clearOld, 0);

            if (character.Id == 0 && ClearOld > 0)
            {
                LauncherEngine.Inst.inputSystem.commandSetInputP1.ClearAll();
            }
            else if (character.Id == 1 && ClearOld > 0)
            {
                LauncherEngine.Inst.inputSystem.commandSetInputP2.ClearAll();
            }

            if (character.Id == 0)
            {
                //LauncherEngine._inst.inputSystem.commandSetInputP1.ClearAll();
                if (m_value == PlayerButton.Down)
                    LauncherEngine.Inst.inputSystem.commandSetInputP1.PressDown = true;
                if (m_value == PlayerButton.Up)
                    LauncherEngine.Inst.inputSystem.commandSetInputP1.PressUp = true;
                if (m_value == PlayerButton.Right)
                    LauncherEngine.Inst.inputSystem.commandSetInputP1.PressRight = true;
                if (m_value == PlayerButton.Left)
                    LauncherEngine.Inst.inputSystem.commandSetInputP1.PressLeft = true;
            }
            else if (character.Id == 1)
            {
                //LauncherEngine._inst.inputSystem.commandSetInputP2.ClearAll();
                if (m_value == PlayerButton.Down)
                    LauncherEngine.Inst.inputSystem.commandSetInputP2.PressDown = true;
                if (m_value == PlayerButton.Up)
                    LauncherEngine.Inst.inputSystem.commandSetInputP2.PressUp = true;
                if (m_value == PlayerButton.Right)
                    LauncherEngine.Inst.inputSystem.commandSetInputP2.PressRight = true;
                if (m_value == PlayerButton.Left)
                    LauncherEngine.Inst.inputSystem.commandSetInputP2.PressLeft = true;
            }
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false) return false;
            if (m_value == PlayerButton.None) return false;

            return true;
        }

    }
}