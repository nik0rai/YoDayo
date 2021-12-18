using System;
using System.IO;
using System.Text;

namespace ConsoleApp17
{
    public class File
    {
        private readonly string input, output;
        StreamReader streamReader;
        StreamWriter streamWriter;
        public File(string inputFilePath, string outputFilePath)
        {
            this.input = inputFilePath;
            this.output = outputFilePath;
        }
        public void Open()
        {
            streamReader = new StreamReader(input, Encoding.Default);
            streamWriter = new StreamWriter(output, false, Encoding.Default);
        }
        public void Close()
        {
            streamReader.Close();
            streamWriter.Close();
        }
        public void Work()
        {
            try
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                    streamWriter.WriteLine(line);               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void Dispose() => Console.WriteLine("Dispose");
    }
    class Program
    {
        static void Main(string[] args)
        {
            string i, j;
            i = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "//copy.txt";
            j = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "//paste.txt";
            File fl = new(i, j);

            fl.Open();
            fl.Work();
            fl.Close();
            fl.Dispose();

            Console.ReadKey();
        }
    }
}
