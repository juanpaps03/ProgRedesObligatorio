using System.Collections.Generic;

namespace Client.Interfaces
{
    public interface IPageNavigation
    {
        const string LandingPage = "LandingPage";
        const string LoginPage = "LoginPage";
        const string SignUpPage = "SignUpPage";
        const string HomePage = "HomePage";
        const string UploadPhotoPage = "UploadPhoto";
        const string PhotoListPage = "PhotoListPage";
        const string UserListPage = "UserListPage";
        const string CommentPhotoPage = "CommentPhotoPage";
        const string CommentListPage = "CommentListPage";
        const string PhotoDetailsPage = "PhotoDetailsPage";

        void GoToPage(string page, Dictionary<string, string> parameters = null);

        void Back();

        IPage Top();
        
        void Exit();
    }
}