using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoxInterpreter.Parser;

namespace LoxInterpreter
{
    namespace Interpreter
    {
        public interface IVisitorInterpreter
        {

            public object visitBinaryExprBase(Binary exprbase);
            public object visitGroupingExprBase(Grouping exprbase);
            public object visitLiteralExprBase(Literal exprbase);
            public object visitUnaryExprBase(Unary exprbase);


        }

    }
}