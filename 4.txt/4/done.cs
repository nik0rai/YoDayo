using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleApp9
{
    class Program
    {
        static void Main(string[] args)
        {

            int[] nums = { 23, 23, 7, 9, 5, 11, 14, 10, 6, 10, 2, 1, 8, 9, 11, 0, 17, 1, 2 };

            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            var result1 = nums.AsParallel().AsOrdered().WithDegreeOfParallelism(2)
                .Select(factorial).GroupBy(n => n % 10).Select(n => n.Sum()).OrderBy(n => n)
                .Where((n, index) => index >= 4 && index <= 6).ToArray();
            stopwatch1.Stop();

            TimeSpan timespan1 = stopwatch1.Elapsed;
            string elapsedTime1 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            timespan1.Hours, timespan1.Minutes, timespan1.Seconds,
            timespan1.Milliseconds / 10);
            Console.WriteLine("RunTime: " + elapsedTime1);

            foreach (int i in result1)
                Console.WriteLine(i);
            
            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            var result2 = nums.AsParallel().AsOrdered().WithDegreeOfParallelism(3)
                .Select(factorial).GroupBy(n => n % 10).Select(n => n.Sum()).OrderBy(n => n)
                .Where((n, index) => index >= 4 && index <= 6).ToArray();
            stopwatch2.Stop();

            TimeSpan timespan2 = stopwatch2.Elapsed;
            string elapsedTime2 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            timespan2.Hours, timespan2.Minutes, timespan2.Seconds,
            timespan2.Milliseconds / 10);
            Console.WriteLine("RunTime: " + elapsedTime2);

            foreach (int i in result2)
                Console.WriteLine(i);            

            Stopwatch stopwatch3 = new Stopwatch();
            stopwatch3.Start();
            var result3 = nums.AsParallel().AsOrdered().WithDegreeOfParallelism(4)
                .Select(factorial).GroupBy(n => n % 10).Select(n => n.Sum()).OrderBy(n => n)
                .Where((n, index) => index >= 4 && index <= 6).ToArray();
            stopwatch3.Stop();

            TimeSpan timespan3 = stopwatch3.Elapsed;
            string elapsedTime3 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            timespan3.Hours, timespan3.Minutes, timespan3.Seconds,
            timespan3.Milliseconds / 10);
            Console.WriteLine("RunTime: " + elapsedTime3);

            foreach (int i in result3)
                Console.WriteLine(i);

            Console.WriteLine("-----");
            Console.ReadKey();
        }

        static int factorial(int c)
        {
            int result = c + 10;
            return result;
        }
    }
}
