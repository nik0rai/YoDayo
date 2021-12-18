using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleApp4
{
    class Program
    {
        static List<Thread> threads = new List<Thread>();
        static int threadCount = 50;
        static ManualResetEvent startEvent = new ManualResetEvent(false); 
        static int starterCount = 0;
        static object LockObject = new object();

        static void Main(string[] args)
        {
            for (int i = 0; i < threadCount; i++)
            {
                Thread thread = new Thread(Work);
                threads.Add(thread);
            }
            foreach (var thread in threads)
            {
                new Thread(Starting).Start(thread);
            }
            while (starterCount < threadCount) Thread.Sleep(1);

            Thread.Sleep(100);

            startEvent.Set();

            while (true);
        }

        static void Starting(object paramThread)
        {
            lock (LockObject)
            {
                starterCount++;
            }
            startEvent.WaitOne();
            (paramThread as Thread).Start();
        }

        static void Work()
        {
            Console.WriteLine("Поток");
            return;
        }
    }
}

