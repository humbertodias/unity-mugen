using System;
using System.Diagnostics;
using UnityEngine;
using UnityMugen.Animations;
using UnityMugen.Drawing;

namespace UnityMugen.Combat
{
    public class Projectile : Entity
    {

        public Projectile Iniciar(Character creator, ProjectileData data)
        {
            if (creator == null) throw new ArgumentNullException(nameof(creator));
            if (data == null) throw new ArgumentNullException(nameof(data));

            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.material = new Material(Shader.Find("UnityMugen/Sprites/ColorSwap"));
            spriteRenderer.sortingOrder = 3;
            spriteRenderer.sortingLayerName = "Entity";

            Id = data.ProjectileId;
            NameSearch = data.ProjectileId.ToString();
            typeEntity = TypeEntity.Projectile;

            IniciarEntity();

            //positionOffset = transform;

            draw = gameObject.AddComponent<DrawColliders>();
            TrainnerSettings trainner = Launcher.trainnerSettings;
            draw.normal = trainner.normal;
            draw.attack = trainner.attack;
            draw.hitable = trainner.hitable;

            Creator = creator;
            BasePlayer = Creator.BasePlayer;

            m_offsetcharacter = data.PositionType == PositionType.P2 ? creator.GetOpponent() : creator;
            Data = data;
            m_animationmanager = Creator.AnimationManager.Clone();
            m_spritemanager = Creator.SpriteManager.Clone();
            GameTicks = 0;
            HitCountdown = 0;
            State = ProjectileState.Normal;
            TotalHits = 0;
            HitPauseCountdown = 0;
            Priority = Data.Priority;
            m_palfx = new PaletteFx();

            PaletteList = Creator.PaletteList;
            CurrentFacing = Creator.CurrentFacing;
            CurrentLocation = GetStartLocation();
            CurrentVelocity = Data.InitialVelocity;
            CurrentAcceleration = Data.Acceleration;
            //CurrentFlip = SpriteEffects.None;

            PlayerConstants pc = creator.BasePlayer.playerConstants;
            if (pc.ProjectileScaling)
                CurrentScale = new Vector2(Data.Scale.x * pc.Scale.x, Data.Scale.y * pc.Scale.y);
            else
                CurrentScale = Data.Scale;

            DrawOrder = Data.SpritePriority;

            SetLocalAnimation(Data.AnimationNumber, 0);
            return this;
        }

        public Vector2 GetStartLocation()
        {
            var camerabounds = Engine.CameraFE.ScreenBounds();
            var facing = m_offsetcharacter.CurrentFacing;
            Vector2 location;

            switch (Data.PositionType)
            {
                case PositionType.P1:
                case PositionType.P2:
                    return Misc.GetOffset(m_offsetcharacter.CurrentLocation, facing, Data.CreationOffset);

                case PositionType.Left:
                    return Misc.GetOffset(new Vector2(camerabounds.xMin, 0), Facing.Right, Data.CreationOffset);

                case PositionType.Right:
                    return Misc.GetOffset(new Vector2(camerabounds.xMax, 0), Facing.Right, Data.CreationOffset);

                case PositionType.Back:
                    location = Misc.GetOffset(Vector2.zero, facing, Data.CreationOffset);
                    location.x += facing == Facing.Right ? camerabounds.xMin : camerabounds.xMax;
                    return location;

                case PositionType.Front:
                    location = Misc.GetOffset(Vector2.zero, facing, Data.CreationOffset);
                    location.x += facing == Facing.Left ? camerabounds.xMin : camerabounds.xMax;
                    return location;

                default:
                    throw new ArgumentOutOfRangeException("postype");
            }
        }

        public override Vector2 GetDrawLocation(bool _ = false)
        {
            return CurrentLocation;
        }

        public override bool RemoveCheck()
        {
            return State == ProjectileState.Kill;
        }

        public override void UpdatePhsyics()
        {
            if (HitPauseCountdown == 0)
            {
                base.UpdatePhsyics();

                CurrentVelocity *= Data.VelocityMultiplier;
            }
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if (GameTicks == 0)
            {
                CurrentLocation = GetStartLocation();
            }

            ++GameTicks;

            if (HitPauseCountdown > 0)
            {
                --HitPauseCountdown;
            }
            else if (HitCountdown > 0)
            {
                --HitCountdown;
            }

            if (State == ProjectileState.Canceling || State == ProjectileState.Removing)
            {
                if (AnimationManager.IsAnimationFinished)
                    State = ProjectileState.Kill;
                return;
            }

            var camera_rect = Engine.CameraFE.ScreenBounds();
            var drawlocation = GetDrawLocation();
            var stage = Engine.stageScreen.Stage;

            // Comentei o [=] de [>=] pois parece que algumas acoes como TE.Time() precisa de 1 frame de atraso para remover o Projectile. - Tiago
            if (Data.RemoveTimeout != -1 && GameTicks != 1 && GameTicks >/*=*/ Data.RemoveTimeout) StartRemoval();

            if (CurrentLocation.x < camera_rect.xMin - Data.StageEdgeBound) StartRemoval();
            if (CurrentLocation.x > camera_rect.xMax + Data.StageEdgeBound) StartRemoval();

            if (CurrentLocation.x < camera_rect.xMin - Data.ScreenEdgeBound) StartRemoval();
            if (CurrentLocation.x > camera_rect.xMax + Data.ScreenEdgeBound) StartRemoval();

#warning This could be an issue.
            var z = -Camera.main.transform.position.z;
            float HeightUpperBound = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height + (Data.HeightUpperBound *(Screen.height / 240)), z)).y;
            var camerabounds = Engine.CameraFE.ScreenBounds();
            if (CurrentVelocity.y < 0 && CurrentLocationYTransform().y > (HeightUpperBound + (Screen.height / (Screen.height + Data.HeightUpperBound))))
                StartRemoval();

