using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityMugen.Combat;

namespace UnityMugen
{

    public class ProfileLoader : MonoBehaviour
    {
        static LauncherEngine Launcher => LauncherEngine.Inst;
        FightEngine Engine => Launcher.mugen.Engine;


        public List<StageProfile> stageProfiles;
        public List<PlayerProfileManager> profiles;
        public List<MusicProfile> musicProfiles;

        public int totalBattlesArcade = 5;
        public int totalBattlesSurvival = 5;

        [NonSerialized] public Dictionary<int, int> orderBattleHistory;
        [NonSerialized] public Dictionary<int, int> orderBattleArcade;
        [NonSerialized] public Dictionary<int, int> orderBattleSurvival;

        //public Dictionary<string, List<StateList>> statesPreLoaded = new Dictionary<string, List<StateList>>();
        Dictionary<string, System.Threading.Tasks.Task> tasks = new Dictionary<string, System.Threading.Tasks.Task>();

        public void SetIDs()
        {
            for (int i = 0; i < profiles.Count; i++)
            {
                profiles[i].charID = i;
            }
            for (int i = 0; i < stageProfiles.Count; i++)
            {
                stageProfiles[i].stageID = i;
            }
        }

        public (PlayerProfileManager, int) GetNextBattleArcade()
        {
            int character = orderBattleArcade.Keys.ElementAt(Engine.MatchNumber);
            int stage = orderBattleArcade.Values.ElementAt(Engine.MatchNumber);
            return (profiles[character], stageProfiles[stage].stageID);
        }

        public Tuple<PlayerProfileManager, int> GetNextBattleSuvival()
        {
            int character = orderBattleSurvival.Keys.ElementAt(Engine.MatchNumber);
            int stage = orderBattleSurvival.Values.ElementAt(Engine.MatchNumber);
            return new Tuple<PlayerProfileManager, int>(profiles[character], stageProfiles[stage].stageID);
        }

        public void GenerateOrderBattleArcade()
        {
            Engine.MatchNumber = 0;

            orderBattleArcade = new Dictionary<int, int>();
            int totalStages = stageProfiles.Count;
            int totalCharaters = profiles.Count;

            for (int i = 0; i < totalBattlesArcade; i++)
            {
                int stage = UnityEngine.Random.Range(0, totalStages);

            reloadRondomCharacter:
                int charac = UnityEngine.Random.Range(0, totalCharaters);
                if (!orderBattleArcade.ContainsKey(charac))
                {
                    orderBattleArcade.Add(charac, stage);
                }
                else
                    goto reloadRondomCharacter;
            }
        }

        public void GenerateOrderBattleSurvival()
        {
            Engine.MatchNumber = 0;

            orderBattleSurvival = new Dictionary<int, int>();
            int totalStages = stageProfiles.Count;
            int totalCharaters = profiles.Count;

            for (int i = 0; i < totalBattlesSurvival; i++)
            {
                int stage = UnityEngine.Random.Range(0, totalStages);

            reloadRondomCharacter:
                int charac = UnityEngine.Random.Range(0, totalCharaters);
                if (!orderBattleSurvival.ContainsKey(charac))
                {
                    orderBattleSurvival.Add(charac, stage);
                }
                else
                    goto reloadRondomCharacter;
            }
        }

        public void PreLoadPalettes()
        {
            foreach (PlayerProfileManager profile in profiles)
            {
                Launcher.paletteSystem.LoadPalette(profile.NamefileSFF());
            }
        }

        public Dictionary<int, string> NamesCharacters()
        {
            Dictionary<int, string> keyValuePairs = new Dictionary<int, string>();
            for (int i = 0; i < profiles.Count; i++)
            {
                keyValuePairs.Add(i, profiles[i].displayName);
            }
            return keyValuePairs;
        }
        public Dictionary<int, string> NamesStages()
        {
            Dictionary<int, string> keyValuePairs = new Dictionary<int, string>();
            for (int i = 0; i < stageProfiles.Count; i++)
            {
                keyValuePairs.Add(i, stageProfiles[i].DisplayName);
            }
            return keyValuePairs;
        }
        public Dictionary<int, string> NamesBMG()
        {
            Dictionary<int, string> keyValuePairs = new Dictionary<int, string>();
            for (int i = 0; i < musicProfiles.Count; i++)
            {
                keyValuePairs.Add(i, musicProfiles[i].nameMusic);
            }
            return keyValuePairs;
        }
        public void PreLoadStates()
        {
            //tasks.Add("Cammy", System.Threading.Tasks.Task.Factory.StartNew(StateCammy));
            //tasks.Add("Feilong", System.Threading.Tasks.Task.Factory.StartNew(StateFeilong));
            //tasks.Add("Ken", System.Threading.Tasks.Task.Factory.StartNew(StateKen));
            //tasks.Add("Kfm", System.Threading.Tasks.Task.Factory.StartNew(StateKfm));
            //tasks.Add("Ryu", System.Threading.Tasks.Task.Factory.StartNew(StateRyu));
            //tasks.Add("Freeza Z2", System.Threading.Tasks.Task.Factory.StartNew(StateFreezaZ2));
            //tasks.Add("Gohan Z2", System.Threading.Tasks.Task.Factory.StartNew(StateGohanZ2));
            //tasks.Add("Shin Satan Z2", System.Threading.Tasks.Task.Factory.StartNew(StateSatanZ2));
            //foreach (System.Threading.Tasks.Task task in tasks.Values)
            //{
            //    StartCoroutine(WaitAndPrint1(task));
            //}
        }

        public void PreLoadStatesCNS()
        {
            //   foreach (string files in Launcher.profileLoader.profiles[5].states)
            {

                // var thread = new Thread(
                // (ThreadStart)delegate
                // {
                //     Launcher.stateSystem.PreLoadStates(Launcher.profileLoader.profiles[5].states);
                // });
                // thread.Start();
                // var thread2 = new Thread(
                //(ThreadStart)delegate
                //{
                //    Launcher.stateSystem.PreLoadStates(Launcher.profileLoader.profiles[6].states);
                //});
                // thread2.Start();

            }
        }

        private IEnumerator WaitAndPrint1(System.Threading.Tasks.Task task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }
            UnityEngine.Debug.Log("States Loaded");
        }
        public void ClearThreads()
        {
            foreach (System.Threading.Tasks.Task task in tasks.Values)
            {
                task.Dispose();
            }
            tasks.Clear();
        }


    }
}