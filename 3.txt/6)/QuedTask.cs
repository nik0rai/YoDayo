using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleApp5
{
    class QuedTask : IDisposable
    {
        EventWaitHandle waitHandle = new AutoResetEvent(false);
        Thread temp;
        readonly object locker = new();
        Queue<string> tasks = new();

        public QuedTask()
        {
            temp = new Thread(Work);
            temp.Start();
        }

        public void Task(string task)
        {
            lock (locker) tasks.Enqueue(task);
            waitHandle.Set();
        }

        private void Work()
        {
            while (true)
            {
                string task = null;
                lock(locker)
                    if (tasks.Count > 0)
                    {
                        task = tasks.Dequeue();
                        if (task == null) return;
                    }
                if (task != null)
                {
                    Random random = new();
                    Thread.Sleep(random.Next(1000, 10000));
                    Console.WriteLine(task);                    
                }
                else waitHandle.WaitOne();
            }
        }

        public void Dispose()
        {
            Task(null); //сигнал на выход
            temp.Join(); //ожидаем завершения рабочего потока
            waitHandle.Close(); //освобождаем ресурсы
        }
    }
}
