using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LoxInterpreter.Parser;

namespace LoxInterpreter
{
    namespace Interpreter
    {
        public class Interpreter : IVisitorInterpreter
        {
            private bool isEqual(Object a, Object b)
            {
                if (a == null && b == null) return true;
                if (a == null) return false;

                return a.Equals(b);
            }
            private bool isTruthy(Object obj)
            {
                if (obj == null) return false;
                if (obj is bool) return (bool)obj;
                return true;
            }

            public void interpret(ExprBase expr)
            {
                try
                {
                    Object value = evaluate(expr);
                    Console.WriteLine(stringify(value));
                }
                catch (Exception e)
                {
                    {
                        Console.WriteLine("Got some fucking ExceptionError in Interpreter: {0}", e.Message);
                    }

                }
            }
            private string stringify(object obj)
            {

                if (obj == null) return "nil";

                if (obj is Double) {
                    String text = obj.ToString();
                    if (text.EndsWith(".0"))
                    {
                        text = text.Substring(0, text.Length - 2);
                    }
                    return text;
                }

                // Have to add the goddamn redundant "stay lowercase" code because C# insists on capitalizing content for stupid Lox semantic
                return Lox.ToLowerCaseIfBool(obj.ToString());

            }

            private Object evaluate(Parser.ExprBase expr)
            {
                return expr.Accept(this);
            }
            public Object visitLiteralExprBase(Parser.Literal expr)
            {
                return expr.value;
            }

            public Object visitGroupingExprBase(Parser.Grouping expr)
            {
                return evaluate(expr.expression);
            }

            public Object visitUnaryExprBase(Parser.Unary expr)
            {
                Object right = evaluate(expr.right);

                switch (expr.oper.type)
                {
                    case TokenType.BANG: return !isTruthy(right);
                    case TokenType.MINUS: return -(double)right;
                }
                // Unreachable.
                return null;
            }

            public object visitBinaryExprBase(Parser.Binary expr)
            {
                Object left = evaluate(expr.left);
                Object right = evaluate(expr.right);

                switch (expr.oper.type)
                {

                    case TokenType.GREATER: return (double)left > (double)right;
                    case TokenType.GREATER_EQUAL: return (double)left >= (double)right;
                    case TokenType.LESS: return (double)left < (double)right;
                    case TokenType.LESS_EQUAL: return (double)left <= (double)right;
                    case TokenType.MINUS: return (double)left - (double)right;
                    case TokenType.PLUS:
                        // Due to wonky-ness of java -> C# translations, We create some temp variables
                        double real_left, real_right;
                        
                        if ((double.TryParse(left.ToString(), out real_left)) && (double.TryParse(right.ToString(), out real_right)))
                        {
                            return real_left + real_right;
                        }

                        if ((left is String) && (right is String))
                        {
                            return (String)left + (String)right;
                        }
                        break;
                    case TokenType.SLASH: return (double)left / (double)right;
                    case TokenType.STAR: return (double)left * (double)right;
                    case TokenType.BANG_EQUAL: return !isEqual(left, right);
                    case TokenType.EQUAL_EQUAL: return isEqual(left, right);
                }

                // Unreachable.
                return null;
            }





        }


    }

}

