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

        public override
        IExprVisitor Accept(IExprVisitor visitor)
        {
            return visitor.visitBinaryExprBase(this);
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

        public override
        IExprVisitor Accept(IExprVisitor visitor)
        {
            return visitor.visitGroupingExprBase(this);
        }
        private ExprBase expression;
    }
    public class Literal : ExprBase
    {

        Literal(Object value)
        {
            this.value = value;
        }

        public override
        IExprVisitor Accept(Visitor visitor)
        {
            return visitor.visitLiteralExprBase(this);
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

        public override
        IExprVisitor Accept(IExprVisitor visitor)
        {
            return visitor.visitUnaryExprBase(this);
        }
        private Token oper;
        private ExprBase right;
    }


}
