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
    }
}
