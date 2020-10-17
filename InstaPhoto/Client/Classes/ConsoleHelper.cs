using System;
using System.Collections.Generic;
using System.Linq;
using Client.Interfaces;

namespace Client.Classes
{
    public class ConsoleHelper: IConsoleHelper
    {
        public void Clear()
        {
            Console.ForegroundColor = IConsoleHelper.DefaultConsoleColor;
            Console.Clear();
        }

        public void Write(string text, ConsoleColor color = IConsoleHelper.DefaultConsoleColor)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = IConsoleHelper.DefaultConsoleColor;
        }

        public void WriteLine(string text, ConsoleColor color = IConsoleHelper.DefaultConsoleColor)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = IConsoleHelper.DefaultConsoleColor;
        }

        public string Read()
        {
            return Console.ReadLine();
        }

        public char ShowMenu(Dictionary<char, string> options)
        {
            WriteLine("Select an option:", ConsoleColor.Yellow);
            foreach (var (id, text) in options)
            {
                Console.WriteLine($"\t<{id}> {text}");
            }
            return ReadUppercaseChar();
        }
        private char ReadUppercaseChar()
        {
            return Console.ReadKey().KeyChar.ToString().ToUpper().First();
        }
    }
}