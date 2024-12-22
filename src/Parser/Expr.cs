﻿namespace LoxInterpreter.Parser
{
    public class Binary : ExprBase
    {

        public Binary(ExprBase left, Token oper, ExprBase right)
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
        public ExprBase left;
        public Token oper;
        public ExprBase right;
    }
    public class Grouping : ExprBase
    {

        public Grouping(ExprBase expression)
        {
            this.expression = expression;
        }

        public override
        IExprVisitor Accept(IExprVisitor visitor)
        {
            return visitor.visitGroupingExprBase(this);
        }
        public ExprBase expression;
    }
    public class Literal : ExprBase
    {
        public Literal(Object value)
        {
            this.value = value;
        }

        public override
        IExprVisitor Accept(IExprVisitor visitor)
        {
            return visitor.visitLiteralExprBase(this);
        }
        public Object value;
    }
    public class Unary : ExprBase
    {

        public Unary(Token oper, ExprBase right)
        {
            this.oper = oper;
            this.right = right;
        }

        public override
        IExprVisitor Accept(IExprVisitor visitor)
        {
            return visitor.visitUnaryExprBase(this);
        }
        public Token oper;
        public ExprBase right;
    }


}
