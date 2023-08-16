using System;
using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{
    [Obsolete("HitVelSet is deprecated.")]
    [StateControllerName("HitVelSet")]
    public class HitVelSet : StateController
    {
        private Expression m_velX;
        private Expression m_velY;

        public HitVelSet(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "x":
                    m_velX = GetAttribute<Expression>(expression, null);
                    break;
                case "y":
                    m_velY = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
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