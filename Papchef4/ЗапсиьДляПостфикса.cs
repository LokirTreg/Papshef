namespace Task4
{
    public class ЗаписьДляПолиз
    {
        public ТипЗаписи Тип { get; set; }
        public int Индекс { get; set; }
        public string Значение { get; set; }

        public ЗаписьДляПолиз(ТипЗаписи тип, string значение, int индекс)
        {
            Тип = тип;
            Индекс = индекс;
            Значение = значение;
        }
    }
}
