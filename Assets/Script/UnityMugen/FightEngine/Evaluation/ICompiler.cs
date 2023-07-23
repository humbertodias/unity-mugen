namespace UnityMugen.Evaluation
{

    public interface ICompiler
    {
        EvaluationCallback Create(Node node);
    }
}