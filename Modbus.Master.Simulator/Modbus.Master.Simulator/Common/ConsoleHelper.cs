using System;

namespace Modbus.Master.Simulator.Common
{
    public class ConsoleHelper
    {
        public static void WriteLine(string text, ConsoleColor color, bool newLine = true)
        {
            Console.ForegroundColor = color;
            if (newLine)
                Console.WriteLine(text);
            else
                Console.Write(text);
            Console.ResetColor();
        }

        public static void Question(string text, bool newLine = true) => WriteLine(text, ConsoleColor.DarkYellow, newLine);
        public static void Success(string text, bool newLine = true) => WriteLine(text, ConsoleColor.Green, newLine);
        public static void Error(string text, bool newLine = true) => WriteLine(text, ConsoleColor.Red, newLine);
        public static void Info(string text, bool newLine = true) => WriteLine(text, ConsoleColor.Cyan, newLine);
        public static void Info2(string text, bool newLine = true) => WriteLine(text, ConsoleColor.White, newLine);
        public static void Warning(string text, bool newLine = true) => WriteLine(text, ConsoleColor.Yellow, newLine);

    }
}
