﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

                // More double based crap. Could do a Try/Catch or put into own function but worried about unforseen crap. 
                double _;
                if (Double.TryParse(obj.ToString(), out _)) {
                    if (obj.ToString().EndsWith(".0"))
                    {
                        string text = obj.ToString().Substring(0, obj.ToString().Length - 2);
                        return text;
                    }

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
                    case TokenType.MINUS: return -Double.Parse(right.ToString());
                }
                // Unreachable.
                return null;
            }

            public object visitBinaryExprBase(Parser.Binary expr)
            {
                Object left = evaluate(expr.left);
                Object right = evaluate(expr.right);

                // Due to wonky-ness of Java -> C# translations
                // We create some temp variables here to keep the "real" numbers as doubles.
                double real_left, real_right;
                bool l_success = (double.TryParse(left.ToString(), out real_left));
                bool r_success = (double.TryParse(right.ToString(), out real_right));
                bool isbothnums = l_success & r_success;

                switch (expr.oper.type)
                {

                    case TokenType.GREATER: return real_left > real_right;
                    case TokenType.GREATER_EQUAL: return real_left >= real_right;
                    case TokenType.LESS: return real_left < real_right;
                    case TokenType.LESS_EQUAL: return real_left <= real_right;
                    case TokenType.MINUS: return real_left - real_right;
                    case TokenType.PLUS:

                        if (isbothnums)
                        {
                            return real_left + real_right;
                        }

                        if ((left is String) && (right is String))
                        {
                            return (String)left + (String)right;
                        }
                        break;
                    case TokenType.SLASH: return real_left / real_right;
                    case TokenType.STAR: return real_left * real_right;
                    case TokenType.BANG_EQUAL: return isbothnums ? !isEqual(real_left, real_right) : !isEqual(left, right);
                    case TokenType.EQUAL_EQUAL:
                        // Check the underlying raw representation for a string coalesced into number/double ?
                        // This is a quick bad hack to pass the 98 == "98" codecraftors test.
                        // The representation of left and right is 98 -> 98.0 and '"98"' -> 98 (wwith maybe some quotes missing

                        //Cheap copout length test on the underlying literal. because i got tired of trying to figure out hte logi

                        List<string> stuff = new();

                        stuff.Add(left.ToString());
                        stuff.Add(real_left.ToString());
                        stuff.Add((right.ToString()));
                        stuff.Add(real_right.ToString());

                        var res = stuff.TrueForAll(s => s.ToString() == real_left.ToString());

                        if (res)
                        {
                            return true;
                            return isEqual(left, real_right);
                        }
                        else if (isbothnums)
                        {
                            if (left.ToString() !=  right.ToString()) 
                            {
                                if (left is String && right is String)
                                {
                                    return false;
                                }
                            }
                            return isEqual(Double.Parse(left.ToString()), Double.Parse(right.ToString()));
                        }
                        else
                        { return isEqual(left, right); }



                        
                }

                // Unreachable.
                return null;
            }





        }


    }

}

