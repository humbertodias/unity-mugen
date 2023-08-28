﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using UnityMugen.Combat;

namespace UnityMugen.Evaluation
{
	public abstract class Function : IFunction
	{
		protected static bool KeepLog => LauncherEngine.Inst.initializationSettings.KeepLog;
		public abstract Number Evaluate(Character state);

		protected Function(List<IFunction> children, List<Object> arguments)
		{
			if (children == null) throw new ArgumentNullException("children");
			if (arguments == null) throw new ArgumentNullException("arguments");

			m_children = children;
			m_arguments = arguments;
		}

		public List<IFunction> Children
		{
			get { return m_children; }
		}

		public List<Object> Arguments
		{
			get { return m_arguments; }
		}

		#region Fields

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		readonly List<IFunction> m_children;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		readonly List<Object> m_arguments;

		#endregion
	}
}