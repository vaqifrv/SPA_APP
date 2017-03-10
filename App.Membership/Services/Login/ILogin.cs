using System.Collections.Generic;

namespace App.Membership.Services.Login
{
    public interface ILogin
    {
        void LogOut();
        IList<string> GetUsersOnline();
        void SetUserOnlineStatus();
    }
    public interface ILogin<T> : ILogin
    {
        T LogIn(string userName, string password);
        T LogIn(string token);
    }
}
