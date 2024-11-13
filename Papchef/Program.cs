public class TerminalSymbol
{
    public int Index { get; set; }
    public string Symbol { get; set; }
    public string Category { get; set; }
    public string Type { get; set; }
    public string Comment { get; set; }
}

public class TerminalSymbolTable
{
    public static List<TerminalSymbol> GetTable()
    {
        return new List<TerminalSymbol>
        {
            new TerminalSymbol { Index = 0, Symbol = "do", Category = "Ключевое слово", Type = "КлючевоеСлово_Do", Comment = "Стартовое слово" },
            new TerminalSymbol { Index = 1, Symbol = "while", Category = "Ключевое слово", Type = "КлючевоеСлово_While", Comment = "Начало заголовка цикла" },
            new TerminalSymbol { Index = 2, Symbol = "loop", Category = "Ключевое слово", Type = "КлючевоеСлово_End", Comment = "Конец тела цикла" },
            new TerminalSymbol { Index = 3, Symbol = "and", Category = "Ключевое слово", Type = "КлючевоеСлово_And", Comment = "Логическая операция 'И'" },
            new TerminalSymbol { Index = 4, Symbol = "or", Category = "Ключевое слово", Type = "КлючевоеСлово_Or", Comment = "Логическая операция 'ИЛИ'" },
            new TerminalSymbol { Index = 5, Symbol = ";", Category = "Ключевое слово", Type = "КонецПредусловияЦикла", Comment = "Конец предусловия цикла" },
            new TerminalSymbol { Index = 6, Symbol = "<", Category = "Специальный символ", Type = "ОператорСравнения", Comment = "Операция сравнения 'меньше'" },
            new TerminalSymbol { Index = 7, Symbol = "<=", Category = "Специальный символ", Type = "ОператорСравнения", Comment = "Операция сравнения 'меньше или равно'" },
            new TerminalSymbol { Index = 8, Symbol = "!=", Category = "Специальный символ", Type = "ОператорСравнения", Comment = "Операция сравнения 'неравно'" },
            new TerminalSymbol { Index = 9, Symbol = "==", Category = "Специальный символ", Type = "ОператорСравнения", Comment = "Операция сравнения 'равно'" },
            new TerminalSymbol { Index = 10, Symbol = "=", Category = "Специальный символ", Type = "ОператорПрисваивания", Comment = "Операция присваивания" },
            new TerminalSymbol { Index = 11, Symbol = "+", Category = "Специальный символ", Type = "АрифметическийОператор", Comment = "Операция сложения" },
            new TerminalSymbol { Index = 12, Symbol = "-", Category = "Специальный символ", Type = "АрифметическийОператор", Comment = "Операция вычитания" },
            new TerminalSymbol { Index = 13, Symbol = "*", Category = "Специальный символ", Type = "АрифметическийОператор", Comment = "Операция умножения" },
            new TerminalSymbol { Index = 14, Symbol = "/", Category = "Специальный символ", Type = "АрифметическийОператор", Comment = "Операция деления" },
            new TerminalSymbol { Index = 15, Symbol = ">", Category = "Специальный символ", Type = "ОператорСравнения", Comment = "Операция сравнения 'больше'" },
            new TerminalSymbol { Index = 16, Symbol = ">=", Category = "Специальный символ", Type = "ОператорСравнения", Comment = "Операция сравнения 'больше или равно'" }
        };
    }

    public static void PrintTable()
    {
        var table = GetTable();
        Console.WriteLine("{0,-5} {1,-10} {2,-20} {3,-25} {4,-40}", "Индекс", "Символ", "Категория", "Тип", "Комментарий");
        foreach (var symbol in table)
        {
            Console.WriteLine("{0,-5} {1,-10} {2,-20} {3,-25} {4,-40}",
                              symbol.Index, symbol.Symbol, symbol.Category, symbol.Type, symbol.Comment);
        }
    }
}
public class Lexem
{
    public string Value { get; }
    public LexemType Type { get; }
    public int Position { get; }

    public Lexem(string value, LexemType type, int position)
    {
        Value = value;
        Type = type;
        Position = position;
    }
}
public class Lexer
{
    private readonly string _input;
    private int _position;

