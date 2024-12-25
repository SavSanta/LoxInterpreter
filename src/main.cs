using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.IO;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Channels;
using LoxInterpreter.Parser;

namespace LoxInterpreter {
    public class Lox {
        private static int exitCode = 0;
        private static bool hadError = false;
        private static readonly List<string> commands = new() { "tokenize", "parse" };
        public static int ExitCode { get => exitCode; set => exitCode = value; }
        public static void Main (string[] args) {

            if (args.Length < 2)
            {
                Console.Error.WriteLine("Usage: ./your_program.sh tokenize <filename>");
                Console.Error.WriteLine("Usage: ./your_program.sh parse <filename>");
                Environment.Exit(1);
            }

            string command = args[0];
            string filename = args[1];

            if (!Lox.commands.Contains(command))
            {
                Console.Error.WriteLine($"Unknown command: {command}");
                Environment.Exit(1);
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
                    TokenParser pars = new TokenParser(parsed_tokens);
                    ExprBase expression = pars.parse();
                    Console.WriteLine((new ASTPrinter().Print(expression)));
                }
                // REFACTOR_NEEDED: Uses a static exitcode to pass a stage. Need to rework into own class with a hasError field like the book for REPL.
                Environment.Exit(ExitCode);
            }
            else
            {
                Console.WriteLine("EOF  null");
            }

        }

        internal static void Error(Token token, String message)
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
            Console.Error.WriteLine($"Processing Error! Line: {line} Lexeme: {lex} Message:{message}"); ;
        }

    }

}