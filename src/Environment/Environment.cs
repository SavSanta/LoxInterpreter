using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoxInterpreter.Parser;

namespace LoxInterpreter
{
    namespace Environment
    {
        public class Environment
        {
            Environment enclosing;
            private Dictionary<String, Object> values = new Dictionary<string, object>();
            public Environment() 
            {
                this.enclosing = null;
            }
            public Environment(Environment closed) 
            {
                this.enclosing = closed;
            }

            public void define(string name, object value)
            {
                values.Add(name, value);
            }

            public object get(Token name)
            {
                if (values.ContainsKey(name.lexeme))
                {
                    return values[name.lexeme];
                }
                // If the var  isn’t found in this environmental scope, Recurse to the next enclosed one.
                if (enclosing != null) return enclosing.get(name);

                throw new ParseErrorException(name,"Undefined variable '" + name.lexeme + "'.");
            }
            public void assign(Token name, object value)
            {
                if (values.ContainsKey(name.lexeme))
                {
                    values[name.lexeme] = value;
                    return;
                }

                // If the var assignment isn’t found in this environmental scope, Recurse to the next enclosed one.
                if (enclosing != null)
                {
                    enclosing.assign(name, value);
                    return;
                }

                throw new ParseErrorException(name, "Undefined variable '" + name.lexeme + "'.");
            }
        }
    }
}