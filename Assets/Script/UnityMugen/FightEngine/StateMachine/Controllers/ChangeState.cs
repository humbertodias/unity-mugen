﻿using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("ChangeState")]
    public class ChangeState : StateController
    {
        private Expression m_stateNumber;
        private Expression m_animationNumber;

        /// <summary>
        /// 0= NoControl, 1= InControl
        /// </summary>
        private Expression m_control;

        public ChangeState(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection)
        {
            m_stateNumber = textSection.GetAttribute<Expression>("value", null);
        }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_control = textSection.GetAttribute<Expression>("ctrl", null);
                m_animationNumber = textSection.GetAttribute<Expression>("anim", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var statenumber = EvaluationHelper.AsInt32(character, m_stateNumber, null);
            var playercontrol = EvaluationHelper.AsBoolean(character, m_control, null);
            var animationnumber = EvaluationHelper.AsInt32(character, m_animationNumber, null);

            if (statenumber == null)
            {
                Debug.Log("ChangeState : value Required");
                return;
            }

            character.StateManager.ChangeState(statenumber.Value);

            if (playercontrol != null)
            {
                if (playercontrol == true) character.PlayerControl = PlayerControl.InControl;
                if (playercontrol == false) character.PlayerControl = PlayerControl.NoControl;
            }

            if (animationnumber != null)
                character.SetLocalAnimation(animationnumber.Value, 0);
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_stateNumber == null)
                return false;

            return true;
        }

    }
}