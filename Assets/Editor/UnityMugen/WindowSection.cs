using UnityEngine;

namespace UnityMugen.EditorTools
{

    public class WindowSection
    {

        private Rect m_rect;
        private Texture2D m_texture2D;

        public WindowSection(Rect rect, Color color)
        {
            m_rect = rect;

            m_texture2D = new Texture2D(1, 1);
            m_texture2D.SetPixel(0, 0, color);
            m_texture2D.Apply();
        }

        public WindowSection(Rect rect, Texture2D texture)
        {
            m_rect = rect;
            m_texture2D = texture;

        }

        public Rect GetRect()
        {
            return m_rect;
        }

        public void SetRect(float width, float height)
        {
            m_rect.width = width;
            m_rect.height = height;
        }
        public void SetRect(Rect rect)
        {
            m_rect = rect;
        }
        public void SetRect(float x, float y, float width, float height)
        {
            m_rect.x = x;
            m_rect.y = y;
            m_rect.width = width;
            m_rect.height = height;
        }
        public void SetTexture(Texture2D texture)
        {
            m_texture2D = texture;
        }
        public Texture2D GetTexture()
        {
            return m_texture2D;
        }
        public void RefreshTextureColor()
        {
            m_texture2D.Apply();
        }
        public Color GetTextureColor(int x, int y)
        {
            return m_texture2D.GetPixel(x, y);
        }

    }
}
