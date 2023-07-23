using UnityEngine;

namespace UnityMugen.Combat
{
    [CreateAssetMenu(fileName = "Player Constants", menuName = "Unity Mugen/Player Constants")]
    public class PlayerConstants : ScriptableObject
    {

        [Header("Data")]
        public float MaximumLife = 1000f;
        public float MaximumPower = 3000f;
        public int AttackPower = 100;
        public int DefensivePower = 100;

        public int FallDefenseUp = 50;
        public float FallDefenceMul => (FallDefenseUp + 100) / 100; // 100 / (FallDefenseUp + 100);

        public int LieDownTime = 60;
        public int AirJuggle = 15;

        public int DefaultSparkNumber = 2;
        public bool DefaultSparkNumberIsCommon = true; // new

        public int DefaultGuardSparkNumber = 40;
        public bool DefaultGuardSparkNumberIsCommon = true; // new

        public bool KOEcho = false;
        public int VolumeOffset = 0;
        public int PersistanceIntIndex = 60;
        public int PersistanceFloatIndex = 40;

        [Header("Size | Dimension")]
        public Vector2 Scale = Vector2.one; // Draw scaling factor
        public float GroundBack = 15; // Player width (back, ground)
        public float GroundFront = 14; // Player width (front, ground)
        public float Airback = 12; // Player width (back, air)
        public float Airfront = 12; //Player width (front, air)
        public float Height = 60; // Height of player (for opponent to jump over)


        [Header("Size")]
        public float Attackdistance = 160; // Default attack distance
        public float Projectileattackdist = 90; //Default attack distance for projectiles
        public bool ProjectileScaling = false; //Set to scale projectiles too
        public Vector2 Headposition = new Vector2(-5, -90); // Approximate position of head
        public Vector2 Midposition = new Vector2(-5, -60); // Approximate position of midsection
        public float Shadowoffset = 0; // Number of pixels to vertically offset the shadow
        public Vector2 Drawoffset = Vector2.zero; // Player drawing offset in pixels


        [Header("Velocity")]
        public float Walk_forward = 2.4f; // Walk forward
        public float Walk_back = -2.2f; // Walk backward
        public Vector2 Run_fwd = new Vector2(4.6f, 0f); // Run forward (x, y)
        public Vector2 Run_back = new Vector2(-4.6f, -3.8f); // Hop backward (x, y)
        public Vector2 Jump_neutral = new Vector2(0f, -8.4f); // Neutral jumping velocity (x, y)
        public Vector2 Jump_back = new Vector2(-2.55f, 0f); // Jump back Speed (x, y)
        public Vector2 Jump_forward = new Vector2(2.5f, 0f); // Jump forward Speed (x, y)
        public Vector2 Runjump_back = new Vector2(-2.55f, -8.1f); // Running jump speeds (opt)
        public Vector2 Runjump_fwd = new Vector2(4f, -8.1f);
        public Vector2 Airjump_neutral = new Vector2(0f, -8.1f);
        public Vector2 Airjump_back = new Vector2(-2.55f, 0f); // Air jump speeds (opt)
        public Vector2 Airjump_forward = new Vector2(2.5f, 0f);
        // NOVOS TESTE
        public Vector2 AirGethitGroundrecover = new Vector2(-.15f, -3.5f);
        public Vector2 AirGethitAirrecoverMul = new Vector2(.5f, .2f);
        public Vector2 AirGethitAirrecoverAdd = new Vector2(0f, -4.5f);
        public float AirGethitAirrecoverBack = -1f;
        public float AirGethitAirrecoverFwd = 0f;
        public float AirGethitAirrecoverUp = -2f;
        public float AirGethitAirrecoverDown = 1.5f;


        [Header("Movement")]
        public int Airjumps = 1; // Number of air jumps allowed (opt)
        public float Airjumpheight = 35; // Minimum distance from ground before you can air jump (opt)
        public float Vert_acceleration = .44f; // Vertical acceleration
        public float Standfriction = .85f; // Friction coefficient when standing
        public float Crouchfriction = .82f; // Friction coefficient when crouching
                                            // NOVOS TESTE
        public float StandFrictionThreshold = 2f;
        public float CrouchFrictionThreshold = .05f;
        public float JumpChangeanimThreshold = 0f;
        public float AirGethitGroundlevel = 25f;
        public float AirGethitGroundrecoverGroundThreshold = -20f;
        public float AirGethitGroundrecoverGroundlevel = 10f;
        public float AirGethitAirrecoverThreshold = -1f;
        public float AirGethitAirrecoverYaccel = .35f;
        public float AirGethitTripGroundlevel = 15f;
        public Vector2 DownBounceOffset = new Vector2(0, 20);
        public float DownBounceYaccel = .4f;
        public float DownBounceGroundlevel = 12f;
        public float DownFrictionThreshold = .05f;

        public PlayerConstants Iniciar()
        {
            if (Jump_back.y == 0) Jump_back.y = Jump_neutral.y;
            if (Jump_forward.y == 0) Jump_forward.y = Jump_neutral.y;
            if (Airjump_back.y == 0) Airjump_back.y = Airjump_neutral.y;
            if (Airjump_forward.y == 0) Airjump_forward.y = Airjump_neutral.y;
            return this;
        }
    }
}