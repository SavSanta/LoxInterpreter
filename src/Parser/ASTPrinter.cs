using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter
{
    namespace Parser
    {
        public class ASTPrinter : IExprVisitor
        {
            public static IExprVisitor print(ExprBase expr)
            {
                return expr.Accept(this);
            }

            public IExprVisitor visitBinaryExprBase(Binary expr)
            {
                return parenthesize(expr.oper.lexeme, expr.left, expr.right);
            }
            public IExprVisitor visitGroupingExprBase(Grouping expr)
            {
                return parenthesize("group", expr.expression);
            }

            public IExprVisitor visitLiteralExprBase(Literal expr)
            {
                if (expr.value == null) return "nil";
                return expr.value.ToString();
            }

            public IExprVisitor visitUnaryExprBase(Unary expr)
            {
                return parenthesize(expr.oper.lexeme, expr.right);
            }

            private string parenthesize(string name, params ExprBase[] exprs)
            {
                StringBuilder builder = new StringBuilder();

                builder.Append("(").Append(name);
                foreach (ExprBase expr in exprs)
                {
                    builder.Append(" ");
                    builder.Append(expr.Accept(this));
                }
                builder.Append(")");

                return builder.ToString();
            }

            public static void Main(String[] args)
            {
                ExprBase expression = new Binary(
                    new Unary(
                        new Token(TokenType.MINUS, "-", null, 1),
                        new Literal(123)),
                    new Token(TokenType.STAR, "*", null, 1),
                    new Grouping(
                        new Literal(45.67)));
                
                Console.WriteLine(ASTPrinter.print(expression));
            }
        }

    }
}

