namespace LoxInterpreter.Parser
{
    public class Binary : ExprBase
    {

        Binary(ExprBase left, Token oper, ExprBase right)
        {
            this.left = left;
            this.oper = oper;
            this.right = right;
        }

        private ExprBase left;
        private Token oper;
        private ExprBase right;
    }
    public class Grouping : ExprBase
    {

        Grouping(ExprBase expression)
        {
            this.expression = expression;
        }

        private ExprBase expression;
    }
    public class Literal : ExprBase
    {

        Literal(Object value)
        {
            this.value = value;
        }

        private Object value;
    }
    public class Unary : ExprBase
    {

        Unary(Token oper, ExprBase right)
        {
            this.oper = oper;
            this.right = right;
        }

        private Token oper;
        private ExprBase right;
    }


}
