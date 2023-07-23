
namespace UnityMugen.Evaluation
{
    public enum Symbol
    {
        None,

        [TokenMapping("(")] LeftParen,
        [TokenMapping(")")] RightParen,
        [TokenMapping("[")] LeftBracket,
        [TokenMapping("]")] RightBracket,
        [TokenMapping(",")] Comma
    }
}