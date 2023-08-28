using System;
using System.Collections.Generic;
using UnityEngine;
using UnityMugen;
using UnityMugen.Animations;
using UnityMugen.Combat;
using UnityMugen.Commands;
using UnityMugen.StateMachine;
using Animation = UnityMugen.Animations.Animation;

public class StatemanagerMemory
{
    public StatemanagerMemory(StateManager manager)
    {
        StateNumber = manager.StateNumber;
        StateTime = manager.StateTime;
        CurrentState = manager.CurrentState;
        PreviousState = manager.PreviousState;
        if (manager.ForeignManager != null)
            ForeignManager = new StatemanagerMemory(manager.ForeignManager);
    }

    public void BackMemory(ref StateManager manager)
    {
        manager.StateNumber = StateNumber;
        manager.StateTime = StateTime;
        manager.CurrentState = CurrentState;
        manager.PreviousState = PreviousState;

        if (ForeignManager != null)
            ForeignManager.BackMemory(ref manager.ForeignManager);
    }

    public int StateNumber { get; }
    public int StateTime { get; }
    public State CurrentState { get; }
    public State PreviousState { get; }
    public StatemanagerMemory ForeignManager { get; }
}

public class AnimationManagerMemory
{
    public AnimationManagerMemory(AnimationManager manager)
    {
        TimeInAnimation = manager.TimeInAnimation;
        Elementswitchtime = manager.Elementswitchtime;
        Animationinloop = manager.Animationinloop;
        IsForeignAnimation = manager.IsForeignAnimation;
        IsAnimationFinished = manager.IsAnimationFinished;

        CurrentAnimation = manager.CurrentAnimation;
        CurrentElement = manager.CurrentElement;
    }

    public void BackMemory(ref AnimationManager manager)
    {
        manager.TimeInAnimation = TimeInAnimation;
        manager.Elementswitchtime = Elementswitchtime;
        manager.Animationinloop = Animationinloop;
        manager.IsForeignAnimation = IsForeignAnimation;
        manager.IsAnimationFinished = IsAnimationFinished;

        manager.CurrentAnimation = CurrentAnimation;
        manager.CurrentElement = CurrentElement;
    }

    public int TimeInAnimation { get; }
    public int Elementswitchtime { get; }
    public bool Animationinloop { get; }
    public bool IsForeignAnimation { get; }
    public bool IsAnimationFinished { get; }

    public Animation CurrentAnimation;
    public AnimationElement CurrentElement;
}

public class EntityMemory
{
    public EntityMemory(Entity entity)
    {
        UniqueID = entity.UniqueID;

        typeEntity = entity.typeEntity;
        CurrentLocation = entity.CurrentLocation;

        CurrentFacing = entity.CurrentFacing;
        CurrentVelocity = entity.CurrentVelocity;
        CurrentAcceleration = entity.CurrentAcceleration;
        CurrentFlip = entity.CurrentFlip;
        CurrentScale = entity.CurrentScale;
        DrawingAngle = entity.DrawingAngle;
        AngleDraw = entity.AngleDraw;
        DrawOrder = entity.DrawOrder;

        PlayerBaseUnityID = entity.BasePlayer.UniqueID;
        Creator = entity.Creator;
    }

    public void BackMemory(ref Entity entity)
    {
        entity.typeEntity = typeEntity;
        entity.CurrentLocation = CurrentLocation;
        entity.CurrentFacing = CurrentFacing;
        entity.CurrentVelocity = CurrentVelocity; //Importante que CurrentVelocity fique abaixo de CurrentFacing
        entity.CurrentAcceleration = CurrentAcceleration; //Importante que CurrentAcceleration fique abaixo de CurrentFacing
        entity.CurrentFlip = CurrentFlip;
        entity.CurrentScale = CurrentScale;
        entity.DrawingAngle = DrawingAngle;
        entity.AngleDraw = AngleDraw;
        entity.DrawOrder = DrawOrder;

        if (entity.Engine.Entities.TryGetValue(PlayerBaseUnityID, out Entity player))
            entity.BasePlayer = player as Player;

        entity.Creator = Creator;
    }

    public TypeEntity typeEntity { get; }
    public long UniqueID { get; }

