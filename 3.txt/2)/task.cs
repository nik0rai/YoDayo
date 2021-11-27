using System;
using System.Threading; 
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        
        Thread t = new Thread(Time);
       
        Thread t2 = new Thread(Time);

        Parallel.Invoke(
            () => {
                t.Start();
            },
            () => {
                t2.Start();
            }
        );
        

        Console.ReadLine();
    }
    
    public static void Time()
    {
    
        Console.WriteLine(DateTime.Now.ToString("hh:mm:ss:fffff"));

    }
}
