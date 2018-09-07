using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class ConsoleExtension
    {
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
            Console.ResetColor();
            Console.Write(">Сервер: ");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void PrintError(string errorMsg)
        {
            ClearLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Ошибка: " + errorMsg);
            Console.ResetColor();
            Console.Write(">Сервер: ");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void PrintText(string text)
        {
            ClearLine();
            Console.ResetColor();
            Console.WriteLine(text);
            Console.Write(">Сервер: ");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
