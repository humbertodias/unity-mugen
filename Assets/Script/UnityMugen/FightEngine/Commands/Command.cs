using System;
using System.Diagnostics;
using UnityMugen.Collections;

namespace UnityMugen.Commands
{
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    public class Command
    {
        public Command(string name, int time, int buffertime, ReadOnlyList<CommandElement> elements)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (time < 0) throw new ArgumentOutOfRangeException(nameof(time), "Time must be greater than or equal to zero");
            if (buffertime <= 0) throw new ArgumentOutOfRangeException(nameof(buffertime), "Buffertime must be greater than zero");
            if (elements == null) throw new ArgumentNullException(nameof(elements));

            Name = name;
            Time = time;
            BufferTime = buffertime;
            Elements = elements;
            IsValid = ValidCheck();
        }

        private bool ValidCheck()
        {
            foreach (var element in Elements)
            {
                if (element.Buttons == CommandButton.None && element.Direction == CommandDirection.None) return false;
            }

            return true;
        }

        public string Name;
        public int Time;
        public int BufferTime;
        public bool IsValid;
        public ReadOnlyList<CommandElement> Elements;

    }
}