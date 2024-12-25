using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxInterpreter
{
    namespace Parser
    {
    public class ParseErrorException : Exception
        {
            public ParseErrorException()
            {
                // MIght as well use the exception here set exitcode as the main can decide on wether to attempt to print or not
                // due to issues with the malformed expr being null objects
                Lox.hasError = true;
                Lox.ExitCode = 65;
            }
            public ParseErrorException(string message): base(message)
            {
               
                Lox.hasError = true;
                Console.WriteLine(message);
            }

            public ParseErrorException(string message, Exception inner) : base(message, inner)
            {
            }
        }
    }
}