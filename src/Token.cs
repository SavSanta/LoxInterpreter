
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LoxInterpreter {
    public class Token {
        public TokenType type;
        public string lexeme;
        public object literal;
        public int line;

        public Token(TokenType type, string lexeme, object? literal, int line) {

            this.type = type;
            this.lexeme = lexeme;
            this.literal = literal;
            this.line = line;
        }

        public override string ToString() {

            if (literal == null)
            {
                literal = "null";
            }

            if (type == TokenType.NUMBER)
            {
                // Essentially dupe code from ASTParser due to bad design of my program and this course itself
                if (literal.ToString().Contains('.') == false)
                {
                    literal = literal.ToString() + ".0";
                }
                else
                {
                    literal = Convert.ToDouble(literal).ToString("F2").TrimEnd('0');
                }
            }

            return this.type + " " + this.lexeme + " " + literal;

        }


    }

}