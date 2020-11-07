using System;
using System.Collections.Generic;
using Client.Classes.Pages;
using Client.Interfaces;

namespace Client.Classes
{
    public class PageNavigation : IPageNavigation
    {
        private Stack<IPage> _stack = new Stack<IPage>();

        public void GoToPage(string page, Dictionary<string, string> parameters = null)
        {
            switch (page)
            {
                case IPageNavigation.LandingPage:
                    _stack.Push(new LandingPage(this));
                    break;
                case IPageNavigation.LoginPage:
                    _stack.Push(new LoginPage(this));
                    break;
                case IPageNavigation.SignUpPage:
                    _stack.Push(new SignUpPage(this));
                    break;
                case IPageNavigation.HomePage:
                    _stack.Push(new HomePage(this));
                    break;
                default:
                    throw new Exception($"Page {page} not found");
            }
        }

        public void Back()
        {
            if (_stack.Count > 0)
                _stack.Pop();
        }

        public IPage Top()
        {
            return _stack.Count > 0 ? _stack.Peek() : null;
        }

        public void Exit()
        {
            _stack.Clear();
        }
    }
}