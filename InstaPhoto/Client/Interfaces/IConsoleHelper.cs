using System;
using System.Collections.Generic;

namespace Client.Interfaces
{
    public interface IConsoleHelper
    {
        public const ConsoleColor DefaultConsoleColor = ConsoleColor.Black;

        void Clear();
        void Write(string text, ConsoleColor color = DefaultConsoleColor);
        void WriteLine(string text, ConsoleColor color = DefaultConsoleColor);
        string Read();
        char ShowMenu(Dictionary<char, string> options);
    }
}