    public Vector2 CurrentLocation { get; set; }
    public Vector2 CurrentVelocity { get; }
    public Vector2 CurrentAcceleration { get; }
    public Facing CurrentFacing { get; }
    public SpriteEffects CurrentFlip { get; set; }
    public Vector2 CurrentScale { get; }
    public float DrawingAngle { get; }
    public bool AngleDraw { get; }
    public int DrawOrder { get; }

    public long PlayerBaseUnityID { get; }
    public Character Creator { get; }
}

public class CharacterMemory : EntityMemory
{

    public CharacterMemory(Character character) : base(character)
    {
        life = character.Life;
        PushFlag = character.PushFlag;
        DrawScale = character.DrawScale;
        DrawOffset = character.DrawOffset;
        RoundsExisted = character.RoundsExisted;
        CameraFollowX = character.CameraFollowX;
        CameraFollowY = character.CameraFollowY;
        ScreenBound = character.ScreenBound;
        ScreenBound = character.ScreenBound;
        JugglePoints = character.JugglePoints;
        StateType = character.StateType;
        MoveType = character.MoveType;
        PlayerControl = character.PlayerControl;
        Physics = character.Physics;
        PositionFreeze = character.PositionFreeze;
        InHitPause = character.InHitPause;
        FallTime = character.FallTime;


        CurrentInput = character.CurrentInput;
        if (character.CommandManager is CommandManager command)
        {

            m_activecommands = new string[command.m_activecommands.Count];
            /*m_activecommands = */
            command.m_activecommands.CopyTo(m_activecommands);

            // m_inputbuffer = new PlayerButton[command.m_inputbuffer.Size];
            m_inputbuffer = (PlayerButton[])command.m_inputbuffer.Buffer.m_data.Clone();
            //for (int i = 0; i < command.m_inputbuffer.Size; i++)
            //    m_inputbuffer[command.m_inputbuffer.Size - i] = [i];

            //m_inputbuffer = new InputBuffer(buffer);

            //command.m_inputbuffer.Buffer

            //m_inputbuffer = command.m_inputbuffer;

            m_commandcount = new Dictionary<string, BufferCount>(command.CommandCount.Count);

            foreach (var cc in command.CommandCount)
            {
                m_commandcount.Add(cc.Key, new BufferCount(cc.Value.m_value));
            }

        }



        Explods = new Dictionary<long, List<long>>();
        foreach (long ex in character.Explods.Keys)
        {
            List<long> listDataExplod = new List<long>();
            foreach (Explod explod in character.Explods[ex])
            {
                listDataExplod.Add(explod.UniqueID);
            }
            Explods.Add(ex, listDataExplod);
        }

        Variables = new CharacterVariables(character.Variables);
        OffensiveInfo = new OffensiveInfo(character.OffensiveInfo);
        DefensiveInfo = new DefensiveInfo(character.DefensiveInfo);
        StatemanagerMemory = new StatemanagerMemory(character.StateManager);
        AnimationManagerMemory = new AnimationManagerMemory(character.AnimationManager);
    }

    public void BackMemory(ref Character character)
    {
        character.Life = life;
        character.PushFlag = PushFlag;
        character.DrawScale = DrawScale;
        character.DrawOffset = DrawOffset;
        character.RoundsExisted = RoundsExisted;
        character.CameraFollowX = CameraFollowX;
        character.CameraFollowY = CameraFollowY;
        character.ScreenBound = ScreenBound;
        character.ScreenBound = ScreenBound;
        character.JugglePoints = JugglePoints;
        character.StateType = StateType;
        character.MoveType = MoveType;
        character.PlayerControl = PlayerControl;
        character.Physics = Physics;
        character.PositionFreeze = PositionFreeze;
        character.InHitPause = InHitPause;
        character.FallTime = FallTime;

        character.CurrentInput = CurrentInput;
        if (character.CommandManager is CommandManager command)
        {
            command.m_activecommands.Clear();
            command.m_activecommands.AddRange(m_activecommands);// = .to;

            command.m_inputbuffer = new InputBuffer(m_inputbuffer);
            command.CommandCount = m_commandcount;
        }

        character.OffensiveInfo = OffensiveInfo;
        character.DefensiveInfo = DefensiveInfo;
        character.Variables = Variables;

        StateManager sm = character.StateManager;
        StatemanagerMemory.BackMemory(ref sm);

        AnimationManager am = character.AnimationManager;
        AnimationManagerMemory.BackMemory(ref am);





        Entity e = character as Entity;
        base.BackMemory(ref e);
    }

