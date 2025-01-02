using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Xml.Serialization;

namespace LoxInterpreter {
public class Scanner {
    private static Dictionary<String, TokenType> keywords = new Dictionary<String, TokenType>() {
        {"and",    TokenType.AND},
        {"class",  TokenType.CLASS},
        {"else",   TokenType.ELSE},
        {"false",  TokenType.FALSE},
        {"for",    TokenType.FOR},
        {"fun",    TokenType.FUN},
        {"if",     TokenType.IF},
        {"nil",    TokenType.NIL},
        {"or",     TokenType.OR},
        {"print",  TokenType.PRINT},
        {"return", TokenType.RETURN},
        {"super",  TokenType.SUPER},
        {"this",   TokenType.THIS},
        {"true",   TokenType.TRUE},
        {"var",    TokenType.VAR},
        {"while",  TokenType.WHILE},
    };
  
    private string source;
    private List<Token> tokens;
    private int start = 0;
    private int current = 0;
    private int line = 1;

    public Scanner(string sourcetext) {
        this.source = sourcetext;
        this.tokens = new List<Token>();
    }

    public List<Token> scanTokens() {

        while (isAtEnd() == false) {
            // Begin targeting next lexeme
            start = current;
            scanToken();  // This is a call to single input scanner
        }

        tokens.Add(new Token(TokenType.EOF, "", null, line));
        return (List<Token>)tokens;

    }

    private void scanToken() {

        char c = advance();

        switch (c) {
            // Singles
            case '(': addToken(TokenType.LEFT_PAREN); break;
            case ')': addToken(TokenType.RIGHT_PAREN); break;
            case '{': addToken(TokenType.LEFT_BRACE); break;
            case '}': addToken(TokenType.RIGHT_BRACE); break;
            case ',': addToken(TokenType.COMMA); break;
            case '.': addToken(TokenType.DOT); break;
            case '-': addToken(TokenType.MINUS); break;
            case '+': addToken(TokenType.PLUS); break;
            case ';': addToken(TokenType.SEMICOLON); break;
            case '*': addToken(TokenType.STAR); break; 
            // Singles that could be doubles
            case '!':
                addToken(match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
            case '=':
                addToken(match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
            case '<':
                addToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
            case '>':
                addToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
            case '/':
                if (match('/')) {
                    // The parser found a comment therefore we skip-read until the end
                    while ((peek() != '\n') && (isAtEnd() != true)) {
                        advance();
                    }
                }
                else {
                    addToken(TokenType.SLASH);
                }
                break; 

            // WhiteSpace Ignorers. No parsing via a fall-thru.
            case ' ':
            case '\r':
            case '\t':
            case '\v':
            break;
            // Newline no parse but increase line num.
            case '\n':
                line++;
                break;
            // String Literal parser
            case '"':
                stringify();
                break;

            default:
                if (isDigit(c))
                {
                    numberify();
                } 
                else if (isAlpha(c))
                {
                    identfy();
                }
                else {

                        // CodeCrafters instruction is to write this out to the STDERR stream.
                        // Can separate this logic into it's own class later.
                        // Long version rather than using Console.Error Write method directly.
                        // REFACTOR_NEEDED: Hacky property method to set hasError/exitCode
                        LoxInterpreter.Lox.ExitCode = 65;
                        TextWriter errorWriter = Console.Error;
                        errorWriter.Write($"[line {line}] Error: Unexpected character: {c}\n");
                        break;
                        
                }

            break;          // default final Break
                
        } // End Switch


    }

    private bool isDigit(char c) {
    return c >= '0' && c <= '9';
    } 

    private bool isAlpha(char c) {
        return (c >= 'a' && c <= 'z') ||
                (c >= 'A' && c <= 'Z') ||
                (c == '_');
    }

    private bool isAlphaNumeric(char c) {
        return isAlpha(c) || isDigit(c);
    }

    private void numberify() {
        while (isDigit(peek())) {
            advance();
        }

        // Look for a fractional part.
        if (peek() == '.' && isDigit(peeknext())) {
          // Consumes the "." in the fractional
          advance();

            while (isDigit(peek())) {
                advance();
            }
        }

        var floated_number = double.Parse(source[start..current]);
        var rounded_oneplace_num = floated_number.ToString("F2");
        //Have to manually add one place if it is being treated as a whole integer.
        if (rounded_oneplace_num.Contains('.') == false)
        {
            rounded_oneplace_num += ".0";
        }

        addToken(TokenType.NUMBER, Double.Parse(rounded_oneplace_num));

    }

    private void identfy() {

        while (isAlphaNumeric(peek())) {
            advance();
        }

        string text = source[start..current];
        TokenType type;
    
        if (!keywords.TryGetValue(text, out type)) {
            type = TokenType.IDENTIFIER;
        }

        addToken(type);

    }

    // Parse the content between a string literal
    private void stringify()
    {
            while ((peek() != '"') && (isAtEnd() != true))
            {
                if (peek() == '\n')
                {
                    line++;
                    advance();
                }
                advance();
            }
                if (isAtEnd())
                {
                    // REFACTOR_NEEDED: Also setting hasError/exitCode using direct WriteLine method. Violating DRY principle. Make own class
                    LoxInterpreter.Lox.ExitCode = 65;
                    System.Console.Error.WriteLine($"[line {line}] Error: Unterminated string.", line.ToString());
                    return;
                }
               
                // Move toward end quote
                advance();
            
            // Add a trimmed string as a token. TOVERIFY
            //version that doesnt save any quotation marks -> string trimmedstring = source.Substring(start+1, current-start )
            string trimmedstring = source.Substring(start+1, current-start-2 );
        addToken(TokenType.STRING, trimmedstring);
    }

    //  Determines if we've reached the end of the file we're lexxing.
    private bool isAtEnd() {
        return current >= source.Length;

    }

    // Performs a LOOKAHEAD with no consumation of char on tracker
    private char peek() {
        if (isAtEnd()) 
            {
                //Explicit textfile null is returned 
                return '\0'; 
            }
        else 
            {
                return source[current];
            }

        }
    
    private char peeknext() {
    
    // Unsure exactl hof what this is doing. TOVERIFY
    if (current + 1 >= source.Length) 
        {
            return '\0'; 
        }
    else 
        {
            return source[current+1];        // May need to TOVERIFY that this inst +2
        }

    }

    private char advance() {
        //  Unlike the Java source we have not charAt method. Therefore we increase the current counter and index into the method to recieve the char
        char c =  source[current];
        current++;
        return c;
    }

    //Overloaded addToken methods for different types.
    private void addToken(TokenType type) {
        addToken(type, null);
    }

    private void addToken(TokenType type, object? literal) {
        //System.Console.WriteLine($"AddToken Call has a start of {start} and and a current of {current}.");
        // POSSIBLE BUG HERE. If issues matching whole string with quotations marks occur. try TOVERF
        string text = source[start..current];
        tokens.Add(new Token(type, text, literal, line));
        //Console.WriteLine("NEW TOKEN ADDED! {0},", type.ToString());
    }

    // Takes the expected input character and returns bool whether it is what we're expecting it to be.
    private bool match(char expectchar) {

        // Checks if we're at the end
        if (( isAtEnd()) || (source[current] != expectchar)) 
        { 
            return false;
        }

        // Cause a char consume if it matches
        current++;
        return true;
    }



}

}