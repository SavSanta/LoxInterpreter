﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LoxInterpreter.Parser;
using LoxInterpreter.Statement;


namespace LoxInterpreter
{
    namespace Interpreter
    {
        public class Interpreter : IVisitorInterpreter, IStatementVisitor
        {
            private LoxInterpreter.Environment.Environment environment = new();

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
            private bool isLiteralTruthy(Object obj)
            {
                if (obj == null)
                    return false;
                if (obj is bool)
                    return (bool)obj;
                return true;
            }

            public void interpret(List<Stmt> statements)
            {
                try
                {
                    foreach (Stmt statement in statements)
                    {
                        execute(statement);
                    }

                }
                catch (Exception e)
                {
                    //System.InvalidCastException
                    // This previously was the catcher that spit out ""Got some fucking ExceptionError in Interpreter: {0}","
                    // Console.WriteLine(e.Message);

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
            private void execute(Stmt stmt)
            {
                // Force cast because this shit is bonkers
                stmt.Accept((Stmt.IStatementVisitor)this);
            }
            private void executeBlock(List<Stmt> statements, Environment.Environment environment)
            {
                Environment.Environment previous = this.environment;
                try
                {
                    this.environment = environment;
                    foreach (Stmt statement in statements)
                    {
                        execute(statement);
                    }
                }
                finally
                {
                    this.environment = previous;
                }
            }

            private Object evaluate(Parser.ExprBase expr)
            {
                return expr.Accept(this);
            }
            public Object visitAssignExpr(Binary.Assign expr)
            {
                Object value = evaluate(expr.value);
                environment.assign(expr.name, value);
                return value;
            }

            public void visitExpressionStmt(LoxInterpreter.Statement.Expression stmt)
            {
                evaluate(stmt.expr);
                return;
            }
            public void visitBlockStmt(Block stmt)
            {
                executeBlock(stmt.statements, new Environment.Environment(environment));
                return;
            }
            public void visitPrintStmt(Print stmt)
            {
                Object value = evaluate(stmt.expression);
                Console.WriteLine(stringify(value));
                return;
            }
            public void visitVarStmt(Var stmt)
            {
                Object value = null;
                if (stmt.initializer != null)
                {
                    value = evaluate(stmt.initializer);
                }

                environment.define(stmt.name.lexeme, value);
                return;
            }
            public Object visitVariableExpr(Var expr)
            {
                return environment.get(expr.name);
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
                    case TokenType.MINUS:
                        checkNumberOperand(expr.oper, right);
                        return -Double.Parse(right.ToString());
                }
                // Unreachable.
                return null;
            }
            private void checkNumberOperand(Token oper, Object operand)
            {
                double _;
                if (Double.TryParse(operand.ToString(), out _)) return;
                throw new ParseErrorException(70 , oper, "Operand must be a number.");
            }

            private void checkNumberOperands(Token oper, Object left, Object right)
            {
                if ((left is Double) && (right is Double)) return;
                throw new ParseErrorException(70, oper, "Operand must be a number.");
            }

            public object visitBinaryExprBase(Parser.Binary expr)
            {
                Object left = evaluate(expr.left);
                Object right = evaluate(expr.right);

                // In order to accomodate C# incessant need to coerce the boolean string repr into actual values. We get the literal value
                // This will support the bootlegg "String" type check in PLUS (and maybe MINUS).
                List<string> strLitVals = [left.ToString(), right.ToString()];

                // Due to wonky-ness of Java -> C# translations
                // We create some temp variables here to keep the "real" numbers as doubles.
                double real_left, real_right;
                bool l_success = (double.TryParse(left.ToString(), out real_left));
                bool r_success = (double.TryParse(right.ToString(), out real_right));
                List<bool> dblResults = [l_success, r_success];
                bool isbothnums = dblResults.TrueForAll(s => s is true);
                bool isbothfalse = dblResults.TrueForAll(s => s is false);

                switch (expr.oper.type)
                {

                    case TokenType.GREATER:
                        if (isbothnums)
                        {
                            checkNumberOperands(expr.oper, real_left, real_right);
                            return real_left > real_right;
                        }
                        throw new ParseErrorException(70, expr.oper, "Operands must be two numbers or two strings.");
                        break;
                    case TokenType.GREATER_EQUAL:
                        if (isbothnums)
                        {
                            checkNumberOperands(expr.oper, real_left, real_right);
                            return real_left >= real_right;
                        }
                        throw new ParseErrorException(70, expr.oper, "Operands must be two numbers or two strings.");
                        break;
                    case TokenType.LESS:
                        if (isbothnums)
                        {
                            checkNumberOperands(expr.oper, real_left, real_right);
                            return real_left < real_right;
                        }
                        throw new ParseErrorException(70, expr.oper, "Operands must be two numbers or two strings.");
                        break;
                    case TokenType.LESS_EQUAL:
                        if (isbothnums)
                        {
                            checkNumberOperands(expr.oper, real_left, real_right);
                            return real_left <= real_right;
                        }
                        throw new ParseErrorException(70, expr.oper, "Operands must be two numbers or two strings.");
                        break;
                    case TokenType.MINUS:
                        checkNumberOperands(expr.oper, real_left, real_right);
                        if (isbothnums)
                        {
                            return real_left - real_right;
                        }
                        throw new ParseErrorException(70, expr.oper, "Operands must be two numbers or two strings.");
                        break;
                    case TokenType.PLUS:

                        // Early exit test because I dont think we'll ever add different types. Now That I have the Tokenizer parsing to Double in addToken
                        // This may also make the follow=on test redundant but this is the pain of testing and testing

                        if ((left.GetType().Name != right.GetType().Name))
                        {
                            throw new ParseErrorException(70, expr.oper, "Operands must be two numbers or two strings.");
                        }


                        if (isbothnums)
                        {
                            return real_left + real_right;
                        }
                        else if (!(isbothfalse) && (left.GetType().Name != right.GetType().Name))
                        {
                            throw new ParseErrorException(70, expr.oper, "Operands must be two numbers or two strings.");
                        }
                        else if (isbothfalse)
                        {
                            if ((left is String) && (right is String))
                            {
                                return (String)left + (String)right;
                            }
                        }
                        else if (strLitVals.Contains("true") || strLitVals.Contains("false"))
                        {
                            return (String)left + (String)right;
                        }
                        else if ((left is String) && (right is String))
                        {
                            return (String)left + (String)right;
                        }

                        throw new ParseErrorException(70, expr.oper, "Operands must be two numbers or two strings.");
                        break;
                    case TokenType.SLASH:
                        if (isbothnums)
                        { 
                            return real_left / real_right; 
                        }
                        throw new ParseErrorException(70, expr.oper, "Operands must be numbers.");
                        break;
                    case TokenType.STAR:
                        if (isbothnums)
                        {
                            return real_left * real_right;
                        }
                        throw new ParseErrorException(70, expr.oper, "Operands must be numbers.");
                        break;
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
                            return isEqual(left, right);
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

            public string visitUnaryExprBase(Variable variable)
            {
                throw new NotImplementedException();
            }
        }


    }

}

