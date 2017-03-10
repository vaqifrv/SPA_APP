using System;
using System.Collections.Generic;
using App.Membership.Services.Logging;
using App.Membership.Services.Logging.Enums;

namespace App.Membership.Services.Login
{
    public class WinFormsLogin : BaseLogin<bool>
    {
        protected override bool ValidateUser(string userName, string password)
        {
            return true;
        }

        public override IList<string> GetUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override void SetUserOnlineStatus()
        {
            
        }

        public override bool LogIn(string userName, string password)
        {
            if (ValidateUser(userName, password))
            {
                LoggingService.Current.WriteLogActionLogin(userName, LogActionResult.Success);
                return true;
            }
            else
            {
                LoggingService.Current.WriteLogActionLogin(userName, LogActionResult.Failed);
                return false;
            }
        }

        public override bool LogIn(string token)
        {
            throw new NotImplementedException();
        }
    }
}
