using UnityEngine;
namespace UnityMugen
{
    [CreateAssetMenu(fileName = "UnityMugenConfig", menuName = "Unity Mugen/Settings/Unity Mugen Config")]
    public class UnityMugenConfig : ScriptableObject
    {
        public int MaxAfterImage = 128;
        public int MaxBgmVolume = 100;
        public int MaxDrawGames = -2;
        public int MaxExplod = 512;
        public int MaxHelper = 56;
        public int MaxPlayerProjectile = 256;
    }
}