namespace Task3
{
    public class Токен
    {
        public ТипЛексемы Тип { get; }
        public string Значение { get; }

        public Токен(ТипЛексемы тип, string значение)
        {
            Тип = тип;
            Значение = значение;
        }

        public override string ToString()
        {
            return $"{Тип} ({Значение})";
        }
    }
}
