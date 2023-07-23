using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("HitFallSet")]
    public class HitFallSet : StateController
    {

        private Expression m_fallSet;
        private Expression m_velx;
        private Expression m_vely;

        public HitFallSet(StateSystem statesystem, string label, TextSection textsection)
                    : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_fallSet = textSection.GetAttribute<Expression>("value", null);
                m_velx = textSection.GetAttribute<Expression>("xvel", null);
                m_vely = textSection.GetAttribute<Expression>("yvel", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

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