using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Task1
{

    class Program
    {
        static void Main(string[] args)
        {
            var time1 = DateTime.Now.TimeOfDay.TotalMilliseconds;
            for (int i = 0; i < 5; i++)
            {
                ThreadPool.QueueUserWorkItem(obj => 
                {
                    Thread.Sleep(1000);
                });
           
            }
            var time2 = DateTime.Now.TimeOfDay.TotalMilliseconds;
            Console.WriteLine("Потоки из пула: " + (time2 - time1) + "мс.");

            time1 = DateTime.Now.TimeOfDay.TotalMilliseconds;

            Thread[] threads = new Thread[6];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(new ThreadStart(() => { Thread.Sleep(1000); }));
                threads[i].Start();
            }
            time2 = DateTime.Now.TimeOfDay.TotalMilliseconds;
            Console.WriteLine("Потоки обычные: " + (time2 - time1) + "мс.");
        }


    }

     
}
