using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityMugen.Collections;

namespace UnityMugen.Commands
{
    public class AiCommandManager : ICommandManager
    {
        private CommandSystem m_commandsystem;
        private TextAsset m_textAsset;
        private ReadOnlyList<Command> m_commands;
        private List<string> m_activecommands;
        private System.Random m_random;
        private PlayerMode m_mode;

        public AiCommandManager(CommandSystem commandsystem, PlayerMode mode, ReadOnlyList<Command> commands)
        {
            if (commandsystem == null) throw new ArgumentNullException(nameof(commandsystem));
            if (commands == null) throw new ArgumentNullException(nameof(commands));

            m_commandsystem = commandsystem;
            m_commands = commands;
            m_activecommands = new List<string>();
            m_random = new System.Random();
            m_mode = mode;
        }

        public ReadOnlyList<Command> Commands => m_commands;

        public Dictionary<string, BufferCount> CommandCount
        {
            get => default(Dictionary<string, BufferCount>);
            set => throw new NotImplementedException();
        }

        public ICommandManager Clone()
        {
            return new AiCommandManager(m_commandsystem, m_mode, m_commands);
        }

        public bool IsActive(string commandname)
        {
            return m_activecommands.Contains(commandname);
        }

        public bool IsHuman()
        {
            return m_mode == PlayerMode.Human;
        }

        public void ResetFE()
        {
            m_activecommands.Clear();
        }

        public void UpdateFE(PlayerButton input, Facing facing, bool paused)
        {
            ResetFE();
            var rnd = m_random.Next(0, 100);
            if (rnd > 85)
            {
                var validCommands = m_commands.Where(c => c.IsValid).ToList();
                var cmdIndex = m_random.Next(0, validCommands.Count);
                m_activecommands.Add(validCommands[cmdIndex].Name);
            }
        }
    }
}