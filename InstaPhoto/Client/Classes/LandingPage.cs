using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Interfaces;

namespace Client.Classes
{
    public class LandingPage : IPage
    {
        private static readonly Dictionary<char, string> MenuOptions =
            new Dictionary<char, string>
            {
                {'l', "Login"},
                {'s', "Sign up"},
                {'x', "Exit"},
            };
        
        private readonly IConsoleHelper _console;

        private readonly Dictionary<char, IPage> _nextPage;

        private bool _error;
        private char _optionSelected = '\0';

        public LandingPage(IConsoleHelper console)
        {
            _console = console;
            _nextPage = new Dictionary<char, IPage>
            {
                {'l', null},
                {'s', null},
                {'x', null},
            };
        }

        public async Task<IPage> RenderAsync()
        {
            _console.Clear();
            _console.WriteLine("\n>> Welcome to InstaPhoto! <<\n", ConsoleColor.Cyan);

            if (_error)
            {
                _console.WriteLine($"Option <{_optionSelected}> not recognized!\n", ConsoleColor.Red);
            }

            _optionSelected = _console.ShowMenu(MenuOptions);
            if (_nextPage.ContainsKey(_optionSelected.ToString().ToLower().First()))
                return _nextPage[_optionSelected];

            // Wrong option, re render the page with error
            _error = true;
            return this;
        }
    }
}