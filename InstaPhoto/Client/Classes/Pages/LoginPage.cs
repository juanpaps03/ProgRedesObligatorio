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

        private readonly IConsoleHelper _console;

        private string _username;
        private string _password;

        private bool _askRetry;
        private bool _retryOptionError;
        private char _selectedOption;

        public LoginPage(IConsoleHelper console)
        {
            _console = console;
        }

        public async Task<IPage> RenderAsync()
        {
            _console.WriteLine("Login\n", ConsoleColor.Cyan);

            return !_askRetry ? await RenderLoginAsync() : RenderRetry();
        }

        private async Task<IPage> RenderLoginAsync()
        {
            _console.Write("Username: ");
            _username = _console.Read();
            _console.Write("Password: ");
            _password = _console.Read();

            var validLogin = await DoLogin();

            if (validLogin)
                return null;

            // Wrong login, retry
            _askRetry = true;
            return this;
        }

        private async Task<bool> DoLogin()
        {
            _console.WriteLine("\nLogging in...");

            // TODO: CALL LOGIN
            Thread.Sleep(500);
            return _username == "admin" && _password == "admin";
        }

        private IPage RenderRetry()
        {
            _console.WriteLine($"Username: {_username}");
            _console.WriteLine($"Password: {_password}");

            _console.WriteLine("\nWrong username or password!\n", ConsoleColor.Red);
            if (_retryOptionError)
            {
                _console.WriteLine($"\nUnrecognized option <{_selectedOption}>\n", ConsoleColor.Red);
            }

            _selectedOption = _console.ShowMenu(RetryOptions);

            switch (_selectedOption)
            {
                case RetryAction:
                    _askRetry = false;
                    return this;
                case BackAction:
                    return new LandingPage(_console);
                case ExitAction:
                    return null;
                default:
                    _retryOptionError = true;
                    return this;
            }
        }
    }
}