    public float life { get; }
    public bool PushFlag { get; }
    public Vector2 DrawScale { get; }
    public Vector2 DrawOffset { get; }
    public int RoundsExisted { get; }
    public bool CameraFollowX { get; }
    public bool CameraFollowY { get; }
    public bool ScreenBound { get; }
    public int JugglePoints { get; }
    public StateType StateType { get; }
    public MoveType MoveType { get; }
    public PlayerControl PlayerControl { get; }
    public Physic Physics { get; }
    public Boolean PositionFreeze { get; }
    public Boolean InHitPause { get; }
    public int FallTime { get; }

    public PlayerButton CurrentInput { get; }
    public string[] m_activecommands { get; }
    public PlayerButton[] m_inputbuffer { get; }
    public Dictionary<string, BufferCount> m_commandcount { get; }

    public CharacterVariables Variables { get; }
    public OffensiveInfo OffensiveInfo { get; }
    public DefensiveInfo DefensiveInfo { get; }
    public StatemanagerMemory StatemanagerMemory { get; }
    public AnimationManagerMemory AnimationManagerMemory { get; }

    public Dictionary<long, List<long>> Explods { get; }
}

public class PlayerMemory : CharacterMemory
{
    public PlayerMemory(Player player) : base(player)
    {
        power = player.Power;
        score = player.Score;
        PaletteNumber = player.PaletteNumber;
        LastTickPowerSet = player.LastTickPowerSet;
    }

    public void BackMemory(ref Player player)
    {
        player.Power = power;
        player.Score = score;
        player.PaletteNumber = PaletteNumber;
        player.LastTickPowerSet = LastTickPowerSet;

        Character p = player as Character;
        base.BackMemory(ref p);
    }

    public float power { get; }
    public int score { get; }
    public int PaletteNumber { get; }
    public float LastTickPowerSet { get; }
}

public class HelperMemory : CharacterMemory
{
    public HelperMemory(Helper helper) : base(helper)
    {
        FirstTick = helper.FirstTick;
        RemoveHelper = helper.RemoveHelper;
        Data = helper.Data;

#warning adicionar depois que todos os entities forem adicionados
        ChildrensUniqueID = new List<long>();
        foreach (Helper childen in helper.Childrens)
            ChildrensUniqueID.Add(childen.UniqueID);
    }

    public void BackMemory(ref Helper helper)
    {
        helper.FirstTick = FirstTick;
        helper.RemoveHelper = RemoveHelper;
        helper.Data = Data;

        Character c = helper as Character;
        base.BackMemory(ref c);
    }

    public bool FirstTick { get; }
    public bool RemoveHelper { get; }
    public HelperData Data { get; }
    public List<long> ChildrensUniqueID { get; }
}

public class ExplodMemory : EntityMemory
{
    public ExplodMemory(Explod explod) : base(explod)
    {
        Data = explod.Data;
        Ticks = explod.Ticks;
        IsValid = explod.IsValid;
        CreationFacing = explod.CreationFacing;
        ForceRemove = explod.ForceRemove;
        Random = explod.Random;
    }

    public void BackMemory(ref Explod explod)
    {
        explod.Data = Data;
        explod.Ticks = Ticks;
        explod.IsValid = IsValid;
        explod.CreationFacing = CreationFacing;
        explod.ForceRemove = ForceRemove;
        explod.Random = Random;

        Entity e = explod as Entity;
        base.BackMemory(ref e);
    }

    public ExplodData Data { get; }
    public int Ticks { get; }
    public bool IsValid { get; }
    public bool RemoveHelper { get; }
    public Facing CreationFacing { get; }
    public bool ForceRemove { get; }
    public Vector2 Random { get; }
}

public class ProjectileMemory : EntityMemory
{
    public ProjectileMemory(Projectile projectile) : base(projectile)
    {
        Data = projectile.Data;
        TotalHits = projectile.TotalHits;
        HitCountdown = projectile.HitCountdown;
        Priority = projectile.Priority;
        HitPauseCountdown = projectile.HitPauseCountdown;
        State = projectile.State;
        GameTicks = projectile.GameTicks;
    }

