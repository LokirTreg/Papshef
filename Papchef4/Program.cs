namespace Papchef4;
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
public class ExecutionStack
{
    private Stack<int> _stack;

    public ExecutionStack()
    {
        _stack = new Stack<int>();
    }

    public int PopVal()
    {
        if (_stack.Count == 0)
            throw new InvalidOperationException("Ошибка: попытка извлечь значение из пустого стека.");
        return _stack.Pop();
    }

    public void PushVal(int value)
    {
        _stack.Push(value);
    }

    public void PrintStack()
    {
        Console.WriteLine("Текущее состояние стека: ");
        foreach (var item in _stack)
        {
            Console.WriteLine(item);
        }
    }
}
public class PostfixForm
{
    private List<PostfixEntry> _postfix;
    private ExecutionStack _stack;
    private Dictionary<string, int> _variables;
    private Dictionary<int, string> _varNames = new Dictionary<int, string>();

    public PostfixForm()
    {
        _postfix = new List<PostfixEntry>();
        _stack = new ExecutionStack();
        _variables = new Dictionary<string, int>();
    }

    public void Interpret()
    {
        int pos = 0;
        while (pos < _postfix.Count)
        {
            var entry = _postfix[pos];
            switch (entry.Type)
            {
                case EEntryType.etCmd:
                    var cmd = (ECmd)entry.Index;
                    pos = ExecuteCommand(cmd, pos);
                    break;
                case EEntryType.etVar:
                    _stack.PushVal(GetVarValue(entry.Index));
                    pos++;
                    break;
                case EEntryType.etConst:
                    _stack.PushVal(entry.Index);
                    pos++;
                    break;
                case EEntryType.etCmdPtr:
                    int condition = _stack.PopVal();
                    if (condition == 0)
                    {
                        pos = entry.Index;
                    }
                    else
                    {
                        pos = pos + 1;
                        // TODO возвращаемся в начало цикла, хардкод
                        _stack.PushVal(0);
                    }
                    break;
                default:
                    throw new Exception($"Неизвестный тип записи: {entry.Type}");
            }
        }
    }

    public int ExecuteCommand(ECmd cmd, int pos)
    {
        switch (cmd)
        {
            case ECmd.SET:
                int value = _stack.PopVal();
                int varHash = _postfix[++pos].Index;
                SetVarValue(varHash, value);
                return pos + 1;

            case ECmd.ADD:
                _stack.PushVal(_stack.PopVal() + _stack.PopVal());
                return pos + 1;

            case ECmd.SUB:
                int subtrahend = _stack.PopVal();
                _stack.PushVal(_stack.PopVal() - subtrahend);
                return pos + 1;

            case ECmd.MUL:
                _stack.PushVal(_stack.PopVal() * _stack.PopVal());
                return pos + 1;

            case ECmd.DIV:
                int divisor = _stack.PopVal();
                if (divisor == 0)
                    throw new DivideByZeroException("Ошибка: деление на ноль.");
                _stack.PushVal(_stack.PopVal() / divisor);
                return pos + 1;

            case ECmd.CMPL:
                _stack.PushVal(_stack.PopVal() > _stack.PopVal() ? 1 : 0);
                return pos + 1;

            case ECmd.CMPLE:
                _stack.PushVal(_stack.PopVal() >= _stack.PopVal() ? 1 : 0);
                return pos + 1;

            case ECmd.CMPG:
                _stack.PushVal(_stack.PopVal() < _stack.PopVal() ? 1 : 0);
                return pos + 1;

            case ECmd.CMPGE:
                _stack.PushVal(_stack.PopVal() <= _stack.PopVal() ? 1 : 0);
                return pos + 1;

            case ECmd.CMPE:
                _stack.PushVal(_stack.PopVal() == _stack.PopVal() ? 1 : 0);
                return pos + 1;

            case ECmd.AND:
                _stack.PushVal((_stack.PopVal() != 0 && _stack.PopVal() != 0) ? 1 : 0);
                return pos + 1;

            case ECmd.OR:
                _stack.PushVal((_stack.PopVal() != 0 || _stack.PopVal() != 0) ? 1 : 0);
                return pos + 1;

            case ECmd.JMP:
                return _stack.PopVal();

            case ECmd.JZ:
                int address = _stack.PopVal();
                int condition = _stack.PopVal();
                return condition == 0 ? address : pos + 1;

            default:
                throw new Exception($"Неизвестная команда: {cmd}");
        }
    }

