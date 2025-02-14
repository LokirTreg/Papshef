﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3
{
    public class Парсер
    {
        private readonly List<Токен> _Токены;
        private ПостфикснаяФормаПолиз _постфикснаяФорма;
        public int _текущийИндексТокена;

        public Парсер(List<Токен> Токены)
        {
            _Токены = Токены;
            _текущийИндексТокена = 0;
            _постфикснаяФорма = new ПостфикснаяФормаПолиз();
        }
        public int ТекущийИндекс()
        {
            return _постфикснаяФорма._индекс;
        }
        private Токен ТекущийТокен
        {
            get
            {
                if (ЕстьЛиЕщеТокены())
                {
                    return _Токены[_текущийИндексТокена];
                }
                else
                {
                    throw new Exception("Ошибка: неожиданный конец выражения.");
                }
            }
        }

        private bool ЕстьЛиЕщеТокены()
        {
            return _текущийИндексТокена < _Токены.Count;
        }

        private void Сопоставление(ТипЛексемы ОжидаемыйТип)
        {
            if (ТекущийТокен.Тип == ОжидаемыйТип)
            {
                _текущийИндексТокена++;
            }
            else
            {
                throw new Exception($"Ошибка: ожидалось {ОжидаемыйТип}, но найдено {ТекущийТокен.Тип}");
            }
        }

        public void СчитатьПрисваивание()
        {
            if (ТекущийТокен.Тип == ТипЛексемы.ID)
            {
                _постфикснаяФорма.ЗаписатьПременную(ТекущийТокен.Значение);
                Сопоставление(ТипЛексемы.ID);
                Сопоставление(ТипЛексемы.Присваивание);

                СчитатьАрифметическуюОперацию();

                _постфикснаяФорма.ЗаписатьКомманду(ТипКомманды.SET);

                if (ТекущийТокен.Тип == ТипЛексемы.Разделитель)
                {
                    Сопоставление(ТипЛексемы.Разделитель);
                }
                else
                {
                    throw new Exception("Ошибка: Ожидалась ';' после присваивания.");
                }
            }
        }

        private void СчитатьАрифметическуюОперацию()
        {
            СчитатьАрифметическуюОперациюУмножения();
            while (ТекущийТокен.Тип == ТипЛексемы.Плюс || ТекущийТокен.Тип == ТипЛексемы.Минус)
            {
                if (ТекущийТокен.Тип == ТипЛексемы.Плюс)
                {
                    Сопоставление(ТипЛексемы.Плюс);
                    СчитатьАрифметическуюОперациюУмножения();
                    _постфикснаяФорма.ЗаписатьКомманду(ТипКомманды.ADD);
                }
                else if (ТекущийТокен.Тип == ТипЛексемы.Минус)
                {
                    Сопоставление(ТипЛексемы.Минус);
                    СчитатьАрифметическуюОперациюУмножения();
                    _постфикснаяФорма.ЗаписатьКомманду(ТипКомманды.SUB);
                }
            }
        }

        private void СчитатьАрифметическуюОперациюУмножения()
        {
            СчитатьПеременную();
            while (ТекущийТокен.Тип == ТипЛексемы.Умножить || ТекущийТокен.Тип == ТипЛексемы.Деление)
            {
                if (ТекущийТокен.Тип == ТипЛексемы.Умножить)
                {
                    Сопоставление(ТипЛексемы.Умножить);
                    СчитатьПеременную();
                    _постфикснаяФорма.ЗаписатьКомманду(ТипКомманды.MUL);
                }
                else if (ТекущийТокен.Тип == ТипЛексемы.Деление)
                {
                    Сопоставление(ТипЛексемы.Деление);
                    СчитатьПеременную();
                    _постфикснаяФорма.ЗаписатьКомманду(ТипКомманды.DIV);
                }
            }
        }

        private void СчитатьПеременную()
        {
            if (ТекущийТокен.Тип == ТипЛексемы.ID)
            {
                _постфикснаяФорма.ЗаписатьПременную(ТекущийТокен.Значение);
                Сопоставление(ТипЛексемы.ID);
            }
            else if (ТекущийТокен.Тип == ТипЛексемы.Константа)
            {
                _постфикснаяФорма.ЗаписатьКонстату(int.Parse(ТекущийТокен.Значение));
                Сопоставление(ТипЛексемы.Константа);
            }
            else
            {
                throw new Exception("Ожидалось арифметическое выражение");
            }
        }
        private void СчитатьСкобку()
        {
            if (ТекущийТокен.Тип == ТипЛексемы.ОткрывающаяСкобка)
            {
                Сопоставление(ТипЛексемы.ОткрывающаяСкобка);
            }
            else if (ТекущийТокен.Тип == ТипЛексемы.ЗакрывающаяСкобка)
            {
                ;
                Сопоставление(ТипЛексемы.ЗакрывающаяСкобка);
            }
            else
            {
                throw new Exception("Ошибка: ожидалась открывающая скобка");
            }
        }
        private void СчитатьУсловие()
        {
            СчитатьСкобку();
            СчитатьСравнение();

            while (ТекущийТокен.Тип == ТипЛексемы.And || ТекущийТокен.Тип == ТипЛексемы.Or)
            {
                if (ТекущийТокен.Тип == ТипЛексемы.And)
                {
                    Сопоставление(ТипЛексемы.And);
                    СчитатьСравнение();
                    _постфикснаяФорма.ЗаписатьКомманду(ТипКомманды.And);
                }
                else if (ТекущийТокен.Тип == ТипЛексемы.Or)
                {
                    Сопоставление(ТипЛексемы.Or);
                    СчитатьСравнение();
                    _постфикснаяФорма.ЗаписатьКомманду(ТипКомманды.Or);
                }
            }
            СчитатьСкобку();
        }

        private void СчитатьСравнение()
        {
            СчитатьОперанд();

            if (ТекущийТокен.Тип == ТипЛексемы.Сравнение)
            {
                string Сравнение = ТекущийТокен.Значение;
                Сопоставление(ТипЛексемы.Сравнение);

                СчитатьОперанд();

                switch (Сравнение)
                {
                    case ">":
                        _постфикснаяФорма.ЗаписатьКомманду(ТипКомманды.CMPG);
                        break;
                    case "<":
                        _постфикснаяФорма.ЗаписатьКомманду(ТипКомманды.CMPL);
                        break;
                    case ">=":
                        _постфикснаяФорма.ЗаписатьКомманду(ТипКомманды.CMPGE);
                        break;
                    case "<=":
                        _постфикснаяФорма.ЗаписатьКомманду(ТипКомманды.CMPLE);
                        break;
                    case "==":
                        _постфикснаяФорма.ЗаписатьКомманду(ТипКомманды.CMPE);
                        break;
                    case "!=":
                        _постфикснаяФорма.ЗаписатьКомманду(ТипКомманды.CMPNE);
                        break;
                    default:
                        throw new Exception($"Неизвестная операция сравнения: {Сравнение}");
                }
            }
        }

        private void СчитатьОперанд()
        {
            if (ТекущийТокен.Тип == ТипЛексемы.ID)
            {
                _постфикснаяФорма.ЗаписатьПременную(ТекущийТокен.Значение);
                Сопоставление(ТипЛексемы.ID);
            }
            else if (ТекущийТокен.Тип == ТипЛексемы.Константа)
            {
                _постфикснаяФорма.ЗаписатьКонстату(int.Parse(ТекущийТокен.Значение));
                Сопоставление(ТипЛексемы.Константа);
            }
            else
            {
                throw new Exception("Ошибка: ожидался операнд");
            }
        }
        public void DoWhile()
        {
            while (ТекущийТокен.Тип != ТипЛексемы.Do)
            {
                СчитатьПрисваивание();
            }
            Сопоставление(ТипЛексемы.Do);
            Сопоставление(ТипЛексемы.While);

            int jmpИндекс = ТекущийИндекс();

            СчитатьУсловие();

            int jzИндекс = _постфикснаяФорма.ЗаписатьУказательНаКоманду(0);
            _постфикснаяФорма.ЗаписатьКомманду(ТипКомманды.JZ);

            while (ТекущийТокен.Тип != ТипЛексемы.Loop)
            {
                СчитатьПрисваивание();
            }

            Сопоставление(ТипЛексемы.Loop);

            _постфикснаяФорма.ЗаписатьУказательНаКоманду(jmpИндекс);
            _постфикснаяФорма.ЗаписатьКомманду(ТипКомманды.JMP);
            _постфикснаяФорма.ИзменитьУказательНаКомманду(jzИндекс, ТекущийИндекс());
            _постфикснаяФорма.ПоследняяЗапись();
        }

        public void Вывод()
        {
            _постфикснаяФорма.Вывод();
        }
        public void ВыводПолиз()
        {
            _постфикснаяФорма.ВыводПолиз();
        }
        public void ВыводПолизИндексы()
        {
            _постфикснаяФорма.ВыводПолизИндексы();
        }
    }
}
