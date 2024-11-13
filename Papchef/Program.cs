public class SymbolTable
{
    private static readonly Dictionary<string, int> _идентификаторы = new Dictionary<string, int>();
    private static readonly Dictionary<string, int> _константы = new Dictionary<string, int>();
    public static List<Tuple<string, string, ЛексемаType, string>> ТаблицаЗаданныхСимволов()
    {
        return new List<Tuple<string, string, ЛексемаType, string>>
        {
            new Tuple<string, string, ЛексемаType, string>("do", "Ключевое слово", ЛексемаType.КлючевоеСлово_Do, "Стартовое слово"),
            new Tuple<string, string, ЛексемаType, string>("while", "Ключевое слово", ЛексемаType.КлючевоеСлово_While, "Начало заголовка цикла"),
            new Tuple<string, string, ЛексемаType, string>("loop", "Ключевое слово", ЛексемаType.КлючевоеСлово_End, "Конец тела цикла"),
            new Tuple<string, string, ЛексемаType, string>("and", "Ключевое слово", ЛексемаType.КлючевоеСлово_And, "Логическая операция 'И'"),
            new Tuple<string, string, ЛексемаType, string>("or", "Ключевое слово", ЛексемаType.КлючевоеСлово_Or, "Логическая операция 'ИЛИ'"),
            new Tuple<string, string, ЛексемаType, string>(";", "Ключевое слово", ЛексемаType.КонецПредусловияЦикла, "Конец предусловия цикла"),
            new Tuple<string, string, ЛексемаType, string>("<", "Специальный символ", ЛексемаType.ОператорСравнения, "Операция сравнения 'меньше'"),
            new Tuple<string, string, ЛексемаType, string>("<=", "Специальный символ", ЛексемаType.ОператорСравнения, "Операция сравнения 'меньше или равно'"),
            new Tuple<string, string, ЛексемаType, string>("!=", "Специальный символ", ЛексемаType.ОператорСравнения, "Операция сравнения 'неравно'"),
            new Tuple<string, string, ЛексемаType, string>("==", "Специальный символ", ЛексемаType.ОператорСравнения, "Операция сравнения 'равно'"),
            new Tuple<string, string, ЛексемаType, string>("=", "Специальный символ", ЛексемаType.ОператорПрисваивания, "Операция присваивания"),
            new Tuple<string, string, ЛексемаType, string>("+", "Специальный символ", ЛексемаType.АрифметическийОператор, "Операция сложения"),
            new Tuple<string, string, ЛексемаType, string>("-", "Специальный символ", ЛексемаType.АрифметическийОператор, "Операция вычитания"),
            new Tuple<string, string, ЛексемаType, string>("*", "Специальный символ", ЛексемаType.АрифметическийОператор, "Операция умножения"),
            new Tuple<string, string, ЛексемаType, string>("/", "Специальный символ", ЛексемаType.АрифметическийОператор, "Операция деления"),
            new Tuple<string, string, ЛексемаType, string>(">", "Специальный символ", ЛексемаType.ОператорСравнения, "Операция сравнения 'больше'"),
            new Tuple<string, string, ЛексемаType, string>(">=", "Специальный символ", ЛексемаType.ОператорСравнения, "Операция сравнения 'больше или равно'")
        };
    }

    public static void ДобавитьИдентификатор(string идентификатор)
    {
        if (!_идентификаторы.ContainsKey(идентификатор))
        {
            _идентификаторы[идентификатор] = _идентификаторы.Count + 1;
        }
    }

    public static void ДобавитьКонстанту(string константа)
    {
        if (!_константы.ContainsKey(константа))
        {
            _константы[константа] = _константы.Count + 1;
        }
    }

    public static int ИдентификаторПоНазванию(string идентификатор)
    {
        return _идентификаторы.ContainsKey(идентификатор) ? _идентификаторы[идентификатор] : -1;
    }

    public static int КонстантаПоНазванию(string константа)
    {
        return _константы.ContainsKey(константа) ? _константы[константа] : -1;
    }
    public static void ВывестиТаблицу()
    {
        Console.WriteLine("{0,-7} | {1,-20} | {2,-25} | {3,-40} |", "Символ", "Категория", "Тип", "Комментарий");
        foreach (var символ in ТаблицаЗаданныхСимволов())
        {
            Console.WriteLine("{0,-7} | {1,-20} | {2,-25} | {3,-40} |",
                              символ.Item1, символ.Item2, символ.Item3, символ.Item4);
        }
    }
    public static void ВывестиКонстанты()
    {
        foreach(var константа in _константы)
        {
            Console.WriteLine($"{константа.Key} = {константа.Value}");
        }
    }
    public static void ВывестиИдентификаторы()
    {
        foreach(var идентификатор in _идентификаторы)
        {
            Console.WriteLine($"{идентификатор.Key} = {идентификатор.Value}");
        }
    }
}
public class Лексема
{
    public string Value { get; }
    public ЛексемаType Type { get; }
    public int Position { get; }

    public Лексема(string value, ЛексемаType type, int position)
    {
        Value = value;
        Type = type;
        Position = position;
    }
}
public class ЛексическийАнализ
{
    private readonly string _входнаяСтрока;
    private int _позиция;

