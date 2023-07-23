using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("StateTypeSet")]
    public class StateTypeSet : StateController
    {

        private StateType m_stateType;
        private MoveType m_moveType;
        private Physic m_physics;

        public StateTypeSet(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_stateType = textSection.GetAttribute("statetype", StateType.Unchanged);
                m_moveType = textSection.GetAttribute("movetype", MoveType.Unchanged);
                m_physics = textSection.GetAttribute("Physics", UnityMugen.Physic.Unchanged);
            }
        }

        public override void Run(Character character)
        {
            Load();

            if (m_stateType != StateType.Unchanged && m_stateType != StateType.None) character.StateType = m_stateType;
            if (m_moveType != MoveType.Unchanged && m_moveType != MoveType.None) character.MoveType = m_moveType;
            if (m_physics != Physic.Unchanged) character.Physics = m_physics;
        }
    }
}