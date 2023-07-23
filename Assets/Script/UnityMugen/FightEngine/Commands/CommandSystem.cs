using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityMugen.Collections;

namespace UnityMugen.Commands
{
    public class CommandSystem
    {
        private static LauncherEngine Launcher => LauncherEngine.Inst;

        public CommandSystem()
        {
            m_commandcache = new Dictionary<string, ReadOnlyList<Command>>(StringComparer.OrdinalIgnoreCase);
            m_numberregex = new Regex("(\\d+)", RegexOptions.IgnoreCase);
        }


        public ICommandManager CreateManager(PlayerMode mode, CommandList commandList)
        {
            if (commandList == null) throw new ArgumentNullException(nameof(commandList));

            ReadOnlyList<Command> commands = GetCommands(commandList);

            return mode == PlayerMode.Human ?
                                     (ICommandManager)new CommandManager(this, mode, commands, commandList) :
                                     new AiCommandManager(this, mode, commands);
        }

        private ReadOnlyList<Command> GetCommands(CommandList commandList)
        {
            if (commandList == null) throw new ArgumentNullException(nameof(commandList));

            if (m_commandcache.ContainsKey(commandList.nameChar)) return m_commandcache[commandList.nameChar];

            var commands = new List<Command>();

            foreach (var command in commandList.commands)
            {
                commands.Add(BuildCommand(command.name, command.command, command.time, command.buffertime));
            }

            var list = new ReadOnlyList<Command>(commands);
            m_commandcache.Add(commandList.nameChar, list);

            return list;
        }

        private Command BuildCommand(string name, string text, int time, int buffertime)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (time < 0) throw new ArgumentOutOfRangeException(nameof(time));
            if (buffertime < 0) throw new ArgumentOutOfRangeException(nameof(buffertime));

            var command = new Command(name, time, buffertime, ParseCommandText(text));
            if (command.IsValid == false)
            {
                UnityEngine.Debug.LogWarning("Invalid command - " + name);
            }

            return command;
        }

        private ReadOnlyList<CommandElement> ParseCommandText(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            var elements = new List<CommandElement>();

            CommandElement lastelement = null;
            foreach (var Token in text.Split(','))
            {
                if (string.IsNullOrEmpty(Token)) continue;
                string token = Token;

                var helddown = false;
                var nothingelse = false;
                int? triggertime = null;
                var dir = CommandDirection.None;
                var buttons = CommandButton.None;

                if (token.IndexOf('~') != -1)
                {
                    var time = 0;
                    var match = m_numberregex.Match(token);
                    if (match.Success) int.TryParse(match.Groups[1].Value, out time);

                    triggertime = time;
                }

                if (token.IndexOf('/') != -1) helddown = true;

                if (token.IndexOf('>') != -1) nothingelse = true;

#warning Hack Tiago Command "~F, DF, $D, B" problem because Token [$D]
                if (elements.Count == 0 && token.IndexOf('$') != -1)
                {
                    if (token.IndexOf("$B") != -1) dir = CommandDirection.B4Way;
                    else if (token.IndexOf("$U") != -1) dir = CommandDirection.U4Way;
                    else if (token.IndexOf("$F") != -1) dir = CommandDirection.F4Way;
                    else if (token.IndexOf("$D") != -1) dir = CommandDirection.D4Way;
                }
                else
                {
                    if (token.IndexOf("DB") != -1) dir = CommandDirection.DB;
                    else if (token.IndexOf("DF") != -1) dir = CommandDirection.DF;
                    else if (token.IndexOf("UF") != -1) dir = CommandDirection.UF;
                    else if (token.IndexOf("UB") != -1) dir = CommandDirection.UB;
                    else if (token.IndexOf("D+B") != -1) dir = CommandDirection.DB;
                    else if (token.IndexOf("D+F") != -1) dir = CommandDirection.DF;
                    else if (token.IndexOf("U+B") != -1) dir = CommandDirection.UB;
                    else if (token.IndexOf("U+F") != -1) dir = CommandDirection.UF;
                    else if (token.IndexOf("D") != -1) dir = CommandDirection.D;
                    else if (token.IndexOf("F") != -1) dir = CommandDirection.F;
                    else if (token.IndexOf("U") != -1) dir = CommandDirection.U;
                    else if (token.IndexOf("B") != -1) dir = CommandDirection.B;

                    if (token.IndexOf("a") != -1) buttons |= CommandButton.A;
                    if (token.IndexOf("b") != -1) buttons |= CommandButton.B;
                    if (token.IndexOf("c") != -1) buttons |= CommandButton.C;
                    if (token.IndexOf("x") != -1) buttons |= CommandButton.X;
                    if (token.IndexOf("y") != -1) buttons |= CommandButton.Y;
                    if (token.IndexOf("z") != -1) buttons |= CommandButton.Z;
                    if (token.IndexOf("s") != -1) buttons |= CommandButton.Taunt;
                }
                

                var element = new CommandElement(dir, buttons, triggertime, helddown, nothingelse);

                if (lastelement != null && SuccessiveDirectionCheck(lastelement, element))
                {
                    var newelement1 = new CommandElement(element.Direction, CommandButton.None, 0, false, true);
                    var newelement2 = new CommandElement(element.Direction, CommandButton.None, null, false, true);

                    elements.Add(newelement1);
                    elements.Add(newelement2);
                }
                else
                {
                    elements.Add(element);
                }

                lastelement = element;
            }

            return new ReadOnlyList<CommandElement>(elements);
        }

        private bool SuccessiveDirectionCheck(CommandElement last, CommandElement now)
        {
            if (last != now) return false;
            if (last.Direction == CommandDirection.None || last.Direction == CommandDirection.B4Way || last.Direction == CommandDirection.D4Way || last.Direction == CommandDirection.F4Way || last.Direction == CommandDirection.U4Way) return false;
            if (last.Buttons != CommandButton.None || last.HeldDown || last.NothingElse || last.TriggerOnRelease != null) return false;

            return true;
        }

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<string, ReadOnlyList<Command>> m_commandcache;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Regex m_numberregex;

        #endregion
    }
}