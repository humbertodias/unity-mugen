using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityMugen.Collections;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine
{

    [DebuggerDisplay("State #{number}, Controllers = {Controllers.Count}")]
    public class State
    {
        public int number;
        public ReadOnlyList<StateController> controllers;
        public StateType type = StateType.Standing;
        public MoveType moveType = MoveType.Idle;
        public UnityMugen.Physic physics = Physic.None;

        public Expression animationNumber;
        public Expression velocity;
        public Expression playerControl;
        public Expression power;
        public Expression hitdefPersistance;
        public Expression movehitPersistance;
        public Expression hitCountPersistance;
        public Expression spritePriority;
        public Expression jugglePoints;
        public Expression faceEnemy;

        public State(int number, List<StateController> controllers)
        {
            if (number < -4) // Atualizado para que funcione o ScoreState, Antigo -3
                throw new ArgumentOutOfRangeException(nameof(number), "State number must be greater then or equal to -4");

            if (controllers == null)
                throw new ArgumentNullException(nameof(controllers));

            this.number = number;
            this.controllers = new ReadOnlyList<StateController>(controllers);
        }

        public State(int number, TextSection textsection, List<StateController> controllers)
        {
            if (number < -4)
                throw new ArgumentOutOfRangeException(nameof(number), "State number must be greater then or equal to -3");
            if (textsection == null)
                throw new ArgumentNullException(nameof(textsection));
            if (controllers == null)
                throw new ArgumentNullException(nameof(controllers));

            this.number = number;
            this.controllers = new ReadOnlyList<StateController>(controllers);
            type = textsection.GetAttribute("type", StateType.Standing);
            moveType = textsection.GetAttribute("MoveType", MoveType.Idle);
            physics = textsection.GetAttribute("Physics", Physic.None);

            animationNumber = textsection.GetAttribute<Expression>("anim", null);
            velocity = textsection.GetAttribute<Expression>("velset", null);
            playerControl = textsection.GetAttribute<Expression>("ctrl", null);
            power = textsection.GetAttribute<Expression>("poweradd", null);
            jugglePoints = textsection.GetAttribute<Expression>("juggle", null);
            faceEnemy = textsection.GetAttribute<Expression>("facep2", null);
            hitdefPersistance = textsection.GetAttribute<Expression>("hitdefpersist", null);
            movehitPersistance = textsection.GetAttribute<Expression>("movehitpersist", null);
            hitCountPersistance = textsection.GetAttribute<Expression>("hitcountpersist", null);
            spritePriority = textsection.GetAttribute<Expression>("sprpriority", null);
        }

        public override string ToString()
        {
            return string.Format("State #{0}, Controllers = {1}", number, controllers.Count);
        }
    }
}