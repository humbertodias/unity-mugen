using System;
using UnityEngine;

namespace UnityMugen
{
    public enum PlayerID { One, Two, Three, Four, None }

    public enum TypeEntity { Player, Helper, Projectile, Explod, GraphicUI }

    public enum Axis { None, X, Y }

    public enum ScreenType { None, Storyboard, Title, Menu, Select, Versus, Combat, Replay }

    public enum FadeDirection { None, In, Out }

    public enum Assertion { None, Intro, Invisible, RoundNotOver, NoBarDisplay, NoBackground, NoForeground, NoStandGuard, NoAirGuard, NoCrouchGuard, NoAutoturn, NoJuggleCheck, NoKOSound, NoKOSlow, NoShadow, GlobalNoShadow, NoMusic, NoWalk, TimerFreeze, Unguardable, NoKO }

    public enum BindToTargetPostion { None, Foot, Mid, Head }

    public enum Victory { None, Normal, Special, Hyper, NormalThrow, Cheese, Time, Suicude, TeamKill }

    public enum TeamSide { None, Left, Right }

    public enum TeamMode { None, Single, Simul, Turns, Double }

    public enum GameSpeed { Normal, Slow }

    public enum ContactType { None, Hit, Block, MissBlock, Reversal }

    [Flags]
    public enum SpeedTime { Infinity = 0, Slow = 1, Normal = 2, Fast = 3, Fastest = 4 }

    public enum DrawMode { None, Normal, Font, OutlinedRectangle, FilledRectangle, Lines }

    public enum CollisionType { None, PlayerPush, CharacterHit, ProjectileHit, ProjectileCollision }

    [Flags]
    public enum AttackStateType { None = 0, Standing = 1, Crouching = 2, Air = 4 }

    public enum AttackPower { None = 0, Normal, Special, Hyper, All }

    public enum AttackClass { None = 0, Normal, Throw, Projectile, All }

    public enum HitFlagCombo { No = 0, Yes, DontCare }

    [Flags]
    public enum AffectTeam { None = 0, Enemy = 1, Friendly = 2, Both = Enemy | Friendly }

    public enum HitAnimationType { None = 0, Light, Medium, Hard, Back, Up, DiagUp }

    public enum PriorityType { None, Hit, Dodge, Miss }

    public enum AttackEffect { None = 0, High, Low, Trip }

    public enum HelperType { Normal = 0, Player, Projectile }

    public enum PositionType
    {
        None = 0,
        P1,
        P2,
        Front,
        Back,
        Left,
        Right,

#warning adicionar a documentacao
        Center // Novo Tiago
    }

    public enum PositionTypeUI
    {
        Center = 0,
        Left,
        Right,
        Top,
        Botton,
        LeftTop,
        LeftBotton,
        RightTop,
        RightBotton
    }

    public enum ClsnType { None, Type1Attack, Type2Normal }

    public enum Facing { Left = 0, Right = 1 }

    public enum BlendType
    {
        None,
        Add,
        Add1,
        AddAlpha,
        Subtract
    }

    public enum SpaceType { Stage, Screen }

    public enum Layer { Front, Back, Full }

    public enum NumberType { None, Int, Float }

    public enum PrintJustification { Left, Right, Center }

    [Flags]
    public enum PlayerButton
    {
        None = 0,
        Up = 1 << 0,//1,
        Down = 1 << 1,//2,
        Left = 1 << 2,//4,
        Right = 1 << 3,//8,
        A = 1 << 4,//16,
        B = 1 << 5,//32,
        C = 1 << 6,//64,
        X = 1 << 7,//128,
        Y = 1 << 8,//256,
        Z = 1 << 9,//512,
        Taunt = 1 << 10,//1024,
        Start = 1 << 11,//2048,
        Select = 1 << 12,//4096
    }

    public enum RoundState { None, PreIntro, Intro, Fight, PreOver, Over }

    public enum RoundInformationType { None, Fight, Round, DrawGame, P1Win, P2Win, Win2, TimeOver, KO }

    public enum IntroState { None, Running, RoundNumber, Fight }

    public enum CommandDirection { None = 0, B, DB, D, DF, F, UF, U, UB, B4Way, U4Way, F4Way, D4Way }

    public enum StateType { None = 0, Unchanged = 1, Standing = 2, Crouching = 3, Airborne = 4, Liedown = 5 }

    public enum MoveType { None = 0, Idle = 1, Attack = 2, BeingHit = 3, Unchanged = 4 }

    public enum Physic { None = 0, Unchanged = 1, Standing = 2, Crouching = 3, Airborne = 4 }

    public enum PlayerControl { Unchanged = 0, InControl = 1, NoControl = 2 }

    public enum PlayerMode { Human, Ai }

    [Flags]
    public enum CommandButton { None = 0, A = 1, B = 2, C = 4, X = 8, Y = 16, Z = 32, Taunt = 64 }

    [Flags]
    public enum ForceFeedbackType { None = 0, Sine = 1, Square = 2, SineSquare = 3 }

    public enum ButtonState { Up, Down, Pressed, Released }

    public enum ProjectileDataType { None, Hit, Guarded, Cancel }

