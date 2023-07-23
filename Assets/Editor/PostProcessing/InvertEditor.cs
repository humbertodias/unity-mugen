using UnityEditor.Rendering.PostProcessing;

[PostProcessEditor(typeof(Invert))]
public sealed class InvertEditor : PostProcessEffectEditor<Invert>
{
    SerializedParameterOverride m_blend;

    public override void OnEnable()
    {
        m_blend = FindParameterOverride(x => x.blend);
    }

    public override void OnInspectorGUI()
    {
        PropertyField(m_blend);
    }
}
