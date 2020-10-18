using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly Dictionary<char, IPage> _nextPage;

        private bool _error;
        private char _optionSelected = '\0';

        public LandingPage(IPageCreator pageCreator)
        {
            _nextPage = new Dictionary<char, IPage>
            {
                {LoginAction, pageCreator.CreatePage(IPageCreator.PageId.LoginPage)},
                {SignUpAction, pageCreator.CreatePage(IPageCreator.PageId.SignUpPage)},
                {ExitAction, null},
            };
        }

        public async Task<IPage> RenderAsync()
        {
            ConsoleHelper.WriteLine("\n>> Welcome to InstaPhoto! <<\n", ConsoleColor.Cyan);

            if (_error)
            {
                ConsoleHelper.WriteLine($"Option <{_optionSelected}> not recognized!\n", ConsoleColor.Red);
            }

            _optionSelected = ConsoleHelper.ShowMenu(MenuOptions);
            if (_nextPage.ContainsKey(_optionSelected))
                return _nextPage[_optionSelected];

            // Wrong option, re render the page with error
            _error = true;
            return this;
        }
    }
}