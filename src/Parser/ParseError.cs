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
                // Might as well use the exception here set exitcode as the main can decide on wether to attempt to print or not
                // due to issues with the malformed expr being null objects
                Lox.hasError = true;
                Lox.ExitCode = 65;
            }
            public ParseErrorException(Token name, string message): base(message)
            {
               
                Lox.hasError = true;
                Console.WriteLine(message);
            }

            //Specifcally this constructor is to support Section 7.4 for "RunTime Errors"
            // without explicitly creating a new type at this moment
            public ParseErrorException(int exitcode, Token oper , string message) : base(message)
            {

                Lox.hasError = true;
                Lox.ExitCode = exitcode;
                Console.Error.WriteLine(message);

            }

            public ParseErrorException(string message, Exception inner) : base(message, inner)
            {
            }
        }
    }
}