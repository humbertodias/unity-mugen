using System;

namespace UnityMugen.Evaluation
{

    public class EvaluationException : Exception
    {
        public EvaluationException()
        {
        }

        public EvaluationException(string message) : base(message)
        {
        }

        public EvaluationException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}