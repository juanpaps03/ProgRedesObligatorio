using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Interfaces;

namespace Client.Classes.Pages
{
    public class LandingPage : IPage
    {
        private static readonly Dictionary<char, string> MenuOptions =
            new Dictionary<char, string>
            {
                {LoginAction, "Login"},
                {SignUpAction, "Sign up"},
                {ExitAction, "Exit"},
            };
        
        private const char LoginAction = 'L';
        private const char SignUpAction = 'S';
        private const char ExitAction = 'X';

        private readonly Dictionary<char, string> _nextPage;

        private bool _error;
        private char _optionSelected = '\0';

        private readonly IPageNavigation _navigation;
        public LandingPage(IPageNavigation navigation)
        {
            _navigation = navigation;
            _nextPage = new Dictionary<char, string>
            {
                {LoginAction, IPageNavigation.LoginPage},
                {SignUpAction, IPageNavigation.SignUpPage},
                {ExitAction, null},
            };
        }

        public async Task RenderAsync()
        {
            ConsoleHelper.WriteLine("\n>> Welcome to InstaPhoto! <<\n", ConsoleColor.Cyan);

            if (_error)
            {
                ConsoleHelper.WriteLine($"Option <{_optionSelected}> not recognized!\n", ConsoleColor.Red);
            }

            _optionSelected = ConsoleHelper.ShowMenu(MenuOptions);
            if (_nextPage.ContainsKey(_optionSelected))
            {
                if (_nextPage[_optionSelected] != null)
                    _navigation.GoToPage(_nextPage[_optionSelected]);
                else
                    _navigation.Exit();
            }

            // Wrong option, re render the page with error
            _error = true;
        }
    }
}