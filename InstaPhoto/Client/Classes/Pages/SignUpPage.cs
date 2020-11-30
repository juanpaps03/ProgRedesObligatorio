using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Client.Interfaces;
using SocketLibrary.Interfaces;
using SocketLibrary.Messages;
using SocketLibrary.Messages.CreateUser;
using SocketLibrary.Messages.Error;
using SocketLibrary.Messages.Login;

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
        private readonly IProtocolCommunication _protocolCommunication;

        public SignUpPage(IPageNavigation navigation, IProtocolCommunication protocolCommunication)
        {
            _navigation = navigation;
            _protocolCommunication = protocolCommunication;
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
            
            Response response = await _protocolCommunication.SendRequestAsync(
                new CreateUserRequest(_username, _password)
            );
            switch (response)
            {
                case ErrorResponse errorResponse:
                    ConsoleHelper.WriteLine(errorResponse.Message, ConsoleColor.Red);
                    _validSignup = false;
                    RenderRetry();
                    break;
                case CreateUserResponse createUserResponse:
                    ConsoleHelper.WriteLine("Success SignUp!\n", ConsoleColor.Green);
                    ConsoleHelper.WriteLine($"Welcome {_username}!\n");
                    _validSignup = true;
                    break;
            }
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