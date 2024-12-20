using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter.Parser
{
    internal class Binary : ExprBase
    {
        ExprBase left;
        Token oper;
        ExprBase right;
        public Binary(ExprBase left, Token oper, ExprBase right) 
        {
            this.left = left;
            this.oper = oper;
            this.right = right;
        }
    }





}
