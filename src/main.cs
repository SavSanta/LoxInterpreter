using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;

namespace LoxInterpreter {
    public class Lox {

        public static void Main (string[] args) {

            if (args.Length < 2)
            {
                Console.Error.WriteLine("Usage: ./your_program.sh tokenize <filename>");
                Environment.Exit(1);
            }

            string command = args[0];
            string filename = args[1];

            if (command != "tokenize")
            {
                Console.Error.WriteLine($"Unknown command: {command}");
                Environment.Exit(1);
            }

            // Getting the file contents using Fil
            string fileContents = File.ReadAllText(filename);

            if (!string.IsNullOrEmpty(fileContents))
            {
                Scanner scann = new Scanner(fileContents);
                List<Token> parsed_tokens = scann.scanTokens();
                // Output the parsed tokens to string format
                parsed_tokens.ForEach(what => Console.WriteLine(what));
                //System.Console.WriteLine("Hopefully finished tokenizing without too much fuss??");

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