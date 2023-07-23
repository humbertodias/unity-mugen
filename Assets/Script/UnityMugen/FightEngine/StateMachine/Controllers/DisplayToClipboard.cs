using System;
using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("DisplayToClipboard")]
    public class DisplayToClipboard : StateController
    {
        private string m_formatString;
        private Expression m_params;

        public DisplayToClipboard(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection)
        {
            m_formatString = textSection.GetAttribute<string>("text", null);
        }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_params = textSection.GetAttribute<Expression>("params", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

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