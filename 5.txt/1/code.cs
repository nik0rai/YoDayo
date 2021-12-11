using System;

namespace ConsoleApp4
{
    class CollectClass
    {
        private const long forgarbage = 100;

        static void Main()
        {
            CollectClass myGCCol = new();

            Console.WriteLine("The highest generation is {0}", GC.MaxGeneration);
            myGCCol.Garbage();
        
            Console.WriteLine("Generation: {0}", GC.GetGeneration(myGCCol));           
            Console.WriteLine("Total Memory: {0}", GC.GetTotalMemory(false));
        
            GC.Collect(0);
            Console.WriteLine("Collected generation 0");
          
            Console.WriteLine("Generation: {0}", GC.GetGeneration(myGCCol));
            Console.WriteLine("Total Memory: {0}", GC.GetTotalMemory(false));

            GC.Collect(2);
           
            Console.WriteLine("Generation: {0}", GC.GetGeneration(myGCCol));
            Console.WriteLine("Total Memory: {0}", GC.GetTotalMemory(false));
        }

        void Garbage()
        {
            int num = 5;
            Console.WriteLine("Generation of created num: {0}", GC.GetGeneration(num));
            Version vers;

            for (int i = 0; i < forgarbage; i++)
                vers = new();
        }        
    }
}
