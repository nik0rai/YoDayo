using System;
using System.Threading;
using System.Diagnostics;
class Program
{
    static AutoResetEvent waitHandler = new(true);
    static int x = 1;
    public static void SomeTask(object obj)
        {
            waitHandler.WaitOne();
            for (int i = 0; i < 10; i++)
                x++;
                //Console.WriteLine(x); //if want to see put inside for loop

            waitHandler.Set();
        }
    static void Main(string[] args)
    {
        Stopwatch stopWatch = new();
        
        stopWatch.Start();
        for (int i = 0; i < 3; i++)
            ThreadPool.QueueUserWorkItem(SomeTask);         
        stopWatch.Stop();

        Console.WriteLine($"Time for creating threadpulls: {stopWatch.Elapsed.TotalMilliseconds}ms");

        Thread.Sleep(1000); //otherwithe main loop will end (пулы работают пока не перестанет работать main loop)

        stopWatch.Reset();
        stopWatch.Start();
        for (int i = 0; i < 2; i++)
            {
                Thread th = new(new ParameterizedThreadStart(SomeTask));
                th.Start();
            }
        stopWatch.Stop();

        Console.WriteLine($"Time for creating threads: {stopWatch.Elapsed.TotalMilliseconds}ms");
        Console.WriteLine($"Job done ans: {x}");
    }
}
