namespace Task4
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Введите выражение или \"выход\" для выхода:");
                string входнаяСтрока = Console.ReadLine();

                if (входнаяСтрока.ToLower() == "выход")
                {
                    Console.WriteLine();
                    break;
                }
                try
                {
                    var лексическийАнализатор = new ЛексическийАнализатор(входнаяСтрока);
                    List<Токен> токены = лексическийАнализатор.Обозначение();

                    var парсер = new Парсер(токены);
                    парсер.DoWhile();
                    Console.WriteLine("Сгенерированный ПОЛИЗ:");
                    парсер.ВыводПолиз();
                    Console.WriteLine();
                    парсер.ВыводПолизИндексы();
                    Console.WriteLine();
                    Console.WriteLine("Начало интерпретации ПОЛИЗа:");
                    Итерпритатор итерпритатор = new Итерпритатор(парсер._постфикснаяФормаПолиз._полиз);
                    итерпритатор.Интерпитация();
                    итерпритатор.ВыводПеременных();
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
}