    public enum PauseState { Unpaused, Paused, PauseStep }

    public enum MainMenuOption { Arcade = 0, Versus = 1, TeamArcade = 2, TeamVersus = 3, TeamCoop = 4, Survival = 5, SurvivalCoop = 6, Training = 7, Watch = 8, Options = 9, Quit = 10 }

    public enum EntityUpdateOrder { Character, Projectile, Explod, GraphicUI };

    public enum ProjectileState { Normal, Removing, Canceling, Kill }

    public enum PlayerSelectType { Profile, Random }

    public enum CursorDirection { Up, Down, Left, Right }

    public enum CombatMode { None, Story, Arcade, Versus, TeamArcade, TeamVersus, TeamCoop, Survival, SurvivalCoop, Training, Network } // , NetworkServer, NetworkClient

    public enum StanceType { Standing, Crouching, Jumping, COM, Controller }

    public enum GuardType { NoGuard, AllGuard, RandomGuard }

    public enum SpriteEffects { None = 0, FlipHorizontally = 1, FlipVertically = 2, Both = 3 }

    public enum TypeCombatOver { None, KO_WIN_P1, KO_WIN_P1_PERFECT, KO_WIN_P2, KO_WIN_P2_PERFECT, TimeOver_P1WIN, TimeOver_P2WIN, TimeOver_P1WIN_PERFECT, TimeOver_P2WIN_PERFECT, TimeOver_DrawGame, Draw_Game }

    public enum TypeController { Joystick, keyboard }

    public enum TypeVar { Var, FVar, SysVar, SysFVar }

    public enum TypeStageVar
    {
        InfoAuthor,
        InfoDisplayname,
        InfoName,
        CameraBoundLeft,
        CameraBoundRight,
        CameraBoundHigh,
        CameraBoundLow,
        CameraVerticalFollow,
        CameraFloorTension,
        CameraTensionHigh,
        CameraTensionLow,
        CameraTension,
        CameraStartZoom,
        CameraZoomOut,
        CameraZooMin,
        CameraUTensionEnable,
        PlayerInfoLeftBound,
        PlayerInfoRightbound,
        ScalingTopScale,
        BoundScreenLeft,
        BoundScreenRight,
        StageInfoZoffSet,
        StageInfoZoffSetLink,
        StageInfoXScale,
        StageInfoYScale,
        ShadowIntensity,
        ShadowColorR,
        ShadowColorG,
        ShadowColorB,
        ShadowYScale,
        ShadowFadeRangeBegin,
        ShadowFadeRangeEnd,
        //ShadowXshear,
        ReflectionIntensity
    }

    public enum CustomKeyCode
    {
        //Controller - 1
        UP1 = 0,
        DOWN1 = 1,
        LEFT1 = 2,
        RIGHT1 = 3,
        X1 = 4,
        Y1 = 5,
        Z1 = 6,
        A1 = 7,
        B1 = 8,
        C1 = 9,
        TAUNT1 = 10,
        START1 = 11,
        SELECT1 = 12,

        //Controller - 2
        UP2 = 13,
        DOWN2 = 14,
        LEFT2 = 15,
        RIGHT2 = 16,
        X2 = 17,
        Y2 = 18,
        Z2 = 19,
        A2 = 20,
        B2 = 21,
        C2 = 22,
        TAUNT2 = 23,
        START2 = 24,
        SELECT2 = 25
    }

    public enum TypeEntityDraw
    {
        Draw_2D = 0, Draw_3D = 1
    }

    public enum TypeConnection
    {
        None = 0, Server = 1, Client = 2
    }

    public enum TipePopupMessage
    {
        None,
        OK,
        Error,
        Warning
    }

    public enum TipeMessageHostClient
    {
        None = 0,
        Chat = 1,
        SelectScreenLoaded = 2,
        CombatScreenLoaded = 3,
        CommandButton = 4,
        RollbackStarted = 5
    }

    public enum TypeSound
    {
        NONE, SELECT, SELECTED, CANCEL
    }

    public enum GamepadIndex
    {
        GamepadOne = 0, GamepadTwo, GamepadThree, GamepadFour
    }


    // NetworkSettings


    public enum NetworkMessageSize
    {
        Size8Bits,
        Size16Bits,
        Size32Bits,
    }

    public enum NetworkRollbackBalancing
    {
        Disabled,
        Conservative,
        Aggressive,
    }

    public enum NetworkFrameDelay
    {
        Disabled,
        Fixed,
        Auto
    }

    public enum NetworkInputMessageFrequency
    {
        EveryFrame,
        EveryOtherFrame,
        EveryFewFrames,
    }

    public enum NetworkSynchronizationMessageFrequency
    {
        Disabled,
        EveryFrame,
        EverySecond,
    }


    // DEBUG

    public enum TypeDrawCollider
    {
        None,
        Frame,
        Paint,
        Both
    }

    public enum MugenVersion
    {
        [InspectorName("2002")] V_2002 = 0,
        [InspectorName("1.0")] V_1_0 = 1,
        [InspectorName("1.1")] V_1_1 = 2,
    }

}