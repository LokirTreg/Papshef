namespace Papshef; 
public class ТаблицаСимволов
{
    public static List<Tuple<string, string, ТипЛексемы, string>> ТаблицаЗаданныхСимволов()
    {
        return new List<Tuple<string, string, ТипЛексемы, string>>
        {
            new Tuple<string, string, ТипЛексемы, string>("do", "Ключевое слово", ТипЛексемы.КлючевоеСлово_Do, "Стартовое слово"),
            new Tuple<string, string, ТипЛексемы, string>("while", "Ключевое слово", ТипЛексемы.КлючевоеСлово_Until, "Начало заголовка цикла"),
            new Tuple<string, string, ТипЛексемы, string>("loop", "Ключевое слово", ТипЛексемы.КлючевоеСлово_End, "Конец тела цикла"),
            new Tuple<string, string, ТипЛексемы, string>("and", "Ключевое слово", ТипЛексемы.КлючевоеСлово_And, "Логическая операция 'И'"),
            new Tuple<string, string, ТипЛексемы, string>("or", "Ключевое слово", ТипЛексемы.КлючевоеСлово_Or, "Логическая операция 'ИЛИ'"),
            new Tuple<string, string, ТипЛексемы, string>(";", "Ключевое слово", ТипЛексемы.КонецПредусловияЦикла, "Конец предусловия цикла"),
            new Tuple<string, string, ТипЛексемы, string>("<", "Специальный символ", ТипЛексемы.ОператорСравнения, "Операция сравнения 'меньше'"),
            new Tuple<string, string, ТипЛексемы, string>("<=", "Специальный символ", ТипЛексемы.ОператорСравнения, "Операция сравнения 'меньше или равно'"),
            new Tuple<string, string, ТипЛексемы, string>("!=", "Специальный символ", ТипЛексемы.ОператорСравнения, "Операция сравнения 'неравно'"),
            new Tuple<string, string, ТипЛексемы, string>("==", "Специальный символ", ТипЛексемы.ОператорСравнения, "Операция сравнения 'равно'"),
            new Tuple<string, string, ТипЛексемы, string>("=", "Специальный символ", ТипЛексемы.ОператорПрисваивания, "Операция присваивания"),
            new Tuple<string, string, ТипЛексемы, string>("+", "Специальный символ", ТипЛексемы.АрифметическийОператор, "Операция сложения"),
            new Tuple<string, string, ТипЛексемы, string>("-", "Специальный символ", ТипЛексемы.АрифметическийОператор, "Операция вычитания"),
            new Tuple<string, string, ТипЛексемы, string>("*", "Специальный символ", ТипЛексемы.АрифметическийОператор, "Операция умножения"),
            new Tuple<string, string, ТипЛексемы, string>("/", "Специальный символ", ТипЛексемы.АрифметическийОператор, "Операция деления"),
            new Tuple<string, string, ТипЛексемы, string>(">", "Специальный символ", ТипЛексемы.ОператорСравнения, "Операция сравнения 'больше'"),
            new Tuple<string, string, ТипЛексемы, string>(">=", "Специальный символ", ТипЛексемы.ОператорСравнения, "Операция сравнения 'больше или равно'")
        };
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
                лексемы.Add(new Лексема(символ, ТипЛексемы.КонецПредусловияЦикла, _позиция));
            }
            else if (ТекущийСимвол() == '<' || ТекущийСимвол() == '=' || ТекущийСимвол() == '>' || ТекущийСимвол() == '!')
            {
                var символ = ReadWhile(c => c == '<' || c == '=' || c == '!' || c == '>');

                var операторыСравнения = ТаблицаСимволов.ТаблицаЗаданныхСимволов().GroupBy(i => i.Item3).FirstOrDefault(i => i.Key == ТипЛексемы.ОператорСравнения).Select(i => i.Item1).ToList();
                if (символ == "=")
                {
                    лексемы.Add(new Лексема(символ, ТипЛексемы.ОператорПрисваивания, _позиция));
                }
                else if (операторыСравнения.Contains(символ))
                {
                    лексемы.Add(new Лексема(символ, ТипЛексемы.ОператорСравнения, _позиция));
                }
                else
                {
                    лексемы.Add(new Лексема(символ, ТипЛексемы.Неизвестно, _позиция));
                }
            }
            else if (ТекущийСимвол() == '+' || ТекущийСимвол() == '-' || ТекущийСимвол() == '/' || ТекущийСимвол() == '*')
            {
                var символ = ReadWhile(c => c == '+' || c == '-' || c == '/' || c == '*');
                var арифметическиеСравнения = ТаблицаСимволов.ТаблицаЗаданныхСимволов().GroupBy(i => i.Item3).FirstOrDefault(i => i.Key == ТипЛексемы.АрифметическийОператор).Select(i => i.Item1).ToList();
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
            case "until": return ТипЛексемы.КлючевоеСлово_Until;
            case "do": return ТипЛексемы.КлючевоеСлово_Do;
            case "loop": return ТипЛексемы.КлючевоеСлово_End;
            case "and": return ТипЛексемы.КлючевоеСлово_And;
            case "or": return ТипЛексемы.КлючевоеСлово_Or;
            default: return ТипЛексемы.Идентификатор;
        }
    }
}
public enum ТипЛексемы
{
    КлючевоеСлово_Until,
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
