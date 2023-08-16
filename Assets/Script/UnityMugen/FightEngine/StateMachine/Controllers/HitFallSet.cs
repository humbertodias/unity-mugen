using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("HitFallSet")]
    public class HitFallSet : StateController
    {

        private Expression m_fallSet;
        private Expression m_velx;
        private Expression m_vely;

        public HitFallSet(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_fallSet = GetAttribute<Expression>(expression, null);
                    break;
                case "xvel":
                    m_velx = GetAttribute<Expression>(expression, null);
                    break;
                case "yvel":
                    m_vely = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var fallset = EvaluationHelper.AsInt32(character, m_fallSet, -1);
            var velx = EvaluationHelper.AsSingle(character, m_velx, null) * Constant.Scale;
            var vely = EvaluationHelper.AsSingle(character, m_vely, null) * Constant.Scale;

            if (fallset == 0) character.DefensiveInfo.HitDef.Fall = false;
            else if (fallset == 1) character.DefensiveInfo.HitDef.Fall = true;

            if (velx != null) character.DefensiveInfo.HitDef.FallVelocityX = velx.Value;
            if (vely != null) character.DefensiveInfo.HitDef.FallVelocityY = vely.Value;
        }

    }
}