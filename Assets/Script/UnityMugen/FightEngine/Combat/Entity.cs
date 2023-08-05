using System;
using UnityEngine;
using UnityMugen.Animations;
using UnityMugen.Drawing;
using UnityMugen.Video;

namespace UnityMugen.Combat
{

    public abstract class Entity : MonoBehaviour
    {
        public LauncherEngine Launcher => LauncherEngine.Inst;
        public FightEngine Engine => Launcher.mugen.Engine;

        public long UniqueID { get; set; }
        public long Id { get; set; }
        public string NameSearch { get; set; }
        public TypeEntity typeEntity { get; set; }
        public PaletteList PaletteList { get; set; }

        [NonSerialized] public SpriteRenderer spriteRenderer;
        [NonSerialized] public SpriteRenderer shadown;
        [NonSerialized] public float shadowOffset;
        [NonSerialized] public SpriteRenderer reflection;
        [NonSerialized] public DrawColliders draw;
        private Transform pointEntity;

        //public Transform positionOffset;

        private AfterImage m_afterimages;
        public AfterImage AfterImages => m_afterimages;

        public abstract bool RemoveCheck();

        private int m_numberCurrentState;

        public Character Creator { get; set; }
        public Player BasePlayer { get; set; }
        public abstract EntityUpdateOrder UpdateOrder { get; }
        public abstract Team Team { get; }
        public abstract AnimationManager AnimationManager { get; }
        public abstract SpriteManager SpriteManager { get; }
        public abstract Vector2 GetDrawLocation(bool startLoacation = false);


        public abstract PaletteFx PaletteFx { get; }

        public Blending Transparency { get; set; }

        public Vector2 CurrentScale { get; set; }
        public Vector2 CurrentVelocity { get; set; }
        public Vector2 CurrentAcceleration { get; set; }
        public float DrawingAngle { get; set; }
        public bool AngleDraw { get; set; }
        public int DrawOrder { get; set; }
        public bool DrawShadow { get; set; }
        public bool DrawReflection { get; set; }

        private Vector2 _CurrentLocation;
        public Vector2 CurrentLocation { 
            get => _CurrentLocation;
            set
            {
                //if (float.IsNaN(value.x) || float.IsInfinity(value.x))
                //    _CurrentLocation = new Vector2(_CurrentLocation.x, value.y);
                //if (float.IsNaN(value.y) || float.IsInfinity(value.y))
                //    _CurrentLocation = new Vector2(value.x, _CurrentLocation.y);

                _CurrentLocation = value;
            }
        }
        
        public SpriteEffects CurrentFlip { get; set; }

        private Facing m_facing;
        public Facing CurrentFacing
        {
            get
            {
                return m_facing;
            }

            set
            {
                if (m_facing != value)
                {
                    CurrentVelocity *= new Vector2(-1, 1);
                    CurrentAcceleration *= new Vector2(-1, 1);
                }
                m_facing = value;
            }
        }

        public void IniciarEntity()
        {
            DrawOrder = 0;
            CurrentLocation = new Vector2(0, 0);
            CurrentVelocity = new Vector2(0, 0);
            CurrentAcceleration = new Vector2(0, 0);
            //m_facing = Facing.Right;
            CurrentFlip = SpriteEffects.None;
            CurrentScale = new Vector2(1, 1);
            Transparency = new Blending();
            m_afterimages = AfterImage.Iniciar(gameObject, this).ResetFE();
            DrawingAngle = 0;

            pointEntity = Misc.PointEntity(gameObject);// Novo
        }

        public virtual void ResetFE()
        {
            DrawOrder = 0;
            CurrentLocation = new Vector2(0, 0);
            CurrentVelocity = new Vector2(0, 0);
            CurrentAcceleration = new Vector2(0, 0);
            //m_facing = Facing.Right;
            CurrentFlip = SpriteEffects.None;
            //CurrentScale = new Vector2(1, 1);
            Transparency = new Blending();
            DrawingAngle = 0;
            AngleDraw = false;
            m_afterimages.ResetFE();
        }

        public virtual void CleanUp()
        {
            AngleDraw = false;
        }

        public virtual void UpdateAfterImages()
        {
            AfterImages.UpdateFE();
        }

        public virtual void UpdatePhsyics()
        {
            Move(CurrentVelocity);

            CurrentVelocity += CurrentAcceleration;
        }

        public virtual void UpdateState() { }

        public virtual void ActionFinish() { }

