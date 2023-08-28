namespace UnityMugen.Evaluation
{
    public enum Operator
    {
        None,

        [BinaryOperatorMapping("||", typeof(Operations.LogicOr), 0)] LogicalOr,
        [BinaryOperatorMapping("^^", typeof(Operations.LogicXor), 1)] LogicalXor,
        [BinaryOperatorMapping("&&", typeof(Operations.LogicAnd), 2)] LogicalAnd,
        [BinaryOperatorMapping("|", typeof(Operations.BitOr), 3)] BinaryOr,
        [BinaryOperatorMapping("^", typeof(Operations.BitXor), 4)] BinaryXor,
        [BinaryOperatorMapping("&", typeof(Operations.BitAnd), 5)] BinaryAnd,
        [BinaryOperatorMapping(":=", typeof(Operations.Assignment), 6)] Assignment,
        [BinaryOperatorMapping("=", typeof(Operations.Equality), 7)] Equals,
        [BinaryOperatorMapping("!=", typeof(Operations.Inequality), 7)] NotEquals,
        [BinaryOperatorMapping("<", typeof(Operations.LesserThan), 8)] Lesser,
        [BinaryOperatorMapping("<=", typeof(Operations.LesserEquals), 8)] LesserEquals,
        [BinaryOperatorMapping(">", typeof(Operations.GreaterThan), 8)] Greater,
        [BinaryOperatorMapping(">=", typeof(Operations.GreaterEquals), 8)] GreaterEquals,
        [BinaryOperatorMapping("+", typeof(Operations.Addition), 9)] Plus,
        [BinaryOperatorMapping("-", typeof(Operations.Substraction), 9)] Minus,
        [BinaryOperatorMapping("/", typeof(Operations.Division), 10)] Divide,
        [BinaryOperatorMapping("*", typeof(Operations.Multiplication), 10)] Multiply,
        [BinaryOperatorMapping("%", typeof(Operations.Modulus), 10)] Modulus,
        [BinaryOperatorMapping("**", typeof(Operations.Exponent), 11)] Exponent,
        
        [UnaryOperatorMappingAttribute("!", typeof(Operations.LogicNot))] LogicalNot,
        [UnaryOperatorMappingAttribute("~", typeof(Operations.BitNot))] BitNot
    }
}