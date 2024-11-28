namespace Papchef3;
public enum ТипЛексемы
{
    ID,
    Константа,
    Присваивание,
    Разделитель,
    Плюс,
    Минус,
    Умножить,
    Деление,
    Openparen,
    Closeparen,
    While,
    Colon,
    Do,
    And,
    Or,
    Сравнение,
    Сlosebrace,
    Openbrace,
    Loop
}
public enum EEntryType
{
    etCmd,
    etVar,
    etConst,
    etCmdPtr
}
public enum ECmd
{
    JMP,
    JZ,
    SET,
    ADD,
    SUB,
    And,
    Or,
    CMPE,
    CMPNE,
    CMPL,
    CMPLE,
    MUL,
    DIV
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
                    tokens.Add(new Token(ТипЛексемы.Do, identifier));
                }
                else if (identifier == "while")
                {
                    tokens.Add(new Token(ТипЛексемы.While, identifier));
                }
                else if (identifier == "and")
                {
                    tokens.Add(new Token(ТипЛексемы.And, identifier));
                }
                else if (identifier == "or")
                {
                    tokens.Add(new Token(ТипЛексемы.Or, identifier));
                }
                else if (identifier == "loop")
                {
                    tokens.Add(new Token(ТипЛексемы.Loop, identifier));
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
                    tokens.Add(new Token(ТипЛексемы.Openparen, "("));
                    _position++;
                    break;
                case ')':
                    tokens.Add(new Token(ТипЛексемы.Closeparen, ")"));
                    _position++;
                    break;
                case '{':
                    tokens.Add(new Token(ТипЛексемы.Openbrace, "{"));
                    _position++;
                    break;
                case '}':
                    tokens.Add(new Token(ТипЛексемы.Сlosebrace, "}"));
                    _position++;
                    break;
                case ';':
                    tokens.Add(new Token(ТипЛексемы.Разделитель, ";"));
                    _position++;
                    break;
                case '/':
                    tokens.Add(new Token(ТипЛексемы.Деление, "/"));
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
public class Parser
{
    private readonly List<Token> _tokens;
    private PostfixForm _postfix;
    private int _currentTokenIndex;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
        _currentTokenIndex = 0;
        _postfix = new PostfixForm();
    }

    private Token CurrentToken
    {
        get
        {
            if (HasMOreTokens())
            {
                return _tokens[_currentTokenIndex];
            }
            else
            {
                throw new Exception("Ошибка: неожиданный конец выражения.");
            }
        }
    }

    private bool HasMOreTokens()
    {
        return _currentTokenIndex < _tokens.Count;
    }

    private void Match(ТипЛексемы expectedType)
    {
        if (CurrentToken.Type == expectedType)
        {
            _currentTokenIndex++;
        }
        else
        {
            throw new Exception($"Ошибка: ожидалось {expectedType}, но найдено {CurrentToken.Type}");
        }
    }

    public void ParseAssignment()
    {
        if (CurrentToken.Type == ТипЛексемы.ID)
        {
            _postfix.WriteVar(CurrentToken.Value);
            Match(ТипЛексемы.ID);
            Match(ТипЛексемы.Присваивание);

            ParseExpression();

            _postfix.WriteCmd(ECmd.SET);

            if (CurrentToken.Type == ТипЛексемы.Разделитель)
            {
                Match(ТипЛексемы.Разделитель);
            }
            else
            {
                throw new Exception("Ошибка: Ожидалась ';' после присваивания.");
            }
        }
    }

    private void ParseExpression()
    {
        ParseTerm();
        while (CurrentToken.Type == ТипЛексемы.Плюс || CurrentToken.Type == ТипЛексемы.Минус)
        {
            if (CurrentToken.Type == ТипЛексемы.Плюс)
            {
                Match(ТипЛексемы.Плюс);
                ParseTerm();
                _postfix.WriteCmd(ECmd.ADD);
            }
            else if (CurrentToken.Type == ТипЛексемы.Минус)
            {
                Match(ТипЛексемы.Минус);
                ParseTerm();
                _postfix.WriteCmd(ECmd.SUB);
            }
        }
    }

    private void ParseTerm()
    {
        ParseFactOr();
        while (CurrentToken.Type == ТипЛексемы.Умножить || CurrentToken.Type == ТипЛексемы.Деление)
        {
            if (CurrentToken.Type == ТипЛексемы.Умножить)
            {
                Match(ТипЛексемы.Умножить);
                ParseFactOr();
                _postfix.WriteCmd(ECmd.MUL);
            }
            else if (CurrentToken.Type == ТипЛексемы.Деление)
            {
                Match(ТипЛексемы.Деление);
                ParseFactOr();
                _postfix.WriteCmd(ECmd.DIV);
            }
        }
    }

    private void ParseFactOr()
    {
        if (CurrentToken.Type == ТипЛексемы.ID)
        {
            _postfix.WriteVar(CurrentToken.Value);
            Match(ТипЛексемы.ID);
        }
        else if (CurrentToken.Type == ТипЛексемы.Константа)
        {
            _postfix.WriteКонстанта(int.Parse(CurrentToken.Value));
            Match(ТипЛексемы.Константа);
        }
        else
        {
            throw new Exception("Ожидалось арифметическое выражение");
        }
    }

    private void ParseLogicalExpression()
    {
        ParseRelationalExpression();

        while (CurrentToken.Type == ТипЛексемы.And || CurrentToken.Type == ТипЛексемы.Or)
        {
            if (CurrentToken.Type == ТипЛексемы.And)
            {
                Match(ТипЛексемы.And);
                ParseRelationalExpression();
                _postfix.WriteCmd(ECmd.And);
            }
            else if (CurrentToken.Type == ТипЛексемы.Or)
            {
                Match(ТипЛексемы.Or);
                ParseRelationalExpression();
                _postfix.WriteCmd(ECmd.Or);
            }
        }
    }

    private void ParseRelationalExpression()
    {
        ParseOperAnd();

        if (CurrentToken.Type == ТипЛексемы.Сравнение)
        {
            string СравнениеOp = CurrentToken.Value;
            Match(ТипЛексемы.Сравнение);

            ParseOperAnd();

            switch (СравнениеOp)
            {
                case ">":
                    _postfix.WriteCmd(ECmd.CMPL);
                    break;
                case "<":
                    _postfix.WriteCmd(ECmd.CMPL);
                    break;
                case ">=":
                    _postfix.WriteCmd(ECmd.CMPLE);
                    break;
                case "<=":
                    _postfix.WriteCmd(ECmd.CMPLE);
                    break;
                case "==":
                    _postfix.WriteCmd(ECmd.CMPE);
                    break;
                case "!=":
                    _postfix.WriteCmd(ECmd.CMPNE);
                    break;
                default:
                    throw new Exception($"Неизвестная операция сравнения: {СравнениеOp}");
            }
        }
    }

    private void ParseOperAnd()
    {
        if (CurrentToken.Type == ТипЛексемы.ID)
        {
            _postfix.WriteVar(CurrentToken.Value);
            Match(ТипЛексемы.ID);
        }
        else if (CurrentToken.Type == ТипЛексемы.Константа)
        {
            _postfix.WriteКонстанта(int.Parse(CurrentToken.Value));
            Match(ТипЛексемы.Константа);
        }
        else
        {
            throw new Exception("Ошибка: ожидался операнд");
        }
    }
    public void DoWhile()
    {
        int startLoopIndex = _postfix.WriteCmdPtr(-1);

        Match(ТипЛексемы.Do);
        Match(ТипЛексемы.While);

        ParseLogicalExpression();

        while (CurrentToken.Type != ТипЛексемы.Loop)
        {
            ParseAssignment();
        }

        Match(ТипЛексемы.Loop);


        int conditionJmpIndex = _postfix.WriteCmd(ECmd.JZ);

        _postfix.WriteCmdPtr(startLoopIndex);
        _postfix.WriteCmd(ECmd.JMP);

        _postfix.SetCmdPtr(conditionJmpIndex, _postfix.GetCurrentAddress() + 1);
    }

    public void PrintPostfix()
    {
        _postfix.PrintPostfix();
    }
    public void PrintPostfixStr()
    {
        _postfix.PrintPostfixStr();
    }
}
public class PostfixEntry
{
    public EEntryType Type { get; set; }
    public int Index { get; set; }
    public string Value { get; set; }

