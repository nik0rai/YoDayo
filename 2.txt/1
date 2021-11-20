using System;
namespace Construct
{
    class Triangle
    {
        int a;
        int b;
        int c;
        public Triangle(int a, int b, int c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            try
            {
                if (a > 0 && b > 0 && c > 0)
                {
                    if (a + b <= c || a + c <= b || b + c <= a)
                        Console.WriteLine("Ошибка! Треугольник не существует. ");
                }
                else { throw new ArgumentException("Значение стороны должно быть больше нуля!"); }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    class Quadrangle
    {
        int a;
        int b;
        int c;
        int d;
        public Quadrangle(int a, int b, int c, int d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            try
            {
                if (a > 0 && b > 0 && c > 0 && d > 0)
                {
                    if (a + b + d <= c || a + b + c <= b || b + c + d <= a || a + b + c <= d)
                        Console.WriteLine("Ошибка! Четырёхугольник не существует. ");
                }
                else { throw new ArgumentException("Значение стороны должно быть больше нуля!"); }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }


class Program
    {
        static void Main(string[] args)
        {
            Triangle tr = new Triangle(3, 4, 5);
            Console.ReadKey();
        }
    }
}
