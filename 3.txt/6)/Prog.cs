using System;
using System.Diagnostics;
using System.Threading;

namespace ConsoleApp5
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKey cs = Console.ReadKey().Key;
            Console.WriteLine();

            if(cs is ConsoleKey)
            {
                using QuedTask quedTask = new();
                for (int i = 0; i < 3; i++) quedTask.Task($"Task {i}");
            }             
            
        }        
    }
}
