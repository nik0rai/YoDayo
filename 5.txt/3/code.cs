using System;
using System.Diagnostics;

namespace ConsoleApp4
{
    public class SomeClass
    {
        private int val;
        Stopwatch stopwatch;
        public SomeClass(int val)
        {
            this.val = val;
            Console.WriteLine("Generation of val: {0}", GC.GetGeneration(val));
            stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Generation of stopwatch: {0}", GC.GetGeneration(stopwatch));

            Console.WriteLine("Object start");
        }

        public void Method() =>        
            Console.WriteLine("Value {0} of instance {1} has been in existence for {2}",
                              val, this, stopwatch.Elapsed);
        
        ~SomeClass() //почему-то не работает, хотя должен
        {
            Console.WriteLine("Finalizing object {0}", this);
            stopwatch.Stop();
            Console.WriteLine("This instance of {0} has been in existence for {1}",
                              this, stopwatch.Elapsed);
        }
    }

    class Programm
    {
        static void Main()
        {
            SomeClass ex = new(5);
            ex.Method();
            //GC.Collect();
            Console.WriteLine("Programm end");
        }
   
    }
}
