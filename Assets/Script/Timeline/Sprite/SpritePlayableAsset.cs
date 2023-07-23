using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityMugen.Timeline
{

    [System.Serializable/*, NotKeyable*/] // Se  adicionar NotKeyable desabilitara o buttom [hide curves view]
    public class SpritePlayableAsset : PlayableAsset, ITimelineClipAsset//, IPropertyPreview
    {

        [SerializeField]
        public SpriteControlBehaviour template = new SpriteControlBehaviour();


        GameObject spriteRen;

        public ClipCaps clipCaps
        {
            get { return ClipCaps.Blending; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            if (template == null || template.image == null)
                return Playable.Create(graph);

            if (spriteRen == null)
            {
                spriteRen = new GameObject(template.image.name) { hideFlags = HideFlags.HideAndDontSave };
                spriteRen.transform.position = template.position;
                spriteRen.transform.localScale = template.scale;
                SpriteRenderer spriteRenderer = spriteRen.AddComponent<SpriteRenderer>();
                spriteRenderer.sortingOrder = template.orderInLayer;
                spriteRenderer.flipX = template.flipX;
                spriteRenderer.flipY = template.flipY;

                template.SetDefaults(spriteRenderer);
                spriteRenderer.sprite = template.image;
            }
            return ScriptPlayable<SpriteControlBehaviour>.Create(graph, template);
        }

    }
}
