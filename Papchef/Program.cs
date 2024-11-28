namespace Papshef; 
public class ТаблицаСимволов
{
    public static List<Tuple<int, string, string, ТипЛексемы, string>> ТаблицаЗаданныхСимволов()
    {
        return new List<Tuple<int, string, string, ТипЛексемы, string>>
        {
            new Tuple<int, string, string, ТипЛексемы, string>(0, "do", "Ключевое слово", ТипЛексемы.Do, "Стартовое слово"),
            new Tuple<int, string, string, ТипЛексемы, string>(1, "while", "Ключевое слово", ТипЛексемы.While, "Начало заголовка цикла"),
            new Tuple< int, string, string, ТипЛексемы, string >(2, "loop", "Ключевое слово", ТипЛексемы.Loop, "Конец тела цикла"),
            new Tuple< int, string, string, ТипЛексемы, string >(3, "and", "Ключевое слово", ТипЛексемы.And, "Логическая операция 'И'"),
            new Tuple< int, string, string, ТипЛексемы, string >(4, "or", "Ключевое слово", ТипЛексемы.Or, "Логическая операция 'ИЛИ'"),
            new Tuple< int, string, string, ТипЛексемы, string >(5, ";", "Ключевое слово", ТипЛексемы.Разделитель, "Конец предусловия цикла"),
            new Tuple< int, string, string, ТипЛексемы, string >(6, "<", "Специальный символ", ТипЛексемы.Сравнение, "Операция сравнения 'меньше'"),
            new Tuple< int, string, string, ТипЛексемы, string >(7, "<=", "Специальный символ", ТипЛексемы.Сравнение, "Операция сравнения 'меньше или равно'"),
            new Tuple< int, string, string, ТипЛексемы, string >(8, "!=", "Специальный символ", ТипЛексемы.Сравнение, "Операция сравнения 'неравно'"),
            new Tuple< int, string, string, ТипЛексемы, string >(9, "==", "Специальный символ", ТипЛексемы.Сравнение, "Операция сравнения 'равно'"),
            new Tuple< int, string, string, ТипЛексемы, string >(10, ">", "Специальный символ", ТипЛексемы.Сравнение, "Операция сравнения 'больше'"),
            new Tuple< int, string, string, ТипЛексемы, string >(11, ">=", "Специальный символ", ТипЛексемы.Сравнение, "Операция сравнения 'больше или равно'"),
            new Tuple< int, string, string, ТипЛексемы, string >(12, "=", "Специальный символ", ТипЛексемы.Присваивание, "Операция присваивания"),
            new Tuple< int, string, string, ТипЛексемы, string >(13, "+", "Специальный символ", ТипЛексемы.АрифметическийОператор, "Операция сложения"),
            new Tuple< int, string, string, ТипЛексемы, string >(14, "-", "Специальный символ", ТипЛексемы.АрифметическийОператор, "Операция вычитания"),
            new Tuple< int, string, string, ТипЛексемы, string >(15, "*", "Специальный символ", ТипЛексемы.АрифметическийОператор, "Операция умножения"),
            new Tuple< int, string, string, ТипЛексемы, string >(16, "/", "Специальный символ", ТипЛексемы.АрифметическийОператор, "Операция деления"),
        };
    }
    public static void ВывестиТаблицу()
    {
        Console.WriteLine("{0,-6} | {1,-7} | {2,-20} | {3,-25} | {4,-40} |", "Индекс", "Символ", "Категория", "Тип", "Комментарий");
        foreach (var символ in ТаблицаЗаданныхСимволов())
        {
            Console.WriteLine("{0,-6} | {1,-7} | {2,-20} | {3,-25} | {4,-40} |",
                              символ.Item1, символ.Item2, символ.Item3, символ.Item4, символ.Item5);
        }
    }
}
public class Лексема
{
    public string Value { get; }
    public ТипЛексемы Type { get; }
    public int Position { get; }

