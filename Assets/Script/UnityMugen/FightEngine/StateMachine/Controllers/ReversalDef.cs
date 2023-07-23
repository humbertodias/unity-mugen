using System;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("ReversalDef")]
    public class ReversalDef : HitDef
    {

        private HitAttribute m_reversalAttr;

        public ReversalDef(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection)
        {
            m_reversalAttr = textsection.GetAttribute<HitAttribute>("reversal.attr", null);
        }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();
            }
        }

#warning tem que ser testado - Tiago
        public override void Run(Character character)
        {
            if (character == null) throw new ArgumentNullException(nameof(character));

            Load();

            //continuar isso ainda não esta funcionando

            character.OffensiveInfo.HitDef.ResetFE();

            m_hitAttr = m_reversalAttr;
            base.Run(character);

            character.OffensiveInfo.MoveReversed = 1;

            character.OffensiveInfo.HitDef.HitAttribute = m_reversalAttr;
        }

        public override bool IsValid()
        {

            if (m_reversalAttr == null)
                return false;

            m_hitAttr = m_reversalAttr;

            if (base.IsValid() == false)
                return false;

            return true;
        }
    }
}