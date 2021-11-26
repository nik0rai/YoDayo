using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp4
{

    class Params
    {
        public int start { get; set; }
        public int end { get; set; }
        public override string ToString() => $"[{start}, {end}]";
    }

    class Program
    {
        private static long sum;
        private static object locker = new();
        public static void Main()
        {

            int temp = 1, count = 100, flag = 1000000 / count;

            Stopwatch stopwatch = new();
            stopwatch.Start();

            for (int i = 0; i < count; i++)
            {
                Params par = new() { start = temp, end = temp + flag};
                Thread thread = new(new ParameterizedThreadStart(Tasker));
                thread.Priority = ThreadPriority.Highest;
                thread.Start(par);
                temp += flag;
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }

        private static void Tasker(object obj)
        {        
            if (obj is null) return;
            var val = obj as Params;

            lock (locker)
                for (int i = val.start; i < val.end; i++) sum += i;          
        }   
    }
}
