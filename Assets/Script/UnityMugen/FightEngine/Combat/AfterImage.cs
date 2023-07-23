using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityMugen.Video;

namespace UnityMugen.Combat
{
    public struct AfterImageSubdata
    {
        public AfterImageSubdata(Entity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            Element = entity.AnimationManager.CurrentElement;
            Scale = entity.transform.localScale;
            Transparency = entity.Transparency == new Blending() ? Element.Blending : entity.Transparency;
            DrawAngle = entity.transform.localEulerAngles;


            var currentelement = entity.AnimationManager.CurrentElement;

            Vector2 position = new Vector2(entity.CurrentFacing == Facing.Right ? currentelement.Offset.x : -currentelement.Offset.x, currentelement.Offset.y);
            Vector2 drawlocation = entity.GetDrawLocation() + position;
            /*transform.localPosition*/
            SpriteLocation = new Vector3(drawlocation.x, -drawlocation.y, entity.transform.localPosition.z);


            //SpriteLocation = entity.spriteRenderer.transform.position;

            FlipX = entity.spriteRenderer.flipX;
            FlipY = entity.spriteRenderer.flipY;

            if (entity is Character)
                Scale *= (entity as Character).DrawScale;

            GameObject afterImage = new GameObject("AfterImage");
            AfterImage = afterImage.AddComponent<SpriteRenderer>();
            AfterImage.material = new Material(Shader.Find("UnityMugen/Sprites/ColorSwap"));

            AfterImage.transform.SetParent(entity.Engine.stageScreen.AfterImages);

            afterImage.SetActive(false);
        }

        public SpriteRenderer AfterImage;

        public bool FlipX;
        public bool FlipY;

        public Animations.AnimationElement Element;

        public Vector3 SpriteLocation;

        public Vector3 Scale;
        public Blending Transparency;
        public Vector3 DrawAngle;

    }

    public class AfterImage : MonoBehaviour
    {
        public AfterImage(Entity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            m_entity = entity;
            m_imagedata = new LinkedList<AfterImageSubdata>();
            m_countdown = new Dictionary<int, int>();

            ResetFE();
        }

        public static AfterImage Iniciar(GameObject where, Entity entity)
        {
            AfterImage AI = where.AddComponent<AfterImage>();
            AI.hideFlags = HideFlags.HideInInspector;
            AI.m_entity = entity;
            AI.m_imagedata = new LinkedList<AfterImageSubdata>();
            AI.m_countdown = new Dictionary<int, int>();
            return AI;
        }

        public void ModifyDisplayTime(int time)
        {
            Time = time;
            m_countdown.Clear();
        }

        public AfterImage ResetFE()
        {
            foreach (AfterImageSubdata af in m_imagedata)
            {
                Destroy(af.AfterImage.gameObject);
            }
            m_imagedata.Clear();
            m_countdown.Clear();
            m_isactive = false;
            m_time = 0;
            m_length = 0;
            m_basecolor = 0;
            m_invertcolor = false;
            m_preadd = Vector3.zero;
            m_constrast = Vector3.zero;
            m_postadd = Vector3.zero;
            m_palettecoloradd = Vector3.zero;
            m_palettecolormul = Vector3.zero;
            m_timegap = 0;
            m_framegap = 0;
            m_transparency = null;
            m_timegapcounter = 0;
            m_firstcreated = false;
            return this;
        }

        public void UpdateFE()
        {
            if (IsActive == false) return;

            ++m_timegapcounter;

            if (m_timegapcounter >= m_timegap)
            {
                m_firstcreated = true;
                m_timegapcounter = 0;
                m_imagedata.AddLast(new AfterImageSubdata(m_entity));

                while (m_imagedata.Count > m_length)
                {
                    Destroy(m_imagedata.First.Value.AfterImage.gameObject);
                    m_imagedata.RemoveFirst();
                }
            }

            var imageIndex = 0;
            var isactive = false;
            foreach (var data in m_imagedata)
            {
                if (TimeCheck_Update(imageIndex))
                    isactive = true;
                ++imageIndex;
            }

            if (isactive == false && m_firstcreated)
            {
                ResetFE();
            }
        }

        private bool TimeCheck_Update(int image_index)
        {
            if (Time == -1) return true;

            if (m_countdown.ContainsKey(image_index))
            {
                if (m_countdown[image_index] <= 0)
                    return false;

                --m_countdown[image_index];
            }
            else
            {
                if (Time <= 0)
                    return false;
                m_countdown.Add(image_index, Time);
            }

            return true;
        }

        private bool TimeCheck(int image_index)
        {
            if (Time == -1) return true;

            if (m_countdown.ContainsKey(image_index))
            {
                if (m_countdown[image_index] <= 0)
                    return false;
            }

            return true;
        }

