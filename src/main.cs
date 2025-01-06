using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Channels;
using LoxInterpreter.Interpreter;
using LoxInterpreter.Parser;
using LoxInterpreter.Statement;

namespace LoxInterpreter {
    public class Lox {
        private static int exitCode = 0;
        public static bool hasError = false;
        public static bool hadRuntimeError = false;
        private static readonly List<string> commands = new() { "tokenize", "parse", "evaluate" };
        public static int ExitCode { get => exitCode; set => exitCode = value;  }
        public static void Main (string[] args) {

            if (args.Length < 2)
            {
                Console.Error.WriteLine("Usage: ./your_program.sh tokenize <filename>");
                Console.Error.WriteLine("Usage: ./your_program.sh parse <filename>");
                Console.Error.WriteLine("Usage: ./your_program.sh evaluate <filename>");
                System.Environment.Exit(1);
            }

            string command = args[0];
            string filename = args[1];

            if (!Lox.commands.Contains(command))
            {
                Console.Error.WriteLine($"Unknown command: {command}");
                System.Environment.Exit(1);
            }

            // Read file contents and check if empty
            string fileContents = File.ReadAllText(filename);
            if (!string.IsNullOrEmpty(fileContents))
            {
                if (command == "tokenize")
                {
                    Scanner scann = new Scanner(fileContents);
                    List<Token> parsed_tokens = scann.scanTokens();
                    // Output the parsed tokens to string format
                    parsed_tokens.ForEach(what => Console.WriteLine(what));

                }
                else if (command == "parse")
                {
                    Scanner scann = new Scanner(fileContents);
                    List<Token> parsed_tokens = scann.scanTokens();
                    //TokenParser pars = new TokenParser(parsed_tokens);
                    //ExprBase expression = pars.parse();
                    TokenParser pars = new TokenParser(parsed_tokens);
                    List<Stmt> statements = pars.parse();
                    if (!hasError)
                    {
                        //Console.WriteLine((new ASTPrinter().Print(expression)));
                        //Console.WriteLine((new ASTPrinter().Print(statements)));
                    }
                }
                else if ( command == "evaluate" | command == "run")
                {
                    Scanner scann = new Scanner(fileContents);
                    List<Token> parsed_tokens = scann.scanTokens();
                    TokenParser pars = new TokenParser(parsed_tokens);
                    List<Stmt> statements = pars.parse();
                    LoxInterpreter.Interpreter.Interpreter interpr = new();
                    interpr.interpret(statements);
                }

                System.Environment.Exit(ExitCode);
            }
            else
            {
                Console.WriteLine("EOF  null");
            }

        }

        internal static void Error(Token token, string message)
        {
            if (token.type == TokenType.EOF)
            {
                Report(token.line, " at end", message);
            }
            else
            {
                Report(token.line, " at '" + token.lexeme + "'", message);
            }
        }

        private static void Report(int line, string lex, string message)
        {
            Console.Error.WriteLine($"[line {line}] Lexeme: {lex} Message:{message}");
        }

        internal static string ToLowerCaseIfBool(string value) 
        {
            if (value.ToString() == "True") return "true";
            if (value.ToString() == "False") return "false";
            return value;
        }

    }

}