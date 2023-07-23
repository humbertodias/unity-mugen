using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityMugen.Collections;
using UnityMugen.IO;

namespace UnityMugen.Animations
{
    /// <summary>
    /// Controls the creation of Animations and AnimationManagers. 
    /// </summary>
    public class AnimationSystem
    {
        private static LauncherEngine Launcher => LauncherEngine.Inst;


        public AnimationSystem()
        {
            m_animationcache = new Dictionary<int, KeyedCollection<int, Animation>>();
            m_loader = new AnimationLoader();
        }

        /// <summary>
        /// Creates a new AnimationManager that governs the animations contained in an AIR file.
        /// </summary>
        /// <param name="filepath">Path to a text file containing animations in the AIR format.</param>
        /// <returns>A new AnimationManager for the animations found in the given file.</returns>
        public AnimationManager CreateManager(string nameFile, Vector2 scale)
        {
            if (m_animationcache.TryGetValue(nameFile.GetHashCode(), out KeyedCollection<int, Animation> result))
                return new AnimationManager(result);

            KeyedCollection<int, Animation> colle = LoadGameData(nameFile, scale);
            m_animationcache.Add(nameFile.GetHashCode(), colle);
            return new AnimationManager(colle);
        }

        private KeyedCollection<int, Animation> LoadGameData(string nameFile, Vector2 scale)
        {
            var text = System.IO.File.ReadAllText(nameFile);
            TextFile textFile = FileToTextFile.Build(text);
            return m_loader.LoadAnimations(textFile, scale);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<int, KeyedCollection<int, Animation>> m_animationcache;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AnimationLoader m_loader;
    }
}