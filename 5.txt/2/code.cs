using System;

namespace ConsoleApp4
{
    class Program
    {
        static void Main()
        {
            object[] firstArray = new object[1000];
            object[] secondArray = new object[10000000];

            Console.WriteLine($"Generation of {nameof(firstArray)} = {GC.GetGeneration(firstArray)}");
            Console.WriteLine($"Generation of {nameof(secondArray)} = {GC.GetGeneration(secondArray)}\n");

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine($"Generation of {nameof(firstArray)} = {GC.GetGeneration(firstArray)}");
            Console.WriteLine($"Generation of {nameof(secondArray)} = {GC.GetGeneration(secondArray)}\n");

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine($"Generation of {nameof(firstArray)} = {GC.GetGeneration(firstArray)}");
            Console.WriteLine($"Generation of {nameof(secondArray)} = {GC.GetGeneration(secondArray)}");
        }
    }
}
