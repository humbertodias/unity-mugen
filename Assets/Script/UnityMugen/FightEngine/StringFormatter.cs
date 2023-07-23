using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityMugen.Evaluation;

namespace UnityMugen
{
    public class StringFormatter
    {
        private readonly object r_lock;
        private readonly StringBuilder r_builder;
        private readonly List<object> r_args;

        public StringFormatter()
        {
            r_lock = new object();
            r_builder = new StringBuilder(100);
            r_args = new List<object>();
        }

        public string BuildString(string format, params object[] args)
        {
            if (format == null) throw new ArgumentNullException(nameof(format));
            if (args == null) throw new ArgumentNullException(nameof(args));

            lock (r_lock)
            {
                r_args.Clear();
                r_args.AddRange(args);

                return Build(format);
            }
        }

        public string BuildString(string format, List<object> args)
        {
            if (format == null) throw new ArgumentNullException(nameof(format));
            if (args == null) throw new ArgumentNullException(nameof(args));

            lock (r_lock)
            {
                r_args.Clear();
                r_args.AddRange(args);

                return Build(format);
            }
        }

        public string BuildString(string format, Number[] args)
        {
            if (format == null) throw new ArgumentNullException(nameof(format));
            if (args == null) throw new ArgumentNullException(nameof(args));

            lock (r_lock)
            {
                r_args.Clear();

                for (var i = 0; i != args.Length; ++i)
                {
                    if (args[i].NumberType == NumberType.Int) r_args.Add(args[i].IntValue);
                    else if (args[i].NumberType == NumberType.Float) r_args.Add(args[i].FloatValue);
                    else r_args.Add(null);
                }

                return Build(format);
            }
        }

        private string Build(string format)
        {
            if (format == null) throw new ArgumentNullException(nameof(format));

            r_builder.Length = 0;
            var currentparam = 0;

            for (var i = 0; i < format.Length; ++i)
            {
                var current = format[i];
                var next = i + 1 < format.Length ? format[i + 1] : (char)0;

                if (current == '%')
                {
                    if (next == '%')
                    {
                        r_builder.Append('%');
                    }
                    else if (next == 'i' || next == 'I' || next == 'd' || next == 'D')
                    {
                        if (currentparam < r_args.Count)
                        {
                            var arg = r_args[currentparam];
                            if (arg is int || arg is float) r_builder.Append(arg);

                            ++currentparam;
                            ++i;
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                    else if (next == 'f' || next == 'F')
                    {
                        if (currentparam < r_args.Count)
                        {
                            var arg = r_args[currentparam];
                            if (arg is int || arg is float) r_builder.Append(arg);

                            ++currentparam;
                            ++i;
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                    else if (next == 's' || next == 'S')
                    {
                        if (currentparam < r_args.Count)
                        {
                            var arg = r_args[currentparam];
                            if (arg is string) r_builder.Append(arg);

                            ++currentparam;
                            ++i;
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else if (current == '\\')
                {
                    if (next == 'n' || next == 'N')
                    {
                        r_builder.Append('\n');
                        ++i;
                    }
                    else if (next == 't' || next == 't')
                    {
                        r_builder.Append("    ");
                        ++i;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    r_builder.Append(current);
                }
            }

            return r_builder.ToString();
        }
    }
}