using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace UnityMugen.Timeline
{

    internal class Dialogue : MonoBehaviour, INotificationReceiver
    {
        public PlayableDirector playableDirector;

        [Header("UIs Text")]
        public Text nameLeftText;
        public Text nameRightText;
        public Text dialogText;

        [Header("UIs Frame")]
        public Image nameLeftFrame;
        public Image nameRightFrame;
        public Image dialogueFrame;
        public Image dialogueFrameDetail;

        [Header("Indicator")]
        public GameObject indicator;


        bool _sceneSkipped = true;
        double _timeToSkipTo;
        bool isPaused = false;
        Playable timelinePlayable;

        public void OnNotify(Playable origin, INotification notification, object context)
        {
            var jumpMarker = notification as JumpMarker;
            if (jumpMarker == null) return;

            var destinationMarker = jumpMarker.destinationMarker;
            if (destinationMarker != null)
            {
                timelinePlayable = origin.GetGraph().GetRootPlayable(0);
                _timeToSkipTo = destinationMarker.time;
                _sceneSkipped = false;
            }
        }

        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Return) && !_sceneSkipped && _timeToSkipTo > 0)
            {
                timelinePlayable.SetTime(_timeToSkipTo);
                _sceneSkipped = true;
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                _sceneSkipped = true;
                _timeToSkipTo = 0;
                playableDirector.time = 0;
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                if (isPaused)
                {
                    isPaused = false;
                    playableDirector.Play();
                    if (!indicator) indicator.GetComponent<Animation>()["Indicator"].speed = 1;
                }
                else
                {
                    isPaused = true;
                    playableDirector.Pause();
                    if (!indicator) indicator.GetComponent<Animation>()["Indicator"].speed = 0;
                }
            }
        }

        internal void Alignment(TextAnchor textAnchor)
        {
            //            if(alignment == TypeAlignment.Left)
            {
                dialogText.alignment = textAnchor;
            }
        }

        public void FadeFrames(float timeFade, TypeSideName typeSideName)
        {
            if (typeSideName == TypeSideName.Left)
            {
                if (nameLeftFrame != null) nameLeftFrame.color = new Color(nameLeftFrame.color.r, nameLeftFrame.color.g, nameLeftFrame.color.b, timeFade);
                if (nameRightFrame != null) nameRightFrame.color = new Color(nameRightFrame.color.r, nameRightFrame.color.g, nameRightFrame.color.b, 0);

                nameLeftText.color = new Color(nameLeftText.color.r, nameLeftText.color.g, nameLeftText.color.b, timeFade);
                if (nameRightText != null) nameRightText.color = new Color(nameRightText.color.r, nameRightText.color.g, nameRightText.color.b, 0);
            }
            else
            {
                if (nameLeftFrame != null) nameLeftFrame.color = new Color(nameLeftFrame.color.r, nameLeftFrame.color.g, nameLeftFrame.color.b, 0);
                if (nameRightFrame != null) nameRightFrame.color = new Color(nameRightFrame.color.r, nameRightFrame.color.g, nameRightFrame.color.b, timeFade);

                nameLeftText.color = new Color(nameLeftText.color.r, nameLeftText.color.g, nameLeftText.color.b, 0);
                if (nameRightText != null) nameRightText.color = new Color(nameRightText.color.r, nameRightText.color.g, nameRightText.color.b, timeFade);
            }
            dialogueFrame.color = new Color(dialogueFrame.color.r, dialogueFrame.color.g, dialogueFrame.color.b, timeFade);
            if (dialogueFrameDetail != null) dialogueFrameDetail.color = new Color(dialogueFrameDetail.color.r, dialogueFrameDetail.color.g, dialogueFrameDetail.color.b, timeFade);
        }

        public void Indicator(float timeFade)
        {
            if (indicator != null)
            {
                if (timeFade == 1)
                    indicator.SetActive(true);
                else
                    indicator.SetActive(false);
            }
        }

        public void OpenYoutubeChannel()
        {
            Application.OpenURL("https://www.youtube.com/@levelalfaomega");
        }

    }

    public enum TypeSideName
    {
        Left,
        Right
    }

}