using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityMugen.Collections;
using UnityMugen.Combat;
using UnityMugen.IO;

namespace UnityMugen.StateMachine
{

    public class StateSystem
    {

        private Dictionary<string, ReadOnlyKeyedCollection<int, State>> m_stateFiles;
        private readonly ReadOnlyDictionary<string, Constructor> m_controllerMap;
        private Regex m_controllerTitleRegex;
        private Regex m_staterTitleRegex;
        private ReadOnlyKeyedCollection<int, State> m_internalStates;
        private ReadOnlyKeyedCollection<int, State> m_trainnerStates;
        private KeyedCollection<int, State> m_scoreStates;

        public StateSystem()
        {
            m_stateFiles = new Dictionary<string, ReadOnlyKeyedCollection<int, State>>(StringComparer.OrdinalIgnoreCase);
            m_controllerTitleRegex = new Regex(@"^State\s+(\S.*)$", RegexOptions.IgnoreCase);
            m_staterTitleRegex = new Regex("Statedef\\s*(-?\\d+).*", RegexOptions.IgnoreCase);

            m_controllerMap = BuildControllerMap();
            m_internalStates = GetStates(Application.streamingAssetsPath + "/Data/Internal.cns");
            m_trainnerStates = GetStates(Application.streamingAssetsPath + "/Data/TrainnerState.cns");
        }

        private static ReadOnlyDictionary<string, Constructor> BuildControllerMap()
        {
            var attribType = typeof(StateControllerNameAttribute);
            var constructortypes = new[] { typeof(StateSystem), typeof(string), typeof(TextSection) };

            var controllermap = new Dictionary<string, Constructor>(StringComparer.OrdinalIgnoreCase);

            foreach (var t in Assembly.GetCallingAssembly().GetTypes())
            {
                if (t.IsSubclassOf(typeof(StateController)) == false || Attribute.IsDefined(t, attribType) == false) continue;
                var attrib = (StateControllerNameAttribute)Attribute.GetCustomAttribute(t, attribType);

                foreach (var name in attrib.Names)
                {
                    if (controllermap.ContainsKey(name))
                    {
                        UnityEngine.Debug.LogWarningFormat("Duplicate definition found for state controller - {0}.", name);
                    }
                    else
                    {
                        controllermap.Add(name, ConstructorDelegate.FastConstruct(t, constructortypes));
                    }
                }
            }

            return new ReadOnlyDictionary<string, Constructor>(controllermap);
        }


        public StateManager CreateManager(Character character, string[] filepaths)
        {
            if (character == null) throw new ArgumentNullException(nameof(character));
            if (filepaths == null) throw new ArgumentNullException(nameof(filepaths));

            var states = new KeyedCollection<int, State>(x => x.number);

            foreach (var filepath in filepaths)
            {
                string filepathR = Application.streamingAssetsPath + filepath;

                var loadedstates = GetStates(filepathR);
                foreach (var state in loadedstates)
                {
                    if (states.Contains(state.number)) states.Remove(state.number);
                    states.Add(state);
                }
            }

            foreach (var state in m_internalStates)
            {
                if (states.Contains(state.number) == false) states.Add(state);
            }

            foreach (var state in m_trainnerStates)
            {
                if (states.Contains(state.number) == false) states.Add(state);
            }
            
            return new StateManager(character, new ReadOnlyKeyedCollection<int, State>(states));
        }

        public void PreLoadStates(string[] filepaths)
        {
            if (filepaths == null) throw new ArgumentNullException(nameof(filepaths));

            var states = new KeyedCollection<int, State>(x => x.number);

            foreach (var filepath in filepaths)
            {
                string filepathR = Application.streamingAssetsPath + filepath;

                var loadedstates = GetStates(filepathR);
                foreach (var state in loadedstates)
                {
                    if (states.Contains(state.number)) states.Remove(state.number);
                    states.Add(state);
                }
            }

        }

        private ReadOnlyKeyedCollection<int, State> GetStates(string filepath)
        {
            if (filepath == null) throw new ArgumentNullException(nameof(filepath));

            if (m_stateFiles.ContainsKey(filepath)) return m_stateFiles[filepath];

            var states = new KeyedCollection<int, State>(x => x.number);
            var textfile = LauncherEngine.Inst.fileSystem.OpenTextFile(filepath);

            TextSection laststatesection = null;
            List<StateController> controllers = null;

            foreach (var textsection in textfile)
            {
                if (m_staterTitleRegex.IsMatch(textsection.Title))
                {
                    if (laststatesection != null)
                    {
                        var newstate = CreateState(laststatesection, controllers);
                        if (newstate != null) AddStateToCollection(states, newstate);
                    }

                    laststatesection = textsection;
                    controllers = new List<StateController>();
                }
                else
                {
                    var controller = CreateController(textsection);
                    if (controller != null) controllers?.Add(controller);
                }
            }

            if (laststatesection != null)
            {
                var newstate = CreateState(laststatesection, controllers);
                if (newstate != null) AddStateToCollection(states, newstate);
            }

            var roStates = new ReadOnlyKeyedCollection<int, State>(states);
            m_stateFiles.Add(filepath, roStates);
            return roStates;
        }

        private static void AddStateToCollection(KeyedCollection<int, State> collection, State state)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (state == null) throw new ArgumentNullException(nameof(state));

            if (collection.Contains(state.number))
            {
                UnityEngine.Debug.LogWarningFormat("Duplicate state #{0}. Discarding duplicate", state.number);
            }
            else
            {
                collection.Add(state);
            }
        }

        private State CreateState(TextSection textsection, List<StateController> controllers)
        {
            if (textsection == null) throw new ArgumentNullException(nameof(textsection));
            if (controllers == null) throw new ArgumentNullException(nameof(controllers));

            var match = m_staterTitleRegex.Match(textsection.Title);
            if (match.Success == false) return null;

            var statenumber = int.Parse(match.Groups[1].Value);

            foreach (var controller in controllers)
            {
                if (controller.IsValid()) continue;

                UnityEngine.Debug.LogWarningFormat("Error parsing state #{0}, controller {1} - '{2}'", statenumber, controller.GetType().Name, controller.label);
            }

            controllers.RemoveAll(x => x.IsValid() == false);

            if (m_internalStates != null && m_internalStates.Contains(statenumber))
            {
                controllers.AddRange(m_internalStates[statenumber].controllers);
            }

            if (m_trainnerStates != null && m_trainnerStates.Contains(statenumber))
            {
                controllers.AddRange(m_trainnerStates[statenumber].controllers);
            }

            var state = new State(statenumber, textsection, controllers);
            return state;
        }

        private StateController CreateController(TextSection textsection)
        {
            if (textsection == null) throw new ArgumentNullException(nameof(textsection));

            string title = "";
            //var match = m_controllerTitleRegex.Match(textsection.Title);
            if (textsection.Title.ToLower().Contains("state "))
                title = textsection.Title.Substring(6);
            else
                return null;

            //if (match.Success == false) 
            //    return null;

            var typename = textsection.GetAttribute<string>("type", null);

            if (typename == null)
            {
                UnityEngine.Debug.LogWarningFormat("Controller '{0}' does not have a type.", textsection);
                return null;
            }


            if (m_controllerMap.ContainsKey(typename) == false)
            {
                UnityEngine.Debug.LogWarningFormat("Controller '{0}' has invalid type - '{1}'.", textsection, typename);
                return null;
            }

            var controller = (StateController)m_controllerMap[typename](this, title/*match.Groups[1].Value*/, textsection, false);
            return controller;
        }
    }
}