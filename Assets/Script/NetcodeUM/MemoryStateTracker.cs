using UnityMugen;
using UnityMugen.Combat;
using System;
using System.Collections.Generic;
using UnityEngine;
/*
public class MemoryStateTracker {

    private static LauncherEngine Launcher => LauncherEngine._inst;
    public static FightEngine Engine => Launcher.mugen.Engine;

    //public static FluxGameHistory LoadGameState(FluxGameHistory history, long frame) {
    //    FluxStates gameState;

    //    if (history.TryGetState(frame, out gameState)) {
    //        LoadGameState(gameState);
    //    } else {
    //        throw new ArgumentOutOfRangeException(
    //            "frame", frame , 
    //            string.Format("O valor do quadro deve estar entre {0} e {1}.", history.FirstStoredFrame, history.LastStoredFrame)
    //        );
    //    }

    //    return history;
    //}

    private static bool LoadSaveStatus(out Player p1, out Player p2)
    {
        p1 = GetPlayer(PlayerID.One);
        p2 = GetPlayer(PlayerID.Two);
        return Engine != null && Engine.CameraFE != null && p1 != null && p2 != null;
    }


    public static void LoadGameState(MemoryState gameState) {
        LoadGameState(gameState, Launcher.networkSettings.ufeTrackers);
    }

    public static void LoadGameState(MemoryState gameState, bool loadTrackers) {
        // Static Variables
        //Engine.TickCount = gameState.networkFrame;

        Player p1, p2;
        if (LoadSaveStatus(out p1, out p2))
        {
            //Engine.Clock.Time = gameState.global.time;
            //Engine.RoundNumber = gameState.global.currentRound;

          //  LoadCamera(gameState.camera);

            // Characters
            LoadCharacterState(gameState.player1, p1);
            LoadCharacterState(gameState.player2, p2);
        }
        else
        {
            gameState.player1.life = 0;
            gameState.player1.power = 0;
            gameState.player1.currentLocation = Vector2.one;

            gameState.player2.life = 0;
            gameState.player2.power = 0;
            gameState.player2.currentLocation = Vector2.one;

          //  gameState.global.time = 0;
        }
    }
    
    //static void LoadCamera(MemoryState.CameraState camera)
    //{
    //    CameraFE cameraFE = Engine.CameraFE;
    //    cameraFE.CameraBounds = camera.CameraBounds;
    //    cameraFE.Location = camera.Location;
    //    cameraFE.transUp = camera.transUp;
    //    cameraFE.transDown = camera.transDown;
    //    cameraFE.transLeft = camera.transLeft;
    //    cameraFE.transRight = camera.transRight;
    //    cameraFE.up = camera.up;
    //    cameraFE.down = camera.down;
    //    cameraFE.left = camera.left;
    //    cameraFE.right = camera.right;
    //}

    protected static void LoadCharacterState(MemoryState.CharacterState state, Player player) {
        if(player != null)
        {
            player.InHitPause = state.inHitPause;
            player.StateManager.StateTime = state.stateTime;
            player.StateManager.CurrentState = state.currentState;
            player.StateManager.PreviousState = state.previousState;

            player.AnimationManager.TimeInAnimation = state.timeInAnimation;
            player.AnimationManager.Elementswitchtime = state.elementswitchtime;
            player.AnimationManager.IsForeignAnimation = state.isForeignAnimation;
            player.AnimationManager.CurrentAnimation = state.currentAnimation;
            player.AnimationManager.CurrentElement = state.currentElement;
            player.AnimationManager.IsAnimationFinished = state.isAnimationFinished;
            player.AnimationManager.Animationinloop = state.animationinloop;

            player.CurrentLocation = state.currentLocation;
            player.Life = state.life;
            player.Power = state.power;

            player.CurrentVelocity = state.currentVelocity;
            player.CurrentFacing = state.currentFacing;
            player.CurrentAcceleration = state.currentAcceleration;
            player.CurrentScale = state.currentScale;

            player.PushFlag = state.pushFlag;

            player.PaletteNumber = state.paletteNumber;
            player.DrawScale = state.drawScale;
            player.DrawOffset = state.drawOffset;
            player.RoundsExisted = state.roundsExisted;
            player.CameraFollowX = state.cameraFollowX;
            player.CameraFollowY = state.cameraFollowY;
            player.ScreenBound = state.screenBound;
            player.JugglePoints = state.jugglePoints;
            player.StateType = state.stateType;
            player.MoveType = state.moveType;
            player.PlayerControl = state.playerControl;
            player.Physics = state.physics;
            player.PositionFreeze = state.positionFreeze;
            player.DrawingAngle = state.drawingAngle;
            player.AngleDraw = state.angleDraw;
            player.DrawOrder = state.drawOrder;
        }
    }

    public static MemoryState SaveGameState(int frame)
    {
        MemoryState gameState = new MemoryState();
        gameState.networkFrame = frame;

        if (LoadSaveStatus(out Player p1, out Player p2))
        {
            //gameState.global.time = Engine.Clock.Time;
            //gameState.global.currentRound = Engine.RoundNumber;

        //    gameState.camera = SaveCameraState();

            // Characters
            gameState.player1 = SaveCharacterState(p1);
            gameState.player2 = SaveCharacterState(p2);
        }
        else
        {
            gameState.player1 = new MemoryState.CharacterState();
            gameState.player1.life = 0;
            gameState.player1.power = 0;
            gameState.player1.currentLocation = Vector2.one;

            gameState.player2 = new MemoryState.CharacterState();
            gameState.player2.life = 0;
            gameState.player2.power = 0;
            gameState.player2.currentLocation = Vector2.one;

           // gameState.global = new MemoryState.GlobalState();
           // gameState.global.time = 0;
        }
        return gameState;
    }

    //protected static MemoryState.CameraState SaveCameraState()
    //{
    //    MemoryState.CameraState state = new MemoryState.CameraState();
    //    CameraFE cameraFE = Engine.CameraFE;
    //    if (cameraFE != null)
    //    {
    //        state.CameraBounds = cameraFE.CameraBounds;
    //        state.Location = cameraFE.Location;
    //        state.transUp = cameraFE.transUp;
    //        state.transDown = cameraFE.transDown;
    //        state.transLeft = cameraFE.transLeft;
    //        state.transRight = cameraFE.transRight;
    //        state.up = cameraFE.up;
    //        state.down = cameraFE.down;
    //        state.left = cameraFE.left;
    //        state.right = cameraFE.right;
    //    }
    //    return state;
    //}

    protected static MemoryState.CharacterState SaveCharacterState(Player player) {

        MemoryState.CharacterState state = new MemoryState.CharacterState();

        if (player != null)
        {
            //state.stateNumber = player.StateManager.StateNumber;
            ////state.inHitPause = player.InHitPause;
            //state.stateTime = player.StateManager.StateTime;
            //state.currentState = player.StateManager.CurrentState;
            //state.previousState = player.StateManager.PreviousState;

            //state.timeInAnimation = player.AnimationManager.TimeInAnimation;
            //state.elementswitchtime = player.AnimationManager.Elementswitchtime;
            //state.isForeignAnimation = player.AnimationManager.IsForeignAnimation;
            //state.currentAnimation = player.AnimationManager.CurrentAnimation;
            //state.currentElement = player.AnimationManager.CurrentElement;
            //state.isAnimationFinished = player.AnimationManager.IsAnimationFinished;
            //state.animationinloop = player.AnimationManager.Animationinloop;

            //state.life = player.Life;
            //state.power = player.Power;
            //state.currentVelocity = player.CurrentVelocity;
            //state.currentLocation = player.CurrentLocation;
            //state.currentFacing = player.CurrentFacing;
            //state.currentAcceleration = player.CurrentAcceleration;
            //state.currentScale = player.CurrentScale;

            //state.pushFlag = player.PushFlag;
            //state.paletteNumber = player.PaletteNumber;
            //state.drawScale = player.DrawScale;
            //state.drawOffset = player.DrawOffset;
            //state.roundsExisted = player.RoundsExisted;
            //state.cameraFollowX = player.CameraFollowX;
            //state.cameraFollowY = player.CameraFollowY;
            //state.screenBound = player.ScreenBound;
            //state.jugglePoints = player.JugglePoints;
            //state.stateType = player.StateType;
            //state.moveType = player.MoveType;
            //state.playerControl = player.PlayerControl;
            //state.physics = player.Physics;
            //state.positionFreeze = player.PositionFreeze;
            //state.drawingAngle = player.DrawingAngle;
            //state.angleDraw = player.AngleDraw;
            //state.drawOrder = player.DrawOrder;
        }
        return state;
    }

    private static Player GetPlayer(PlayerID playerID)
    {
        foreach(Player player in GetPlayers())
        {
            if (player.m_PlayerNumber == playerID)
                return player;
        }
        return null;
    }

    private static List<Player> GetPlayers()
    {
        List<Player> players = new List<Player>();
        foreach (var entity in Engine.Entities)
        {
            var player = entity as Player;
            if (player == null || Misc.HelperCheck(player)) continue;
            if (player != null)
                players.Add(player);
        }
        return players;
    }

}
*/