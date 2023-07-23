using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityMugen.Timeline
{

    [DisplayName("Jump/JumpMarker")]
    [CustomStyle("JumpMarker")]
    public class JumpMarker : Marker, INotification, INotificationOptionProvider
    {
        [SerializeField] public DestinationMarker destinationMarker;

        public PropertyName id { get; }

        NotificationFlags INotificationOptionProvider.flags => NotificationFlags.TriggerInEditMode;
    }

}