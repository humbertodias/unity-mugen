namespace UnityMugen.Evaluation
{
    public enum Operator
    {
        None,

        [BinaryOperatorMapping("||", "LogicalOr", 0)] LogicalOr,
        [BinaryOperatorMapping("^^", "LogicalXor", 1)] LogicalXor,
        [BinaryOperatorMapping("&&", "LogicalAnd", 2)] LogicalAnd,
        [BinaryOperatorMapping("|", "BinaryOr", 3)] BinaryOr,
        [BinaryOperatorMapping("^", "BinaryXor", 4)] BinaryXor,
        [BinaryOperatorMapping("&", "BinaryAnd", 5)] BinaryAnd,
        [BinaryOperatorMapping(":=", "_Assignment", 6)] Assignment,
        [BinaryOperatorMapping("=", "op_Equality", 7)] Equals,
        [BinaryOperatorMapping("!=", "op_Inequality", 7)] NotEquals,
        [BinaryOperatorMapping("<", "op_LessThan", 8)] Lesser,
        [BinaryOperatorMapping("<=", "op_LessThanOrEqual", 8)] LesserEquals,
        [BinaryOperatorMapping(">", "op_GreaterThan", 8)] Greater,
        [BinaryOperatorMapping(">=", "op_GreaterThanOrEqual", 8)] GreaterEquals,
        [BinaryOperatorMapping("+", "op_Addition", 9)] Plus,
        [BinaryOperatorMapping("-", "op_Subtraction", 9)] Minus,
        [BinaryOperatorMapping("/", "op_Division", 10)] Divide,
        [BinaryOperatorMapping("*", "op_Multiply", 10)] Multiply,
        [BinaryOperatorMapping("%", "op_Modulus", 10)] Modulus,
        [BinaryOperatorMapping("**", "Power", 11)] Exponent,
        
        [UnaryOperatorMappingAttribute("!", "LogicalNot")] LogicalNot,
        [UnaryOperatorMappingAttribute("~", "BinaryNot")] BitNot
    }
}