using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public static class ConsoleExtension
    {
        public static string UserName;

        public static void ReadLineDrow()
        {
            Console.ResetColor();
            Console.Write($"{UserName}: ");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static string ReadLine()
        {
            string result = Console.ReadLine();
            ReadLineDrow();
            return result;
        }

        public static void ClearLine(int setLine)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, setLine);
        }

        public static void ClearLine()
        {
            int line = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, line);
        }

        public static void PrintBadInput(string errorMsg)
        {
            ClearLine(Console.CursorTop-1);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Ошибка: " + errorMsg);
            ReadLineDrow();
        }

        public static void PrintError(string errorMsg)
        {
            ClearLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Ошибка: " + errorMsg);
            ReadLineDrow();
        }

        public static void PrintText(string text)
        {
            ClearLine();
            Console.ResetColor();
            Console.WriteLine(text);
            ReadLineDrow();
        }

        public static void PrintMessage(string sender, string text)
        {
            ClearLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(sender + ": ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(text);

            ReadLineDrow();
        }
    }
}
