using System;
using System.Threading;

namespace ConsoleApp4
{
    class Program
    {
        private static Semaphore pool;
        private static int padding;

        public static void Main()
        {
            pool = new Semaphore(0, 3);

            for (int i = 1; i < 4; i++) new Thread(new ParameterizedThreadStart(Job)).Start(i);

            pool.Release(3); //позволяем начать работу
        }

        private static void Job(object obj)
        {  
            pool.WaitOne();
            int padding = Interlocked.Add(ref Program.padding, 1000);
            Thread.Sleep(1000 + padding);
            Console.WriteLine($"Thread {obj} count = {pool.Release()}");
        }
    }
}
