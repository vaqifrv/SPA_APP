using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using App.Membership.Providers;
using App.Membership.Repositories.NHibernate;
using App.Membership.Services.Locking;

namespace App.Membership.Services.Login
{
    public class WcfLogin : BaseLogin<LoginStatus>
    {

        private static readonly HashSet<LoginAttempt> Attempts = new HashSet<LoginAttempt>();
        private static LockSettings _lockSetting;
        private static LockSettings LockSettings => _lockSetting ?? new LockSettings();

        public static string EndpointAddress { get; set; }

        protected override LoginStatus ValidateUser(string userName, string password)
        {
            DateTime currentTime = DateTime.Now;
            string ip = EndpointAddress;
            if (Attempts.Count > 2000) Thread.Sleep(2000);
            LoginAttempt attempt = Attempts.Where(m => m.Ip == ip && m.UserName == userName).Select(m => m).FirstOrDefault();

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

            TimeSpan interval = currentTime - attempt.AttemptDate;

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


            UserRepository repository = new UserRepository();
            AgMembershipProvider provider = new AgMembershipProvider(repository);

            if (provider.ValidateUser(userName, password))
            {
                return LoginStatus.Successful;
            }
            else
            {
                if (Attempts.Count % LockSettings.MaxAttemptCount - 1 == 0)
                {
                    //  var t=m.AttemptDate.AddHours(LockSettings.UnlockPeriod.Hours)
                    Attempts.RemoveWhere(m => m.AttemptDate.AddHours(LockSettings.UnlockPeriod.Hours).AddMinutes(LockSettings.UnlockPeriod.Minutes).AddSeconds(LockSettings.UnlockPeriod.Seconds).AddDays(LockSettings.UnlockPeriod.Days) < currentTime);
                }
                if (interval <= LockSettings.AttemptPeriod)
                {
                    attempt.FailedAttemptCount += 1;
                    if (attempt.FailedAttemptCount >= LockSettings.MaxAttemptCount) attempt.AttemptDate = currentTime; //Deməli  d artıq last attempt olmalıdır
                }
                else
                {
                    attempt.AttemptDate = currentTime;
                    attempt.FailedAttemptCount = 1;
                }
                Attempts.Add(attempt);
                return LoginStatus.Unsuccessful;
            }
        }

        public override IList<string> GetUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override void SetUserOnlineStatus()
        {
            throw new NotImplementedException();
        }

        public override LoginStatus LogIn(string userName, string password)
        {
            return ValidateUser(userName, password);
        }

        public override LoginStatus LogIn(string token)
        {
            throw new NotImplementedException();
        }
    }
}
