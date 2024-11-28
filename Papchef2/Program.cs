namespace Papshef2;
public enum ТипЛексемы
{
    ID,
    Константа,
    Присваивание,
    Разделитель,
    Плюс,
    Умножить,
    OPENPAREN,
    CLOSEPAREN,
    While,
    Colon,
    DO,
    END,
    AND,
    OR,
    Сравнение,
    CLOSEBRACE,
    OPENBRACE,
    FOR,
    LOOP,
    DIV
}
public class Parser
{
    public readonly List<Token> _tokens;
    private int _currentTokenIndex;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
        _currentTokenIndex = 0;
    }

    private Token CurrentToken
    {
        get
        {
            if (HasMoreTokens())
            {
                return _tokens[_currentTokenIndex];
            }
            else
            {
                throw new Exception("Ошибка: неожиданный конец выражения.");
            }
        }
    }

    private bool HasMoreTokens()
    {
        return _currentTokenIndex < _tokens.Count;
    }

    private void Match(ТипЛексемы expectedType)
    {
        if (_currentTokenIndex >= _tokens.Count)
        {
            throw new Exception($"Ошибка: ожидался {expectedType}, но входная строка завершилась. В позиции {_currentTokenIndex}");
        }
        if (CurrentToken.Type == expectedType)
        {
            _currentTokenIndex++;
        }
        else
        {
            throw new Exception($"Ошибка: ожидалось {expectedType}, но найдено {CurrentToken.Type}. В позиции {_currentTokenIndex}");
        }
    }
    public void ParseAssignment()
    {
        if (HasMoreTokens() && CurrentToken.Type == ТипЛексемы.ID)
        {
            Match(ТипЛексемы.ID);
            Match(ТипЛексемы.Присваивание);
            ParseExpression();

            if (HasMoreTokens() && CurrentToken.Type == ТипЛексемы.Разделитель)
            {
                Match(ТипЛексемы.Разделитель);
            }
            else
            {
                throw new Exception($"Ошибка: ожидается ';' в конце выражения. В позиции {_currentTokenIndex}");
            }
        }
        else
        {
            throw new Exception($"Ошибка: ожидался идентификатор перед присваиванием. В позиции {_currentTokenIndex}");
        }
    }

    private void ParseExpression()
    {
        ParseTerm();

        while (HasMoreTokens() && CurrentToken.Type == ТипЛексемы.Плюс)
        {
            Match(ТипЛексемы.Плюс);
            ParseTerm();
        }
    }

    private void ParseTerm()
    {
        ParseFactor();

        while (CurrentToken.Type == ТипЛексемы.Умножить || CurrentToken.Type == ТипЛексемы.DIV)
        {
            if (CurrentToken.Type == ТипЛексемы.Умножить)
            {
                Match(ТипЛексемы.Умножить);
            }
            else if (CurrentToken.Type == ТипЛексемы.DIV)
            {
                Match(ТипЛексемы.DIV);
            }

            ParseFactor();
        }
    }
    private void ParseParameters()
    {
        ParseExpression();

        while (CurrentToken.Type == ТипЛексемы.Colon)
        {
            Match(ТипЛексемы.Colon);
            ParseExpression();
        }
    }
    private void ParseFactor()
    {
        if (HasMoreTokens() && CurrentToken.Type == ТипЛексемы.ID)
        {
            Match(ТипЛексемы.ID);
            if (HasMoreTokens() && CurrentToken.Type == ТипЛексемы.OPENPAREN)
            {
                Match(ТипЛексемы.OPENPAREN);
                ParseParameters();
                Match(ТипЛексемы.CLOSEPAREN);
            }
        }else if (HasMoreTokens() && CurrentToken.Type == ТипЛексемы.Константа)
        {
            Match(ТипЛексемы.Константа);
        }
        else if (HasMoreTokens() && CurrentToken.Type == ТипЛексемы.OPENPAREN)
        {
            Match(ТипЛексемы.OPENPAREN);
            ParseExpression();
            Match(ТипЛексемы.CLOSEPAREN);
        }
        else
        {
            throw new Exception($"Ошибка: ожидался идентификатор, константа или выражение в скобках. В позиции {_currentTokenIndex}");
        }
    }
    private void ParseLogicalExpression()
    {
        ParseRelationalExpression();

        while (CurrentToken.Type == ТипЛексемы.AND || CurrentToken.Type == ТипЛексемы.OR)
        {
            if (CurrentToken.Type == ТипЛексемы.AND)
            {
                Match(ТипЛексемы.AND);
            }
            else
            {
                Match(ТипЛексемы.OR);
            }

            ParseRelationalExpression();
        }
    }

    private void ParseRelationalExpression()
    {
        ParseOperand();

        if (CurrentToken.Type == ТипЛексемы.Сравнение)
        {
            Match(ТипЛексемы.Сравнение);
            ParseOperand();
        }
    }

    private void ParseOperand()
    {
        if (CurrentToken.Type == ТипЛексемы.ID)
        {
            Match(ТипЛексемы.ID);
        }
        else if (CurrentToken.Type == ТипЛексемы.Константа)
        {
            Match(ТипЛексемы.Константа);
        }
        else
        {
            throw new Exception($"Ошибка: ожидался операнд индентификатор или константа. В позиции {_currentTokenIndex}");
        }
    }
    public void ParseWhileLoop()
    {
        if (CurrentToken.Type == ТипЛексемы.DO)
        {
            Match(ТипЛексемы.DO);
            Match(ТипЛексемы.While);
            ParseLogicalExpression();

            ParseStatement();

            Match(ТипЛексемы.LOOP);
        }
        else
        {
            throw new Exception($"Ошибка: ожидался оператор do. В позиции {_currentTokenIndex}");
        }
    }

    public void ParseStatement()
    {
        if (CurrentToken.Type == ТипЛексемы.ID)
        {
            ParseAssignment();
        }
        else if (CurrentToken.Type == ТипЛексемы.OPENBRACE)
        {
            Match(ТипЛексемы.OPENBRACE);
            while (HasMoreTokens() && CurrentToken.Type != ТипЛексемы.CLOSEBRACE)
            {
                ParseStatement();
            }
            Match(ТипЛексемы.CLOSEBRACE);
        }
        else
        {
            throw new Exception($"Ошибка: неизвестный оператор {CurrentToken.Value}. В позиции {_currentTokenIndex}");
        }
    }
}
public class Token
{
    public ТипЛексемы Type { get; }
    public string Value { get; }

