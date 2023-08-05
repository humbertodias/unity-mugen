using System;
using UnityEngine;
using UnityEngine.UI;
using UnityMugen.Animations;
using UnityMugen.Drawing;
using UnityMugen.Video;

namespace UnityMugen.Combat
{
    public class GraphicUIEntity : Entity
    {
        private RectTransform rectTransform;
        public Image image;

        public GraphicUIEntity Iniciar(Character creator, GraphicUIData data)
        {
            if (creator == null) throw new ArgumentNullException(nameof(creator));
            if (data == null) throw new ArgumentNullException(nameof(data));

            m_data = data;

            image = gameObject.AddComponent<Image>();
            image.material = new Material(Shader.Find("UnityMugen/Sprites/ColorSwap"));
            image.type = Image.Type.Filled;
            image.raycastTarget = false;
            image.fillMethod = Image.FillMethod.Horizontal;
            image.fillAmount = m_data.fillAmount.Value;

            rectTransform = image.GetComponent<RectTransform>();

            Id = data.id.Value;
            typeEntity = TypeEntity.GraphicUI;

            IniciarEntity();

            m_forceremove = false;
            m_tickcount = 0;

            Creator = creator;
            BasePlayer = Creator.BasePlayer;
            if (m_data.commonAnimation == false)
            {
                m_animationmanager = Creator.AnimationManager.Clone();
                m_spritemanager = Creator.SpriteManager.Clone();
            }
            else
            {
                m_animationmanager = Engine.FxAnimations.Clone();
                m_spritemanager = Engine.FxSprites.Clone();
            }

            m_palfx = new PaletteFx();

            PaletteList = Creator.PaletteList;
            CurrentScale = Data.scale.Value;
            DrawOrder = Data.sprpriority.Value;
            Transparency = Data.transparency.Value;

            if (AnimationManager.HasAnimation(Data.anim.Value))
            {
                SetLocalAnimation(Data.anim.Value, 0);
                m_valid = true;
            }
            else
            {
                m_valid = false;
            }

            return this;
        }

        public override Vector2 GetDrawLocation(bool _ = false)
        {
            var drawlocation = Data.pos.Value;

            switch (Data.postype)
            {
                case PositionTypeUI.Left:
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    break;
                case PositionTypeUI.Right:
                    rectTransform.anchorMin = new Vector2(1, 1);
                    rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    break;
                case PositionTypeUI.Top:
                    rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    rectTransform.anchorMax = new Vector2(1, 1);
                    break;
                case PositionTypeUI.Botton:
                    rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    rectTransform.anchorMax = new Vector2(0, 0);
                    break;
                case PositionTypeUI.LeftTop:
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(1, 1);
                    break;
                case PositionTypeUI.RightTop:
                    rectTransform.anchorMin = new Vector2(1, 1);
                    rectTransform.anchorMax = new Vector2(1, 1);
                    break;
                case PositionTypeUI.LeftBotton:
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(0, 0);
                    break;
                case PositionTypeUI.RightBotton:
                    rectTransform.anchorMin = new Vector2(1, 0);
                    rectTransform.anchorMax = new Vector2(1, 0);
                    break;
                case PositionTypeUI.Center:
                default:
                    rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    throw new InvalidOperationException("Data.PositionType");
            }

            return drawlocation;
        }

        public override bool IsPaused(Pause pause)
        {
            if (pause == null) throw new ArgumentNullException(nameof(pause));

            //if (pause.IsActive == false) return false;
            return false;
            //return true;
        }

        public override bool RemoveCheck()
        {
            if (m_forceremove)
                return true;

            if (m_valid == false)
                return true;

            if (m_data.removetime == -1)
                return false;

            if (m_data.removetime == -2)
                return AnimationManager.IsAnimationFinished;

            if (m_data.removetime >= 0)
                return Ticks > m_data.removetime;

            return true;
        }

