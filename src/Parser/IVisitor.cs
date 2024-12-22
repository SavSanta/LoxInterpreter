namespace LoxInterpreter
{
    namespace Parser
    {
        public interface IExprVisitor
        {
            string visitBinaryExprBase(Binary exprbase);
            string visitGroupingExprBase(Grouping exprbase);
            string visitLiteralExprBase(Literal exprbase);
            string visitUnaryExprBase(Unary exprbase);
        }
    }
}