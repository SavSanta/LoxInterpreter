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
            public string Print(ExprBase expr)
            {
                return expr.Accept(this);
            }

            public string visitBinaryExprBase(Binary expr)
            {
                return parenthesize(expr.oper.lexeme, expr.left, expr.right);
            }
            public string visitGroupingExprBase(Grouping expr)
            {
                return parenthesize("group", expr.expression);
            }

            public string visitLiteralExprBase(Literal expr)
            {
                if (expr.value == null) return "nil";
                // The early boolean lowering is because ToString returns it with "True/False"
                if (expr.value == "true") return "true"; 
                if (expr.value == "false") return "false"; 
                return expr.value.ToString();
            }

            public string visitUnaryExprBase(Unary expr)
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

            public static void FakeMain(String[] args)
            {
                ExprBase expression = new Binary(
                    new Unary(
                        new Token(TokenType.MINUS, "-", null, 1),
                        new Literal(123)),
                    new Token(TokenType.STAR, "*", null, 1),
                    new Grouping(
                        new Literal(45.67)));
                
                Console.WriteLine(new ASTPrinter().Print(expression));
            }
        }

    }
}

