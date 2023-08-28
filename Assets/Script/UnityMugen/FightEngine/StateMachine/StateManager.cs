using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityMugen.Collections;
using UnityMugen.Combat;
using UnityMugen.Evaluation.Triggers;

namespace UnityMugen.StateMachine
{

    public class StateManager
    {

        public Character Character => m_character;
        public ReadOnlyKeyedCollection<int, State> States => m_states;

        public int StateTime;
        public State CurrentState;
        public State PreviousState;
        public StateManager ForeignManager;
        public int StateNumber;

        private readonly StateSystem m_statesystem;
        private readonly Character m_character;
        private readonly ReadOnlyKeyedCollection<int, State> m_states;
        private readonly Dictionary<StateController, int> m_persistencemap;

        public StateManager(Character character, ReadOnlyKeyedCollection<int, State> states)
        {
            if (character == null) throw new ArgumentNullException(nameof(character));
            if (states == null) throw new ArgumentNullException(nameof(states));

            m_character = character;
            m_states = states;
            m_persistencemap = new Dictionary<StateController, int>();
            ForeignManager = null;
            StateTime = 0;

            CurrentState = null;
            PreviousState = null;
        }

        public void Clear()
        {
            ForeignManager = null;
            m_persistencemap.Clear();
            StateTime = 0;

            CurrentState = null;
            PreviousState = null;
        }

        public StateManager Clone(Character character)
        {
            if (character == null) throw new ArgumentNullException(nameof(character));

            return new StateManager(character, States);
        }

        private void ApplyState(State state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            m_persistencemap.Clear();

            if (state.physics != UnityMugen.Physic.Unchanged) Character.Physics = state.physics;
            if (state.type != StateType.Unchanged) Character.StateType = state.type;
            if (state.moveType != MoveType.Unchanged) Character.MoveType = state.moveType;

            int? ctrl = EvaluationHelper.AsInt32(m_character, state.playerControl, null);
            if (ctrl != null) Character.PlayerControl = ctrl.Value > 0 ? PlayerControl.InControl : PlayerControl.NoControl;

            int? anim = EvaluationHelper.AsInt32(m_character, state.animationNumber, null);
            if (anim != null) Character.SetLocalAnimation(anim.Value, 0);

            int? sprpriority = EvaluationHelper.AsInt32(m_character, state.spritePriority, null);
            if (sprpriority != null) Character.DrawOrder = sprpriority.Value;

            int? poweradd = EvaluationHelper.AsInt32(m_character, state.power, null);
            if (poweradd != null) Character.BasePlayer.Power += poweradd.Value;

            Vector2? velset = EvaluationHelper.AsVector2(m_character, state.velocity, null) * Constant.Scale;
            if (velset != null) Character.CurrentVelocity = velset.Value;

            bool facep2 = EvaluationHelper.AsBoolean(m_character, state.faceEnemy, false);

            if (facep2 && Misc.P2Dist(m_character).x < 0)
                Character.CurrentFacing = Character.FlipFacing(Character.CurrentFacing);

            int? jugglePoints = EvaluationHelper.AsInt32(m_character, state.jugglePoints, null);
            if (jugglePoints != null) Character.JugglePoints = jugglePoints.Value;

            bool hitdefpersist = EvaluationHelper.AsBoolean(m_character, state.hitdefPersistance, false);
            if (hitdefpersist == false)
            {
                Character.OffensiveInfo.ActiveHitDef = false;
                Character.OffensiveInfo.HitPauseTime = 0;
            }

            bool movehitpersist = EvaluationHelper.AsBoolean(m_character, state.movehitPersistance, false);
            if (movehitpersist == false)
            {
                Character.OffensiveInfo.MoveReversed = 0;
                Character.OffensiveInfo.MoveHit = 0;
                Character.OffensiveInfo.MoveGuarded = 0;
                Character.OffensiveInfo.MoveContact = 0;
            }

            bool hitcountpersist = EvaluationHelper.AsBoolean(m_character, state.hitCountPersistance, false);
            if (hitcountpersist == false)
            {
                Character.OffensiveInfo.HitCount = 0;
                Character.OffensiveInfo.UniqueHitCount = 0;
            }
        }