        public virtual Vector2 Move(Vector2 p)
        {
            Vector2 oldlocation = CurrentLocation;

            if (CurrentFacing == Facing.Right)
                CurrentLocation += p;

            if (CurrentFacing == Facing.Left)
                CurrentLocation += new Vector2(-p.x, p.y);

            Bounding();

            Vector2 movement = CurrentLocation - oldlocation;
            return movement;
        }

        protected virtual void Bounding() { }
        public virtual void UpdateInput() { }


        public virtual bool IsPaused(Pause pause)
        {
            if (pause == null) throw new ArgumentNullException(nameof(pause));

            if (pause.IsActive == false) return false;
            if (this == pause.Creator && pause.ElapsedTime <= pause.MoveTime) return false;

            return true;
        }

        public Vector2 MoveLeft(Vector2 p)
        {
            if (CurrentFacing == Facing.Right) return Move(new Vector2(-p.x, p.y));

            if (CurrentFacing == Facing.Left) return Move(p);

            return new Vector2();
        }

        public Vector2 MoveRight(Vector2 p)
        {
            if (CurrentFacing == Facing.Right) return Move(p);

            if (CurrentFacing == Facing.Left) return Move(new Vector2(-p.x, p.y));

            return new Vector2();
        }

        public Vector3 CurrentLocationYTransform()
        {
            return new Vector3(CurrentLocation.x, -CurrentLocation.y, 0);
        }
        public virtual void Draw(int orderDraw)
        {
            AfterImages.Draw();

            var currentelement = AnimationManager.CurrentElement;
            if (currentelement == null) return;

            float angle = AngleDraw ? Misc.FaceScalar(CurrentFacing, DrawingAngle) : 0f;
            angle += currentelement.Rotate;
            transform.localEulerAngles = new Vector3(0, 0, angle);

            Vector2 position = new Vector2(CurrentFacing == Facing.Right ? currentelement.Offset.x : -currentelement.Offset.x, currentelement.Offset.y);
            Vector2 drawlocation = GetDrawLocation() + position;
            transform.localPosition = new Vector3(drawlocation.x, -drawlocation.y, transform.localPosition.z);

            bool isChar = (typeEntity == TypeEntity.Player || typeEntity == TypeEntity.Helper);
            var drawscale = CurrentScale * currentelement.Scale;
            if (isChar)
            {
                Vector2 s = (this as Character).DrawScale;
                drawscale *= s;
            }
            transform.localScale = new Vector3(drawscale.x, drawscale.y, 1f);

            var spriteFE = SpriteManager.GetSprite(currentelement.SpriteId);
            if (spriteFE == null)
            {
                DrawInvisible();
                return;
            }

            if (spriteRenderer)
            {
                float width = spriteFE.sprite.texture.width;

                if (isChar && (this as Character).Assertions.Invisible == true)
                    spriteRenderer.enabled = false;
                else
                    spriteRenderer.enabled = true;

                spriteRenderer.sprite = spriteFE.sprite;
                spriteRenderer.sortingOrder = orderDraw;

                spriteRenderer.flipX = GetDrawFlip() == SpriteEffects.FlipHorizontally || GetDrawFlip() == SpriteEffects.Both;
                spriteRenderer.flipY = GetDrawFlip() == SpriteEffects.FlipVertically || GetDrawFlip() == SpriteEffects.Both;


                var drawstate = SpriteManager.SetupDrawing(currentelement.SpriteId, PaletteList);
                PaletteFx.SetShader(drawstate.ShaderParameters);
                drawstate.Blending = Transparency == new Blending() ? currentelement.Blending : Transparency;
                drawstate.Use(spriteRenderer.material);
            }

        }

        public void DrawInvisible()
        {
            if (spriteRenderer)
            {
                spriteRenderer.flipX = false;
                spriteRenderer.flipY = false;
                //    spriteRenderer.transform.localPosition = Vector2.zero;
                spriteRenderer.sortingOrder = 0;
                spriteRenderer.sprite = null;
            }
        }