    public Lexer(string input)
    {
        _input = input;
        _position = 0;
    }

    public List<Lexem> Analyze()
    {
        var tokens = new List<Lexem>();
        while (_position < _input.Length)
        {
            if (char.IsWhiteSpace(CurrentChar()))
            {
                _position++;
            }
            else if (IsLetter(CurrentChar()))
            {
                var Идентификатор = ReadWhile(IsLetterOrDigit);
                tokens.Add(new Lexem(Идентификатор, IdentifyKeyword(Идентификатор), _position));
            }
            else if (char.IsDigit(CurrentChar()))
            {
                var Константа = ReadWhile(char.IsDigit);
                tokens.Add(new Lexem(Константа, LexemType.Константа, _position));
            }
            else if (CurrentChar() == ';')
            {
                var symbol = ReadWhile(c => c == ';');
                tokens.Add(new Lexem(symbol, LexemType.КонецПредусловияЦикла, _position));
            }
            else if (CurrentChar() == '<' || CurrentChar() == '='|| CurrentChar() == '>' || CurrentChar() == '!')
            {
                var h = CurrentChar();
                var symbol = ReadWhile(c => c == '<' || c == '=' || c == '!'|| c == '>');
                if (symbol == "=")
                {
                    tokens.Add(new Lexem(symbol, LexemType.ОператорПрисваивания, _position));
                }
                else
                {
                    tokens.Add(new Lexem(symbol, LexemType.ОператорСравнения, _position));
                }
            }
            else if (CurrentChar() == '+' || CurrentChar() == '-' || CurrentChar() == '/' || CurrentChar() == '*')
            {
                var symbol = ReadWhile(c => c == '+' || c == '-' || c == '/' || c == '*');
                tokens.Add(new Lexem(symbol, LexemType.АрифметическийОператор, _position));
            }
            else
            {
                tokens.Add(new Lexem(CurrentChar().ToString(), LexemType.Неизвестно, _position));
                _position++;
            }
        }

        return tokens;
    }

    private char CurrentChar()
    {
        return _input[_position];
    }

    private string ReadWhile(Func<char, bool> condition)
    {
        var start = _position;
        while (_position < _input.Length && condition(_input[_position]))
        {
            _position++;
        }

        return _input.Substring(start, _position - start);
    }

    private bool IsLetter(char ch) => char.IsLetter(ch);

    private bool IsLetterOrDigit(char ch) => char.IsLetterOrDigit(ch);

    private LexemType IdentifyKeyword(string value)
    {
        switch (value)
        {
            case "while": return LexemType.КлючевоеСлово_While;
            case "do": return LexemType.КлючевоеСлово_Do;
            case "loop": return LexemType.КлючевоеСлово_End;
            case "and": return LexemType.КлючевоеСлово_And;
            case "or": return LexemType.КлючевоеСлово_Or;
            default: return LexemType.Идентификатор;
        }
    }
}
public enum LexemType
{
    КлючевоеСлово_While,
    КлючевоеСлово_Do,
    КлючевоеСлово_End,
    КлючевоеСлово_And,
    КлючевоеСлово_Or,
    КонецПредусловияЦикла,
    ОператорСравнения,
    АрифметическийОператор,
    ОператорПрисваивания,
    Идентификатор,
    Константа,
    Неизвестно
}

public class Program
{
    static void Main(string[] args)
    {
        TerminalSymbolTable.PrintTable();
        Console.WriteLine();

        while (true)
        {
            Console.WriteLine("Введите строку для анализа или exit для завершения программы или menu для показа таблицы терминала:");
            string input = Console.ReadLine();
            Console.WriteLine();

            if (input.ToLower() == "exit")
            {
                break;
            }
            else if (input.ToLower() == "menu")
            {
                TerminalSymbolTable.PrintTable();
            }
            else
            {
                var lexer = new Lexer(input);
                var tokens = lexer.Analyze();

                foreach (var token in tokens)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    if (token.Type == LexemType.Неизвестно)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.Write($"{token.Type} : ");
                    Console.ResetColor();
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{ token.Value}");
                    Console.ResetColor();
                    Console.Write($" столбец ");
                    Console.WriteLine($"{ token.Position}");
                }
            }
            Console.WriteLine();
        }
    }
}
