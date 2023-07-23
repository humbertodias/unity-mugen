using System;
using UnityEngine;
using UnityEngine.Playables;

namespace UnityMugen.Timeline
{

    [Serializable]
    public class SpriteControlBehaviour : PlayableBehaviour
    {

        public Sprite image;

        [Tooltip("The color of the sprite")]
        [SerializeField]
        private Color color = Color.white;

        [Tooltip("The position of the sprite")]
        [SerializeField]
        public Vector3 position = Vector3.zero;
        public Vector3 scale = Vector3.one;

        public int orderInLayer = 0;
        public bool flipX = false;
        public bool flipY = false;

        float m_DefaultColorAlpha;
        SpriteRenderer m_spriteRenderer;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (m_spriteRenderer == null)
                return;

            m_spriteRenderer.gameObject.name = image.name;
            m_spriteRenderer.sprite = image;
            m_spriteRenderer.enabled = info.effectiveWeight > 0;
            m_spriteRenderer.color = new Color(color.r, color.g, color.b, info.effectiveWeight);
            m_spriteRenderer.sortingOrder = orderInLayer;
            m_spriteRenderer.flipX = flipX;
            m_spriteRenderer.flipY = flipY;

            m_spriteRenderer.transform.position = position;
            m_spriteRenderer.transform.localScale = scale;
        }

        // Invoked when the playable graph is destroyed, typically when PlayableDirector.Stop is called or the timeline
        // is complete.
        public override void OnPlayableDestroy(Playable playable)
        {
            RestoreDefaults();
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (m_spriteRenderer == null)
                return;

            m_spriteRenderer.enabled = false;

            base.OnBehaviourPause(playable, info);
        }


        public void SetDefaults(SpriteRenderer spriteRenderer)
        {
            if (spriteRenderer == m_spriteRenderer)
                return;

            RestoreDefaults();

            m_spriteRenderer = spriteRenderer;
            if (spriteRenderer != null)
            {
                m_DefaultColorAlpha = m_spriteRenderer.color.a;
            }
        }

        void RestoreDefaults()
        {
            if (m_spriteRenderer == null)
                return;

            m_spriteRenderer.enabled = false;
            m_spriteRenderer.color = new Color(color.r, color.g, color.b, m_DefaultColorAlpha);
        }

    }
}