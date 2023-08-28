using System;
using UnityMugen.Combat;

namespace UnityMugen.Evaluation
{
	public interface IFunction
	{
		Number Evaluate(Character state);
	}
}
