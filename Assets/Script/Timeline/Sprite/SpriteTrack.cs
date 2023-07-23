using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityMugen.Timeline
{

    [Serializable]
    [TrackColor(1, 1, 1)]
    [TrackClipType(typeof(SpritePlayableAsset))]
    public class SpriteTrack : TrackAsset, ISerializationCallbackReceiver, IPropertyPreview
    {
        // Creates a runtime instance of the track, represented by a PlayableBehaviour.
        // The runtime instance performs mixing on the timeline clips.
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<SpriteControlBehaviour>.Create(graph, inputCount);
        }

        // Invoked by the timeline editor to put properties into preview mode. This permits the timeline
        // to temporarily change fields for the purpose of previewing in EditMode.
        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            SpriteRenderer trackBinding = director.GetGenericBinding(this) as SpriteRenderer;
            if (trackBinding == null)
                return;

            // The field names are the name of the backing serializable field. These can be found from the class source,
            // or from the unity scene file that contains an object of that type.
            driver.AddFromName<SpriteRenderer>(trackBinding.gameObject, "m_Color");

            base.GatherProperties(director, driver);
        }
    }

}