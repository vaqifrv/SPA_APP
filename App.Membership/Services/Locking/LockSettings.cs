using System;
using System.Configuration;
using App.Membership.Services.Configuration;

namespace App.Membership.Services.Locking
{
    public class LockSettings
    {

        //private static LockingSection _configs;

        //private static LockingSection Configs
        //{
        //    get
        //    {
        //         if (_configs==null) 
        //         {
        //             _configs = ConfigurationManager.GetSection("Locking") as LockingSection;
        //         }
        //         return _configs;
        //    }

        //}


        public LockSettings()
        {
            LockingSection configs = ConfigurationManager.GetSection("Locking") as LockingSection;
            if (configs == null)
            {
                IsActive = false;
            }
            else
            {
                AttemptPeriod = new TimeSpan(0, configs.LockingSettings.AttemptPeriod, 0);
                UnlockPeriod = new TimeSpan(0, configs.LockingSettings.UnlockPeriod, 0);
                MaxAttemptCount = configs.LockingSettings.MaxAttemptCount;
                IsActive = true;
            }

        }


        public LockSettings(int appId) { }
        public int AppId { get; set; }
        public int MaxAttemptCount { get; private set; }
        public TimeSpan AttemptPeriod { get; private set; }
        public TimeSpan UnlockPeriod { get; private set; }
        public bool IsActive { get; private set; }

    }
}
