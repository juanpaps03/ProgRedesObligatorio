using System;
using System.Collections.Generic;

namespace Client.Classes
{
    public class Menu
    {
        private readonly IList<(string, string)> _options;
        private readonly Action<string> _onSelect;

        private int _selectedIndex = 0;

        public Menu(IList<(string, string)> options, Action<string> onSelect)
        {
            if (options.Count == 0)
                throw new Exception("Menu with no options");

            _options = options;
            _onSelect = onSelect;
        }

        public void Render()
        {
            ConsoleHelper.WriteLine("Select an option:", ConsoleColor.Yellow);

            var n = _options.Count;
            for (var i = 0; i < n; i++)
            {
                var prefix = "  ";
                var color = ConsoleHelper.DefaultColor;
                var background = ConsoleHelper.DefaultBackground;
                if (i == _selectedIndex)
                {
                    color = ConsoleHelper.DefaultBackground;
                    background = ConsoleHelper.DefaultColor;
                    prefix = "> ";
                }

                ConsoleHelper.WriteLine($"{prefix}{_options[i].Item2}", color, background);
            }
            
            ConsoleHelper.WriteLine();
            ConsoleHelper.WriteLine();

            switch (ConsoleHelper.ReadKey())
            {
                case ConsoleKey.Enter:
                    _onSelect(_options[_selectedIndex].Item1);
                    break;
                case ConsoleKey.UpArrow:
                    _selectedIndex = ((_selectedIndex - 1) % n + n) % n;
                    break;
                case ConsoleKey.DownArrow:
                    _selectedIndex = ((_selectedIndex + 1) % n + n) % n;
                    break;
            }
        }
    }
}