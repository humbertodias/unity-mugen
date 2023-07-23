using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.Evaluation.Triggers;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("ScoreAdd")]
    public class ScoreAdd : StateController
    {
        private Expression m_value;

        public ScoreAdd(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_value = textSection.GetAttribute<Expression>("value", null);
            }
        }

#warning Novo State Adicionar a documentacao posteriormente
        public override void Run(Character character)
        {
            Load();

            var Value = EvaluationHelper.AsInt32(character, m_value, null);
            if (Value == null)
            {
                Debug.Log("ScoreAdd : value Required");
                return;
            }

            bool error = false;
            if (character is Player player)
                player.Score += Value.Value;
            else if (character is UnityMugen.Combat.Helper helper)
                (Root.RedirectState(helper, ref error) as Player).Score += Value.Value;
        }
    }
}