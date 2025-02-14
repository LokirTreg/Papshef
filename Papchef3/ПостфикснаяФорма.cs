﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3
{
    public class ПостфикснаяФормаПолиз
    {
        private List<ЗаписьДляПолиз> _полиз;
        public int _индекс = 0;

        public ПостфикснаяФормаПолиз()
        {
            _полиз = new List<ЗаписьДляПолиз>();
        }

        public int ПоследняяЗапись()
        {
            _полиз.Add(new ЗаписьДляПолиз(ТипЗаписи.ПоследняяЗапись, "", _индекс));
            _индекс += 1;
            return _полиз.Count - 1;
        }
        public int ЗаписатьКомманду(ТипКомманды названиеКомманды)
        {
            _полиз.Add(new ЗаписьДляПолиз(ТипЗаписи.Комманда, названиеКомманды.ToString(), _индекс));
            _индекс += 1;
            return _полиз.Count - 1;
        }
        public int ЗаписатьУказательНаКоманду(int индекс)
        {
            _полиз.Add(new ЗаписьДляПолиз(ТипЗаписи.УказательНаКоманду, индекс.ToString(), _индекс));
            _индекс += 1;
            return _полиз.Count - 1;
        }

        public int ЗаписатьПременную(string название)
        {
            int Индекс = 0;
            _полиз.Add(new ЗаписьДляПолиз(ТипЗаписи.Переменная, название, _индекс));
            _индекс += 1;
            return _полиз.Count - 1;
        }

        public int ЗаписатьКонстату(int значение)
        {
            _полиз.Add(new ЗаписьДляПолиз(ТипЗаписи.Константа, значение.ToString(), _индекс));
            _индекс += 1;
            return _полиз.Count - 1;
        }

        public void ИзменитьУказательНаКомманду(int индекс, int значение)
        {
            _полиз[индекс].Значение = значение.ToString();
        }

        public void Вывод()
        {
            foreach (var запись in _полиз)
            {
                string выводЗаписи = "";

                switch (запись.Тип)
                {
                    case ТипЗаписи.Комманда:
                        выводЗаписи = $"Команда: {запись.Значение}";
                        break;
                    case ТипЗаписи.Переменная:
                        выводЗаписи = $"Переменная: {запись.Значение}";
                        break;
                    case ТипЗаписи.Константа:
                        выводЗаписи = $"Константа: {запись.Значение}";
                        break;
                    case ТипЗаписи.УказательНаКоманду:
                        выводЗаписи = $"Указатель команды на адрес: {запись.Значение}";
                        break;
                    case ТипЗаписи.ПоследняяЗапись:
                        break;
                    default:
                        выводЗаписи = "Неизвестный тип";
                        break;
                }

                Console.WriteLine(выводЗаписи);
            }
        }
        public void ВыводПолиз()
        {
            foreach (var запись in _полиз)
            {
                string выводЗаписи = "";

                switch (запись.Тип)
                {
                    case ТипЗаписи.Комманда:
                        выводЗаписи = $"{запись.Значение}";
                        break;
                    case ТипЗаписи.Переменная:
                        выводЗаписи = $"{запись.Значение}";
                        break;
                    case ТипЗаписи.Константа:
                        выводЗаписи = $"{запись.Значение}";
                        break;
                    case ТипЗаписи.УказательНаКоманду:
                        выводЗаписи = $"{запись.Значение}";
                        break;
                    case ТипЗаписи.ПоследняяЗапись:
                        break;
                    default:
                        выводЗаписи = "Неизвестный тип";
                        break;
                }
                if (выводЗаписи != "")
                {
                    Console.Write("{0, -5}|", выводЗаписи);
                }
            }
        }
        public void ВыводПолизИндексы()
        {
            foreach (var запись in _полиз)
            {
                string выводЗаписи = "";

                switch (запись.Тип)
                {
                    case ТипЗаписи.Комманда:
                        выводЗаписи = $"{(запись.Индекс)}";
                        break;
                    case ТипЗаписи.Переменная:
                        выводЗаписи = $"{запись.Индекс}";
                        break;
                    case ТипЗаписи.Константа:
                        выводЗаписи = $"{запись.Индекс}";
                        break;
                    case ТипЗаписи.УказательНаКоманду:
                        выводЗаписи = $"{запись.Индекс}";
                        break;
                    case ТипЗаписи.ПоследняяЗапись:
                        выводЗаписи = $"{запись.Индекс}";
                        break;
                    default:
                        выводЗаписи = "Неизвестный тип";
                        break;
                }

                if (выводЗаписи != "")
                {
                    Console.Write("{0, -5}|", выводЗаписи);
                }
            }
        }
    }
}
