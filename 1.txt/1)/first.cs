using System;
using System.Collections;
using System.Collections.Generic;

namespace Task1
{
    class Animal : IComparable, IComparable<Animal>
    {
        public int Age { get; set; }
        public string Name { get; set; }

        public int CompareTo(object obj)
        {
            if (obj is Animal an) 
                return CompareTo(an);
            else throw new Exception("Невозможно сравнить два объекта");

        }
        public int CompareTo(Animal other)
        {
            return this.Age.CompareTo(other.Age);
        }

        public override string ToString()
        {
            return $"Меня зовут {this.Name}, мой возраст - {this.Age}";
        }

    }


    class Program
    {
        static void Main(string[] args)
        {
            Animal Lion = new Animal { Age = 4, Name = "Alex" };
            Animal Giraffe = new Animal { Age = 9, Name = "Melman" };
            Animal Hippo = new Animal { Age = 6, Name = "Gloria" };
            Animal Zebra = new Animal { Age = 3, Name = "Marty" };

            ArrayList listok = new ArrayList { Lion, Giraffe, Hippo, Zebra };
            List<Animal> listochek = new List<Animal> { Lion, Giraffe, Hippo, Zebra };


            // Sort по умолчанию работает только для наборов примитивных типов
            // Для сортировки наборов сложных объектов применяется интерфейс IComparable.
            listok.Sort();
            listochek.Sort();


            foreach (object o in listok)
            {
                Console.WriteLine(o);
            }
            Console.WriteLine("___________________________________\n");
            foreach (Animal o in listochek)
            {
                Console.WriteLine(o);
            }

            Console.ReadLine();
        }
    }
}
