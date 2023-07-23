using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("VelAdd")]
    public class VelAdd : StateController
    {
        private Expression m_x;
        private Expression m_y;

        public VelAdd(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_x = textSection.GetAttribute<Expression>("x", null);
                m_y = textSection.GetAttribute<Expression>("y", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var x = EvaluationHelper.AsSingle(character, m_x, 0) * Constant.Scale;
            var y = EvaluationHelper.AsSingle(character, m_y, 0) * Constant.Scale;

            character.CurrentVelocity += new Vector2(x, y);
        }
    }
}