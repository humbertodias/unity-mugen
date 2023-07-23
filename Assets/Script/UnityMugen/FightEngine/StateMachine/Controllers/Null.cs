using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("Null")]
    public class Null : StateController
    {

        // Continuar usando isso pois existem muitos codigos que usam c.SetVar (equivalente a :=) nos trigger,
        // Mesmo estando em [State Null] esses codigos podem ser acessados.
        public Null(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection)
        {
        }

        public override void Run(Character character)
        {
            base.Load();
        }
    }
}