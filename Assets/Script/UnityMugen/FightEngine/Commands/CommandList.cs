using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMugen.Commands
{

    [CreateAssetMenu(fileName = "Command List", menuName = "Unity Mugen/Command List")]
    public class CommandList : ScriptableObject
    {

        public string nameChar;
        public List<CommandFE> commands;

        public CommandList Internal()
        {
            commands.Add(new CommandFE("press forward", "$F", 1, 1));
            commands.Add(new CommandFE("press back", "$B", 1, 1));
            commands.Add(new CommandFE("press down", "$D", 1, 1));
            commands.Add(new CommandFE("press up", "$U", 1, 1));
            return this;
        }

    }

    [Serializable]
    public class CommandFE
    {
        public string name;
        public int time;
        public int buffertime;
        public string command;

        public CommandFE() { }

        public CommandFE(string name, string command, int time = 15, int buffertime = 1)
        {
            this.name = name;
            this.time = time;
            this.buffertime = buffertime;
            this.command = command;
        }
    }
}