using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("BindToRoot")]
    public class BindToRoot : StateController
    {
        private Expression m_time;
        private Expression m_facing;
        private Expression m_position;

        public BindToRoot(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_time = textSection.GetAttribute<Expression>("time", null);
                m_facing = textSection.GetAttribute<Expression>("facing", null);
                m_position = textSection.GetAttribute<Expression>("pos", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var helper = character as UnityMugen.Combat.Helper;
            if (helper == null)
            {
                Debug.Log("BindToRoot : helper Required");
                return;
            }

            var time = EvaluationHelper.AsInt32(character, m_time, 1);
            var facing = EvaluationHelper.AsInt32(character, m_facing, 0);
            var offset = EvaluationHelper.AsVector2(character, m_position, Vector2.zero) * Constant.Scale;

            helper.Bind.Set(helper.BasePlayer, offset, time, facing, false);
        }

    }
}