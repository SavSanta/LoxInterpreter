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
            }

            public ParseErrorException(string message): base(message)
            {
                Console.WriteLine(message);
            }

            public ParseErrorException(string message, Exception inner) : base(message, inner)
            {
            }
        }
    }
}