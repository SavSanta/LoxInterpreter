﻿
using System.Security.Claims;
using System;
using System.Text.RegularExpressions;
using LoxInterpreter.Statement;

namespace LoxInterpreter
{
    namespace Parser
    {
        public class TokenParser
        {
            private List<Token> tokens;
            private int current = 0;

            public TokenParser(List<Token> tokens) 
            {
                this.tokens = tokens;
            
            }

            private ExprBase assignment()
            {
                ExprBase expr = equality();

                if (match(TokenType.EQUAL))
                {
                    Token equals = previous();
                    ExprBase value = assignment();

                    if (expr is Var) {
                        //Token name = ((Var)expr).name;
                        Token name = ((Variable)expr).name;
                        return new Binary.Assign(name, value);
                    }

                    Lox.Error(equals, "Invalid assignment target.");
                }

                return expr;

            }

            private ExprBase expression()
            {
                return assignment();
            }
            private Stmt declaration()
            {
                try
                {
                    if (match(TokenType.VAR))
                    {
                        return varDeclaration();
                    }

                    return statement();
                }
                catch (ParseErrorException error)
                {
                    synchronize();
                    return null;
                }
            }
            private List<Stmt> block()
            {
                List<Stmt> statements = new ();

                while (!check(TokenType.RIGHT_BRACE) && !isAtEnd())
                {
                    statements.Add(declaration());
                }

                consume(TokenType.RIGHT_BRACE, "Expect '}' after block.");
                return statements;
            }
            private Stmt statement()
            {
                if (match(TokenType.PRINT)) return printStatement();
                if (match(TokenType.LEFT_BRACE)) return new Block(block());

                return expressionStatement();
            }
            private Stmt printStatement()
            {
                ExprBase value = expression();
                consume(TokenType.SEMICOLON, "Expect ';' after value.");
                return new Print(value);
            }
            private Stmt expressionStatement()
            {
                ExprBase expr = expression();
                consume(TokenType.SEMICOLON, "Expect ';' after expression.");
                return new Expression(expr);
            }

            private Stmt varDeclaration()
            {
                Token name = consume(TokenType.IDENTIFIER, "Expect variable name.");

                ExprBase initializer = null;
                if (match(TokenType.EQUAL))
                {
                    initializer = expression();
                }

                consume(TokenType.SEMICOLON, "Expect ';' after variable declaration.");
                return new Var(name, initializer);
            }
            private ExprBase equality()
            {
                ExprBase expr = comparison();

                while (this.match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
                {
                    Token oper = previous();
                    ExprBase right = comparison();
                    expr = new Binary(expr, oper, right);
                }
                return expr;
            }

            private bool match(params TokenType[] types)
            {
                foreach (TokenType type in types)
                {
                    if (check(type))
                    {
                        advance();
                        return true;
                    }
                }
                return false;
            }

            private bool check(TokenType type)
            {
                if (isAtEnd())
                {
                    return false;
                }

                return peek().type == type;
            }

            private Token advance()
            {
                if (!isAtEnd()) 
                {
                    current++;   
                }

                return previous();
            }

            private bool isAtEnd()
            {
                return peek().type == TokenType.EOF;
            }

            private Token peek()
            {
                return tokens[current];
            }

            private Token previous()
            {
                return tokens[(current-1)];
            }

            private ExprBase comparison()
            {
                ExprBase expr = term();

                while (match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
                {
                    Token oper = previous();
                    ExprBase right = term();
                    expr = new Binary(expr, oper, right);
                }

                return expr;
            }

            private ExprBase term()
            {
                ExprBase expr = factor();

                while (match(TokenType.MINUS, TokenType.PLUS))
                {
                    Token oper = previous();
                    ExprBase right = factor();
                    expr = new Binary(expr, oper, right);
                }
                return expr;
            }

            private ExprBase factor() 
            {
                ExprBase expr = unary();

                while (match(TokenType.SLASH, TokenType.STAR))
                {
                    Token oper = previous();
                    ExprBase right = unary();
                    expr = new Binary(expr, oper, right);
                }
                return expr;
            }

            private ExprBase unary()
            {
                if (match(TokenType.BANG, TokenType.MINUS))
                {
                    Token oper = previous();
                    ExprBase right = unary();
                    return new Unary(oper, right);
                }
                return primary();
            }

            private ExprBase primary()
            {
                if (match(TokenType.FALSE)) return new Literal(false);
                if (match(TokenType.TRUE)) return new Literal(true);
                if (match(TokenType.NIL)) return new Literal(null);
                          
                if (match(TokenType.NUMBER, TokenType.STRING))
                {
                    return new Literal(previous().literal);
                }

                if (match(TokenType.IDENTIFIER))
                {
                    return new Variable(previous());
                }

                if (match(TokenType.LEFT_PAREN))
                {
                    ExprBase expr = expression();
                    consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
                    return new Grouping(expr);
                }

                throw gen_parse_error(peek(), "Expect expression.");
                /* Below no longer necesary due to custom exception handling */
                // Should not reach this codepath. However, CS0161 checker requires this codepath 
                //return new Literal("UNREACHABLE_CODE_PATH_ERROR");
            }

            private Token consume(TokenType type, string message)
            {
                if (check(type))
                {
                    return advance();
                }

                gen_parse_error(peek(), message);
                // Should not reach this codepath. However, CS0161 checker requires this codepath 
                return new Token(TokenType.EOF, "UNREACHABLE_CODE_PATH_ERROR", null, 0);
            }

            private ParseErrorException gen_parse_error(Token token, string message) 
            {
                Lox.Error(peek(), message);
                return new ParseErrorException();
            }

            public List<Stmt> parse()
            {
                List<Stmt> statements = new ();
                while (!isAtEnd())
                {
                    statements.Add(declaration());
                }

                return statements;
            }
            

            // Recursive Descents Tracker Reset via discarding tokens until we’re right at the beginning of the next statement.
            // As After a semicolon, we’re probably finished with a statement. Most statements start with a keyword—for, if, return, var, etc. When the next token is any of those, we’re probably about to start a statement.
            private void synchronize()
            {
                advance();

                while (!isAtEnd())
                {
                    if (previous().type == TokenType.SEMICOLON) 
                    {
                        return;
                    }

                    switch (peek().type)
                    {
                        case TokenType.CLASS:
                        case TokenType.FUN:
                        case TokenType.VAR:
                        case TokenType.FOR:
                        case TokenType.IF:
                        case TokenType.WHILE:
                        case TokenType.PRINT:
                        case TokenType.RETURN:
                            return;
                    }

                    advance();
                }
            }

        }
    }
}