using System;
using System.Collections.Generic;
using UnityEngine;
using UnityMugen.Animations;
using UnityMugen.Commands;
using UnityMugen.Drawing;
using UnityMugen.StateMachine;

namespace UnityMugen.Combat
{

    public class Player : Character
    {
        //public GameObject entity2D;
        //public PaletteManager paletteManager;

        public PlayerConstants playerConstants { get; set; }
        public PlayerProfileManager profile { get; set; }
        //public AudiosClipsManager audiosClipsManager;
        //public CommandList commandList;
        //public AnimationFEManager animationFEManager { get; set; }
        public SpriteManager spriteManager { get; set; }

        private float m_power;
        private int m_score;
        private int m_palettenumber;

        private Transform[] dimensions = new Transform[2];

        private UnityMugen.Audio.SoundManager m_soundmanager;
        public override UnityMugen.Audio.SoundManager SoundManager => m_soundmanager;

        private AnimationManager m_animationmanager;
        public override UnityMugen.Animations.AnimationManager AnimationManager => m_animationmanager;

        private SpriteManager m_spriteManager;
        public override SpriteManager SpriteManager => m_spriteManager;

        private StateManager m_statemanager;
        public override StateManager StateManager => m_statemanager;

        private ICommandManager m_commandmanager;
        public override ICommandManager CommandManager => m_commandmanager;

        private CharacterDimensions m_dimensions;
        public override CharacterDimensions Dimensions => m_dimensions;

        private Team m_team;
        public override Team Team => m_team;

        private Dictionary<long, List<Helper>> m_helpers;
        public Dictionary<long, List<Helper>> Helpers => m_helpers;

        private PaletteFx m_palfx;
        public override PaletteFx PaletteFx => m_palfx;


        public float LastTickPowerSet { get; set; } // Usado no Modo Trainner

        [NonSerialized] public bool iniciado = false;

        public Player Iniciar(PlayerMode mode, Team team, PlayerProfileManager profile, int paletteIndex)
        {
            if (team == null) throw new ArgumentNullException(nameof(team));

            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.material = new Material(Shader.Find("UnityMugen/Sprites/ColorSwap"));
            spriteRenderer.sortingLayerName = "Entity";

            this.profile = profile;
            NameSearch = profile.displayName;
            playerConstants = Instantiate(profile.playerConstants);
            playerConstants.Iniciar();

            UniqueID = Id = Engine.GenerateCharacterId();
            PaletteNumber = paletteIndex;
            typeEntity = TypeEntity.Player;
            BasePlayer = this;

            IniciarCharacter();
            IniciarEntity();

            if (Engine.Initialization.Mode == CombatMode.Training && Id == 1)
            {
                TrainnerSettings settings = LauncherEngine.Inst.trainnerSettings;
                if (settings.stanceType == StanceType.COM)
                    mode = PlayerMode.Ai;
                else
                    mode = PlayerMode.Human;
            }

            PlayerMode = mode;
            CurrentScale = playerConstants.Scale;

            draw = gameObject.AddComponent<DrawColliders>();
            TrainnerSettings trainner = Launcher.trainnerSettings;
            draw.normal = trainner.normal;
            draw.attack = trainner.attack;
            draw.hitable = trainner.hitable;

            m_soundmanager = Launcher.soundSystem.CreateManager(profile.NamefileSND());
            m_spriteManager = Launcher.spriteSystem.CreateManager(profile.NamefileSFF(), profile.NamePalettes());
            m_animationmanager = Launcher.animationSystem.CreateManager(profile.NamefileAIR(), CurrentScale);
            m_commandmanager = Launcher.commandSystem.CreateManager(mode, Instantiate(profile.commandsList).Internal());
            m_dimensions = new CharacterDimensions(playerConstants);
            
            PaletteList = m_spriteManager.Palettes;
            
            PaletteList.PalTable.TryGetValue(PaletteId.Default, out int sourceIndex);
            PaletteList.PalTable.TryGetValue(new PaletteId(1, PaletteNumber), out int destIndex);
            PaletteList.PalTex[sourceIndex] = PaletteList.PalTexBackup[destIndex];

            m_power = 0;
            m_palfx = new PaletteFx();
            m_team = team;
            m_helpers = new Dictionary<long, List<Helper>>();

            SetLocalAnimation(0, 0);

            PushFlag = true;

            shadowOffset = playerConstants.Shadowoffset;
            DrawShadow = true;
            DrawReflection = true;

            dimensions[0] = Dimension(Color.cyan, DrawColliders.SpriteDimensionFront, "DimensionFront");
            dimensions[1] = Dimension(Color.magenta, DrawColliders.SpriteDimensionBack, "DimensionBack");

            if (profile.states.Length > 0)
            {
                m_statemanager = Launcher.stateSystem.CreateManager(this as Player, profile.states);
            }

            return this;
        }