    public PostfixEntry(EEntryType type, int index)
    {
        Type = type;
        Index = index;
    }
    public PostfixEntry(EEntryType type, int index, string value)
    {
        Type = type;
        Index = index;
        Value = value;
    }
}
public class PostfixForm
{
    private List<PostfixEntry> _postfix;

    public PostfixForm()
    {
        _postfix = new List<PostfixEntry>();
    }

    public int WriteCmd(ECmd cmd)
    {
        _postfix.Add(new PostfixEntry(EEntryType.etCmd, (int)cmd));
        return _postfix.Count - 1;
    }

    public int WriteVar(string varName)
    {
        int index = varName.GetHashCode();
        _postfix.Add(new PostfixEntry(EEntryType.etVar, index, varName));
        return _postfix.Count - 1;
    }

    public int WriteКонстанта(int ind)
    {
        _postfix.Add(new PostfixEntry(EEntryType.etConst, ind));
        return _postfix.Count - 1;
    }

    public int WriteCmdPtr(int ptr)
    {
        _postfix.Add(new PostfixEntry(EEntryType.etCmdPtr, ptr));
        return _postfix.Count - 1;
    }

    public void SetCmdPtr(int ind, int ptr)
    {
        _postfix[ind] = new PostfixEntry(EEntryType.etCmdPtr, ptr);
    }

    public int GetCurrentAddress()
    {
        return _postfix.Count - 1;
    }

    public void PrintPostfix()
    {
        foreach (var entry in _postfix)
        {
            string entryDescription = "";

            switch (entry.Type)
            {
                case EEntryType.etCmd:
                    entryDescription = $"Команда: {((ECmd)entry.Index)}";
                    break;
                case EEntryType.etVar:
                    entryDescription = $"Переменная: {entry.Value}";
                    break;
                case EEntryType.etConst:
                    entryDescription = $"Константа: {entry.Index}";
                    break;
                case EEntryType.etCmdPtr:
                    entryDescription = $"Указатель команды на адрес: {entry.Index}";
                    break;
                default:
                    entryDescription = "Неизвестный тип";
                    break;
            }

            Console.WriteLine(entryDescription);
        }
    }
    public void PrintPostfixStr()
    {
        foreach (var entry in _postfix)
        {
            string entryDescription = "";

            switch (entry.Type)
            {
                case EEntryType.etCmd:
                    entryDescription = $"{((ECmd)entry.Index)}";
                    break;
                case EEntryType.etVar:
                    entryDescription = $"{entry.Value}";
                    break;
                case EEntryType.etConst:
                    entryDescription = $"{entry.Index}";
                    break;
                case EEntryType.etCmdPtr:
                    break;
                default:
                    entryDescription = "Неизвестный тип";
                    break;
            }

            Console.Write("{0} ", entryDescription);
        }
    }
}
class Program
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
                List<Token> tokens = lexer.Tokenize();

                var parser = new Parser(tokens);
                parser.DoWhile();

                parser.PrintPostfix();
                Console.WriteLine("Сгенерированный ПОЛИЗ:");
                parser.PrintPostfixStr();
                var t = 0;
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
