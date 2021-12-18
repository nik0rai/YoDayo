using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] array = new int[1000];
            Random rand = new();

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = rand.Next();
                Console.WriteLine(array[i]);
            }

            string[] lines = File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\startfile.txt");
            int j = 0;

            Parallel.Invoke(() =>
            {
                try
                {
                    foreach (string line in lines)
                    {
                        string newfile = line + ".txt";
                        using (StreamWriter sw = File.CreateText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) +"\\"+ newfile))
                        {
                            for (int i = j; i < j + 100; i++)
                                sw.WriteLine(array[i]);             
                        }
                        j += 100;
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            });

            Console.ReadKey();
        }
    }
}