        Transform Dimension(Color color, Sprite sprite, string name)
        {
            GameObject dimension = new GameObject();
            dimension.hideFlags = HideFlags.HideAndDontSave;
            dimension.name = name;
            Vector2 pos = this.CurrentLocationYTransform();
            SpriteRenderer spriteRenderer = dimension.AddComponent<SpriteRenderer>();
            spriteRenderer.color = color;
            spriteRenderer.sortingOrder = 99;
            spriteRenderer.sortingLayerName = "Entity";
            spriteRenderer.sprite = sprite;
            dimension.transform.SetParent(gameObject.transform);
            return dimension.transform;
        }


        public override void Draw(int orderDraw)
        {
            base.Draw(orderDraw);
            base.Shadow();
            base.Reflection(orderDraw);
        }

        public override void DebugDraw()
        {
            base.DebugDraw();

            if (Launcher.trainnerSettings.typeDrawCollider != TypeDrawCollider.None)
            {
                dimensions[0].gameObject.SetActive(true);
                dimensions[1].gameObject.SetActive(true);
                if (CurrentFacing == Facing.Right)
                    dimensions[0].localScale = new Vector3(Dimensions.GetFrontWidth(StateType) * Constant.Scale2, 1, 1);
                else
                    dimensions[0].localScale = new Vector3(-(Dimensions.GetFrontWidth(StateType) * Constant.Scale2), 1, 1);

                if (CurrentFacing == Facing.Right)
                    dimensions[1].localScale = new Vector3(Dimensions.GetBackWidth(StateType) * Constant.Scale2, 1, 1);
                else
                    dimensions[1].localScale = new Vector3(-(Dimensions.GetBackWidth(StateType) * Constant.Scale2), 1, 1);
            }
            else
            {
                dimensions[0].gameObject.SetActive(false);
                dimensions[1].gameObject.SetActive(false);
            }
        }

        public override void ResetFE()
        {
            base.ResetFE();

            PushFlag = true;
            m_helpers.Clear();
        }

        public override void CleanUp()
        {
            base.CleanUp();

            if (InHitPause == false)
            {
                PushFlag = true;
            }
        }

        public override bool RemoveCheck()
        {
            return false;
        }

#warning testar futuramente o trecho de helper com KeyControl true para ver como ele esta se comportando
        public override void UpdateInput()
        {
            foreach (var entity in Engine.Entities)
            {
                var helper = entity as Helper;
                if (helper == null || helper.BasePlayer != this) continue;

                if (helper.Data.KeyControl)
                {
                    helper.CurrentInput = CurrentInput;
                }

            }

            base.UpdateInput();
        }

        public int PaletteNumber
        {
            get { return m_palettenumber; }
            set { m_palettenumber = value; }
        }

        //public void UpdatePalette()
        //{
        //    if (PaletteList.PalTex.Count > 0)
        //    {
        //        if (m_palettenumber >= PaletteList.PalTex.Count)
        //            m_palettenumber = PaletteList.PalTex.Count - 1;

        //    //    CurrentPalette = PaletteList.PalTex[m_palettenumber];
        //    }
        //}

        public float Power
        {
            get { return m_power; }
            set
            {
                LastTickPowerSet = Engine.TickCount;
                value = Misc.Clamp(value, 0, playerConstants.MaximumPower);
                if (value > m_power)
                {
                    if (m_power < 1000 && value >= 1000 && value < 2000) Engine.RoundInformation.PlaySoundBar(0);
                    if (m_power < 2000 && value >= 2000 && value < 3000) Engine.RoundInformation.PlaySoundBar(1);
                    if (m_power < 3000 && value >= 3000 && value < 4000) Engine.RoundInformation.PlaySoundBar(2);
                    if (m_power < 4000 && value >= 4000 && value < 5000) Engine.RoundInformation.PlaySoundBar(3);
                    if (m_power < 5000 && value >= 5000 && value < 6000) Engine.RoundInformation.PlaySoundBar(4);
                    if (m_power < 6000 && value >= 6000 && value < 7000) Engine.RoundInformation.PlaySoundBar(5);
                    if (m_power < 7000 && value >= 7000 && value < 8000) Engine.RoundInformation.PlaySoundBar(6);
                    if (m_power < 8000 && value >= 8000 && value < 9000) Engine.RoundInformation.PlaySoundBar(7);
                    if (m_power < 9000 && value >= 9000 && value < 10000) Engine.RoundInformation.PlaySoundBar(8);
                }
                m_power = value;
            }
        }

        public int Score
        {
            get { return m_score; }
            set
            {
                m_score = value;
                m_score = (int)Math.Round((decimal)m_score / 100, 0) * 100;
            }
        }

    }
}