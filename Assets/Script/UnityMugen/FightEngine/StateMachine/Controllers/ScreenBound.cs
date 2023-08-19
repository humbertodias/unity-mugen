using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("ScreenBound")]
    public class ScreenBound : StateController
    {
        private Expression m_boundFlag;
        private Expression m_moveCamera;

        public ScreenBound(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_boundFlag = GetAttribute<Expression>(expression, null);
                    break;
                case "movecamera":
                    m_moveCamera = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var boundflag = EvaluationHelper.AsBoolean(character, m_boundFlag, false);
            var movecamera = EvaluationHelper.AsVector2(character, m_moveCamera, new Vector2(0, 0));

            character.ScreenBound = boundflag;
            character.CameraFollowX = movecamera.x > 0;
            character.CameraFollowY = movecamera.y > 0;
        }
    }
}