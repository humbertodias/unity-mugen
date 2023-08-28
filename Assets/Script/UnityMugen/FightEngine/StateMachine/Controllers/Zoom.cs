using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMugen;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.StateMachine;

[StateControllerName("Zoom")]
public class Zoom : StateController
{
    private Expression m_scale;
    private Expression m_pos;
    private Expression m_time;
    private Expression m_lag;

    public Zoom(string label) : base(label) { }

    public override void SetAttributes(string idAttribute, string expression)
    {
        base.SetAttributes(idAttribute, expression);
        switch (idAttribute)
        {
            case "scale":
                m_scale = GetAttribute<Expression>(expression, null);
                break;
            case "pos":
                m_pos = GetAttribute<Expression>(expression, null);
                break;
            case "time":
                m_time = GetAttribute<Expression>(expression, null);
                break;
            case "lag":
                m_lag = GetAttribute<Expression>(expression, null);
                break;
        }
    }

    public override void Run(Character character)
    {
        var scale = EvaluationHelper.AsSingle(character, m_scale, 1f);
        var pos = EvaluationHelper.AsVector2(character, m_pos, Vector2.zero) * Constant.Scale;
        var time = EvaluationHelper.AsInt32(character, m_time, 1);
        var lag = EvaluationHelper.AsSingle(character, m_lag, 1f);

        character.Engine.CameraFE.SetStateZoom(scale, pos, time, lag);

        //// isso tambem ten que ficar dentro de CmeraFE
        //if(time > 0)
        //{
        //    s.zoomlag = 0;
        //    s.zoomPosXLag = 0;
        //    s.zoomPosYLag = 0;
        //    s.zoomScale = 1;
        //    s.zoomPos = [2]float32{ 0, 0};
        //    s.drawScale = s.cam.Scale;
        //}
        ///////
        

        //isso esta em CameraFE, quando o time for maior que zero, preservar up, down, left, right.
        //    acho que a melhhor coisa que eu posso fazer é deixar esses parametros no proprio CameraFE. 
        //up = transUp.position.y;
        //down = transDown.position.y;
        //left = transLeft.position.x;
        //right = transRight.position.x;

        //sys.zoomPos[0] = sys.zoomScale * zoompos[0]
        //sys.zoomPos[1] = zoompos[1]
        //sys.enableZoomtime = t;
    }
}
