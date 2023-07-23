using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine
{

    public class TriggerMap
    {
        public bool IsValid => r_isvalid;

        private readonly bool r_isvalid;
        private readonly SortedDictionary<int, List<Expression>> r_triggers;

        public TriggerMap(string title, SortedDictionary<int, List<Expression>> triggers)
        {
            if (triggers == null) throw new ArgumentNullException(nameof(triggers));

            r_triggers = triggers;
            r_isvalid = ValidCheck(title);
        }

        private bool ValidCheck(string title)
        {
            if (r_triggers.Count == 0) return false;

            tryAgain:
            var indexnumber = 0;
            foreach (var trigger in r_triggers)
            {
                if (trigger.Key == indexnumber)
                {
                    indexnumber += 1;
                }
                else if (indexnumber == 0 && trigger.Key == 1)
                {
                    indexnumber = 2;
                }
                else
                {
                    if (trigger.Key > indexnumber)
                    {
                        UnityEngine.Debug.LogWarningFormat("Error in state: {0}, Trigger: {1}, Trigger not read correctly", title, trigger.Key);
                        r_triggers.Remove(trigger.Key);
                        goto tryAgain;
                    }
                    return false;
                }
            }

            return true;
        }

        public bool Trigger(Character character)
        {
            if (character == null) throw new ArgumentNullException(nameof(character));

            foreach (var trigger in r_triggers)
            {
                var ok = true;
                foreach (var exp in trigger.Value)
                {
                    if (exp.EvaluateFirst(character).BooleanValue == false)
                    {
                        ok = false;
                        break;
                    }
                }

                if (ok)
                {
                    if (trigger.Key != 0)
                        return true;
                }
                else
                {
                    if (trigger.Key == 0) return false;
                }
            }

            return false;
        }
    }
}