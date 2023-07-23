using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityMugen.Collections;

namespace UnityMugen.Commands
{
    public interface ICommandManager
    {
        bool IsActive(string commandname);
        void UpdateFE(PlayerButton input, Facing facing, bool paused);
        ICommandManager Clone();
        bool IsHuman();
        ReadOnlyList<Command> Commands { get; }
        Dictionary<string, BufferCount> CommandCount { get; set; }
    }

    public class CommandManager : ICommandManager
    {
        public CommandManager(CommandSystem commandsystem, PlayerMode mode, ReadOnlyList<Command> commands, CommandList commandList)
        {
            if (commandsystem == null) throw new ArgumentNullException(nameof(commandsystem));
            if (commands == null) throw new ArgumentNullException(nameof(commands));

            m_commandsystem = commandsystem;
            m_commands = commands;
            m_commandList = commandList;
            m_commandcount = new Dictionary<string, BufferCount>(StringComparer.Ordinal);
            m_inputbuffer = new InputBuffer();
            m_activecommands = new List<string>();
            m_mode = mode;

            foreach (var command in Commands)
            {
                if (m_commandcount.ContainsKey(command.Name)) continue;

                m_commandcount.Add(command.Name, new BufferCount());
            }
        }

        ICommandManager ICommandManager.Clone()
        {
            return m_commandsystem.CreateManager(PlayerMode.Human, m_commandList);
        }

        public void ResetFE()
        {
            m_activecommands.Clear();
            m_inputbuffer.ResetFE();

            foreach (var data in m_commandcount) data.Value.ResetFE();
        }

        public void UpdateFE(PlayerButton input, Facing facing, bool paused)
        {
            m_inputbuffer.Add(input, facing);

            if (paused == false)
            {
                foreach (var count in m_commandcount.Values) count.Tick();
            }

            foreach (var command in Commands)
            {
                if (command.IsValid == false) continue;

                if (CommandChecker.Check(command, m_inputbuffer))
                {
                    var time = command.BufferTime;
                    if (paused) ++time;

                    m_commandcount[command.Name].Set(time);
                }
            }

            m_activecommands.Clear();
            foreach (var data in m_commandcount) if (data.Value.IsActive) m_activecommands.Add(data.Key);
        }

        public bool IsActive(string commandname)
        {
            if (commandname == null) throw new ArgumentNullException(nameof(commandname));

            return m_activecommands.Contains(commandname);
        }

        public bool IsHuman()
        {
            return m_mode == PlayerMode.Human;
        }

        public ReadOnlyList<Command> Commands => m_commands;

        public Dictionary<string, BufferCount> CommandCount { 
            get => m_commandcount; 
            set => m_commandcount = value; 
        }

        public List<string> m_activecommands;
        public InputBuffer m_inputbuffer;
        


        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly CommandSystem m_commandsystem;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ReadOnlyList<Command> m_commands;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly CommandList m_commandList;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly PlayerMode m_mode;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<string, BufferCount> m_commandcount;

        #endregion
    }
}