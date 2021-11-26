using System;
using System.Threading;

namespace Zadanie
{
    class Program
    {
        static void mythread1()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Поток 1: " + i);
            }
        }

        static void mythread2()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Поток 2: " + i);
            }
        }

        static void WriteF()
        {
            while (true)
                Console.Write("F");
        }

static void Main(string[] args)
        {
            Thread thread1 = new Thread(mythread1);
            Thread thread2 = new Thread(mythread2);
            thread2.IsBackground = true;

            thread1.Start();
            thread2.Start();

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Поток 3: " + i);
            }
            Thread treadF = new Thread(WriteF);
            treadF.Start();
            Console.ReadLine();
        }
    }
}
