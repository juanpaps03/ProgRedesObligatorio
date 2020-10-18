using System;
using Client.Classes.Pages;
using Client.Interfaces;

namespace Client.Classes
{
    public class PageCreator: IPageCreator
    {
        public IPage CreatePage(IPageCreator.PageId id)
        {
            return id switch
            {
                IPageCreator.PageId.LandingPage => new LandingPage(this),
                IPageCreator.PageId.LoginPage => new LoginPage(this),
                IPageCreator.PageId.SignUpPage => new SignUpPage(this),
                IPageCreator.PageId.HomePage => new HomePage(this),
                _ => throw new Exception("Unknown page")
            };
        }
    }
}