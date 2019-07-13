using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskVer2
{
    class TestClass
    {
        public string Expression { get; private set; }
        //массив строк после первого разделения по рангу операций
        public string[] Separated;
        //соответствующие индексаторы двух массивов
        private int SeparatedIndex = 0;
        //специальные строки для проверки обрабатываемого символа
        //Вероятно, было бы лучше сделать их константами?
        private string MediumLevelActions = "|&";
        private string SpecialActions = "+-*/><=()|&";


        public void Process()
        {
            //убираем все пробелы
            Expression = Console.ReadLine().Replace(" ", "");
            //созание двух массивов на основе длинны введенного значения
            Separated = new string[Expression.Length];
        }


        public void FirstSeparator()
        {
            //временная строка для хранения значений будущего массива первого прохода
            string temp = "";
            for (int i = 0; i < Expression.Length; i++)
            {
                //посимвольная проверка, все, что не является скобкой или |/& добавляется в временную строку
                if (!SpecialActions.Contains(Expression[i]))
                {
                    temp += Expression[i];
                }


                //"столкновение" с сепараторами первого прохода записывает строку в массив первого разделения и опусташает временную строку, сам сепаратор записывается в отдельную ячейку для дальнейшего прохода
                if (SpecialActions.Contains(Expression[i]))
                {

                    Separated[SeparatedIndex] = temp;
                    SeparatedIndex++;

                    Separated[SeparatedIndex] = Expression[i].ToString();
                    SeparatedIndex++;

                    temp = "";
                }
            }
            //дозапись последнего отрезка в массив первого разделения
            Separated[SeparatedIndex] = temp;
        }



        public void Show(string[] a)
        {
            foreach (string s in a)
            {
                Console.WriteLine(s);
            }
        }
    }




    class Program
    {
        static void Main(string[] args)
        {
            TestClass a = new TestClass();
            a.Process();
            a.FirstSeparator();     
            a.Show(a.Separated);
        }
    }
}