    public void BackMemory(ref Projectile projectile)
    {
        projectile.Data = Data;
        projectile.TotalHits = TotalHits;
        projectile.HitCountdown = HitCountdown;
        projectile.Priority = Priority;
        projectile.HitPauseCountdown = HitPauseCountdown;
        projectile.State = State;
        projectile.GameTicks = GameTicks;

        Entity e = projectile as Entity;
        base.BackMemory(ref e);
    }

    public ProjectileData Data { get; }
    public int TotalHits { get; }
    public int HitCountdown { get; }
    public int Priority { get; }
    public int HitPauseCountdown { get; }
    public ProjectileState State { get; }
    public int GameTicks { get; }
}

public struct CameraMemory
{
    public CameraMemory(CameraFE cameraFE)
    {
        lastPosCamFollow = cameraFE.lastPosCamFollow;

        transUp = cameraFE.transUp;
        transDown = cameraFE.transDown;
        transLeft = cameraFE.transLeft;
        transRight = cameraFE.transRight;

        CameraBounds = cameraFE.CameraBounds;
        Location = cameraFE.Location;

        //up = cameraFE.up;
        //down = cameraFE.down;
        //left = cameraFE.left;
        //right = cameraFE.right;
        Corner = cameraFE.Corners;

        isNormalSpeed = cameraFE.isNormalSpeed;
        zoomLimiter = cameraFE.zoomLimiter;
    }

    public void BackMemory(ref CameraFE cameraFE)
    {
        cameraFE.lastPosCamFollow = lastPosCamFollow;

        cameraFE.transUp = transUp;
        cameraFE.transDown = transDown;
        cameraFE.transLeft = transLeft;
        cameraFE.transRight = transRight;

        cameraFE.CameraBounds = CameraBounds;
        cameraFE.Location = Location;

        //cameraFE.up = up;
        //cameraFE.down = down;
        //cameraFE.left = left;
        //cameraFE.right = right;
        cameraFE.Corners = Corner;

        cameraFE.isNormalSpeed = isNormalSpeed;
        cameraFE.zoomLimiter = zoomLimiter;

        cameraFE.camera.transform.position = lastPosCamFollow;
    }

    public Vector3 lastPosCamFollow { get; }
    public Bound CameraBounds { get; }
    public Vector2 Location { get; }
    public Transform transUp { get; }
    public Transform transDown { get; }
    public Transform transLeft { get; }
    public Transform transRight { get; }
    //public float up { get; }
    //public float down { get; }
    //public float left { get; }
    //public float right { get; }
    public Rect Corner { get; }

    public bool isNormalSpeed { get; }
    public float zoomLimiter { get; }
}

public class MemoryState
{
    public MemoryState(FightEngine Engine)
    {
        tickCount = Engine.TickCount;
        roundNumber = Engine.RoundNumber;
        drawGames = Engine.DrawGames;
        matchNumber = Engine.MatchNumber;

        time = Engine.Clock.Time;
        gameSpeed = Engine.Speed;

        pause = new Pause(Engine.Pause);
        superPause = new Pause(Engine.SuperPause);
        engineAssertions = new EngineAssertions(Engine.Assertions);
        environmentColor = new EnvironmentColor(Engine.EnvironmentColor);
        combatChecker = new CombatChecker(Engine.m_combatcheck);
        cameraMemory = new CameraMemory(Engine.CameraFE);

        entityMemories = new Dictionary<long, EntityMemory>(Engine.Entities.EntitiesCount);
        m_removelist = new HashSet<long>(Engine.Entities.m_removelist);

        foreach (var entity in Engine.Entities)
        {
            if (entity is Player player)
                entityMemories[entity.UniqueID] = new PlayerMemory(player);
            else if (entity is Helper helper)
                entityMemories[entity.UniqueID] = new HelperMemory(helper);
            else if (entity is Explod explod)
                entityMemories[entity.UniqueID] = new ExplodMemory(explod);
            else if (entity is Projectile projectile)
                entityMemories[entity.UniqueID] = new ProjectileMemory(projectile);
        }
    }