    private int GetVarValue(int varHash)
    {
        string varKey = varHash.ToString();

        if (!_variables.ContainsKey(varKey))
        {
            Console.WriteLine($"Инициализация переменной: {varKey} со значением по умолчанию 0.");
            _variables[varKey] = 0;
        }

        return _variables[varKey];
    }


    private void SetVarValue(int varHash, int value)
    {
        string varKey = varHash.ToString();
        _variables[varKey] = value;
    }

    public int WriteCmd(ECmd cmd)
    {
        _postfix.Add(new PostfixEntry(EEntryType.etCmd, (int)cmd));
        return _postfix.Count - 1;
    }

    public int PushVar(string varName)
    {
        int index = varName.GetHashCode();
        _postfix.Add(new PostfixEntry(EEntryType.etVar, index));

        if (!_varNames.ContainsKey(index))
        {
            _varNames.Add(index, varName);
        }

        return _postfix.Count - 1;
    }

    public int PushConst(int value)
    {
        _postfix.Add(new PostfixEntry(EEntryType.etConst, value));
        return _postfix.Count - 1;
    }

    public int WriteCmdPtr(int ptr)
    {
        _postfix.Add(new PostfixEntry(EEntryType.etCmdPtr, ptr));
        return _postfix.Count - 1;
    }

    public void SetCmdPtr(int index, int ptr)
    {
        _postfix[index] = new PostfixEntry(EEntryType.etCmdPtr, ptr);
    }

    public int GetCurrentAddress()
    {
        return _postfix.Count - 1;
    }

    public void PrintPostfix()
    {
        Console.WriteLine("ПОЛИЗ:");
        foreach (var entry in _postfix)
        {
            string entryDescription;
            switch (entry.Type)
            {
                case EEntryType.etCmd:
                    entryDescription = $"Команда: {((ECmd)entry.Index)}";
                    break;
                case EEntryType.etVar:
                    entryDescription = $"Переменная (хэш): {entry.Index}";
                    break;
                case EEntryType.etConst:
                    entryDescription = $"Константа: {entry.Index}";
                    break;
                case EEntryType.etCmdPtr:
                    entryDescription = $"Указатель команды на адрес: {entry.Index}";
                    break;
                default:
                    entryDescription = "Неизвестный тип записи";
                    break;
            }
            Console.WriteLine(entryDescription);
        }
    }

    public void SetVarAndPop(string varName)
    {
        int varHash = varName.GetHashCode();
        int value = _stack.PopVal();
        SetVarValue(varHash, value);
    }

    public void PrintVariables()
    {
        Console.WriteLine("Значения переменных:");
        foreach (var kvp in _variables)
        {
            int varHash = int.Parse(kvp.Key);
            int value = kvp.Value;

            if (_varNames.ContainsKey(varHash))
            {
                string varName = _varNames[varHash];
                Console.WriteLine($"{varName} = {value}");
            }
            else
            {
                Console.WriteLine($"Переменная с хэшом {varHash} = {value}");
            }
        }
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
    public void ParseDoWhile()
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
    public PostfixForm GetPostfixForm()
    {
        return _postfix;
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
                parser.ParseDoWhile();

                Console.WriteLine("Сгенерированный ПОЛИЗ:");
                parser.PrintPostfix();
                Console.WriteLine("Начало интерпретации ПОЛИЗа:");
                var postfix = parser.GetPostfixForm();
                postfix.Interpret();
                postfix.PrintVariables();
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
