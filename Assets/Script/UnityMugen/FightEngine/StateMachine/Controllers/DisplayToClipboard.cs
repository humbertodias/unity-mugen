using System;
using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("DisplayToClipboard")]
    public class DisplayToClipboard : StateController
    {
        protected string m_formatString;
        protected Expression m_params;

        public DisplayToClipboard(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "text":
                    m_formatString = GetAttribute<string>(expression, null);
                    break;
                case "params":
                    m_params = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            if (m_params == null && m_formatString == null)
            {
                Debug.Log("DisplayToClipboard : value Required");
                return;
            }

            if (m_params != null)
            {
                character.Clipboard.Length = 0;
                character.Clipboard.Append(BuildString(m_params.Evaluate(character)));
            }
            else
            {
                character.Clipboard.Length = 0;
                character.Clipboard.Append(m_formatString);
            }
        }

        protected string BuildString(Number[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));

            return new StringFormatter().BuildString(m_formatString, args);
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_formatString == null)
                return false;

            return true;
        }

    }
}