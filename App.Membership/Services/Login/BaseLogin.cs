using System.Collections.Generic;
using App.Membership.Services.Logging;

namespace App.Membership.Services.Login
{
    public abstract class BaseLogin<T> : ILogin<T>
    {
        protected abstract T ValidateUser(string userName, string password);
        public abstract T LogIn(string userName, string password);
        public abstract T LogIn(string token);
        public virtual void LogOut()
        {
            LoggingService.Current.WriteLogActionLogOut();
        }

        public abstract IList<string> GetUsersOnline();

        public abstract void SetUserOnlineStatus();


        
    }
}