        public bool ChangeState(int statenumber)
        {
            if (statenumber < 0) throw new ArgumentOutOfRangeException(nameof(statenumber), "Cannot change to state with number less than zero");

            var state = GetState(statenumber, false);

            if (state == null)
            {
                //State s = new State();
                //s.type = StateType.Unchanged;
                //s.moveType = MoveType.Unchanged;
                //s.physics = Physic.Unchanged;
                CurrentState = state = GetState(int.MaxValue, false);
                StateTime = -1;
                return false;
            }

            PreviousState = CurrentState;
            CurrentState = state;

            StateNumber = statenumber;
            StateTime = -1;
            return true;
        }

        private void RunCurrentStateLoop(bool hitpause)
        {
            while (true)
            {
                if (StateTime == -1)
                {
                    StateTime = 0;
                    if (CurrentState == null) break;
                    ApplyState(CurrentState);
                }
                if (CurrentState == null) break;

                if (RunState(CurrentState, hitpause) == false) break;
            }
        }
        public void Run(bool hitpause)
        {
            if (Character is Combat.Helper)
            {
                if ((Character as Combat.Helper).Data.KeyControl)
                {
                    RunState(-4, true, hitpause);
                    RunState(-1, true, hitpause);
                }
            }
            else
            {
                RunState(-4, true, hitpause);

                if (ForeignManager == null)
                    RunState(-3, true, hitpause);

                RunState(-2, true, hitpause);
                RunState(-1, true, hitpause);
            }

            RunCurrentStateLoop(hitpause);

            if (hitpause == false)
            {
                ++StateTime;
            }
        }

        private bool RunState(int statenumber, bool forcelocal, bool hitpause)
        {
            var state = GetState(statenumber, forcelocal);
            if (state != null)
            {
                return RunState(state, hitpause);
            }

            return false;
        }

        private State GetState(int statenumber, bool forcelocal)
        {
            if (ForeignManager != null && forcelocal == false) return ForeignManager.GetState(statenumber, true);

            return States.Contains(statenumber) ? States[statenumber] : null;
        }

        private bool RunState(State state, bool hitpause)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            if ((state.number == 20)
                && m_character.Assertions.NoWalk == true)
            {
                ChangeState(0);
                return false;
            }

            foreach (var controller in state.controllers)
            {
                int persistent = EvaluationHelper.AsInt32(m_character, controller.persistence, 1);
                var persistencecheck = state.number < 0 || PersistenceCheck(controller, persistent);
                if (persistencecheck == false) continue;

                //if (controller.label == "105, End")
                //    ;

                var triggercheck = controller.triggerMap.Trigger(m_character);
                if (triggercheck == false) continue;

#warning ainda nao esta perfeito
                if (hitpause && state.number >= 0 &&
                    controller.ignorehitpause == false &&
                    (controller is UnityMugen.StateMachine.Controllers.ChangeState || controller is UnityMugen.StateMachine.Controllers.SelfState))
                {
                    if (!scHitTime.Contains(controller))
                        scHitTime.Add(controller);
                }
                else if (!hitpause && scHitTime.Count > 0)
                {
                    var stHitPause = scHitTime[0];
                    if (persistent == 0 || persistent > 1)
                        m_persistencemap[stHitPause] = persistent;

                    stHitPause.Run(m_character);

                    scHitTime.Remove(stHitPause);

                    if (stHitPause is UnityMugen.StateMachine.Controllers.ChangeState || stHitPause is UnityMugen.StateMachine.Controllers.SelfState)
                        return true;
                }

                if (hitpause && controller.ignorehitpause == false) continue;

                if (persistent == 0 || persistent > 1)
                    m_persistencemap[controller] = persistent;

                controller.Run(m_character);

                if (controller is UnityMugen.StateMachine.Controllers.ChangeState || controller is UnityMugen.StateMachine.Controllers.SelfState)
                    return true;
            }

            return false;
        }

        private List<StateController> scHitTime = new List<StateController>();

        private bool PersistenceCheck(StateController controller, int persistence)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));

            if (m_persistencemap.ContainsKey(controller) == false) return true;

            if (persistence == 0)
            {
                return false;
            }

            if (persistence == 1)
            {
                return true;
            }

            if (persistence > 1)
            {
                m_persistencemap[controller] += -1;

                return m_persistencemap[controller] <= 0;
            }

            return false;
        }

    }
}