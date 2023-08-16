using UnityMugen.Combat;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("Null")]
    public class Null : StateController
    {

        // Continuar usando isso pois existem muitos codigos que usam c.SetVar (equivalente a :=) nos trigger,
        // Mesmo estando em [State Null] esses codigos podem ser acessados.
        public Null(string label) : base(label) { }

        public override void Run(Character character) { }
    }
}