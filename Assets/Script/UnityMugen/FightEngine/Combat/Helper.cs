using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityMugen.Commands;
using UnityMugen.Drawing;
using UnityMugen.StateMachine;

namespace UnityMugen.Combat
{

    public class Helper : Character
    {

        public Helper Iniciar(Character creator, HelperData data)
        {
            if (creator == null) throw new ArgumentNullException(nameof(creator));
            if (data == null) throw new ArgumentNullException(nameof(data));

            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.material = new Material(Shader.Find("UnityMugen/Sprites/ColorSwap"));
            spriteRenderer.sortingOrder = 3;
            spriteRenderer.sortingLayerName = "Entity";

            Creator = creator;
            BasePlayer = Creator.BasePlayer;
            m_team = BasePlayer.Team;

            Childrens = new List<Helper>();

            Id = data.HelperId;
            NameSearch = data.Name;
            gameObject.name = "Helper_" + Id.ToString() + "_" + data.Name;
            typeEntity = TypeEntity.Helper;

            IniciarCharacter();
            IniciarEntity();

            draw = gameObject.AddComponent<DrawColliders>();
            TrainnerSettings trainner = Launcher.trainnerSettings;
            draw.normal = trainner.normal;
            draw.attack = trainner.attack;
            draw.hitable = trainner.hitable;


            m_offsetcharacter = data.PositionType == PositionType.P2 ? creator.GetOpponent() : creator;
            RemoveHelper = false;
            Data = data;
            FirstTick = true;
            shadowOffset = data.ShadowOffset;
            DrawShadow = true;
            DrawReflection = true;
            m_statemanager = Creator.StateManager.Clone(this as Character);
            m_spritemanager = Creator.SpriteManager.Clone();
            m_animationmanager = Creator.AnimationManager.Clone();
            m_commandmanager = Creator.CommandManager.Clone();
            m_soundmanager = Creator.SoundManager.Clone();
            m_dimensions = new CharacterDimensions(Data.GroundFront, Data.GroundBack, Data.AirFront, Data.AirBack, Data.Height);
            m_palfx = Data.OwnPaletteFx ? new PaletteFx() : Creator.PaletteFx;

            PaletteList = Creator.PaletteList;
            CurrentFacing = GetFacing(Data.PositionType, m_offsetcharacter.CurrentFacing, Data.FacingFlag < 0);
            CurrentLocation = GetStartLocation();
            CurrentScale = Data.Scale;

            SetLocalAnimation(0, 0);

            if (!StateManager.ChangeState(Data.InitialStateNumber))
                   AnimationManager.CurrentElement = null;

            return this;
        }

        public override void Draw(int orderDraw)
        {
            base.Draw(orderDraw);
            base.Shadow();
            base.Reflection(orderDraw);
        }

        public Vector2 GetStartLocation()
        {
            var offset = Data.CreationOffset;
            var camerabounds = Engine.CameraFE.ScreenBounds();
            var facing = m_offsetcharacter.CurrentFacing;
            var x = ((offset.x * Constant.Scale2) * (Screen.width / Constant.LocalCoord.x));
            var y = Screen.height - ((offset.y * Constant.Scale2) * (Screen.height / 240));
            var z = -Camera.main.transform.position.z;
            Vector2 point = Camera.main.ScreenToWorldPoint(new Vector3(x, y, z));

            switch (Data.PositionType)
            {
                // Posição de Helper é diferente de Explod y = 0 em Helper 
                // representa o ground, já em Explod representa o topo da tela
                case PositionType.P1:
                case PositionType.P2:
                    return Misc.GetOffset(m_offsetcharacter.CurrentLocation, facing, offset);

                case PositionType.Left:
                    //localcoord = 320, 240 [valor encontrado em .def de stages];
                    float leftCam = Misc.GetOffset(camerabounds.xMin, Facing.Right, offset.x);
                    CurrentFacing = Facing.Right;
                    return new Vector3(leftCam, offset.y, transform.localPosition.z);

                case PositionType.Right:
                    float rightCam = Misc.GetOffset(camerabounds.xMax, Facing.Right, offset.x);
                    CurrentFacing = Facing.Right;
                    return new Vector3(rightCam, offset.y, transform.localPosition.z);

                case PositionType.Back:
                    Vector2 location = Misc.GetOffset(Vector2.zero, facing, offset);
                    location.x += facing == Facing.Right ? camerabounds.xMin : camerabounds.xMax;
                    //location.y = -point.y;
                    return location;

                case PositionType.Front:
                    Vector2 location2 = Misc.GetOffset(Vector2.zero, m_offsetcharacter.CurrentFacing, offset);
                    location2.x += facing == Facing.Left ? camerabounds.xMin : camerabounds.xMax;
                    //location2.y = -point.y;
                    return location2;

                default:
                    throw new InvalidOperationException("Data.PositionType");
            }
        }

