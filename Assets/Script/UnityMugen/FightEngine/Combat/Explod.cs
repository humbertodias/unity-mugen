using System;
using System.Diagnostics;
using UnityEngine;
using UnityMugen.Animations;
using UnityMugen.Drawing;
using UnityMugen.Evaluation.Triggers;

namespace UnityMugen.Combat
{

    public class Explod : Entity
    {
        public Explod Iniciar(ExplodData data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.material = new Material(Shader.Find("UnityMugen/Sprites/ColorSwap"));
            spriteRenderer.sortingOrder = 3;
            spriteRenderer.sortingLayerName = "Entity";

            Id = data.ExplodId;
            NameSearch = Id.ToString();
            gameObject.name = "Explod_" + Id.ToString();
            typeEntity = TypeEntity.Explod;

            IniciarEntity();

            Data = data;
            Ticks = 0;
            ForceRemove = false;
            Creator = data.Creator;
            BasePlayer = Creator.BasePlayer;

            if (Data.CommonAnimation == false)
            {
                m_spritemanager = BasePlayer.SpriteManager.Clone();
                m_animationmanager = BasePlayer.AnimationManager.Clone();
                PaletteList = Creator.PaletteList;
            }
            else
            {
                m_spritemanager = Engine.FxSprites.Clone();
                m_animationmanager = Engine.FxAnimations.Clone();
                PaletteList = Engine.PaletteListFX;
            }

            CreationFacing = Data.Offseter.CurrentFacing;

            CurrentFacing = GetStartFacing();

            CurrentLocation = GetDrawLocation(true);
            CurrentVelocity = Data.Velocity;
            CurrentAcceleration = Data.Acceleration;
            //CurrentFlip = SpriteEffects.None;
            CurrentScale = Data.Scale;
            
            DrawingAngle = Data.Angle;//Novo

            DrawOrder = Data.DrawOnTop ? 11 : Data.SpritePriority;
            Transparency = Data.Transparency;
            DrawShadow = Data.Shadow;
            DrawReflection = Data.Reflection;

            var rng = Launcher.random;
            float x = rng.NewFloat(-Data.Random.x, Data.Random.x);
            float y = rng.NewFloat(-Data.Random.y, Data.Random.y);
            Random = new Vector2(x, y);
            
            UnityEngine.Random.Range(-Data.Random.x, Data.Random.x);

            m_palfx = Data.OwnPalFx ? new PaletteFx() : Creator.PaletteFx;

            if (AnimationManager.HasAnimation(Data.AnimationNumber))
            {
                SetLocalAnimation(Data.AnimationNumber, 0);
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }
            return this;
        }

        public override void Draw(int orderDraw)
        {
            base.Draw(orderDraw);
            base.Shadow();
            base.Reflection(orderDraw);
        }



