namespace Client.Interfaces
{
    public interface IPageCreator
    {
        enum PageId
        {
            LandingPage,
            LoginPage,
            SignUpPage,
            HomePage,
        }

        public IPage CreatePage(PageId id);
    }
}