﻿using System;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("AssertSpecial")]
    public class AssertSpecial : StateController
    {
        private Assertion m_assert1;
        private Assertion m_assert2;
        private Assertion m_assert3;

        public AssertSpecial(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection)
        {
        }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_assert1 = textSection.GetAttribute("flag", Assertion.None);
                m_assert2 = textSection.GetAttribute("flag2", Assertion.None);
                m_assert3 = textSection.GetAttribute("flag3", Assertion.None);
            }
        }

        public override void Run(Character character)
        {
            Load();

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