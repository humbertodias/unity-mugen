using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UnityMugen.Interface
{

    [RequireComponent(typeof(Slider))]
    public class MixerSliderLink : MonoBehaviour
    {
        public AudioMixer mixer;
        public string mixerParameter;

        protected Slider m_Slider;

        //If we map the 0..1 range of the slider to the full volume attenuation range of the mixer (-80Db to 20Db), it looks like
        //the lower half the slider have no use, as under -20Db the clip is already silent. So we set 0 of the slider to equal -20Db instead
        static float MIN_VALUE = -80;
        static float MAX_VALUE = 20;
        static float VALUE_RANGE = MAX_VALUE - MIN_VALUE;

        void Awake()
        {
            m_Slider = GetComponent<Slider>();

            float value;
            mixer.GetFloat(mixerParameter, out value);

            m_Slider.value = (value - MIN_VALUE) / VALUE_RANGE;

            m_Slider.onValueChanged.AddListener(SliderValueChange);
        }


        void SliderValueChange(float value)
        {
            mixer.SetFloat(mixerParameter, MIN_VALUE + value * VALUE_RANGE);
        }
    }
}