        public override void Draw(int orderDraw)
        {
            var currentelement = AnimationManager.CurrentElement;
            if (currentelement == null) return;

            Vector2 position = new Vector2(CurrentFacing == Facing.Right ? currentelement.Offset.x : -currentelement.Offset.x, currentelement.Offset.y);
            position *= Constant.Scale2 * CurrentScale;
            Vector2 drawlocation = GetDrawLocation() - position;
            rectTransform.anchoredPosition = new Vector3(drawlocation.x, drawlocation.y, transform.localPosition.z);

            var spriteFE = SpriteManager.GetSprite(currentelement.SpriteId);
            if (spriteFE == null)
            {
                image.sprite = null;
                return;
            }

            if (image)
            {
                float width = spriteFE.sprite.texture.width;
                float height = spriteFE.sprite.texture.height;
                rectTransform.sizeDelta = new Vector2(width, height);
                rectTransform.pivot = new Vector2(spriteFE.sprite.pivot.x / width, spriteFE.sprite.pivot.y / height);
                bool isChar = (typeEntity == TypeEntity.Player || typeEntity == TypeEntity.Helper);

                image.sprite = spriteFE.sprite;
                image.color = Data.color.Value;
                image.fillAmount = Data.fillAmount.Value;
                image.fillOrigin = Data.fillOrigin.Value;
                //image.sortingOrder = orderDraw;

                //image.flipX = GetDrawFlip() == SpriteEffects.FlipHorizontally || GetDrawFlip() == SpriteEffects.Both;
                //image.flipY = GetDrawFlip() == SpriteEffects.FlipVertically || GetDrawFlip() == SpriteEffects.Both;

                var drawscale = CurrentScale * currentelement.Scale;

                transform.SetSiblingIndex(orderDraw);
                transform.localScale = new Vector3(drawscale.x, drawscale.y, 1f);

                float angle = AngleDraw ? Misc.FaceScalar(CurrentFacing, DrawingAngle) : 0f;
                angle += currentelement.Rotate;
                transform.localEulerAngles = new Vector3(0, 0, angle);

                var drawstate = SpriteManager.SetupDrawing(currentelement.SpriteId, PaletteList);
                PaletteFx.SetShader(drawstate.ShaderParameters);
                drawstate.Blending = Transparency == new Blending() ? currentelement.Blending : Transparency;
                drawstate.Use(image.material);
            }
        }

        public override void UpdateState()
        {
            ++m_tickcount;
        }

        public void Kill()
        {
            m_forceremove = true;
            Engine.Entities.Remove(this);
        }

        public void Modify(GraphicUIData modifydata)
        {
            if (modifydata == null) throw new ArgumentNullException("data");
            if (modifydata.id != Id) throw new ArgumentException("data");

            if (modifydata.pos != null)
                Data.pos = modifydata.pos;
            if (modifydata.postype != null)
                Data.postype = modifydata.postype;

            if (modifydata.scale != null)
            {
                Data.scale = modifydata.scale;
                CurrentScale = Data.scale.Value;
            }
            if (modifydata.commonAnimation != null)
            {
                Data.commonAnimation = modifydata.commonAnimation;
            }
            if (modifydata.anim != null)
            {
                Data.anim = modifydata.anim;
                if (AnimationManager.HasAnimation(Data.anim.Value))
                {
                    SetLocalAnimation(Data.anim.Value, 0);
                    m_valid = true;
                }
                else
                {
                    m_valid = false;
                }
            }

            if (modifydata.sprpriority != null)
                Data.sprpriority = modifydata.sprpriority;
            if (modifydata.layer != null)
                Data.layer = modifydata.layer;
            if (modifydata.fillAmount != null)
                Data.fillAmount = modifydata.fillAmount;
            if (modifydata.removetime != null)
                Data.removetime = modifydata.removetime;
            if (modifydata.color != null)
                Data.color = modifydata.color;

            if (modifydata.transparency != null)
            {
                Data.transparency = modifydata.transparency.Value;
                Transparency = Data.transparency.Value;
            }

        }

        private GraphicUIData m_data;
        public GraphicUIData Data => m_data;

        public override Team Team => BasePlayer.Team;

        private AnimationManager m_animationmanager;
        public override AnimationManager AnimationManager => m_animationmanager;

        private SpriteManager m_spritemanager;
        public override SpriteManager SpriteManager => m_spritemanager;

        private PaletteFx m_palfx;
        public override PaletteFx PaletteFx => m_palfx;

        private int m_tickcount;
        private int Ticks => m_tickcount;

        private bool m_valid;
        public bool IsValid => m_valid;

        private bool m_forceremove;
        public override EntityUpdateOrder UpdateOrder => EntityUpdateOrder.GraphicUI;
    }
}