using LoxInterpreter.Interpreter;

namespace LoxInterpreter.Parser
{
    public partial class Binary : ExprBase
    {

        public partial class Assign : ExprBase
        {
            public Assign(Token name, ExprBase value)
            {
                this.name = name;
                this.value = value;
            }
            public override
            string Accept(IExprVisitor visitor)
            {
                return visitor.visitAssignExpr(this);
            }

            public override
            object Accept(IVisitorInterpreter visitor)
            {
                return visitor.visitAssignExpr(this);
            }

            public ExprBase value;
            public Token name;

        }

        public Binary(ExprBase left, Token oper, ExprBase right)
        {
            this.left = left;
            this.oper = oper;
            this.right = right;
        }

        public override
        string Accept(IExprVisitor visitor)
        {
            return visitor.visitBinaryExprBase(this);
        }
        public ExprBase left;
        public Token oper;
        public ExprBase right;
    }
    public partial class Grouping : ExprBase
    {

        public Grouping(ExprBase expression)
        {
            this.expression = expression;
        }

        public override
        string Accept(IExprVisitor visitor)
        {
            return visitor.visitGroupingExprBase(this);
        }
        public ExprBase expression;
    }
    public partial class Literal : ExprBase
    {
        public Literal(Object value)
        {
            this.value = value;
        }

        public override
        string Accept(IExprVisitor visitor)
        {
            return visitor.visitLiteralExprBase(this);
        }
        public Object value;
    }
    public partial class Unary : ExprBase
    {

        public Unary(Token oper, ExprBase right)
        {
            this.oper = oper;
            this.right = right;
        }

        public override
        string Accept(IExprVisitor visitor)
        {
            return visitor.visitUnaryExprBase(this);
        }
        public Token oper;
        public ExprBase right;
    }

    public class Variable : ExprBase
    {
        public Variable(Token name) {
            this.name = name;
        }

        public override
        string Accept(IExprVisitor visitor)
        {
            return visitor.visitUnaryExprBase(this);
        }

        public override string Accept(IVisitorInterpreter visitor)
        {
            return visitor.visitUnaryExprBase(this);
        }

        public Token name;
    }
}
