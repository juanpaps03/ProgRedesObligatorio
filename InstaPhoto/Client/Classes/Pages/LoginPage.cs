using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Client.Interfaces;

namespace Client.Classes.Pages
{
    public class LoginPage : IPage
    {
        private const string RetryAction = "R";
        private const string BackAction = "Z";
        private const string ExitAction = "X";

        private string _username;
        private string _password;

        private bool _askRetry;
        private bool _validLogin;

        private readonly IPageNavigation _navigation;
        private readonly Menu _menu;

        public LoginPage(IPageNavigation navigation)
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
            ConsoleHelper.WriteLine("Login\n", ConsoleColor.Cyan);

            if (!_askRetry)
                await RenderLoginAsync();
            else
                RenderRetry();
        }

        private async Task RenderLoginAsync()
        {
            ConsoleHelper.Write("Username: ");
            _username = ConsoleHelper.ReadLine();
            ConsoleHelper.Write("Password: ");
            _password = ConsoleHelper.ReadLine();

            await DoLogin();

            if (_validLogin)
                _navigation.GoToPage(IPageNavigation.HomePage);

            // Wrong login, retry
            _askRetry = true;
        }

        private async Task DoLogin()
        {
            ConsoleHelper.WriteLine("\nLogging in...");

            // TODO: CALL LOGIN
            Thread.Sleep(500);
            _validLogin = _username == "admin" && _password == "admin";
        }

        private void RenderRetry()
        {
            ConsoleHelper.WriteLine($"Username: {_username}");
            ConsoleHelper.WriteLine($"Password: {_password}");

            ConsoleHelper.WriteLine("\nWrong username or password!\n", ConsoleColor.Red);
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