        private bool FrameGapCheck(int index)
        {
            if (index <= 0)
                return false;

            return index % m_framegap == 0;
        }

        public void Draw()
        {
            if (IsActive == false) return;

            var index = m_imagedata.Count;
            foreach (var data in m_imagedata)
            {
                --index;

                //Para o ataque de palma triplo só cinco devem estar ativos pos vez

                //	Length / FrameGap
                if (FrameGapCheck(index) == false)
                    continue;

                if (TimeCheck(index) == false)
                    continue;

                var sprite = m_entity.SpriteManager.GetSprite(data.Element.SpriteId);
                if (sprite == null)
                    continue;

                data.AfterImage.transform.localScale = data.Scale;
                data.AfterImage.transform.localEulerAngles = data.DrawAngle;

                data.AfterImage.transform.position = data.SpriteLocation;
                data.AfterImage.flipX = data.FlipX;
                data.AfterImage.flipY = data.FlipY;
                data.AfterImage.sprite = sprite.sprite;
                data.AfterImage.sortingOrder = -10;
                data.AfterImage.sortingLayerName = "Entity";
            }


            int testeIndex = m_imagedata.Count / FrameGap;
            for (int i = m_imagedata.Count - 1; i >= 0; i--)
            {
                var data = m_imagedata.ElementAt(m_imagedata.Count - 1 - i);
                if ((m_imagedata.Count + 1 - i) % FrameGap == 0)
                {
                    var drawstate = m_entity.SpriteManager.SetupDrawing(data.Element.SpriteId, m_entity.PaletteList, m_entity.CurrentPalette);
                    drawstate.Blending = Transparency ?? data.Transparency;
                    drawstate.ShaderParameters.AfterImageEnable = true;
                    drawstate.ShaderParameters.AfterImageColor = m_basecolor;
                    drawstate.ShaderParameters.AfterImageConstrast = m_constrast;
                    drawstate.ShaderParameters.AfterImageInvert = m_invertcolor;
                    drawstate.ShaderParameters.AfterImageNumber = --testeIndex;
                    drawstate.ShaderParameters.AfterImagePaletteAdd = m_palettecoloradd;
                    drawstate.ShaderParameters.AfterImagePaletteMultiply = m_palettecolormul;
                    drawstate.ShaderParameters.AfterImagePostAdd = m_postadd;
                    drawstate.ShaderParameters.AfterImagePreAdd = m_preadd;
                    drawstate.Use(data.AfterImage.material);

                    data.AfterImage.gameObject.SetActive(true);
                }
                else
                {
                    data.AfterImage.gameObject.SetActive(false);
                }
            }

        }

        public bool IsActive
        {
            get { return m_isactive; }
            set { m_isactive = value; }
        }

        public int Time
        {
            get { return m_time; }
            set { m_time = value; }
        }

        public int Length
        {
            get { return m_length; }
            set { m_length = value; }
        }

        public float BaseColor
        {
            get { return m_basecolor; }
            set { m_basecolor = Misc.Clamp(value, 0.0f, 1.0f); }
        }

        public bool InvertColor
        {
            get { return m_invertcolor; }
            set { m_invertcolor = value; }
        }

        public Vector3 ColorPreAdd
        {
            get { return m_preadd; }
            set { m_preadd = value; }
        }

        public Vector3 ColorContrast
        {
            get { return m_constrast; }
            set { m_constrast = value; }
        }

        public Vector3 ColorPostAdd
        {
            get { return m_postadd; }
            set { m_postadd = value; }
        }

        public Vector3 ColorPaletteAdd
        {
            get { return m_palettecoloradd; }
            set { m_palettecoloradd = value; }
        }

        public Vector3 ColorPaletteMultiply
        {
            get { return m_palettecolormul; }
            set { m_palettecolormul = value; }
        }

        public int TimeGap
        {
            get { return m_timegap; }
            set { m_timegap = value; }
        }

        public int FrameGap
        {
            get { return m_framegap; }
            set { m_framegap = value; }
        }

        public Blending? Transparency
        {
            get => m_transparency;

            set { m_transparency = value; }
        }

        #region Fields

        private Entity m_entity;
        private LinkedList<AfterImageSubdata> m_imagedata;
        private Dictionary<int, int> m_countdown;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isactive;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_time;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_length;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float m_basecolor;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_invertcolor;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector3 m_preadd;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector3 m_constrast;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector3 m_postadd;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector3 m_palettecoloradd;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector3 m_palettecolormul;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_timegap;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_framegap;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Blending? m_transparency;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_timegapcounter;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_firstcreated;

        #endregion
    }
}