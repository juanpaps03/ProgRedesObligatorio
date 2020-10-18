using System;
using System.Collections.Generic;
using System.Linq;
using Client.Interfaces;

namespace Client.Classes
{
    public class ConsoleHelper
    {
        public const ConsoleColor DefaultConsoleColor = ConsoleColor.Black;
        
        public static void Clear()
        {
            Console.ForegroundColor = DefaultConsoleColor;
            Console.Clear();
        }

        public static void Write(string text, ConsoleColor color = DefaultConsoleColor)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = DefaultConsoleColor;
        }

        public static void WriteLine(string text, ConsoleColor color = DefaultConsoleColor)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = DefaultConsoleColor;
        }

        public static string Read()
        {
            return Console.ReadLine();
        }

        public static char ShowMenu(Dictionary<char, string> options)
        {
            WriteLine("Select an option:", ConsoleColor.Yellow);
            foreach (var (id, text) in options)
            {
                Console.WriteLine($"\t<{id}> {text}");
            }
            return ReadUppercaseChar();
        }
        private static char ReadUppercaseChar()
        {
            return Console.ReadKey().KeyChar.ToString().ToUpper().First();
        }
    }
}