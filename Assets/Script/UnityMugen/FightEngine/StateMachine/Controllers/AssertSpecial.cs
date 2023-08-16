using System;
using UnityMugen.Combat;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("AssertSpecial")]
    public class AssertSpecial : StateController
    {
        private Assertion m_assert1;
        private Assertion m_assert2;
        private Assertion m_assert3;

        public AssertSpecial(string label) : base(label)
        {
            m_assert1 = Assertion.None;
            m_assert2 = Assertion.None;
            m_assert3 = Assertion.None;
        }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "flag":
                    m_assert1 = GetAttribute(expression, Assertion.None);
                    break;
                case "flag2":
                    m_assert2 = GetAttribute(expression, Assertion.None);
                    break;
                case "flag3":
                    m_assert3 = GetAttribute(expression, Assertion.None);
                    break;
            }
        }

        public override void Run(Character character)
        {
            if (HasAssert(Assertion.NoAutoturn))
            {
                character.Assertions.NoAutoTurn = true;
            }

            if (HasAssert(Assertion.NoJuggleCheck))
            {
                character.Assertions.NoJuggleCheck = true;
            }

            if (HasAssert(Assertion.NoKOSound))
            {
                character.Engine.Assertions.NoKOSound = true;
            }

            if (HasAssert(Assertion.NoKOSlow))
            {
                character.Engine.Assertions.NoKOSlow = true;
            }

            if (HasAssert(Assertion.NoShadow))
            {
                character.Assertions.NoShadow = true;
            }

#warning GlobalNoShadow é novo adicionar a documentação
            if (HasAssert(Assertion.GlobalNoShadow)) // ok
            {
                character.Engine.Assertions.GlobalNoShadow = true;
            }

            if (HasAssert(Assertion.NoMusic)) // ok
            {
                character.Engine.Assertions.NoMusic = true;
            }

            if (HasAssert(Assertion.TimerFreeze)) // ok
            {
                character.Engine.Assertions.TimerFreeze = true;
            }

            if (HasAssert(Assertion.Unguardable)) // ok
            {
                character.Assertions.UnGuardable = true;
            }

            if (HasAssert(Assertion.Invisible)) // ok
            {
                character.Assertions.Invisible = true;
            }

            if (HasAssert(Assertion.NoWalk)) // ok
            {
                character.Assertions.NoWalk = true;
            }

            if (HasAssert(Assertion.NoStandGuard)) // ok
            {
                character.Assertions.NoStandingGuard = true;
            }

            if (HasAssert(Assertion.NoCrouchGuard)) // ok
            {
                character.Assertions.NoCrouchingGuard = true;
            }

            if (HasAssert(Assertion.NoAirGuard)) // ok
            {
                character.Assertions.NoAirGuard = true;
            }

            if (HasAssert(Assertion.Intro)) // ok
            {
                character.Engine.Assertions.Intro = true;
            }

            if (HasAssert(Assertion.NoBarDisplay)) // ok
            {
                character.Engine.Assertions.NoBarDisplay = true;
            }

            if (HasAssert(Assertion.RoundNotOver)) // ok
            {
                character.Engine.Assertions.WinPose = true;
            }

            if (HasAssert(Assertion.NoForeground)) // ok
            {
                character.Engine.Assertions.NoFrontLayer = true;
            }

            if (HasAssert(Assertion.NoBackground)) // ok
            {
                character.Engine.Assertions.NoBackLayer = true;
            }

#warning NoKO é novo adicionar a documentação
            if (HasAssert(Assertion.NoKO)) // ok
            {
                character.Assertions.NoKO = true;
            }
        }

        private bool HasAssert(Assertion assert)
        {
            if (assert == Assertion.None) throw new ArgumentOutOfRangeException(nameof(assert));

            return m_assert1 == assert || m_assert2 == assert || m_assert3 == assert;
        }

    }
}