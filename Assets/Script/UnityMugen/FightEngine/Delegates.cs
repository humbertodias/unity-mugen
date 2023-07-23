using System.Collections.Generic;
using UnityMugen.Combat;

namespace UnityMugen
{
    public delegate object Constructor(params object[] args);
}

namespace UnityMugen.Evaluation
{
    public delegate Node NodeParse(ParseState state);
    public delegate Node NodeBuild(List<Token> tokens, ref int tokenindex);
    public delegate Number EvaluationCallback(Character character);
}