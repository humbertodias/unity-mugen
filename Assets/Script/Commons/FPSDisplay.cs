using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityMugen.Interface
{

    public class FPSDisplay : MonoBehaviour
    {

        public Text FramerateText;

        Queue<float> deltaTimeSamples = new Queue<float>();
        const float smoothDeltaTimePeriod = 0.5f;

        void Update()
        {
            if (FramerateText == null)
                return;

            FramerateText.text = (1.0f / GetSmoothDeltaTime()).ToString("F1");
        }

        float GetSmoothDeltaTime()
        {
            float time = Time.unscaledTime;
            deltaTimeSamples.Enqueue(time);

            if (deltaTimeSamples.Count > 1)
            {
                float startTime = deltaTimeSamples.Peek();
                float delta = time - startTime;

                float smoothDelta = delta / deltaTimeSamples.Count;

                if (delta > smoothDeltaTimePeriod)
                    deltaTimeSamples.Dequeue();

                return smoothDelta;
            }
            else
                return Time.unscaledDeltaTime;
        }

    }
}