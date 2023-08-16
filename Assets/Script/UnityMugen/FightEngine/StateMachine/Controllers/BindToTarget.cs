using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

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

        public BindToTarget(string label) : base(label) { }

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

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "time":
                    m_time = GetAttribute<Expression>(expression, null);
                    break;
                case "id":
                    m_targetId = GetAttribute<Expression>(expression, null);
                    break;
                case "pos":
                    m_pos = GetAttribute<string>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            ParsePositionString(m_pos, out Expression m_position, out BindToTargetPostion m_bindpos);

            var time = EvaluationHelper.AsInt32(character, m_time, 1);
            var targetId = EvaluationHelper.AsInt32(character, m_targetId, -1);
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