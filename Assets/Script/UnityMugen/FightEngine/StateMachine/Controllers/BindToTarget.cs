using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("BindToTarget")]
    public class BindToTarget : StateController
    {
        private Expression m_time;
        private Expression m_targetId;
        private string m_pos;
        private BindToTargetPostion m_bindPos;
        private Expression m_position;

        public BindToTarget(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection) { }

        private void ParsePositionString(string input, out Expression expression, out BindToTargetPostion postype)
        {
            expression = null;
            postype = BindToTargetPostion.None;

            if (input == null) return;

            var sepIndex = input.LastIndexOf(',');
            if (sepIndex == -1)
            {
                expression = LauncherEngine.Inst.evaluationSystem.CreateExpression(input);
                return;
            }

            var strExp = input.Substring(0, sepIndex).Trim();
            var strPostype = input.Substring(sepIndex + 1).Trim();

            if (LauncherEngine.Inst.stringConverter.TryConvert(strPostype, out BindToTargetPostion bttp))
            {
                expression = LauncherEngine.Inst.evaluationSystem.CreateExpression(strExp);
                postype = bttp;
            }
            else
            {
                expression = LauncherEngine.Inst.evaluationSystem.CreateExpression(input);
            }
        }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_time = textSection.GetAttribute<Expression>("time", null);
                m_targetId = textSection.GetAttribute<Expression>("id", null);
                m_pos = textSection.GetAttribute<string>("pos", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            ParsePositionString(m_pos, out Expression m_position, out BindToTargetPostion m_bindpos);

            var time = EvaluationHelper.AsInt32(character, m_time, 1);
            var targetId = EvaluationHelper.AsInt32(character, m_targetId, int.MinValue);
            var offset = EvaluationHelper.AsVector2(character, m_position, Vector2.zero) * Constant.Scale;

            foreach (var target in character.GetTargets(targetId))
            {
                switch (m_bindpos)
                {
                    case BindToTargetPostion.Mid:
                        offset += (target.BasePlayer.playerConstants.Midposition * Constant.Scale);
                        break;

                    case BindToTargetPostion.Head:
                        offset += (target.BasePlayer.playerConstants.Headposition * Constant.Scale);
                        break;
                }

                character.Bind.Set(target, offset, time, 0, false);
                break;
            }
        }
    }
}