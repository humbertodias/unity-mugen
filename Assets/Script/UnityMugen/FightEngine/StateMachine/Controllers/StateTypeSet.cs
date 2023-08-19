using UnityMugen.Combat;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("StateTypeSet")]
    public class StateTypeSet : StateController
    {

        private StateType m_stateType;
        private MoveType m_moveType;
        private Physic m_physics;

        public StateTypeSet(string label) : base(label)
        {
            m_stateType = StateType.Unchanged;
            m_moveType = MoveType.Unchanged;
            m_physics = UnityMugen.Physic.Unchanged;
        }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "statetype":
                    m_stateType = GetAttribute(expression, StateType.Unchanged);
                    break;
                case "movetype":
                    m_moveType = GetAttribute(expression, MoveType.Unchanged);
                    break;
                case "physics":
                    m_physics = GetAttribute(expression, UnityMugen.Physic.Unchanged);
                    break;
            }
        }

        public override void Run(Character character)
        {
            if (m_stateType != StateType.Unchanged && m_stateType != StateType.None) character.StateType = m_stateType;
            if (m_moveType != MoveType.Unchanged && m_moveType != MoveType.None) character.MoveType = m_moveType;
            if (m_physics != Physic.Unchanged) character.Physics = m_physics;
        }
    }
}