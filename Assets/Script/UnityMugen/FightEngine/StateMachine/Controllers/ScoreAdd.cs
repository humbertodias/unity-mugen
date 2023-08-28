using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.Evaluation.Triggers;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("ScoreAdd")]
    public class ScoreAdd : StateController
    {
        private Expression m_value;

        public ScoreAdd(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_value = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

#warning Novo State Adicionar a documentacao posteriormente
        public override void Run(Character character)
        {
            var Value = EvaluationHelper.AsInt32(character, m_value, null);
            if (Value == null)
            {
                Debug.Log("ScoreAdd : value Required");
                return;
            }

            if (character is Player player)
                player.Score += Value.Value;
            else if (character is UnityMugen.Combat.Helper helper)
                helper.BasePlayer.Score += Value.Value;
        }
    }
}