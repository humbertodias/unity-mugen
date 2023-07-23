using UnityMugen.Combat;

namespace UnityMugen.Evaluation
{

    public interface IExpression
    {
        Number[] Evaluate(Character character);

        bool IsValid { get; }
    }
}