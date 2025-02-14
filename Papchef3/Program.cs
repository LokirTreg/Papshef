namespace Task3
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
                    var ЛексическийАнализатор = new ЛексическийАнализатор(входнаяСтрока);
                    List<Токен> Токены = ЛексическийАнализатор.Обозначение();

                    var парсер = new Парсер(Токены);
                    парсер.DoWhile();

                    парсер.Вывод();
                    Console.WriteLine("Сгенерированный ПОЛИЗ:");
                    парсер.ВыводПолиз();
                    Console.WriteLine();
                    парсер.ВыводПолизИндексы();
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