        private Facing GetStartFacing()
        {
            var facing = Facing.Left;

            switch (Data.PositionType)
            {
                case PositionType.P1:
                case PositionType.P2:
                case PositionType.Back:
                    facing = Data.Offseter.CurrentFacing;
                    break;

                case PositionType.Center:
                case PositionType.Front:
                case PositionType.Left:
                case PositionType.Right:
                    facing = Creator.CurrentFacing;
                    bool facep2 = EvaluationHelper.AsBoolean(Creator, Creator.StateManager.CurrentState.faceEnemy, false);

                    facing = Misc.FlipFacing(Creator.CurrentFacing);
                    // em analize verificar o gohan, m_creator.P2Dist(Axis.X) < 0
                    //Gohan teste - Seed = 402259687

                    bool error = false;
                    if (facep2 && P2Dist.Evaluate(Creator, ref error, Axis.X) < 0 && error == false)
                    {
                        CurrentFlip = (Creator.GetDrawFlip() == SpriteEffects.FlipHorizontally ? SpriteEffects.None : SpriteEffects.FlipHorizontally);

                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException("Data.PositionType");
            }

            if ((Data.Flip & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally)
                facing = Misc.FlipFacing(facing);

            return facing;
        }

        public override bool IsPaused(Pause pause)
        {
            if (pause == null) throw new ArgumentNullException(nameof(pause));

            if (pause.IsActive == false) return false;

            if (Data.IsHitSpark) return false;

            if (pause.IsSuperPause)
            {
                if ((Data.SuperMove || pause.ElapsedTime <= Data.SuperMoveTime) || Data.SuperMoveTime == -1) 
                    return false;
            }
            else
            {
                if ((pause.ElapsedTime <= Data.PauseTime) || Data.PauseTime == -1) 
                    return false;
            }

            return true;
        }

        public override Vector2 Move(Vector2 p)
        {
            if (IsBound) return new Vector2();

            switch (Data.PositionType)
            {
                case PositionType.P1:
                case PositionType.P2:
                case PositionType.Front:
                case PositionType.Back:
                    return base.Move(p);

                case PositionType.Left:
                    return base.Move(new Vector2(-p.x, p.y));
                //return MoveRight(p);

                case PositionType.Right:
                    return base.Move(new Vector2(p.x, p.y));
                //return MoveLeft(p);

                default:
                    return new Vector2();
            }
        }

        public override void UpdatePhsyics()
        {
            if (IsBound == false)
            {
                base.UpdatePhsyics();
            }
        }

        public override void UpdateAnimations()
        {
            if (HitPauseExplod() == false && Ticks > 0)
            {
                base.UpdateAnimations(Data.OwnPalFx);
            }
        }

        public override void UpdateState()
        {
            if (HitPauseExplod() == false)
            {
                if (Ticks == 0)
                {
                    CurrentLocation = GetDrawLocation(true);
                }

                ++Ticks;
            }
        }

        //bool HitPauseExplod => Data.IgnoreHitPause ? false : (Data.Offseter as Character).InHitPause;

        bool HitPauseExplod()
        {
            if (Data.IgnoreHitPause)
                return false;
            else if (IsBound && (Data.PositionType == PositionType.P1 || Data.PositionType == PositionType.P2))
                if (Data.Offseter is Character chara)
                    return chara.InHitPause;
                else return false;
            else
                return false;
        }

        public override bool RemoveCheck()
        {
            if (ForceRemove)
                return true;

            if (IsValid == false)
                return true;

            if (Data.RemoveOnGetHit)
            {
                if ((Data.BindTime == -1 || Ticks <= Data.BindTime) &&
                    Data.Offseter.BasePlayer.MoveType == MoveType.BeingHit)
                    return true;
            }

            if (Data.RemoveTime == -1)
                return false;

            if (Data.RemoveTime == -2)
                return AnimationManager.IsAnimationFinished;

            if (Data.RemoveTime >= 0)
                return Ticks > Data.RemoveTime;

            return true;
        }

        public override Vector2 GetDrawLocation(bool startLocation = false)
        {
            var offset = Data.Location + Random;
            var camerabounds = Engine.CameraFE.ScreenBounds();
            var x = ((offset.x * Constant.Scale2) * (Screen.width / Constant.LocalCoord.x));
            var y = Screen.height - ((offset.y * Constant.Scale2) * (Screen.height / Constant.LocalCoord.y));
            var z = -Camera.main.transform.position.z;
            Vector2 point = Camera.main.ScreenToWorldPoint(new Vector3(x, y, z));

            switch (Data.PositionType)
            {
                // Posição de Helper é diferente de Explod y = 0 em Helper 
                // representa o ground, já em Explod representa o topo da tela
                case PositionType.P1:
                case PositionType.P2:
                    if (IsBound || startLocation)
                        return Misc.GetOffset(Data.Offseter.CurrentLocation, CreationFacing, offset);
                    else
                        return CurrentLocation;

                case PositionType.Left:
                    //localcoord = 320, 240 [valor encontrado em .def de stages];
                    CurrentFacing = Facing.Right;
                    //float leftCam = Misc.GetOffset(camerabounds.xMin, Facing.Right, offset.x);
                    if (IsBound || startLocation)
                        return new Vector3(point.x, -point.y, transform.localPosition.z);
                    else
                        return CurrentLocation;

                case PositionType.Right:
                    CurrentFacing = Facing.Right;
                    if ((Data.Flip & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally)
                        CurrentFacing = Misc.FlipFacing(CurrentFacing);
                    if (IsBound || startLocation)
                    {
                        float rightCam = Misc.GetOffset(camerabounds.xMax, Facing.Right, offset.x);
                        return new Vector3(rightCam, -point.y, transform.localPosition.z);
                    }
                    else
                        return CurrentLocation;

                case PositionType.Back:
                    Vector2 location = Misc.GetOffset(Vector2.zero, CreationFacing, offset);
                    if (IsBound || startLocation)
                    {
                        location.x += CreationFacing == Facing.Right ? camerabounds.xMin : camerabounds.xMax;
                        location.y = -point.y;
                        return location;
                    }
                    else
                        return CurrentLocation;

                case PositionType.Front:
                    Vector2 location2 = Misc.GetOffset(Vector2.zero, CreationFacing, offset);
                    location2.y = -point.y;
                    if (IsBound || startLocation)
                    {
                        location2.x += CreationFacing == Facing.Left ? camerabounds.xMin : camerabounds.xMax;
                        return location2;
                    }
                    else
                        return CurrentLocation;

                case PositionType.Center:
                    Vector2 posCam = Engine.CameraFE.transform.localPosition;
                    return new Vector2(posCam.x, -posCam.y);

                default:
                    throw new InvalidOperationException("Data.PositionType");
            }
        }

        public void Kill()
        {
            ForceRemove = true;
            Engine.Entities.Remove(this);
        }

        public void Modify(ModifyExplodData modifydata)
        {
            if (modifydata == null) throw new ArgumentNullException("data");
            if (modifydata.Id != Id) throw new ArgumentException("data");

            if (modifydata.Scale != null)
            {
                Data.Scale = modifydata.Scale.Value;
                CurrentScale = Data.Scale;
            }

            if (modifydata.SpritePriority != null)
            {
                Data.SpritePriority = modifydata.SpritePriority.Value;
                DrawOrder = Data.SpritePriority;
            }

            if (modifydata.DrawOnTop != null)
            {
                Data.DrawOnTop = modifydata.DrawOnTop.Value;
                if (Data.DrawOnTop) DrawOrder = 11;
            }

            if (modifydata.RemoveOnGetHit != null)
            {
                Data.RemoveOnGetHit = modifydata.RemoveOnGetHit.Value;
            }

            if (modifydata.SuperMove != null)
            {
                Data.SuperMove = modifydata.SuperMove.Value;
            }

            if (modifydata.SuperMoveTime != null)
            {
                Data.SuperMoveTime = modifydata.SuperMoveTime.Value;
            }

            if (modifydata.PauseTime != null)
            {
                Data.PauseTime = modifydata.PauseTime.Value;
            }

            if (modifydata.RemoveTime != null)
            {
                Data.RemoveTime = modifydata.RemoveTime.Value;
            }

            if (modifydata.BindTime != null)
            {
                Data.BindTime = modifydata.BindTime.Value;
            }

            if (modifydata.Acceleration != null)
            {
                Data.Acceleration = modifydata.Acceleration.Value;
                CurrentAcceleration = Data.Acceleration;
            }

            if (modifydata.Velocity != null)
            {
                Data.Velocity = modifydata.Velocity.Value;
                CurrentVelocity = Data.Velocity;
            }

            if (modifydata.IgnoreHitPause != null)
            {
                Data.IgnoreHitPause = modifydata.IgnoreHitPause.Value;
            }

            if (modifydata.Flip != null)
            {
                Data.Flip = modifydata.Flip.Value;
            }

            if (modifydata.PositionType != null)
            {
                Data.PositionType = modifydata.PositionType.Value;
            }

            if (modifydata.Location != null)
            {
                Data.Location = modifydata.Location.Value;
            }

            if (modifydata.Random != null)
            {
                Data.Random = modifydata.Random.Value;
            }

            if (modifydata.Transparency != null)
            {
                Data.Transparency = modifydata.Transparency.Value;
                Transparency = Data.Transparency;
            }

            if (modifydata.PositionType != null || modifydata.Location != null)
            {
                CurrentLocation = GetDrawLocation(true);
            }

            if (modifydata.Flip != null)
            {
                CurrentFacing = GetStartFacing();
            }

            /*
            Data.CommonAnimation = data.CommonAnimation;
            Data.AnimationNumber = data.AnimationNumber;
            */
        }

        public ExplodData Data { get; set; }
        public int Ticks { get; set; }
        public bool IsValid { get; set; }
        public Facing CreationFacing { get; set; }
        public bool ForceRemove { get; set; }
        public Vector2 Random { get; set; }

        public override EntityUpdateOrder UpdateOrder => EntityUpdateOrder.Explod;

        public override SpriteManager SpriteManager => m_spritemanager;
        public override AnimationManager AnimationManager => m_animationmanager;
        public override Team Team => BasePlayer.Team;


        private bool IsBound => Data.BindTime == -1 || Ticks <= Data.BindTime;
        public override PaletteFx PaletteFx => m_palfx;

        #region Fields


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SpriteManager m_spritemanager;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AnimationManager m_animationmanager;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private PaletteFx m_palfx;

        #endregion
    }
}