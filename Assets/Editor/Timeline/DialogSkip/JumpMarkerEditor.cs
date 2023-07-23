using UnityEditor.Timeline;
using UnityEngine.Timeline;
using UnityMugen.Timeline;

namespace UnityMugenEditor.Timeline
{

    [CustomTimelineEditor(typeof(JumpMarker))]
    internal class JumpMarkerEditor : MarkerEditor
    {
        public override void OnCreate(IMarker marker, IMarker clonedFrom)
        {
            DestinationMarker destin = marker.parent.CreateMarker<DestinationMarker>(marker.time + 2);
            destin.name = (marker.parent.GetMarkerCount() / 2).ToString();

            var jump = marker as JumpMarker;
            jump.destinationMarker = destin;

            base.OnCreate(marker, clonedFrom);
        }
    }

}