            float HeightLowerBound = ((240+Data.HeightLowerBound) * (Screen.height / 240));
            if (CurrentVelocity.y > 0 && CurrentLocationYTransform().y < HeightLowerBound)
                StartRemoval();

            if (Data.RemoveOnHit && TotalHits >= Data.HitsBeforeRemoval)
                StartHitRemoval();
        }

#warning em analize 19.07.2023
        public override bool IsPaused(Pause pause)
        {
            if (base.IsPaused(pause) == false) return false;

            if (pause.IsSuperPause)
            {
                if ((pause.ElapsedTime <= Data.SuperPauseMoveTime) || Data.SuperPauseMoveTime == -1)
                    return false;
            }
            else
            {
                if ((pause.ElapsedTime <= Data.PauseMoveTime) || Data.PauseMoveTime == -1)
                    return false;
            }

            return true;
        }

        private Facing GetStartFacing()
        {
            var facing = Facing.Left;

            switch (Data.PositionType)
            {
                case PositionType.P1:
                case PositionType.P2:
                case PositionType.Back:
                    facing = m_offsetcharacter.CurrentFacing;
                    break;

                case PositionType.Front:
                case PositionType.Left:
                case PositionType.Right:
                    facing = Facing.Right;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("Data.PositionType");
            }

            return facing;
        }

        public void StartHitRemoval()
        {
            if (State == ProjectileState.Removing) return;

            if (AnimationManager.HasAnimation(Data.HitAnimationNumber) && AnimationManager.CurrentAnimation.Number != Data.HitAnimationNumber)
            {
                State = ProjectileState.Removing;
                SetLocalAnimation(Data.HitAnimationNumber, 0);
                CurrentVelocity = Data.RemoveVelocity;
            }
            else
            {
                State = ProjectileState.Kill;
            }
        }

        public void StartRemoval()
        {
            if (State == ProjectileState.Removing) return;

            if (AnimationManager.HasAnimation(Data.RemoveAnimationNumber) && AnimationManager.CurrentAnimation.Number != Data.RemoveAnimationNumber)
            {
                State = ProjectileState.Removing;
                SetLocalAnimation(Data.RemoveAnimationNumber, 0);
                CurrentVelocity = Data.RemoveVelocity;
            }
            else
            {
                State = ProjectileState.Kill;
            }
        }

        public void StartCanceling()
        {
            if (State == ProjectileState.Canceling) return;

            if (AnimationManager.HasAnimation(Data.CancelAnimationNumber) && AnimationManager.CurrentAnimation.Number != Data.CancelAnimationNumber)
            {
                State = ProjectileState.Canceling;
                SetLocalAnimation(Data.CancelAnimationNumber, 0);
                BasePlayer.OffensiveInfo.ProjectileInfo.Set(Id, ProjectileDataType.Cancel);
            }
            else
            {
                State = ProjectileState.Kill;
            }
        }

        public bool CanAttack()
        {
            if (State != ProjectileState.Normal) return false;
            if (Data.HitDef == null) return false;
            if (HitPauseCountdown > 0) return false;
            if (HitCountdown > 0) return false;
            if (TotalHits >= Data.HitsBeforeRemoval) return false;

            return true;
        }


        public ProjectileData Data;
        public int TotalHits;
        public int HitCountdown;
        public int Priority;
        public int HitPauseCountdown;
        public ProjectileState State;
        public int GameTicks;

        public override SpriteManager SpriteManager => m_spritemanager;
        public override Animations.AnimationManager AnimationManager => m_animationmanager;
        public override EntityUpdateOrder UpdateOrder => EntityUpdateOrder.Projectile;
        public override PaletteFx PaletteFx => m_palfx;
        public override Team Team => BasePlayer.Team;



        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Character m_offsetcharacter;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SpriteManager m_spritemanager;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AnimationManager m_animationmanager;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private PaletteFx m_palfx;

        #endregion
    }
}