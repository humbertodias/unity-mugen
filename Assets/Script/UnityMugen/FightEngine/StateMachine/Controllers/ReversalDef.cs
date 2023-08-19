using System;
using UnityMugen.Combat;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("ReversalDef")]
    public class ReversalDef : HitDef
    {
        public ReversalDef(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "reversal.attr":
                    m_hitAttr = GetAttribute<HitAttribute>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            if (character == null) throw new ArgumentNullException(nameof(character));

            character.OffensiveInfo.HitDef.ResetFE();

            base.Run(character);

            character.OffensiveInfo.MoveReversed = 1;

            character.OffensiveInfo.HitDef.HitAttribute = m_hitAttr;
        }

        public override bool IsValid()
        {
            if (m_hitAttr == null)
                return false;

            if (base.IsValid() == false)
                return false;

            return true;
        }
    }
}