using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("VelSet")]
    public class VelSet : StateController
    {
        private Expression m_x;
        private Expression m_y;

        public VelSet(StateSystem statesystem, string label, TextSection textsection)
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

            float? x = EvaluationHelper.AsSingle(character, m_x, null) * Constant.Scale;
            float? y = EvaluationHelper.AsSingle(character, m_y, null) * Constant.Scale;

            x = x ?? character.CurrentVelocity.x;
            y = y ?? character.CurrentVelocity.y;

            character.CurrentVelocity = new Vector2(x.Value, y.Value);
        }
    }
}