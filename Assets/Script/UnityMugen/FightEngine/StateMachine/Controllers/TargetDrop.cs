using System.Collections.Generic;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("TargetDrop")]
    public class TargetDrop : StateController
    {
        private Expression m_id;
        private Expression m_keepOne;

        public TargetDrop(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_id = textSection.GetAttribute<Expression>("excludeID", null);
                m_keepOne = textSection.GetAttribute<Expression>("keepone", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

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