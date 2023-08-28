using System;
using System.Diagnostics;
using UnityMugen.Combat;

namespace UnityMugen.Evaluation
{
	public class NumberReturn : IFunction
	{
		public NumberReturn(Number number)
		{
			m_number = number;
		}

		public override string ToString()
		{
			return m_number.ToString();
		}

		[DebuggerHidden]
		public Number Evaluate(Character state)
		{
			return m_number;
		}

		#region Fields

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		readonly Number m_number;

		#endregion
	}
}