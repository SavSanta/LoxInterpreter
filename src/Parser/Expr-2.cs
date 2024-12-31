using LoxInterpreter.Interpreter;

// This extension class is implemented for hte Parser versions of hte visitor behaviours

namespace LoxInterpreter.Parser
{
    public partial class Binary : ExprBase
    {
        public override
        object Accept(IVisitorInterpreter visitor)
        {
            return visitor.visitBinaryExprBase(this);
        }

    }
    public partial class Grouping : ExprBase
    {
        public override
        object Accept(IVisitorInterpreter visitor)
        {
            return visitor.visitGroupingExprBase(this);
        }
    }
    public partial  class Literal : ExprBase
    {

        public override
        object Accept(IVisitorInterpreter visitor)
        {
            return visitor.visitLiteralExprBase(this);
        }
    }
    public partial class Unary : ExprBase
    {

        public override
        object Accept(IVisitorInterpreter visitor)
        {
            return visitor.visitUnaryExprBase(this);
        }

    }


}
