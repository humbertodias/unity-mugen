using System;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [Obsolete("This controller is deprecated; use the Explod controller.")]
    [StateControllerName("MakeDust")]
    public class MakeDust : StateController
    {
        private Expression m_pos;
        private Expression m_pos2;
        private Expression m_spacing;

        public MakeDust(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "pos":
                    m_pos = GetAttribute<Expression>(expression, null);
                    break;
                case "pos2":
                    m_pos2 = GetAttribute<Expression>(expression, null);
                    break;
                case "spacing":
                    m_spacing = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {

        }

    }
}