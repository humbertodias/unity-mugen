using System;
using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{
    [Obsolete("HitVelSet is deprecated.")]
    [StateControllerName("HitVelSet")]
    public class HitVelSet : StateController
    {
        private Expression m_velX;
        private Expression m_velY;

        public HitVelSet(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_velX = textSection.GetAttribute<Expression>("x", null);
                m_velY = textSection.GetAttribute<Expression>("y", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var velx = EvaluationHelper.AsBoolean(character, m_velX, true);
            var vely = EvaluationHelper.AsBoolean(character, m_velY, true);

            var vel = character.DefensiveInfo.GetHitVelocity();

            if (character.DefensiveInfo.Attacker.CurrentFacing == character.CurrentFacing)
            {
                vel *= new Vector2(-1, 1);
            }

            if (velx == false) vel.x = character.CurrentVelocity.x;
            if (vely == false) vel.y = character.CurrentVelocity.y;

            character.CurrentVelocity = vel;

        }
    }
}