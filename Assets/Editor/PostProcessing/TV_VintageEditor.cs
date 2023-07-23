using UnityEditor.Rendering.PostProcessing;

[PostProcessEditor(typeof(TV_Vintage))]
public sealed class TV_VintageEditor : PostProcessEffectEditor<TV_Vintage>
{
    //  SerializedParameterOverride m_Time;
    SerializedParameterOverride m_distortion;
    SerializedParameterOverride m_intensity;

    public override void OnEnable()
    {
        //    m_Time = FindParameterOverride(x => x.time);
        m_distortion = FindParameterOverride(x => x.distortion);
        m_intensity = FindParameterOverride(x => x.intensity);
    }

    public override void OnInspectorGUI()
    {
        //    PropertyField(m_Time);
        PropertyField(m_distortion);
        PropertyField(m_intensity);
    }
}
