using UnityEditor.Rendering.PostProcessing;

[PostProcessEditor(typeof(_2xsal))]
public sealed class _2xsalEditor : PostProcessEffectEditor<_2xsal>
{
    SerializedParameterOverride m_blend;
    SerializedParameterOverride m_color;
    SerializedParameterOverride m_resolution;

    public override void OnEnable() { }

    public override void OnInspectorGUI() { }
}
