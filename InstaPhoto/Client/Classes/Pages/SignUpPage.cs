using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Client.Interfaces;

namespace Client.Classes.Pages
{
    public class SignUpPage : IPage
    {
        private const string RetryAction = "R";
        private const string BackAction = "Z";
        private const string ExitAction = "X";

        private string _username;
        private string _password;

        private bool _askRetry;
        private bool _validSignup;
        
        private readonly IPageNavigation _navigation;
        private readonly Menu _menu;

        public SignUpPage(IPageNavigation navigation)
        {
            _navigation = navigation;
            _menu = new Menu(
                options: new List<(string, string)>
                {
                    (RetryAction, "Try Again"),
                    (BackAction, "Back"),
                    (ExitAction, "Exit"),
                },
                RetryActionSelected
            );
        }

        public async Task RenderAsync()
        {
            ConsoleHelper.WriteLine("Sign up\n", ConsoleColor.Cyan);

            if (_askRetry)
                RenderRetry();
            else
                await RenderLoginAsync();
        }

        private async Task RenderLoginAsync()
        {
            ConsoleHelper.Write("Username: ");
            _username = ConsoleHelper.ReadLine();
            ConsoleHelper.Write("Password: ");
            _password = ConsoleHelper.ReadLine();

            await DoSignUp();

            if (_validSignup)
                _navigation.GoToPage(IPageNavigation.HomePage);

            // Wrong login, retry
            _askRetry = true;
        }

        private async Task DoSignUp()
        {
            ConsoleHelper.WriteLine("\nSigning up...");

            // TODO: CALL Sign up
            Thread.Sleep(500);
            _validSignup = _username != "admin";
        }

        private void RenderRetry()
        {
            ConsoleHelper.WriteLine($"Username: {_username}");
            ConsoleHelper.WriteLine($"Password: {_password}");

            ConsoleHelper.WriteLine("\nUsername Already in use!\n", ConsoleColor.Red);
            
            _menu.Render();
        }

        private void RetryActionSelected(string selectedOption)
        {
            switch (selectedOption)
            {
                case RetryAction:
                    _askRetry = false;
                    break;
                case BackAction:
                    _navigation.Back();
                    break;
                case ExitAction:
                    _navigation.Exit();
                    break;
            }
        }
    }
}