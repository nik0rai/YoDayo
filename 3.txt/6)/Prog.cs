using System;
using System.Diagnostics;
using System.Threading;

namespace ConsoleApp5
{
    public delegate void KeyPressEventDelegate();
    class KeyLogger
    {
        public event KeyPressEventDelegate DigitKeyPressed;
        public event KeyPressEventDelegate AnyKeyPressed;

        public void DigitKeyPressedInvoke() => DigitKeyPressed?.Invoke();
        public void AnyKeyPressedInvoke() => AnyKeyPressed?.Invoke();
    }
    class Subscribers
    {
        private static ConsoleKey key;
        public static void DigitPressed()
        {
            while (true)
            {
                key = Console.ReadKey(true).Key;
                if (char.IsDigit((char)key)) Run();
            }
        }
        public static void AnyKeyPressed() => Run();

        private static void Run()
        {
            using QuedTask quedTask = new();
            for (int i = 0; i < 3; i++) quedTask.Task($"Task {i}");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            KeyLogger keyLog = new();

            keyLog.DigitKeyPressed += Subscribers.DigitPressed;
            keyLog.AnyKeyPressed += Subscribers.AnyKeyPressed;
            keyLog.AnyKeyPressedInvoke();
        }
    }
}
