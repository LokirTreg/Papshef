using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