        public void Shadow()
        {
            if (shadown == null)
                shadown = Misc.Shadow(gameObject.name, Engine.stageScreen.Stage.ShadowColor);

            bool isChar = (typeEntity == TypeEntity.Player || typeEntity == TypeEntity.Helper);
            if (DrawShadow && spriteRenderer.sprite != null &&
                Engine.Assertions.NoBackLayer == false &&
                Engine.Assertions.GlobalNoShadow == false &&
                ((isChar && (this as Character).Assertions.NoShadow == false) || !isChar) &&
                ((isChar && (this as Character).Assertions.Invisible == false) || !isChar))
            {
                shadown.gameObject.SetActive(true);
                Vector3 posi = spriteRenderer.transform.position;
                float scale = Engine.stageScreen.Stage.ShadowScale;

                float posZ = (scale < 0 ? (posi.y / 2) : -(posi.y / 2)) + shadowOffset;
                shadown.transform.position = new Vector3(posi.x, 0.00001f, posZ);
                shadown.transform.localScale = new Vector3(this.transform.localScale.x, scale, this.transform.localScale.z);
                shadown.transform.eulerAngles = new Vector3(-90f, 0f, 0f);

                shadown.flipX = spriteRenderer.flipX;
                shadown.flipY = spriteRenderer.flipY;
                shadown.sprite = spriteRenderer.sprite;

                // Apply fade shadow by distance
                float maxDistanceShadow = Engine.stageScreen.Stage.ShadowFade;
                Color baseShadown = Engine.stageScreen.Stage.ShadowColor;
                float percentDistance = (Misc.Clamp(Mathf.Abs(posZ), 0, maxDistanceShadow) / maxDistanceShadow) * 100f;
                float updateAlpha = ((percentDistance * Engine.stageScreen.Stage.ShadowColor.a) / 100f) / Engine.stageScreen.Stage.ShadowColor.a;
                float alpha = Engine.stageScreen.Stage.ShadowColor.a - updateAlpha;
                shadown.material.SetColor("_Color", new Color(baseShadown.r, baseShadown.g, baseShadown.b, alpha));
            }
            else
                shadown.gameObject.SetActive(false);
        }

        public void Reflection(int orderDraw)
        {
            if (reflection == null)
                reflection = Misc.Reflection(gameObject.name);

            bool isChar = (typeEntity == TypeEntity.Player || typeEntity == TypeEntity.Helper);
            if (DrawReflection && spriteRenderer.sprite != null &&
                Engine.Assertions.NoBackLayer == false &&
                ((isChar && (this as Character).Assertions.Invisible == false) || !isChar))
            {
                reflection.gameObject.SetActive(true);
                Vector3 posi = spriteRenderer.transform.position;
                reflection.transform.position = new Vector3(posi.x, -(posi.y / 2), 0.014f);

                Vector3 scale = spriteRenderer.transform.localScale;
                reflection.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
                reflection.transform.eulerAngles = new Vector3(-180f, 0f, 0f);

                reflection.flipX = spriteRenderer.flipX;
                reflection.flipY = spriteRenderer.flipY;
                reflection.sprite = spriteRenderer.sprite;
                reflection.sortingOrder = -orderDraw - 20;

                var currentelement = AnimationManager.CurrentElement;
                var drawstate = SpriteManager.SetupDrawing(currentelement.SpriteId, PaletteList);
                PaletteFx.SetShader(drawstate.ShaderParameters);

                drawstate.Blending = new Blending(BlendType.Add, 255, 127);
                drawstate.Use(reflection.material);
            }
            else
            {
                reflection.gameObject.SetActive(false);
            }
        }

        public virtual void DebugDraw()
        {
            if (draw != null)
            {
                if (Launcher.trainnerSettings.typeDrawCollider != TypeDrawCollider.None)
                {
                    draw.UpdateFE(CurrentFacing);
                    pointEntity.gameObject.SetActive(true);
                }
                else
                {
                    draw.RemoveBoxColliders();
                    pointEntity.gameObject.SetActive(false);
                }
            }
        }

        public SpriteEffects GetDrawFlip()
        {
            var currentelement = AnimationManager.CurrentElement;

            var flip = CurrentFlip ^ currentelement.Flip;

            if (CurrentFacing == Facing.Left)
                flip ^= SpriteEffects.FlipHorizontally;

            return flip;
        }

        public virtual void UpdateAnimations()
        {
            UpdateAnimations(true);
        }

        public virtual void UpdateAnimations(bool updatepalfx)
        {
            AnimationManager.UpdateFE();

            if (updatepalfx) PaletteFx.UpdateFE();
        }

        public void SetLocalAnimation(int animnumber, int elementnumber)
        {
            if (AnimationManager.SetLocalAnimation(animnumber, elementnumber))
            {
                //      SpriteManager.LoadSprites(AnimationManager.CurrentAnimation);
            }
        }

        public void SetForeignAnimation(AnimationManager animationmanager, int animationnumber, int elementnumber)
        {
            if (AnimationManager.SetForeignAnimation(animationmanager, animationnumber, elementnumber))
            {
                //    SpriteManager.LoadSprites(AnimationManager.CurrentAnimation);
            }
        }

    }
}