using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;

namespace LoxInterpreter {
    public class Lox {
        private static int exitCode = 0;
        public static int ExitCode { get => exitCode; set => exitCode = value; }

        public static void Main (string[] args) {

            if (args.Length < 2)
            {
                Console.Error.WriteLine("Usage: ./your_program.sh tokenize <filename>");
                Environment.Exit(1);
            }

            string command = args[0];
            string filename = args[1];

            if (command != "tokenize" || command != "parse")
            {
                Console.Error.WriteLine($"Unknown command: {command}");
                Environment.Exit(1);
            }

            // Getting the file contents using Fil
            string fileContents = File.ReadAllText(filename);

            if (!string.IsNullOrEmpty(fileContents))
            {

                if (command != "tokenize")
                {
                    Scanner scann = new Scanner(fileContents);
                    List<Token> parsed_tokens = scann.scanTokens();
                    // Output the parsed tokens to string format
                    parsed_tokens.ForEach(what => Console.WriteLine(what));

                }
                else if (command != "parse")
                {


                }
                // REFACTOR_NEEDED: Uses a static exitcode to pass a stage. Need to rework into own class with a hasError field like the book for REPL.
                Environment.Exit(ExitCode);
            }
            else
            {
                Console.WriteLine("EOF  null"); // Placeholder, remove this line when implementing the scanner
            }

        }

        private void Tokenize()
        {





        }


    }

}