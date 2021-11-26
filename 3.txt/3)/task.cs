using System;
using System.Threading;

namespace Task1
{

    class Program
    {
        static int value = 0;
        private static object locker = new object();
        static void Main(string[] args)
        {
            Thread thread = new Thread(increment);
            Thread thread2 = new Thread(increment);
            Thread thread3 = new Thread(increment);
            Thread thread4 = new Thread(increment);
            Thread thread5 = new Thread(increment);

            thread.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();
            thread5.Start();

        }
        static void increment()
        {
            lock (locker) // Когда выполнение доходит до оператора lock, объект locker блокируется,            
            {             // и на время его блокировки доступ к блоку кода имеет только один поток.
                value++;
                Console.WriteLine(value);
            }
        }
    }
}
