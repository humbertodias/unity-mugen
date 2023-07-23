using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{
#warning falta aplicar
    [StateControllerName("VictoryQuote")]
    public class VictoryQuote : StateController
    {

        private Expression m_index;
        private string m_language;

        public VictoryQuote(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_index = textSection.GetAttribute<Expression>("value ", null);
                m_language = textSection.GetAttribute<string>("language ", "Def");
            }
        }

        public override void Run(Character character)
        {
            Load();
        }

    }
}