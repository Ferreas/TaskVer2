using System;
using System.Collections.Generic;
using System.Linq;


namespace TaskVer2
{
    class PN
    {
        private static string SpecialActions = "+-*/><=()|&";
        /// <summary>
        /// Преобразует обычное математическое выражение в обратную польскую запись
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
                // Если текущий символ - не операция и не скобка, добавляем его к результирующей строке
                //
                if (!SpecialActions.Contains(initialString[i]))
                {
                    result += initialString[i];
                    continue;
                }

                // Если текущий символ - операция
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
        /// Выводит последовательность выполнения, используя запись в обратной польской нотации
        /// </summary>
        /// <param name="rpnString"> Обратная польская запись выражения </param>
        /// <returns> Результат выражения </returns>
        public static void ProceedRPN(string rpnString)
        {
            // В стеке будут храниться операнды
            Stack<string> numbersStack = new Stack<string>();

            string op1, op2;

            for (int i = 0; i < rpnString.Length; i++)
            {
                // Если символ - не операция и не скобка,
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
        }

        /// <summary>
        /// Проверяет, является ли символ операцией
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
        /// Выполняет операцию над двумя операндами
        /// </summary>
        /// <param name="operation"> Символ операции </param>
        /// <param name="op1"> Первый операнд </param>
        /// <param name="op2"> Второй операнд </param>
        /// <returns> Результат операции </returns>
        private static string ApplyOperation(char operation, string op1, string op2)
        {
            Console.WriteLine(op1 + operation + op2);
            return op1 + operation + op2;
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
                "((a + b > с) & (c < d)) | (s+s) & (e-d|s) = e",
                "((a + b > с) & (c < d)) & (s+s) + (e-d|s) = e",
                "((a + b > с) & (c < d)) | (s+s) + (e-d|s) = e",
                "a+b>c|c<d&(d=e)",
                "a+b&a+b&a+b|a-s|a-s|a&b|a&s",
            };

            foreach (string a in tests)
            {
                Console.WriteLine("======================================================");
                string result = PN.ToRPN(a);
                Console.WriteLine("Initial Expression: " + a);
                Console.WriteLine("RPN Expression: " + result);
                PN.ProceedRPN(result);
            }
        }
    }
}
