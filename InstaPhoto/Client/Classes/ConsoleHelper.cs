using System;
using System.Collections.Generic;
using System.Linq;
using Client.Interfaces;

namespace Client.Classes
{
    public class ConsoleHelper
    {
        public const ConsoleColor DefaultColor = ConsoleColor.Black;
        public const ConsoleColor DefaultBackground = ConsoleColor.White;

        public static void Clear()
        {
            Console.ForegroundColor = DefaultColor;
            Console.BackgroundColor = DefaultBackground;
            Console.Clear();
        }

        public static void Write(
            string text = "",
            ConsoleColor color = DefaultColor,
            ConsoleColor background = DefaultBackground
        )
        {
            Console.ForegroundColor = color;
            Console.BackgroundColor = background;
            Console.Write(text);
            Console.ForegroundColor = DefaultColor;
            Console.BackgroundColor = DefaultBackground;
        }

        public static void WriteLine(
            string text = "",
            ConsoleColor color = DefaultColor,
            ConsoleColor background = DefaultBackground
        )
        {
            Write(text + "\n", color, background);
        }

        public static string ReadLine()
        {
            return Console.ReadLine();
        }

        public static ConsoleKey ReadKey()
        {
            return Console.ReadKey().Key;
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