    public Token(ТипЛексемы type, string value)
    {
        Type = type;
        Value = value;
    }

    public override string ToString()
    {
        return $"{Type} ({Value})";
    }
}
public class Lexer
{
    private readonly string _input;
    private int _position;

    public Lexer(string input)
    {
        _input = input.ToLower();
        _position = 0;
    }

    public List<Token> Tokenize()
    {
        var tokens = new List<Token>();

        while (_position < _input.Length)
        {
            var currentChar = _input[_position];

            if (char.IsWhiteSpace(currentChar))
            {
                _position++;
                continue;
            }

            if (char.IsLetter(currentChar))
            {
                var identifier = ReadIdentifier();
                if (identifier == "do")
                {
                    tokens.Add(new Token(ТипЛексемы.DO, identifier));
                }
                else if (identifier == "while")
                {
                    tokens.Add(new Token(ТипЛексемы.While, identifier));
                }
                else if (identifier == "and")
                {
                    tokens.Add(new Token(ТипЛексемы.AND, identifier));
                }
                else if (identifier == "or")
                {
                    tokens.Add(new Token(ТипЛексемы.OR, identifier));
                }
                else if (identifier == "loop")
                {
                    tokens.Add(new Token(ТипЛексемы.LOOP, identifier));
                }
                else
                {
                    tokens.Add(new Token(ТипЛексемы.ID, identifier));
                }
                continue;
            }

            if (char.IsDigit(currentChar))
            {
                var number = ReadNumber();
                tokens.Add(new Token(ТипЛексемы.Константа, number));
                continue;
            }

            switch (currentChar)
            {
                case '=':
                    if (_position + 1 < _input.Length && _input[_position + 1] == '=')
                    {
                        tokens.Add(new Token(ТипЛексемы.Сравнение, "=="));
                        _position += 2;
                    }
                    else
                    {
                        tokens.Add(new Token(ТипЛексемы.Присваивание, "="));
                        _position++;
                    }
                    break;
                case '!':
                    if (_position + 1 < _input.Length && _input[_position + 1] == '=')
                    {
                        tokens.Add(new Token(ТипЛексемы.Сравнение, "!="));
                        _position += 2;
                    }
                    break;
                case '<':
                    if (_position + 1 < _input.Length && _input[_position + 1] == '=')
                    {
                        tokens.Add(new Token(ТипЛексемы.Сравнение, "<="));
                        _position += 2;
                    }
                    else
                    {
                        tokens.Add(new Token(ТипЛексемы.Сравнение, "<"));
                        _position++;
                    }
                    break;
                case '>':
                    if (_position + 1 < _input.Length && _input[_position + 1] == '=')
                    {
                        tokens.Add(new Token(ТипЛексемы.Сравнение, ">="));
                        _position += 2;
                    }
                    else
                    {
                        tokens.Add(new Token(ТипЛексемы.Сравнение, ">"));
                        _position++;
                    }
                    break;
                case '+':
                    tokens.Add(new Token(ТипЛексемы.Плюс, "+"));
                    _position++;
                    break;
                case '-':
                    tokens.Add(new Token(ТипЛексемы.Плюс, "-"));
                    _position++;
                    break;
                case '*':
                    tokens.Add(new Token(ТипЛексемы.Умножить, "*"));
                    _position++;
                    break;
                case '(':
                    tokens.Add(new Token(ТипЛексемы.OPENPAREN, "("));
                    _position++;
                    break;
                case ')':
                    tokens.Add(new Token(ТипЛексемы.CLOSEPAREN, ")"));
                    _position++;
                    break;
                case '{':
                    tokens.Add(new Token(ТипЛексемы.OPENBRACE, "{"));
                    _position++;
                    break;
                case '}':
                    tokens.Add(new Token(ТипЛексемы.CLOSEBRACE, "}"));
                    _position++;
                    break;
                case ';':
                    tokens.Add(new Token(ТипЛексемы.Разделитель, ";"));
                    _position++;
                    break;
                case '/':
                    tokens.Add(new Token(ТипЛексемы.DIV, "/"));
                    _position++;
                    break;
                default:
                    throw new Exception($"Неизвестный символ: {currentChar}");
            }
        }

        return tokens;
    }

    private string ReadIdentifier()
    {
        var start = _position;
        while (_position < _input.Length && char.IsLetterOrDigit(_input[_position]))
        {
            _position++;
        }

        return _input.Substring(start, _position - start);
    }

    private string ReadNumber()
    {
        var start = _position;
        while (_position < _input.Length && char.IsDigit(_input[_position]))
        {
            _position++;
        }

        return _input.Substring(start, _position - start);
    }
}


public class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Введите выражение или exit для выхода:");
            string input = Console.ReadLine();

            if (input.ToLower() == "exit")
            {
                Console.WriteLine();
                break;
            }
            try
            {
                var lexer = new Lexer(input);
                var tokens = lexer.Tokenize();

                var parser = new Parser(tokens);

                parser.ParseWhileLoop();
                Console.WriteLine("Анализ успешно завершен.");
            }
            catch (Exception ex)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            Console.WriteLine();
        }
    }
}
