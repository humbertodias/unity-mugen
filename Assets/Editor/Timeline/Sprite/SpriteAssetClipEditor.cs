using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;
using UnityMugen.Timeline;

namespace UnityMugenEditor.Timeline
{

    // Editor used by the TimelineEditor to customize the view of a SpritePlayableAsset
    [CustomTimelineEditor(typeof(SpritePlayableAsset))]
    internal class SpriteAssetClipEditor : ClipEditor
    {
        // Called by the Timeline Editor to draw the background of the timeline clip
        // when the clip has a SpritePlayableAsset attached
        public override void DrawBackground(TimelineClip clip, ClipBackgroundRegion region)
        {
            SpritePlayableAsset spriteAsset = clip.asset as SpritePlayableAsset;
            if (spriteAsset != null && spriteAsset.template.image != null)
            {
                // Load the preview or the thumbnail for the video
                Texture texturePreview = AssetPreview.GetAssetPreview(spriteAsset.template.image);
                if (texturePreview == null)
                    texturePreview = AssetPreview.GetMiniThumbnail(spriteAsset.template.image);

                if (texturePreview != null)
                {
                    Rect rect = region.position;
                    rect.width = texturePreview.width * rect.height / texturePreview.height;
                    GUI.DrawTexture(rect, texturePreview, ScaleMode.StretchToFill);
                }
            }
        }

        // Called when a clip value, it's attached PlayableAsset, or an animation curve on a template is changed from the TimelineEditor.
        // This is used to keep the displayName of the clip matching the text of the PlayableAsset.
        public override void OnClipChanged(TimelineClip clip)
        {
            var textPlayableasset = clip.asset as SpritePlayableAsset;
            if (textPlayableasset != null && !string.IsNullOrEmpty(textPlayableasset.template.image.name))
                clip.displayName = textPlayableasset.template.image.name;
        }
    }
}
