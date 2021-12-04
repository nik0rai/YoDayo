using System.Threading;
using System;
using System.Diagnostics;
class Program
{
    static float number = 0, znam = 1; //number for method, znam => znamenatel
    static int sign = 1; //sign
    static AutoResetEvent waitHandler = new(true);

    struct Tester
    {
        private uint thread_count; 
        private uint oneIter_count; //for one thread
        private float exact_pi; //pi in exact iteration
        private uint iterations_count;

        public Tester(uint a, uint b, float c, uint d)
        {
            this.thread_count = a;
            this.oneIter_count = b;
            this.exact_pi = c;
            this.iterations_count = d;
        }
        public uint Thread_Count
        {
            get {return thread_count; }
        }
        public uint OneIter_count
        {
            get { return oneIter_count; }
        }
        public float Pi
        {
            get { return exact_pi; }
        }
        public uint Count_iterations
        {
            get { return iterations_count; }
        }
    }

    static void Pi(object obj)
    {
        Tester counter = (Tester)obj;
        Stopwatch stopWatch = new();
        waitHandler.WaitOne();
        stopWatch.Start();

        int stepper = 0;
        while (stepper < counter.OneIter_count)
        {
            number += 4 * (1 / znam) * sign;
            znam += 2;
            sign *= (-1);
            stepper++;
            //Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}: {number}");
        }
    
        if ((number < counter.Pi) && (counter.Count_iterations % counter.Thread_Count != 0)) //in case of nechet thread ammount
        {
            number += 4 * (1 / znam) * sign;
            znam += 2;
            sign *= (-1);
            //Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}: {number}");
        }
        if (number == counter.Pi)
        {
            stopWatch.Stop();
            Console.WriteLine($"Elapsed time: {stopWatch.Elapsed.TotalMilliseconds}ms");
            Environment.Exit(0);
        }
        waitHandler.Set();
    }

    static void Main()
    {
        float numd = 1, c = 0;
        int s = 1;

        Console.Write("How much threads: "); uint f = Convert.ToUInt32(Console.ReadLine());
        Console.Write("How much iters: "); uint k = Convert.ToUInt32(Console.ReadLine());
        uint u = k / f; //ammount of iters for thread

        for (int i = 0; i < k; i++) //checker
        {
            c += 4 * (1 / numd) * s;
            numd += 2;
            s *= (-1);
        }

        var counter = new Tester(f, u, c, k);

        for (int i = 0; i < f; i++)
            ThreadPool.QueueUserWorkItem(new WaitCallback(Pi), counter);

        Console.WriteLine($"Threads: {f}, Iterations: {k}, Pi: {counter.Pi}");

    }
}
