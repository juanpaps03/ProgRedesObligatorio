using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Client.Interfaces;

namespace Client.Classes.Pages
{
    public class LoginPage : IPage
    {
        private const char RetryAction = 'R';
        private const char BackAction = 'Z';
        private const char ExitAction = 'X';
        
        private static readonly Dictionary<char, string> RetryOptions =
            new Dictionary<char, string>
            {
                {RetryAction, "Try Again"},
                {BackAction, "Back"},
                {ExitAction, "Exit"},
            };

        private string _username;
        private string _password;

        private bool _askRetry;
        private bool _retryOptionError;
        private char _selectedOption;

        private readonly IPageNavigation _navigation;
        public LoginPage(IPageNavigation navigation)
        {
            _navigation = navigation;
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
            _username = ConsoleHelper.Read();
            ConsoleHelper.Write("Password: ");
            _password = ConsoleHelper.Read();

            var validLogin = await DoLogin();

            if (validLogin)
                _navigation.GoToPage(IPageNavigation.HomePage);

            // Wrong login, retry
            _askRetry = true;
        }

        private async Task<bool> DoLogin()
        {
            ConsoleHelper.WriteLine("\nLogging in...");

            // TODO: CALL LOGIN
            Thread.Sleep(500);
            return _username == "admin" && _password == "admin";
        }

        private void RenderRetry()
        {
            ConsoleHelper.WriteLine($"Username: {_username}");
            ConsoleHelper.WriteLine($"Password: {_password}");

            ConsoleHelper.WriteLine("\nWrong username or password!\n", ConsoleColor.Red);
            if (_retryOptionError)
            {
                ConsoleHelper.WriteLine($"\nUnrecognized option <{_selectedOption}>\n", ConsoleColor.Red);
            }

            _selectedOption = ConsoleHelper.ShowMenu(RetryOptions);

            switch (_selectedOption)
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
                default:
                    _retryOptionError = true;
                    break;
            }
        }
    }
}