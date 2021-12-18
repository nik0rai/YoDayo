using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    class Program
    {
        static int i = 0;
        static List<string> firstList = new();
        static List<string> secondList = new();

        static public void FirstOperation()
        {
            string str = "FirstOP";
            for (int i = 0; i < 10000; i++)
            {
                firstList.Add(Convert.ToString(i));
                secondList.Add(Convert.ToString(i));
            }
            firstList.Add(str);
            secondList.Add(str);

            Console.WriteLine($"First List, {firstList[firstList.Count - 1]}");
            Console.WriteLine($"Second List, {secondList[secondList.Count - 1]}");
            Console.WriteLine();
            i = 1;
        }
        static void SecondOperation()
        {
            string str = "SecondOP";

            while (i == 0)
                Thread.Sleep(1);           

            for (int i = 0; i < 20000; i++)
                firstList.Add(Convert.ToString(i));
            
            firstList.Add(str);
            Console.WriteLine($"First List, {firstList[firstList.Count - 1]}");
            Console.WriteLine();
            ThirdOperation();
        }
        static void ThirdOperation()
        {
            string str = "ThirdOP";
            for (int i = 0; i < 7000; i++)
                firstList.Add(Convert.ToString(i));
            
            firstList.Add(str);
            Console.WriteLine($"First List, {firstList[firstList.Count - 1]}");
            Console.WriteLine();
        }
        static public void RunerFirst()
        {
            string str = "RunnerOPA";
            while (i == 0)
                Thread.Sleep(1);       
            
            for (int i = 0; i < 5000; i++)
                secondList.Add(Convert.ToString(i));
            
            secondList.Add(str);
            Console.WriteLine($"Second List, {secondList[secondList.Count - 1]}");
            Console.WriteLine();
            RunnerSecond();
        }
        static public void RunnerSecond()
        {
            string str = "RunnerOPS";
            for (int i = 0; i < 10000; i++)
                secondList.Add(Convert.ToString(i));
            
            secondList.Add(str);
            Console.WriteLine($"Second List, {secondList[secondList.Count - 1]}");
            Console.WriteLine();
            Operation();
        }
        static public void Operation()
        {
            string str = "OPERATION";
            for (int i = 0; i < 10000; i++)
                secondList.Add(Convert.ToString(i));           

            secondList.Add(str);
            Console.WriteLine($"Second List, {secondList[secondList.Count - 1]}");
            Console.WriteLine();
        }
        static public void SomeOperation()
        {
            string str = "SomeOp";
            for (int i = 0; i < 10000; i++)
            {
                firstList.Add(Convert.ToString(i));
                secondList.Add(Convert.ToString(i));
            }
            firstList.Add(str);
            secondList.Add(str);
            Console.WriteLine($"First List, {firstList[firstList.Count - 1]}");
            Console.WriteLine($"Second List, {secondList[secondList.Count - 1]}");
        }

        static void Main(string[] args)
        {
            FirstOperation();

            var task1 = new Task(SecondOperation);
            var task2 = new Task(RunerFirst);

            task1.Start();
            task2.Start();
            Task.WaitAll(task1, task2);

            SomeOperation();
        }
    }
}