    public Лексема(string value, ТипЛексемы type, int position)
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
                лексемы.Add(new Лексема(Константа, ТипЛексемы.Константа, _позиция));
            }
            else if (ТекущийСимвол() == ';')
            {
                var символ = ReadWhile(c => c == ';');
                лексемы.Add(new Лексема(символ, ТипЛексемы.Разделитель, _позиция));
            }
            else if (ТекущийСимвол() == '<' || ТекущийСимвол() == '=' || ТекущийСимвол() == '>' || ТекущийСимвол() == '!')
            {
                var символ = ReadWhile(c => c == '<' || c == '=' || c == '!' || c == '>');

                var операторыСравнения = ТаблицаСимволов.ТаблицаЗаданныхСимволов().GroupBy(i => i.Item4).FirstOrDefault(i => i.Key == ТипЛексемы.Сравнение).Select(i => i.Item2).ToList();
                if (символ == "=")
                {
                    лексемы.Add(new Лексема(символ, ТипЛексемы.Присваивание, _позиция));
                }
                else if (операторыСравнения.Contains(символ))
                {
                    лексемы.Add(new Лексема(символ, ТипЛексемы.Сравнение, _позиция));
                }
                else
                {
                    лексемы.Add(new Лексема(символ, ТипЛексемы.Неизвестно, _позиция));
                }
            }
            else if (ТекущийСимвол() == '+' || ТекущийСимвол() == '-' || ТекущийСимвол() == '/' || ТекущийСимвол() == '*')
            {
                var символ = ReadWhile(c => c == '+' || c == '-' || c == '/' || c == '*');
                var арифметическиеСравнения = ТаблицаСимволов.ТаблицаЗаданныхСимволов().GroupBy(i => i.Item4).FirstOrDefault(i => i.Key == ТипЛексемы.АрифметическийОператор).Select(i => i.Item2).ToList();
                if (арифметическиеСравнения.Contains(символ))
                {
                    лексемы.Add(new Лексема(символ, ТипЛексемы.АрифметическийОператор, _позиция));
                }
                else
                {
                    лексемы.Add(new Лексема(символ, ТипЛексемы.Неизвестно, _позиция));
                }
            }
            else
            {
                лексемы.Add(new Лексема(ТекущийСимвол().ToString(), ТипЛексемы.Неизвестно, _позиция));
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

    private ТипЛексемы IdentifyKeyword(string value)
    {
        switch (value)
        {
            case "while": return ТипЛексемы.While;
            case "do": return ТипЛексемы.Do;
            case "loop": return ТипЛексемы.Loop;
            case "and": return ТипЛексемы.And;
            case "or": return ТипЛексемы.Or;
            default: return ТипЛексемы.Идентификатор;
        }
    }
}
public enum ТипЛексемы
{
    While,
    Do,
    Loop,
    And,
    Or,
    Разделитель,
    Сравнение,
    АрифметическийОператор,
    Присваивание,
    Идентификатор,
    Константа,
    Неизвестно
}

public class Program
{
    static void Main(string[] args)
    {
        ТаблицаСимволов.ВывестиТаблицу();
        Console.WriteLine();

        Console.ResetColor();
        while (true)
        {
            Console.WriteLine("Введите строку для анализа или exit для завершения программы или ter для показа таблицы терминала:");
            string input = Console.ReadLine();
            Console.WriteLine();

            if (input == "exit")
            {
                break;
            }
            else if (input == "ter")
            {
                ТаблицаСимволов.ВывестиТаблицу();
            }
            else
            {
                var лексемы = new ЛексическийАнализ(input).Анализ();

                Console.WriteLine("{0,-25} | {1,-8} | {2,-8} |", "Тип", "Значение", "Позиция");
                foreach (var лексема in лексемы)
                {
                        Console.WriteLine("{0,-25} | {1,-8} | {2,-8} |",
                                          лексема.Type, лексема.Value, лексема.Position);
                }

                Console.BackgroundColor = ConsoleColor.Red;
                if (лексемы.Any(i => i.Type == ТипЛексемы.Неизвестно))
                {
                    Console.WriteLine("Ошибка : Неизвестные токены :");
                    foreach (var err in лексемы.Where(i => i.Type == ТипЛексемы.Неизвестно))
                    {
                        Console.WriteLine("{0,-8} на позиции {1,-8}", err.Value, err.Position);
                    }
                }
            }
            Console.ResetColor();
        }
    }
}
