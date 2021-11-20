using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace ConsoleApp7
{
    struct MyStruct
    {     
        public int X { get; init; }
        public int Y { get; init; }
        public override string ToString() => $"X = {X}, Y = {Y}";              
    }

    struct XintYdouble
    {
        public int X { get; init; }
        public double Y { get; init; }
        public override string ToString() => $"X = {X}, Y = {Y}";
    }

    public class Worker
    {
        public string Name { get; set; }
        public int Salary { get; set; }
    }

    class Program
    {
        #region sneky
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleOutputCP(uint wCodePageID);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCP(uint wCodePageID);
        #endregion
        static void Menu()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("PRESS ESC TO EXIT.");
            Console.ResetColor();
            Console.WriteLine("Select task: " +
               "\nа) Дан массив целых чисел, отобразить максимальный элемент." +
               "\nб) Дан массив целых чисел, отобразить индекс максимального элемента." +
               "\nв) Дан массив структур {X:int, Y:int} - отобразить максимальный по Y элемент." +
               "\nг) Дан массив структур {X:int, Y:double} - отсортировать его в порядке возрастания Y и преобразовать в массив элементов {X: double,Y:int}" +
               "\nд) Дано два целочислельных массива положительных чисел, определить все возможные пары из элементов массивов(первая цифра - число из первого массива, вторая цифра - число из второго массива) кратные 5." +
               "\nе) Отсортировать слова по алфавиту из предложенного строкового массива, содержащих слог \"от\". Не забыть про сравнение в любом регистре!" +
               "\nж) Даны 2 строки s1 и s2. Из каждой можно читать по одному символу. Выяснить, является ли строка s2 обратной s1." +
               "\nз) Дан массив целых чисел. Сгруппировать их по четности и отсортировать по возрастанию." +
               "\nи) Дан массив целых чисел. Сгруппировать их по четности.Для каждой группы посчитать сумму входящих в нее элементов. Итоговая коллекция должна содержать для каждой группы поле, с суммой группы." +
               "\nк) Дана коллекция пар {Фамилия, Сумма} - Фамилия не ключевое поле(т.е.значения в поле Фамилия повторяются в коллекции. Необходимо составить итоговую коллекцию пар: {Фамилия, Сумма всех Сумм для данной фамилии}" +
               "\nл) Дана коллекция повторяющихя элементов. Необходимо составить новую коллекцию, в которую попадут в одном экземпляре только элементы, встречающиеся ровно три раза в исходной коллекции." +
               "\nм) Отсортировать коллекцию пар значений сначала по - первому элементу по возрастанию, затем по - второму элементу по убыванию" +
               "\nн) Есть три коллекции arr1, arr2, arr3 - необходимо создать коллекцию, состоящую из всех возможных троек элементов. Каждый элемент тройки представляет собой один элемент из соответствующе коллекции. Преобразовать итоговую коллекцию в строку типа: (a1, b1, c1), (a2, b1, c1), ... !!!Последнего символа запятая быть не должно :) !!!Использовать только LINQ!");
        }
        static void Main()
        {
            #region glob vars
            #pragma warning disable CA1416 
            Console.SetWindowSize(126, 40);
            #pragma warning restore CA1416
            var temp = int.MinValue;
            int[] array = { 123, 64, 332, 8, 2, -2 };
            #endregion
            char selector;
            Menu();            
            do
            {
                Console.Write(">> "); selector = Console.ReadKey().KeyChar; Console.WriteLine();
                switch (selector)
                {
                    case 'a' or 'а':
                        #region TASK A       
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\n\t\t\b\b\bTASK А");
                        Console.ResetColor();
                        for (int i = 0; i < array.Length; i++)
                            if (array[i] > temp) temp = array[i];
                        Console.WriteLine($"without LINQ: {temp}");
                        Console.WriteLine("--------------------------------");
                        Console.WriteLine("   with LINQ: " + array.Max());                       
                        #endregion
                        break;
                    case 'б':
                        #region TASK Б                  
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\n\t\t\b\b\bTASK Б");
                        Console.ResetColor();
                        temp = int.MinValue;
                        int index = 0;
                        for (var item = 0; item < array.Length; item++)
                            if (array[item] > temp)
                            {
                                temp = array[item];
                                index = item;
                            }
                        Console.WriteLine($"without LINQ: {index}");
                        Console.WriteLine("--------------------------------");
                        Console.WriteLine("   with LINQ: " + array.Select((item, index) => (item, index)).Max().index);                      
                        #endregion
                        break;
                    case 'в':
                        #region TASK B
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\n\t\t\b\b\bTASK В");
                        Console.ResetColor();
                        List<MyStruct> arrayOfStruct = new() { new MyStruct { X = 2, Y = 4 }, new MyStruct { X = 0, Y = 11 }, new MyStruct { X = 6, Y = 3 }, new MyStruct { X = 9, Y = 5 } };
                        MyStruct temp1 = new() { Y = int.MinValue };
                        foreach (var item in arrayOfStruct)
                            if (item.Y > temp1.Y) temp1 = item;
                        Console.WriteLine($"without LINQ: {temp1}");
                        Console.WriteLine("--------------------------------");
                        Console.WriteLine("   with LINQ: " + arrayOfStruct.OrderByDescending(obj => obj.Y).FirstOrDefault()); // max by y                       
                        #endregion
                        break;
                    case 'г':
                        #region TASK Г
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\n\t\t\b\b\bTASK Г");
                        Console.ResetColor();
                        List<XintYdouble> xintYdoubles = new() { new XintYdouble { X = -6, Y = 2.7 }, new XintYdouble { X = 2, Y = 1.5 }, new XintYdouble { X = 7, Y = 5.2 }, new XintYdouble { X = -3, Y = -7.8 } };

                        Console.WriteLine("with LINQ:");
                        Stopwatch stopwatch1 = new();
                        stopwatch1.Start();
                        var ans = xintYdoubles.AsEnumerable()
                                .Select(x => Convert.ToDouble(x.X))
                                .Zip(xintYdoubles.Select(y => Convert.ToInt32(y.Y)))
                                .OrderBy(item => item.Second);
                        foreach (var item in ans)
                            Console.WriteLine(item);
                        stopwatch1.Stop();
                        Console.WriteLine($"\t\t\t{stopwatch1.Elapsed.TotalMilliseconds}ms");

                        Console.WriteLine("--------------------------------");

                        Console.WriteLine("without LINQ:");
                        stopwatch1.Restart();
                        xintYdoubles.Sort((x, y) => x.Y.CompareTo(y.Y)); //это не LINQ 
                        var Templist = new List<KeyValuePair<double, int>>();
                        foreach (var item in xintYdoubles)
                            Templist.Add(new KeyValuePair<double, int>(Convert.ToDouble(item.X), Convert.ToInt32(item.Y)));
                        foreach (var item in Templist)
                            Console.WriteLine(item);
                        stopwatch1.Stop();
                        Console.WriteLine($"\t\t\t{stopwatch1.Elapsed.TotalMilliseconds}ms");
                        #endregion
                        break;
                    case 'д':
                        #region TASK Д
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\n\t\t\b\b\bTASK Д");
                        Console.ResetColor();
                        int[] a = { 2, 4, 5, 15, 7 };
                        int[] b = { 5, 1, -2, 4 };

                        var end = a.Where(it1 => it1 % 5 == 0)
                            .SelectMany(first => b.Where(it2 => it2 % 5 == 0)
                            .Select(second => new { first, second }));
                        Console.WriteLine("with LINQ:");
                        foreach (var item in end)
                            Console.WriteLine(item);

                        Console.WriteLine("--------------------------------");

                        Console.WriteLine("without LINQ:");
                        for (int i = 0; i < a.Length; i++)
                            for (int j = 0; j < b.Length; j++)
                                if (a[i] % 5 == 0 && b[j] % 5 == 0)
                                    Console.WriteLine($"{a[i]}, {b[j]}");                       
                        #endregion
                        break;
                    case 'е':
                        #region TASK E
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\n\t\t\b\b\bTASK Е");
                        Console.ResetColor();
                        var stringArray = new string[] { "рок", "енот","Азот","саМолет", "поЛЕТ", "ОтхоД"};

                        Console.WriteLine("with LINQ:");
                        var well = stringArray.Where(ite => ite.ToLower().Contains("от")).OrderBy(it => it);
                        foreach (var item in well)                       
                            Console.WriteLine(item);

                        Console.WriteLine("--------------------------------");

                        Console.WriteLine("without LINQ:");
                        var temparray = new List<string>();
                        foreach (var item in stringArray)
                            if (item.ToLower().Contains("от"))
                                temparray.Add(item);
                        temparray.Sort((x, y) => x.CompareTo(y));
                        foreach (var item in temparray)
                            Console.WriteLine(item);                      
                        #endregion
                        break;
                    case 'ж':
                        #region TASK Ж
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\n\t\t\b\b\bTASK Ж");
                        Console.ResetColor();
                        string s1 = "dela", s2 = "aled";

                        Console.WriteLine("without LINQ:");
                        int flag = s2.Length - 1;
                        bool result = true;                      
                        for (int j = 0; j < s1.Length; j++)
                        {
                            if (s1[j] != s2[flag])
                            {
                                result = false;
                                break;
                            }
                            else result = true;
                            flag--;
                        }                      
                        Console.WriteLine(result);

                        Console.WriteLine("--------------------------------");

                        Console.WriteLine("with LINQ:");
                        var resultstring = (new string(s2.Reverse().ToArray())).Equals(s1);                    
                        Console.WriteLine(resultstring);
                        #endregion
                        break;
                    case 'з':
                        #region TASK З
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\n\t\t\b\b\bTASK З");
                        Console.ResetColor();
                        var pol = new int[] { 2, 6, 9, 4, -3, 7, 14, 5, 1, 9 };

                        Console.WriteLine("with LINQ:");
                        Array.Sort(pol);
                        var welp = (pol.Where(x => x % 2 == 0), pol.Where(y => y % 2 != 0));

                        Console.WriteLine("Chet:");
                        foreach (var item in welp.Item1)
                            Console.WriteLine(item);
                        Console.WriteLine("Nechet:");
                        foreach (var item in welp.Item2)
                            Console.WriteLine(item);

                        Console.WriteLine("--------------------------------");

                        Console.WriteLine("without LINQ:");
                        Array.Sort(pol);

                        var chet = new List<int>();
                        var nechet = new List<int>();

                        foreach (var item in pol)
                            if (item % 2 == 0)
                                chet.Add(item);
                            else nechet.Add(item);

                        Console.WriteLine("Chet:");
                        foreach (var item in chet)
                            Console.WriteLine(item);

                        Console.WriteLine("Nechet:");

                        foreach (var item in nechet)
                            Console.WriteLine(item);
                        #endregion
                        break;
                    case 'и':
                        #region TASK И
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\n\t\t\b\b\bTASK И");
                        Console.ResetColor();
                        var ara = new int[] { 2, 6, 9, 4, -3, 7, 14, 5, 1, 9 };

                        Console.WriteLine("with LINQ:");
                        var yoyo = ((ara.Where(x => x % 2 == 0), ara.Where(x => x % 2 == 0).Sum()), (ara.Where(x => x % 2 != 0), ara.Where(x => x % 2 != 0).Sum()));

                        foreach (var item in yoyo.Item1.Item1) //chet   
                            Console.WriteLine(item);
                        Console.WriteLine("SUM: " + yoyo.Item1.Item2); //sum chet


                        foreach (var item in yoyo.Item2.Item1) //nechet
                            Console.WriteLine(item);
                        Console.WriteLine("SUM: " + yoyo.Item2.Item2); //sum nechet

                        Console.WriteLine("--------------------------------");
                       

                        Console.WriteLine("without LINQ:");
                        var chett = new List<int>();
                        var nechett = new List<int>();

                        foreach (var item in ara)
                            if (item % 2 == 0)
                                chett.Add(item);
                            else nechett.Add(item);

                        #region SUM for chet
                        int sum = 0;
                        foreach (var item in chett)
                            sum += item;
                        #endregion
                        #region SUM for nechet
                        int sum1 = 0;
                        foreach (var item in nechett)
                            sum1 += item;
                        #endregion

                        var together = ((chett, sum), (nechett, sum1));

                        Console.WriteLine("Chet:");
                        foreach (var item in together.Item1.chett)
                            Console.WriteLine(item);
                        Console.WriteLine("SUM: " + together.Item1.sum);

                        Console.WriteLine("Nechet:");
                        foreach (var item in together.Item2.nechett)
                            Console.WriteLine(item);
                        Console.WriteLine("SUM: " + together.Item2.sum1);
                        #endregion
                        break;
                    case 'к':
                        #region TASK К
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\n\t\t\b\b\bTASK К");
                        Console.ResetColor();
                        var arr = new Worker[]
{
                new Worker{Name = "Петров", Salary = 100},
                new Worker{Name = "Сидоров", Salary = 200},
                new Worker{Name = "Петров", Salary = 130}
};

                        Console.WriteLine("without LINQ:");
                        var mapped = new Dictionary<string, int>();
                        foreach (var item in arr)
                            if (!mapped.ContainsKey(item.Name))
                                mapped.Add(item.Name, 0);

                        int workers_sum = 0;
                        foreach (var item in mapped)
                        {
                            foreach (var worker in arr)
                            {
                                if (item.Key.Equals(worker.Name))
                                    workers_sum += worker.Salary;
                            }
                            mapped[item.Key] = workers_sum;
                            workers_sum = 0;
                        }

                        foreach (var item in mapped)
                            Console.WriteLine(item);
                        Console.WriteLine("--------------------------------");

                        Console.WriteLine("with LINQ:");
                        var lmml = arr.GroupBy(s => s.Name).Select(sel => new { sel.Key, Sum = sel.Sum(y => y.Salary) });
                        foreach (var item in lmml)
                            Console.WriteLine(item);                      
                        #endregion
                        break;
                    case 'л':
                        #region TASK Л
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\n\t\t\b\b\bTASK Л");
                        Console.ResetColor();

                        var lst = new int[] { 2,7,2,5,3,1,7,2,7,3};

                        Console.WriteLine("with LINQ:");
                        var queue = lst.GroupBy(x => x)
                            .Where(g => g.Count() > 2) //3 times
                            .Select(y => y.Key)
                            .ToList();
                        foreach (var item in queue)
                            Console.WriteLine(item);
                        Console.WriteLine("--------------------------------");

                        Console.WriteLine("without LINQ:");
                        int counter = 0;
                        var tempa = new HashSet<int>(); //заюзаем фичи hashset`a
                        for (int i = 0; i < lst.Length; i++)
                        {
                            if (!tempa.Contains(lst[i]))
                            {
                                for (int j = i; j < lst.Length - 1; j++)
                                    if (lst[i] == lst[j]) counter++;

                                if (counter == 3) tempa.Add(lst[i]);
                            }
                            counter = 0;
                        }

                        foreach (var item in tempa)
                            Console.WriteLine(item);                        
                        #endregion
                        break;
                    case 'м':
                        #region TASK М
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\n\t\t\b\b\bTASK М");
                        Console.ResetColor();
                        var hehe = new List<KeyValuePair<int, int>>() {
                new KeyValuePair<int, int>(13, 7),
                new KeyValuePair<int, int>(2, 1),
                new KeyValuePair<int, int>(13, 3),
                new KeyValuePair<int, int>(-4, 2),
            };

                        Console.WriteLine("with LINQ:");
                        var me = hehe.OrderBy(x => x.Key).OrderByDescending(c => c.Value);
                        foreach (var item in me)
                            Console.WriteLine(item);
                        Console.WriteLine("--------------------------------");

                        Console.WriteLine("without LINQ:");
                        hehe.Sort((x, y) => x.Key.CompareTo(y.Key)); //это не LINQ :)
                        hehe.Sort((x, y) => y.Value.CompareTo(x.Value));
                        foreach (var item in hehe)
                            Console.WriteLine(item);
                        #endregion
                        break;
                    case 'н':
                        #region TASK Н                     
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\n\t\t\b\b\bTASK Н");
                        Console.ResetColor();
                        var arr1 = new string[] { "a1", "a2", "a3" };
                        var arr2 = new string[] { "b1", "b2", "b3" };
                        var arr3 = new string[] { "c1", "c2", "c3" };

                        var del = string.Join(", ", arr3.Where(x => x != null)
                             .SelectMany(item1 => arr2.Where(c => c != null)
                             .SelectMany(item3 => arr1.Where(it => it != null)
                             .Select(item2 => new { item2, item3, item1 }))));

                        del = del.Replace("item1 = ", null); del = del.Replace("item2 = ", null); del = del.Replace("item3 = ", null); del = del.Replace("{ ", "("); del = del.Replace(" }", ")");

                        Console.WriteLine(del);
                        #endregion
                        break;
                    case 'c' or 'C' or 'с' or 'C':
                        {
                            SetConsoleOutputCP(866);
                            SetConsoleCP(866);
                            Console.Clear();
                            Menu();
                        }
                        break;
                    case (char)ConsoleKey.Escape:
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("GGoodbye!"); //bcz of prev cw we should duplicate
                            Console.ResetColor();
                        }
                        break;
                    #region ???
                    case (char)ConsoleKey.Y:
                        Console.OutputEncoding = System.Text.Encoding.Unicode;
                        for (int i = 1; i < 11; i++)
                        {
                            SetConsoleOutputCP(932);
                            SetConsoleCP(932);
                            Console.ForegroundColor = (ConsoleColor)i;
                            Console.WriteLine("やめてください！");
                        }
                        Console.ResetColor();                     
                        break;
                    #endregion
                    default:
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Unkown command!");
                            Console.ResetColor();
                        }
                        break;
                }
            } while (selector != Convert.ToChar(ConsoleKey.Escape));           
        }
    }
}