    public void BackMemory(ref FightEngine engine)
    {
        engine.TickCount = tickCount;
        engine.RoundNumber = roundNumber;
        engine.DrawGames = drawGames;
        engine.MatchNumber = matchNumber;
        engine.Clock.Time = time;
        engine.Speed = gameSpeed;

        engine.Pause.BackMemory(pause);
        engine.SuperPause.BackMemory(superPause);
        engine.Assertions.BackMemory(engineAssertions);
        engine.EnvironmentColor.BackMemory(environmentColor);
        engine.m_combatcheck.BackMemory(combatChecker);
        cameraMemory.BackMemory(ref engine.CameraFE);

        Dictionary<long, HelperMemory> createNewHelper = new Dictionary<long, HelperMemory>();
        Dictionary<long, ExplodMemory> createNewExplod = new Dictionary<long, ExplodMemory>();
        Dictionary<long, ProjectileMemory> createNewProjectile = new Dictionary<long, ProjectileMemory>();

        foreach (Entity entity in engine.Entities)
        {
            if (!(entity is Player))
            {
                engine.Entities.Remove(entity);
            }
        }


        foreach (EntityMemory entityMemory in entityMemories.Values)
        {
            foreach (var entity in engine.Entities)
            {
                if (entity.UniqueID == entityMemory.UniqueID)
                {
                    if (entity is Player player)
                        (entityMemories[entity.UniqueID] as PlayerMemory).BackMemory(ref player);
                    else if (entity is Helper helper)
                        (entityMemories[entity.UniqueID] as HelperMemory).BackMemory(ref helper);
                    else if (entity is Explod explod)
                        (entityMemories[entity.UniqueID] as ExplodMemory).BackMemory(ref explod);
                    else if (entity is Projectile projectile)
                        (entityMemories[entity.UniqueID] as ProjectileMemory).BackMemory(ref projectile);

                    goto End;

                }
            }

            if (entityMemory is HelperMemory helperMemory)
                createNewHelper.Add(helperMemory.UniqueID, helperMemory);
            else if (entityMemory is ExplodMemory explodMemory)
                createNewExplod.Add(explodMemory.UniqueID, explodMemory);
            else if (entityMemory is ProjectileMemory projectileMemory)
                createNewProjectile.Add(projectileMemory.UniqueID, projectileMemory);

            End:;
        }

        engine.Entities.m_removelist = m_removelist;

        foreach (long id in createNewHelper.Keys)
        {
            createNewHelper[id].Creator.InstanceHelper(createNewHelper[id].Data, id);
        }
        foreach (long id in createNewExplod.Keys)
        {
            createNewExplod[id].Creator.InstanceExplod(createNewExplod[id].Data, id);
        }
        foreach (long id in createNewProjectile.Keys)
        {
            createNewProjectile[id].Creator.InstanceProjectile(createNewProjectile[id].Data, id);
        }

        foreach (Entity ent in engine.Entities)
        {
            if (ent is Helper helper)
            {
                PopuleHelper(helper, entityMemories[helper.UniqueID] as HelperMemory);
            }
            if (ent is Character character)
            {
                PopuleCharacter(character, entityMemories[character.UniqueID] as CharacterMemory);
            }
        }
    }

    private void PopuleHelper(Helper helper, HelperMemory memory)
    {
        helper.Childrens.Clear();
        foreach (long unityID in memory.ChildrensUniqueID)
        {
            helper.Engine.Entities.TryGetValue(unityID, out Entity entity);
            helper.Childrens.Add(entity as Helper);
        }
    }

    private void PopuleCharacter(Character character, CharacterMemory memory)
    {
#warning problema lá, por causa daqui, mas aqui é necessario
        character.Explods.Clear(); 
        foreach (long ex in memory.Explods.Keys)
        {
            List<Explod> explods = new List<Explod>();
            foreach (var unityID in memory.Explods[ex])
            {
                character.Engine.Entities.TryGetValue(unityID, out Entity entity);
                explods.Add(entity as Explod);
            }
            if (character.Explods.ContainsKey(ex))
                character.Explods[ex] = explods;
            else
               character.Explods.Add(ex, explods);
        }
    }


    int tickCount;
    int roundNumber;
    int drawGames;
    int matchNumber;
    int time;
    GameSpeed gameSpeed;
    Pause pause;
    Pause superPause;
    EngineAssertions engineAssertions;
    EnvironmentColor environmentColor;
    CombatChecker combatChecker;
    CameraMemory cameraMemory;


    Dictionary<long, EntityMemory> entityMemories;
    List<Entity> m_addlist;
    HashSet<long> m_removelist;
}
