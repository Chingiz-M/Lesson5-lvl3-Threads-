using System;
using System.Threading;

namespace ProjectTestThread
{
    class Program
    {
        private static readonly object Sync = new object(); 
        static void Main(string[] args)
        {
            int N = Validation();

            new Thread(() => FactorialMethod(N)).Start();
            new Thread(() => SumMethod(N)).Start();

            Console.ReadKey(true);
        }

        private static int Validation()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("Введите целое положительное число: ");
                if (int.TryParse(Console.ReadLine(), out int N) && N >= 0)
                    return N;
                else
                {
                    Console.WriteLine("Некорректное число, повторите попытку\n(для продолжения нажмите любую клавишу)");
                    Console.ReadKey(true);
                }
            }
        }

        private static void FactorialMethod(int N)
        {
            int res = CalculationFactorial(N);
            DisplayConcole(N, res, "Factorial");
        }

        private static void SumMethod(int N)
        {
            int res = CalculationSum(N);
            DisplayConcole(N, res, "Sum");
        }

        private static void DisplayConcole(int N, int res, string method)
        {
            var thread = Thread.CurrentThread;

            lock (Sync)
            {
                Console.WriteLine($"Th {thread.ManagedThreadId}: {method} {N} = {res}");
            }
        }

        private static int CalculationFactorial(int N) => N == 0 ? 1 : N * CalculationFactorial(N - 1);
        private static int CalculationSum(int N)
        {
            int res = 0;
            while(N != 0)
            {
                res += N;
                N--;
            }
            return res;
        }
    }
}