        public override void UpdateState()
        {
            if (FirstTick)
            {
                FirstTick = false;

                CurrentLocation = GetStartLocation();
            }

            base.UpdateState();
        }

        public override void UpdateInput()
        {
            if (Data.KeyControl)
            {
                base.UpdateInput();
            }
        }


        //public override void RecieveInput(PlayerButton button, bool pressed)
        //{
        //    if (Data.KeyControl)
        //    {
        //        base.RecieveInput(button, pressed);
        //    }
        //}

        public override bool IsPaused(Pause pause)
        {
            if (base.IsPaused(pause) == false) return false;

            if (pause.IsSuperPause)
            {
                if ((pause.ElapsedTime <= Data.SuperPauseTime) || Data.SuperPauseTime == -1)
                    return false;
            }
            else
            {
                if ((pause.ElapsedTime <= Data.PauseTime) || Data.PauseTime == -1)
                    return false;
            }

            return true;
        }

        private static Facing GetFacing(PositionType ptype, Facing characterfacing, bool facingflag)
        {
            switch (ptype)
            {
                case PositionType.P1:
                case PositionType.Front:
                case PositionType.Back:
                case PositionType.P2:
                    return facingflag ? Misc.FlipFacing(characterfacing) : characterfacing;

                case PositionType.Left:
                case PositionType.Right:
                    return facingflag ? Facing.Left : Facing.Right;

                default:
                    throw new ArgumentNullException(nameof(ptype));
            }
        }

        public void Remove(bool recursive = false, bool removeExplods = false)
        {
            RemoveHelper = true;
            Engine.Entities.Remove(this);

            // Adicionado Novo Teste Tiago
            if (removeExplods)
            {
                var removelist = new List<Combat.Explod>(GetExplods(-1));
                foreach (var explod in removelist)
                {
                    if (explod.Creator is Helper && explod.Creator == this)
                        explod.Kill();
                }
            }
            if (recursive)
            {
                foreach (Helper h in Childrens)
                {
                    h.Remove(recursive, removeExplods);
                }
            }
            ///////////////////////////
        }

        public override bool RemoveCheck()
        {
            return RemoveHelper;
        }

        protected override void Bounding()
        {
            if (Data.Type == HelperType.Player)
            {
                base.Bounding();
            }
        }


        public bool RemoveHelper { set; get; }
        public bool FirstTick { set; get; }
        public HelperData Data;

        public List<Helper> Childrens { set; get; }

        public override SpriteManager SpriteManager => m_spritemanager;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Drawing.SpriteManager m_spritemanager;

        public override Audio.SoundManager SoundManager => m_soundmanager;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Audio.SoundManager m_soundmanager;

        public override ICommandManager CommandManager => m_commandmanager;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ICommandManager m_commandmanager;

        public override StateManager StateManager => m_statemanager;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private StateManager m_statemanager;

        public override CharacterDimensions Dimensions => m_dimensions;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private CharacterDimensions m_dimensions;

        public override UnityMugen.Animations.AnimationManager AnimationManager => m_animationmanager;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private UnityMugen.Animations.AnimationManager m_animationmanager;
                
        public override PaletteFx PaletteFx => m_palfx;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private PaletteFx m_palfx;

        public override Team Team => m_team;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Team m_team;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Character m_offsetcharacter;

    }
}