    public ЛексическийАнализ(string input)
    {
        _входнаяСтрока = input;
        _позиция = 0;
    }

    public List<Лексема> Анализ()
    {
        var лексемы = new List<Лексема>();
        while (_позиция < _входнаяСтрока.Length)
        {
            if (char.IsWhiteSpace(ТекущийСимвол()))
            {
                _позиция++;
            }
            else if (IsLetter(ТекущийСимвол()))
            {
                var Идентификатор = ReadWhile(IsLetterOrDigit);
                лексемы.Add(new Лексема(Идентификатор, IdentifyKeyword(Идентификатор), _позиция));
            }
            else if (char.IsDigit(ТекущийСимвол()))
            {
                var Константа = ReadWhile(char.IsDigit);
                лексемы.Add(new Лексема(Константа, ЛексемаType.Константа, _позиция));
            }
            else if (ТекущийСимвол() == ';')
            {
                var символ = ReadWhile(c => c == ';');
                лексемы.Add(new Лексема(символ, ЛексемаType.КонецПредусловияЦикла, _позиция));
            }
            else if (ТекущийСимвол() == '<' || ТекущийСимвол() == '='|| ТекущийСимвол() == '>' || ТекущийСимвол() == '!')
            {
                var символ = ReadWhile(c => c == '<' || c == '=' || c == '!'|| c == '>');

                var операторыСравнения = SymbolTable.ТаблицаЗаданныхСимволов().GroupBy(i => i.Item3).FirstOrDefault(i => i.Key == ЛексемаType.ОператорСравнения).Select(i => i.Item1).ToList();
                if (символ == "=")
                {
                    лексемы.Add(new Лексема(символ, ЛексемаType.ОператорПрисваивания, _позиция));
                }else if(операторыСравнения.Contains(символ)){
                    лексемы.Add(new Лексема(символ, ЛексемаType.ОператорСравнения, _позиция));
                }
                else
                {
                    лексемы.Add(new Лексема(символ, ЛексемаType.Неизвестно, _позиция));
                }
            }
            else if (ТекущийСимвол() == '+' || ТекущийСимвол() == '-' || ТекущийСимвол() == '/' || ТекущийСимвол() == '*')
            {
                var символ = ReadWhile(c => c == '+' || c == '-' || c == '/' || c == '*'); 
                var арифметическиеСравнения = SymbolTable.ТаблицаЗаданныхСимволов().GroupBy(i => i.Item3).FirstOrDefault(i => i.Key == ЛексемаType.АрифметическийОператор).Select(i => i.Item1).ToList();
                if (арифметическиеСравнения.Contains(символ))
                {
                    лексемы.Add(new Лексема(символ, ЛексемаType.АрифметическийОператор, _позиция));
                }
                else
                {
                    лексемы.Add(new Лексема(символ, ЛексемаType.Неизвестно, _позиция));
                }
            }
            else
            {
                лексемы.Add(new Лексема(ТекущийСимвол().ToString(), ЛексемаType.Неизвестно, _позиция));
                _позиция++;
            }
        }

        return лексемы;
    }

    private char ТекущийСимвол()
    {
        return _входнаяСтрока[_позиция];
    }

    private string ReadWhile(Func<char, bool> condition)
    {
        var start = _позиция;
        while (_позиция < _входнаяСтрока.Length && condition(_входнаяСтрока[_позиция]))
        {
            _позиция++;
        }

        return _входнаяСтрока.Substring(start, _позиция - start);
    }

    private bool IsLetter(char ch) => char.IsLetter(ch);

    private bool IsLetterOrDigit(char ch) => char.IsLetterOrDigit(ch);

    private ЛексемаType IdentifyKeyword(string value)
    {
        switch (value)
        {
            case "while": return ЛексемаType.КлючевоеСлово_While;
            case "do": return ЛексемаType.КлючевоеСлово_Do;
            case "loop": return ЛексемаType.КлючевоеСлово_End;
            case "and": return ЛексемаType.КлючевоеСлово_And;
            case "or": return ЛексемаType.КлючевоеСлово_Or;
            default: return ЛексемаType.Идентификатор;
        }
    }
}
public enum ЛексемаType
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
        SymbolTable.ВывестиТаблицу();
        Console.WriteLine();

        while (true)
        {
            Console.WriteLine("Введите строку для анализа или exit для завершения программы или menu для показа таблицы терминала:");
            string input = Console.ReadLine();
            Console.WriteLine();

            if (input == "exit")
            {
                break;
            }
            else if (input == "ter")
            {
                SymbolTable.ВывестиТаблицу();
            }
            else
            {
                var лексемы = new ЛексическийАнализ(input).Анализ();

                foreach (var лексема in лексемы)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    if (лексема.Type == ЛексемаType.Неизвестно)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.Write($"{лексема.Type} : ");
                    Console.ResetColor();
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{ лексема.Value}");
                    Console.ResetColor();
                    Console.Write($" столбец ");
                    Console.WriteLine($"{ лексема.Position}");
                }
            }
            Console.WriteLine();
        }
    }
}
