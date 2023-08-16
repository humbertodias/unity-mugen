using System;
using System.Collections.Generic;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine
{

    public abstract class StateController
    {
        public string label;
        public bool ignorehitpause = false;

        public Expression persistence;
        public TriggerMap triggerMap;

        protected StateController() { }

        protected StateController(string label)
        {
            if (label == null) throw new ArgumentNullException(nameof(label));
            this.label = label.Replace('"', '\'');
        }

        public virtual void Complete() { }

        public virtual void SetAttributes(string idAttribute, string expression)
        {
            switch (idAttribute)
            {
                case "persistent":
                    persistence = GetAttribute<Expression>(expression, null);
                    break;
                case "ignorehitpause":
                    ignorehitpause = GetAttribute(expression, false);
                    break;
            }
        }


        public T GetAttribute<T>(string expression, T failover)
        {
            T returnvalue;
            if (expression == null) return failover;

            if (LauncherEngine.Inst != null && LauncherEngine.Inst.stringConverter.TryConvert(expression, out returnvalue))
            {
                return returnvalue;
            }
            else if (new StringConverter().TryConvert(expression, out returnvalue))
            {
                return returnvalue;
            }
            else
            {
                UnityEngine.Debug.LogWarning("Cannot convert " + expression);
            }
            return failover;
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

        public virtual TriggerMap BuildTriggers(List<KeyValuePair<string, string>> ParsedLines)
        {
            var triggers = new SortedDictionary<int, List<Expression>>();

            var evalsystem = LauncherEngine.Inst.evaluationSystem;

            foreach (var parsedline in ParsedLines)
            {
                int triggernumber;
                if (string.Compare(parsedline.Key, 7, "all", 0, 3, StringComparison.OrdinalIgnoreCase) == 0) triggernumber = 0;
                else if (int.TryParse(parsedline.Key.Substring(7), out triggernumber) == false) continue;

                var trigger = evalsystem.CreateExpression(parsedline.Value);

                if (triggers.ContainsKey(triggernumber) == false) triggers.Add(triggernumber, new List<Expression>());
                triggers[triggernumber].Add(trigger);
            }
            return new TriggerMap(label, triggers);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", GetType().Name, label);
        }
    }
}