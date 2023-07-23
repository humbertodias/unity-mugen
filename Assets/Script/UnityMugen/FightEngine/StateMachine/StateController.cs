using System;
using System.Collections.Generic;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine
{

    public abstract class StateController
    {
        public bool isLoaded;

        public string label;
        public bool ignorehitpause = false;

        public StateSystem stateSystem;
        public TextSection textSection;
        public Expression persistence;
        public TriggerMap triggerMap;

        protected StateController() { }

        protected StateController(StateSystem stateSystem, string label, TextSection textSection)
        {
            if (stateSystem == null) throw new ArgumentNullException(nameof(stateSystem));
            if (label == null) throw new ArgumentNullException(nameof(label));
            if (textSection == null) throw new ArgumentNullException(nameof(textSection));

            this.stateSystem = stateSystem;
            this.textSection = textSection;

            triggerMap = BuildTriggers(textSection);

            persistence = textSection.GetAttribute<Expression>("persistent", null);
            ignorehitpause = textSection.GetAttribute("ignorehitpause", false);

            this.label = label.Replace('"', '\'');
        }

        public virtual void Load()
        {
            if (isLoaded == false)
                isLoaded = true;
        }

        public virtual bool IsValid()
        {
            return triggerMap.IsValid;
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode() ^ label.GetHashCode();
        }

        public abstract void Run(Character character);

        private TriggerMap BuildTriggers(TextSection textsection)
        {
            if (textsection == null) throw new ArgumentNullException(nameof(textsection));

            var triggers = new SortedDictionary<int, List<Expression>>();

            var evalsystem = LauncherEngine.Inst.evaluationSystem;

            foreach (var parsedline in textsection.ParsedLines)
            {
                if (string.Compare(parsedline.Key, 0, "trigger", 0, 7, StringComparison.OrdinalIgnoreCase) != 0) continue;

                int triggernumber;
                if (string.Compare(parsedline.Key, 7, "all", 0, 3, StringComparison.OrdinalIgnoreCase) == 0) triggernumber = 0;
                else if (int.TryParse(parsedline.Key.Substring(7), out triggernumber) == false) continue;

                var trigger = evalsystem.CreateExpression(parsedline.Value);

                if (triggers.ContainsKey(triggernumber) == false) triggers.Add(triggernumber, new List<Expression>());
                triggers[triggernumber].Add(trigger);
            }
            return new TriggerMap(textsection.Title, triggers);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", GetType().Name, label);
        }
    }
}