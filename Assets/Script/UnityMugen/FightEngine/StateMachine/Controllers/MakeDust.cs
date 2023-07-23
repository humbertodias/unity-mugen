using System;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [Obsolete("This controller is deprecated; use the Explod controller.")]
    [StateControllerName("MakeDust")]
    public class MakeDust : StateController
    {
        private Expression m_pos;
        private Expression m_pos2;
        private Expression m_spacing;

        public MakeDust(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_pos = textSection.GetAttribute<Expression>("pos", null);
                m_pos2 = textSection.GetAttribute<Expression>("pos2", null);
                m_spacing = textSection.GetAttribute<Expression>("spacing", null);
            }
        }

        public override void Run(Character character)
        {
            Load();
        }

    }
}