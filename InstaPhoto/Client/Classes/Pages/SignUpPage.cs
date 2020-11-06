using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Client.Interfaces;

namespace Client.Classes.Pages
{
    public class SignUpPage : IPage
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

        public SignUpPage(IPageNavigation navigation)
        {
            _navigation = navigation;
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
            _username = ConsoleHelper.Read();
            ConsoleHelper.Write("Password: ");
            _password = ConsoleHelper.Read();

            var validSignUp = await DoSignUp();

            if (validSignUp)
                _navigation.GoToPage(IPageNavigation.HomePage);

            // Wrong login, retry
            _askRetry = true;
        }

        private async Task<bool> DoSignUp()
        {
            ConsoleHelper.WriteLine("\nSigning up...");

            // TODO: CALL Sign up
            Thread.Sleep(500);
            return _username != "admin";
        }

        private void RenderRetry()
        {
            ConsoleHelper.WriteLine($"Username: {_username}");
            ConsoleHelper.WriteLine($"Password: {_password}");

            ConsoleHelper.WriteLine("\nUsername Already in use!\n", ConsoleColor.Red);
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
                    _navigation.GoToPage(IPageNavigation.LandingPage);
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