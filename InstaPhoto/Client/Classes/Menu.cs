using System;
using System.Collections.Generic;

namespace Client.Classes
{
    public class Menu
    {
        private readonly IList<(string, string)> _options;
        private readonly Action<string> _onSelect;
        private readonly Action _onEscPressed;
        private readonly string _escapeActionName;

        private int _selectedIndex = 0;

        public Menu(
            IList<(string, string)> options, 
            Action<string> onSelect,
            Action onEscPressed = null,
            string escapeActionName = null
        )
        {
            _options = options;
            _onSelect = onSelect;
            _onEscPressed = onEscPressed;
            _escapeActionName = escapeActionName;
        }

        public void Render()
        {
            ConsoleHelper.WriteLine("Select an option:", ConsoleColor.Yellow);

            var n = _options.Count;
            if (n > 0)
            {
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
            }
            else
            {
                ConsoleHelper.WriteLine("  <None>");
            }

            if(_onEscPressed != null)
                ConsoleHelper.WriteLine($"Esc - {_escapeActionName}");
            
            ConsoleHelper.WriteLine();
            ConsoleHelper.WriteLine();

            switch (ConsoleHelper.ReadKey())
            {
                case ConsoleKey.Enter:
                    _onSelect(_options[_selectedIndex].Item1);
                    break;
                case ConsoleKey.UpArrow:
                    if (n > 0)
                        _selectedIndex = ((_selectedIndex - 1) % n + n) % n;
                    break;
                case ConsoleKey.DownArrow:
                    if (n > 0)
                        _selectedIndex = ((_selectedIndex + 1) % n + n) % n;
                    break;
                case ConsoleKey.Escape:
                    _onEscPressed?.Invoke();
                    break;
            }
        }
    }
}