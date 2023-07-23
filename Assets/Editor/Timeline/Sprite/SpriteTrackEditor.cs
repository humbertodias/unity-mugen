using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;
using UnityMugen.Timeline;

namespace UnityMugenEditor.Timeline
{

    [CustomTimelineEditor(typeof(SpriteTrack))]
    public class SpriteTrackEditor : TrackEditor
    {
        Texture2D m_iconTexture;
        TrackEditor m_trackEditor;

        public SpriteTrackEditor(TrackEditor trackEditor)
        {
            m_trackEditor = trackEditor;
        }

        public override TrackDrawOptions GetTrackOptions(TrackAsset track, Object binding)
        {
            //track.name = "Sprite Track";

            if (!m_iconTexture)
            {
                m_iconTexture = Resources.Load<Texture2D>("sprite-renderer-icon");
            }

            var options = m_trackEditor.GetTrackOptions(track, binding);
            //options.trackColor = Color.blue;
            options.icon = m_iconTexture;
            return options;
        }
    }

}