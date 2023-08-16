using System.Collections.Generic;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("TargetDrop")]
    public class TargetDrop : StateController
    {
        private Expression m_id;
        private Expression m_keepOne;

        public TargetDrop(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "excludeid":
                    m_id = GetAttribute<Expression>(expression, null);
                    break;
                case "keepone":
                    m_keepOne = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var excludeId = EvaluationHelper.AsInt32(character, m_id, -1);
            var keepone = EvaluationHelper.AsBoolean(character, m_keepOne, true);

            var removelist = new List<Character>();
            foreach (var target in character.GetTargets(int.MinValue))
            {
                if (excludeId != -1 && target.DefensiveInfo.HitDef.TargetId == excludeId) continue;

                removelist.Add(target);
            }

            if (removelist.Count > 0 && keepone) removelist.RemoveAt(0);

            foreach (var target in removelist) character.OffensiveInfo.TargetList.Remove(target);
        }
    }
}