namespace Task4
{
    public class СтэкВыполнения
    {
        private Stack<Tuple<int, string>> _стэк;

        public СтэкВыполнения()
        {
            _стэк = new Stack<Tuple<int, string>>();
        }

        public Tuple<int, string> ДостатьЗначение()
        {
            if (_стэк.Count == 0)
                return new Tuple<int, string>(int.MinValue, "");
            return _стэк.Pop();
        }

        public void ПоложитьЗначение(int значение, string название)
        {
            _стэк.Push(new Tuple<int, string>(значение, название));
        }

        public string СостояниеСтэка()
        {
            string состояние = " { ";
            foreach (var элемент in _стэк)
            {
                if (string.IsNullOrEmpty(элемент.Item2))
                    состояние += $"{элемент.Item1} ";
                else состояние += $"{элемент.Item2} ";
            }
            состояние += "}";
            return состояние;
        }
    }
}
