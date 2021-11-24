using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleApp4
{ 
    public class MyClass
    {
        static void Main(string[] args)
        {
            Stopwatch stp = new();
            ArrayList arrays = new();
            List<int> arr_int = new();
            List<string> arr_str = new();
            int check = 999999;

            stp.Start();
            for (int i = 0; i < check; i++)
                arrays.Add(i.ToString());
            stp.Stop();

            TimeSpan timeSpan = stp.Elapsed;
            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
            Console.WriteLine("\nRunTime add string to ArrayList " + elapsedTime);
            stp.Restart();
            stp.Start();
            foreach (var i in arrays)
                arrays.IndexOf(i);
            arrays.GetType();
            stp.Stop();
            timeSpan = stp.Elapsed;
            elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
            Console.WriteLine("\nRunTime GetInfo from ArrayList string " + elapsedTime);
            stp.Restart();

            stp.Start();
            for (int i = 0; i < check; i++)
                arr_str.Add(i.ToString());
            stp.Stop();
            timeSpan = stp.Elapsed;
            elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
            Console.WriteLine("\nRunTime Add string to List<string> " + elapsedTime);
            stp.Restart();
            stp.Start();
            foreach (var i in arr_str)
                arr_str.IndexOf(i);
            arr_str.GetType();
            stp.Stop();
            timeSpan = stp.Elapsed;
            elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
            Console.WriteLine("\nRunTime GetInfo from List<string> " + elapsedTime);
            stp.Restart();

            arrays.Clear();
            stp.Start();
            for (int i = 0; i < check; i++)
                arrays.Add(i);
            stp.Stop();
            timeSpan = stp.Elapsed;
            elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
            Console.WriteLine("\nRunTime Add int to ArrayList " + elapsedTime);
            stp.Restart();
            stp.Start();
            foreach (var i in arrays)
                arrays.IndexOf(i);
            arrays.GetType();
            stp.Stop();
            timeSpan = stp.Elapsed;
            elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
            Console.WriteLine("\nRunTime GetInfo from ArrayList int " + elapsedTime);
            stp.Restart();
           
            stp.Start();
            for (int i = 0; i < check; i++)
                arr_int.Add(i);
            stp.Stop();
            timeSpan = stp.Elapsed;
            elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
            Console.WriteLine("\nRunTime Add int to List<int> " + elapsedTime);
            stp.Restart();
            stp.Start();
            foreach (var i in arr_int)
                arr_int.IndexOf(i);
            arr_int.GetType();
            stp.Stop();
            timeSpan = stp.Elapsed;
            elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
            Console.WriteLine("\nRunTime GetInfo from List<int> " + elapsedTime);
        }
    }
}