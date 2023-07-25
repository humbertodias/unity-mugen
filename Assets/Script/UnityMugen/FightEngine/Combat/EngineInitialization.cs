using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMugen.Combat
{

    public class EngineInitialization
    {

        public void SetSeed()
        {
            Seed = (!LauncherEngine.Inst.initializationSettings.SaveSeedDebug ? Environment.TickCount : Seed);
        }

        public CombatMode Mode;
        public int stageID;
        public int musicID;
        public int Seed;

        [Header("Player1")]
        public TeamMode Team1Mode;
        public List<PlayerCreation> Team1 = new List<PlayerCreation>();

        [Header("Player2")]
        public TeamMode Team2Mode;
        public List<PlayerCreation> Team2 = new List<PlayerCreation>();


    }
}