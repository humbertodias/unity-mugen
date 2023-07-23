using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("ScreenBound")]
    public class ScreenBound : StateController
    {
        private Expression m_boundFlag;
        private Expression m_moveCamera;

        public ScreenBound(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_boundFlag = textSection.GetAttribute<Expression>("value", null);
                m_moveCamera = textSection.GetAttribute<Expression>("movecamera", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var boundflag = EvaluationHelper.AsBoolean(character, m_boundFlag, false);
            var movecamera = EvaluationHelper.AsVector2(character, m_moveCamera, new Vector2(0, 0));

            character.ScreenBound = boundflag;
            character.CameraFollowX = movecamera.x > 0;
            character.CameraFollowY = movecamera.y > 0;
        }
    }
}