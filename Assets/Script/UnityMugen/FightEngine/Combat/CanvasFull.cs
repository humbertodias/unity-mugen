using UnityEngine;
using UnityEngine.UI;
using UnityMugen.Animations;
using UnityMugen.Drawing;

namespace UnityMugen.Combat
{
    public class CanvasFull : MonoBehaviour
    {

        void Awake()
        {
            m_image = gameObject.GetComponent<Image>();
            m_graphicUIData = new GraphicUIData();
        }

        public void ResetFE()
        {
            m_graphicUIData.removetime = 0;
            m_graphicUIData.color = Color.white;

            m_animationNumber = null;
            m_spritemanager = null;
            m_animationmanager = null;

            m_image.enabled = false;
            m_running = false;
        }

        public void UpdateFE()
        {
            if (IsActive)
            {
                m_image.enabled = true;

                if (HasAnimation)
                {
                    m_animationmanager.UpdateFE();
                    Draw();
                }

                if (m_graphicUIData.removetime > 0)
                    m_graphicUIData.removetime--;
            }
            else if (DisableCheck())
            {
                if (m_running)
                    ResetFE();
            }
        }

        public bool DisableCheck()
        {
            if (m_graphicUIData.removetime == -1)
                return false;

            if (m_graphicUIData.removetime == -2)
                return m_animationmanager.IsAnimationFinished;

            if (m_graphicUIData.removetime == 0)
                return true;

            return true;
        }

        private void Draw()
        {
            var currentelement = m_animationmanager.CurrentElement;
            if (currentelement == null) return;

            var spriteFE = m_spritemanager.GetSprite(currentelement.SpriteId);
            if (spriteFE == null)
            {
                m_image.sprite = null;
                return;
            }

            if (m_image)
                m_image.sprite = spriteFE.sprite;
        }

        public void Set(Character character, GraphicUIData graphicUIData)
        {
            m_running = true;
            m_graphicUIData = graphicUIData;

            m_image.color = m_graphicUIData.color.Value;

            m_animationNumber = graphicUIData.anim;
            if (m_animationNumber != null)
            {
                m_spritemanager = character.SpriteManager.Clone();
                m_animationmanager = character.AnimationManager.Clone();
                m_animationmanager.SetLocalAnimation(m_animationNumber.Value, 0);
            }
        }

        private bool m_running = false;

        private bool IsActive => m_graphicUIData.removetime > 0 || m_graphicUIData.removetime == -1;
        private bool HasAnimation =>
            m_animationNumber != null &&
            m_spritemanager != null &&
            m_animationmanager != null;

        private Image m_image;
        private GraphicUIData m_graphicUIData;

        private int? m_animationNumber;
        private SpriteManager m_spritemanager;
        private AnimationManager m_animationmanager;

    }
}