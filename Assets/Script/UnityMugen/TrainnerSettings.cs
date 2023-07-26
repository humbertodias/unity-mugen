using UnityEngine;
using UnityMugen;

namespace UnityMugen
{
    [CreateAssetMenu(fileName = "TrainnerSettings", menuName = "Unity Mugen/Settings/Trainner Settings")]
    public class TrainnerSettings : ScriptableObject
    {
        [Header("Commons")]
        public Sprite boxCollider;
        public Color normal = new Color(0, 0.05882353f, 1, 0.3843137f);
        public Color attack = new Color(1, 0, 0, 0.3843137f);
        public Color hitable = new Color(1, 0.9921569f, 0, 0.3843137f);

        public Sprite pointEntity;
        public Color pointEntityColor = new Color(0.7372549f, 1, 0, 0.3843137f);

        [Header("Display Information")]
        public bool showCommands = false;
        public bool showInputHistory = false;
        public TypeDrawCollider typeDrawCollider = TypeDrawCollider.Frame;
        public bool showInfoP1;
        public bool showInfoP2;
        public bool showInfoGeral;
        public bool showFPS = true;
        public bool showInfoREC;

        [Header("Gauge Settings")]
        public HPRecovery hpRecovery = HPRecovery.Immediate;
        public int percentP1HPMax = 10;
        public int percentP2HPMax = 10;
        public PowerRecovery powerRecovery = PowerRecovery.Normal;

        [Header("Action Enemy")]
        public StanceType stanceType = StanceType.Controller;
        public GuardType guard = GuardType.NoGuard;
        public GuardTimeType guardTime = GuardTimeType.Default;
        public TechingType teching = TechingType.Off;
        [Range(1, 8)]
        public int COMLevel = 4;

    }


    public enum GuardTimeType
    {
        Default,
        FirtHit,
        Ever,
        Random
    }

    public enum TechingType
    {
        Off,
        On,
        Random
    }

    public enum HPRecovery
    {
        Normal,
        Immediate,
        Gradual
    }

    public enum PowerRecovery
    {
        Normal,
        Immediate,
        Infinity
    }

    public static class TrainnerSettingsExtensions
    {
        public static string ToName(this GuardType me)
        {
            switch (me)
            {
                case GuardType.NoGuard:
                    return "No Guard";
                case GuardType.AllGuard:
                    return "All Guard";
                case GuardType.RandomGuard:
                    return "Random Guard";
                default:
                    return "";
            }
        }
        public static string ToName(this GuardTimeType me)
        {
            switch (me)
            {
                case GuardTimeType.Default:
                    return "Default";
                case GuardTimeType.FirtHit:
                    return "Firt Hit";
                case GuardTimeType.Ever:
                    return "Ever";
                case GuardTimeType.Random:
                    return "Random";
                default:
                    return "";
            }
        }
    }
}