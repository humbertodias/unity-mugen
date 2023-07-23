using UnityEngine;
using UnityEngine.Playables;

namespace UnityMugen.Timeline
{

    // The runtime instance of a the DialogieTrack. It is responsible for blending and setting the final data
    // on the Text binding
    internal class DialogueTrackMixerBehaviour : PlayableBehaviour
    {
        Color m_DefaultColor;
        string m_DefaultDialogText;

        Dialogue m_TrackBinding;

        // Called every frame that the timeline is evaluated. ProcessFrame is invoked after its' inputs.
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            SetDefaults(playerData as Dialogue);
            if (m_TrackBinding == null)
                return;

            int inputCount = playable.GetInputCount();

            float greatestWeight = 0f;
            TypeSideName typeSideName = TypeSideName.Left;
            string nameText = "";
            string dialogText = m_DefaultDialogText;
            TextAnchor textAnchor = TextAnchor.UpperLeft;

            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                ScriptPlayable<DialoguePlayableBehaviour> inputPlayable = (ScriptPlayable<DialoguePlayableBehaviour>)playable.GetInput(i);
                DialoguePlayableBehaviour input = inputPlayable.GetBehaviour();

                // use the text with the highest weight
                if (inputWeight > greatestWeight)
                {
                    nameText = input.nameText;
                    dialogText = input.dialogText;
                    textAnchor = input.textAnchor;

                    greatestWeight = inputWeight;
                    typeSideName = input.typeSideName;

                    float calcFade = input.fadeDialogText / 0.05f;
                    m_TrackBinding.dialogText.color = new Color(m_DefaultColor.r, m_DefaultColor.g, m_DefaultColor.b, calcFade);
                }

            }

            m_TrackBinding.FadeFrames(greatestWeight, typeSideName);
            m_TrackBinding.Indicator(greatestWeight);

            m_TrackBinding.nameLeftText.text = nameText;
            if (m_TrackBinding.nameRightText != null) m_TrackBinding.nameRightText.text = nameText;
            m_TrackBinding.dialogText.text = dialogText;
            m_TrackBinding.Alignment(textAnchor);

        }

        // Invoked when the playable graph is destroyed, typically when PlayableDirector.Stop is called or the timeline
        // is complete.
        public override void OnPlayableDestroy(Playable playable)
        {
            RestoreDefaults();
        }

        void SetDefaults(Dialogue dialogue)
        {
            if (dialogue == m_TrackBinding)
                return;

            RestoreDefaults();

            m_TrackBinding = dialogue;
            if (m_TrackBinding != null)
            {
                m_DefaultColor = m_TrackBinding.dialogText.color;
                m_DefaultDialogText = m_TrackBinding.dialogText.text;
            }
        }


        void RestoreDefaults()
        {
            if (m_TrackBinding == null)
                return;

            m_TrackBinding.dialogText.color = m_DefaultColor;
            m_TrackBinding.dialogText.text = m_DefaultDialogText;
        }
    }
}
