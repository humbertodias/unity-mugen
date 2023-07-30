using System;
using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;
using UnityMugen.Video;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("GameMakeAnim")]
    public class GameMakeAnim : StateController
    {
        private Expression m_anim;
        private Expression m_under;
        private Expression m_position;
        private Expression m_random;

        public GameMakeAnim(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_anim = textSection.GetAttribute<Expression>("value", null);
                m_under = textSection.GetAttribute<Expression>("under", null);
                m_position = textSection.GetAttribute<Expression>("pos", null);
                m_random = textSection.GetAttribute<Expression>("random", null);
            }
        }

        [Obsolete("GameMakeAnim is deprecated, use Explod.")]
        public override void Run(Character character)
        {
            Load();

            var animationnumber = EvaluationHelper.AsInt32(character, m_anim, 0);
            var drawunder = EvaluationHelper.AsBoolean(character, m_under, false);
            var offset = EvaluationHelper.AsVector2(character, m_position, Vector2.zero) * Constant.Scale;
            var randomdisplacement = EvaluationHelper.AsInt32(character, m_random, 0);

            var data = new ExplodData();
            data.Scale = Vector2.one;
            data.AnimationNumber = animationnumber;
            data.CommonAnimation = true;
            data.Location = offset;
            data.PositionType = PositionType.P1;
            data.RemoveTime = -2;
            data.DrawOnTop = false;
            data.OwnPalFx = true;
            data.SpritePriority = drawunder ? -9 : 9;
            data.Random = new Vector2(randomdisplacement / 2, randomdisplacement / 2);
            data.Transparency = new Blending();
            data.Creator = character;
            data.Offseter = character;

            character.InstanceExplod(data);
        }
    }
}