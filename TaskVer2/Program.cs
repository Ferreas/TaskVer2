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
        private string MidLevelActions = "|&";
        private string HighLevelActions = "+-*/><=";
        private string SpecialActions = "+-*/><=()|&";
        private int end_index;

        public void Process()
        {
            //убираем все пробелы
            Expression = Console.ReadLine().Replace(" ", "");
            //созание двух массивов на основе длинны введенного значения
            Separated = new string[Expression.Length];
            end_index = Expression.Length;
            FirstSeparator();
            Execute(0);
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

        string LeftOperand = "";
        string RightOperand = "";
        public string Execute(int starting_index)
        {
            int i = starting_index;
            if (i < end_index - 1)
            {
                if (GetPriority(Separated[i]) > 0)
                {
                    LeftOperand += ((LeftOperand == "") ? (Separated[i - 1]) : ("")) + Separated[i] + GetRightOp(i);
                    Console.WriteLine(LeftOperand);
                    RightOperand = LeftOperand;
                    i++;
                    Execute(i);
                }
                i++;
                Execute(i);
            }
            return RightOperand;
        }

        private string GetRightOp(int index)
        {
            if (GetPriority(Separated[index + 2]) <= GetPriority(Separated[index]))
            {
                RightOperand = "";
                return Execute(index + 2);
            }

            return Separated[index + 1];
        }

        private static int GetPriority(string c)
        {
            switch (c)
            {
                case "+": return 3;
                case "-": return 3;
                case "*": return 3;
                case "/": return 3;
                case ">": return 3;
                case "<": return 3;
                case "=": return 3;
                case "|": return 1;
                case "&": return 2;
                default: return 0;
            }
        }

        private int HighPrFind(int index)
        {
            for (int i = index; i < Separated.Length; i++)
            {
                if (HighLevelActions.Contains(Separated[i]))
                {
                    return i;
                }
            }
            return Separated.Length - 1;
        }


        public void Show(string[] a)
        {
            foreach (string s in a)
            {
                Console.WriteLine(s);
            }
        }
    }


    class ReversePN
    {
        private static string SpecialActions = "+-*/><=()|&";
        /// <summary>
        /// Преобразует обычное математическое выр-е в обратную польскую запись
        /// </summary>
        /// <param name="initialString"> Начальное выражение </param>
        /// <returns> Обратная польская запись выражения </returns>
        public static string ToRPN(string initialString)
        {
            // В стеке будут содержаться операции из выражения
            Stack<char> operationsStack = new Stack<char>();

            char lastOperation;

            // Результирующая строка
            string result = string.Empty;
            // Удаляем из входной строки лишние пробелы
            initialString = initialString.Replace(" ", "");

            for (int i = 0; i < initialString.Length; i++)
            {
                // Если текущий символ - число, добавляем его к результирующей строке
                //
                if (!SpecialActions.Contains(initialString[i]))
                {
                    result += initialString[i];
                    continue;
                }

                // Если текущий символ - операция (+, -, *, /)
                //
                if (IsOperation(initialString[i]))
                {
                    // Если это не первая операция в выражении,
                    // то нам необходимо будет сравнить ее
                    // с последней операцией, хранящейся в стеке.
                    // Для этого сохраняем ее в переменной lastOperation
                    //
                    if (!(operationsStack.Count == 0))
                        lastOperation = operationsStack.Peek();

                    // Иначе (если это первая операция), кладем ее в стек,
                    // и переходим к следующему символу
                    else
                    {
                        operationsStack.Push(initialString[i]);
                        continue;
                    }

                    // Если приоритет текущей операции больше приоритета
                    // последней, хранящейся в стеке, то кладем ее в стек
                    //
                    if (GetOperationPriority(lastOperation) < GetOperationPriority(initialString[i]))
                    {
                        operationsStack.Push(initialString[i]);
                        continue;
                    }

                    // иначе, выталкиваем последнюю операцию,
                    // а текущую сохраняем в стек
                    else
                    {
                        result += operationsStack.Pop();
                        operationsStack.Push(initialString[i]);
                        continue;
                    }
                }

                // Если текущий символ - '(', кладем его в стек
                if (initialString[i].Equals('('))
                {
                    operationsStack.Push(initialString[i]);
                    continue;
                }

                // Если текущий символ - ')', то выталкиваем из стека
                // все операции в результирующую строку, пока не встретим знак '('.
                // Его в строку не закидываем.
                if (initialString[i].Equals(')'))
                {
                    while (operationsStack.Peek() != '(')
                    {
                        result += operationsStack.Pop();
                    }
                    operationsStack.Pop();
                }
            }

            // После проверки всей строки, выталкиваем из стека оставшиеся операции
            while (!(operationsStack.Count == 0))
            {
                result += operationsStack.Pop();
            }

            // Возвращаем результат
            return result;
        }

        /// <summary>
        /// Вычисляет результат выражения, записанного в обратной польской нотации
        /// </summary>
        /// <param name="rpnString"> Обратная польская запись выражения </param>
        /// <returns> Результат выражения </returns>
        public static void CalculateRPN(string rpnString)
        {
            // В стеке будут храниться цифры из ОПН
            Stack<string> numbersStack = new Stack<string>();

            string op1, op2;

            for (int i = 0; i < rpnString.Length; i++)
            {
                // Если символ - цифра, помещаем его в стек,
                if (!SpecialActions.Contains(rpnString[i]))
                    numbersStack.Push(rpnString[i].ToString());

                // иначе (символ - операция), выполняем эту операцию
                // для двух последних значений, хранящихся в стеке.
                // Результат помещаем в стек
                else
                {
                    op2 = numbersStack.Pop();
                    op1 = numbersStack.Pop();
                    numbersStack.Push(ApplyOperation(rpnString[i], op1, op2));
                }
            }

            // Возвращаем результат
        }

        /// <summary>
        /// Проверяет, является ли символ математической операцией
        /// </summary>
        /// <param name="c"> Символ для проверки</param>
        /// <returns> true, если символ - операция, иначе false</returns>
        private static bool IsOperation(char c)
        {
            if (c == '+' ||
                c == '-' ||
                c == '*' ||
                c == '/' ||
                c == '=' ||
                c == '<' ||
                c == '>' ||
                c == '|' ||
                c == '&')

                return true;
            else
                return false;
        }

        /// <summary>
        /// Определяет приоритет операции
        /// </summary>
        /// <param name="c"> Символ операции </param>
        /// <returns> Ее приоритет </returns>
        private static int GetOperationPriority(char c)
        {
            switch (c)
            {
                case '+': return 3;
                case '-': return 3;
                case '*': return 3;
                case '/': return 3;
                case '>': return 3;
                case '<': return 3;
                case '=': return 3;
                case '|': return 1;
                case '&': return 2;
                default: return 0;
            }
        }

        /// <summary>
        /// Выполняет матем. операцию над двумя числами
        /// </summary>
        /// <param name="operation"> Символ операции </param>
        /// <param name="op1"> Первый операнд </param>
        /// <param name="op2"> Второй операнд </param>
        /// <returns> Результат операции </returns>
        private static string ApplyOperation(char operation, string op1, string op2)
        {
            Console.WriteLine(op1 + operation + op2);
            return "("+ op1 + operation + op2 + ")";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<string> tests = new List<string>()
            {
                "a+b+c",
                "a+b&c",
                "(a + b > с | c < d) & d = e",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                ""
            };

            string result = ReversePN.ToRPN(tests[2]);
            Console.WriteLine("Initial Expression: " + tests[2]);
            Console.WriteLine("RPN Expression: " + result);
            ReversePN.CalculateRPN(result);
        }
    }
}
