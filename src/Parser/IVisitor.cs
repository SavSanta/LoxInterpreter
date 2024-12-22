namespace LoxInterpreter
{
    namespace Parser
    {
        public interface IExprVisitor
        {
            IExprVisitor visitBinaryExprBase(Binary exprbase);
            IExprVisitor visitGroupingExprBase(Grouping exprbase);
            IExprVisitor visitLiteralExprBase(Literal exprbase);
            IExprVisitor visitUnaryExprBase(Unary exprbase);
        }
    }
}