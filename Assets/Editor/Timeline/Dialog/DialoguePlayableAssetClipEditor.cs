using UnityEditor.Timeline;
using UnityEngine.Timeline;
using UnityMugen.Timeline;

namespace UnityMugenEditor.Timeline
{

    // Editor used by the TimelineEditor to customize the view of a DialoguePlayableAsset
    [CustomTimelineEditor(typeof(DialoguePlayableAsset))]
    internal class DialoguePlayableAssetClipEditor : ClipEditor
    {
        // Called when a clip value, it's attached PlayableAsset, or an animation curve on a template is changed from the TimelineEditor.
        // This is used to keep the displayName of the clip matching the text of the PlayableAsset.
        public override void OnClipChanged(TimelineClip clip)
        {
            var textPlayableasset = clip.asset as DialoguePlayableAsset;
            if (textPlayableasset != null && !string.IsNullOrEmpty(textPlayableasset.template.dialogText))
                clip.displayName = textPlayableasset.template.dialogText;
        }
    }

}