using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using App.Membership.Services.Locking;
using SWS = System.Web.Security;

namespace App.Membership.Services.Login
{
    public class AspnetLogin : BaseLogin<LoginStatus>
    {
        private static readonly IDictionary<string, UserOnlineInfo> UsersOnline = new Dictionary<string, UserOnlineInfo>();
        private static readonly HashSet<LoginAttempt> Attempts = new HashSet<LoginAttempt>();
        private static LockSettings _lockSetting;
        private static LockSettings LockSettings
        {
            get
            {
                if (_lockSetting == null)
                    LockSettings = new LockSettings();

                return _lockSetting;
            }
            set
            {
                _lockSetting = new LockSettings();
            }
        }



        protected override LoginStatus ValidateUser(string userName, string password)
        {
            TimeSpan interval = new TimeSpan();
            LoginAttempt attempt = null;
            DateTime currentTime = DateTime.Now;
            if (LockSettings.IsActive)
            {
                string ip = HttpContext.Current.Request.UserHostAddress;
                if (Attempts.Count > 2000) Thread.Sleep(2000);
                attempt = Attempts.Where(m => m.Ip == ip && m.UserName == userName).Select(m => m).FirstOrDefault();

                if (attempt == null)
                {
                    attempt = new LoginAttempt(ip, userName);
                }

                //            d= AttemptDate
                //Pa=   period unlock   (S@)
                //m=MaxLoginAttemptCount (S@)
                //p=AttemptPeriod (S@)
                //t=Current Time (@local)
                //c=FailedAttemptCount 

                interval = currentTime - attempt.AttemptDate;

                if (attempt.FailedAttemptCount >= LockSettings.MaxAttemptCount && (interval < LockSettings.UnlockPeriod))  //lockdur
                {
                    return LoginStatus.AttemptsExceed;
                }
                else if (attempt.FailedAttemptCount >= LockSettings.MaxAttemptCount && (interval >= LockSettings.UnlockPeriod))
                {
                    // deməli lock idi ancaq unlock period keçdiyi üçün lockdan çıxıb
                    attempt.FailedAttemptCount = 0;
                    //d=t;
                }
            }
            try
            {
                if (SWS.Membership.ValidateUser(userName, password))
                {
                    SWS.FormsAuthentication.SetAuthCookie(userName, false);
                    AddUserToOnlineList(userName);
                    return LoginStatus.Successful;
                }
                else if (LockSettings.IsActive)
                {
                    if (Attempts.Count % 3 == 0)
                    {
                        //  var t=m.AttemptDate.AddHours(LockSettings.UnlockPeriod.Hours)
                        Attempts.RemoveWhere(m => m.AttemptDate.AddHours(LockSettings.UnlockPeriod.Hours).AddMinutes(LockSettings.UnlockPeriod.Minutes).AddSeconds(LockSettings.UnlockPeriod.Seconds).AddDays(LockSettings.UnlockPeriod.Days) < currentTime);
                    }
                    if (interval <= LockSettings.AttemptPeriod)
                    {
                        if (attempt != null)
                        {
                            attempt.FailedAttemptCount += 1;
                            if (attempt.FailedAttemptCount >= LockSettings.MaxAttemptCount) attempt.AttemptDate = currentTime; //Deməli  d artıq last attempt olmalıdır
                        }
                    }
                    else
                    {
                        if (attempt != null)
                        {
                            attempt.AttemptDate = currentTime;
                            attempt.FailedAttemptCount = 1;
                        }
                    }
                    Attempts.Add(attempt);
                    return LoginStatus.Unsuccessful;
                }
                else
                    return LoginStatus.Unsuccessful;
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}", ex);
                return LoginStatus.Unsuccessful;
            }
        }

        private string UserSessionId
        {
            get
            {
                if (HttpContext.Current.Session["Agile.Solutions.Infrastructure.SessionID"] == null)
                {
                    HttpContext.Current.Session["Agile.Solutions.Infrastructure.SessionID"] = Guid.NewGuid().ToString();
                    return UserSessionId;
                }
                else
                    return HttpContext.Current.Session["Agile.Solutions.Infrastructure.SessionID"].ToString();
            }
        }

        private void AddUserToOnlineList(string userName)
        {
            if (!UsersOnline.Keys.Contains(UserSessionId))
                UsersOnline.Add(UserSessionId, new UserOnlineInfo
                {
                    Key = UserSessionId,
                    UserName = userName,
                    LastActivityTime = DateTime.Now
                });
        }

        public override void LogOut()
        {
            if (UsersOnline.ContainsKey(UserSessionId))
                UsersOnline.Remove(UserSessionId);
            base.LogOut();
            HttpContext.Current.Session.Abandon();
            SWS.FormsAuthentication.SignOut();


        }

        public override IList<string> GetUsersOnline()
        {
            return UsersOnline.Values
                .Where(x => x.LastActivityTime > DateTime.Now.AddMinutes(-30))
                .Select(x => x.UserName)
                .ToList();
        }


        public override void SetUserOnlineStatus()
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                string userName = HttpContext.Current.User.Identity.Name;
                if (UsersOnline.ContainsKey(UserSessionId))
                    UsersOnline[UserSessionId].LastActivityTime = DateTime.Now;
                else
                    AddUserToOnlineList(userName);
            }
        }



        public override LoginStatus LogIn(string userName, string password)
        {
            return ValidateUser(userName, password);
        }

        public override LoginStatus LogIn(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                System.Web.Security.FormsAuthentication.SetAuthCookie(userName, false);

                AddUserToOnlineList(userName);
                return LoginStatus.Successful;
            }
            else
                return LoginStatus.Unsuccessful;
        }
    }
}
