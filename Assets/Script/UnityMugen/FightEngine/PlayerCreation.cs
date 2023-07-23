using System;

namespace UnityMugen
{
    [Serializable]
    public class PlayerCreation
    {

        public PlayerProfileManager profile;
        public int paletteIndex;
        public PlayerMode mode;

        public PlayerCreation(PlayerProfileManager profile, int paletteIndex, PlayerMode mode)
        {
            if (profile == null) throw new ArgumentNullException(nameof(profile));

            this.profile = profile;
            this.paletteIndex = paletteIndex;
            this.mode = mode;
        }

        public override int GetHashCode()
        {
            return profile.GetHashCode();
        }

    }
}