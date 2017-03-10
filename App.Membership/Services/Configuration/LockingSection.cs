using System.Configuration;

namespace App.Membership.Services.Configuration
{
    public class LockingSection : ConfigurationSection
    {
        [ConfigurationProperty("LockingSettings")]
        public LockingSettingsElement LockingSettings
        {
            get
            {
                return (LockingSettingsElement)this["LockingSettings"];
            }
            set
            {
                this["LockingSettings"] = value;
            }
        }

    }

    public class LockingSettingsElement : ConfigurationElement
    {
        [ConfigurationProperty("MaxAttemptCount", DefaultValue = "5")]
        public int MaxAttemptCount
        {
            get
            {
                return (int)this["MaxAttemptCount"];
            }
            set
            {

                this["MaxAttemptCount"] = value;
            }
        }

        [ConfigurationProperty("AttemptPeriod", DefaultValue = "20")]
        public int AttemptPeriod
        {
            get
            {
                return (int)this["AttemptPeriod"];
            }
            set
            {

                this["AttemptPeriod"] = value;
            }
        }

        [ConfigurationProperty("UnlockPeriod", DefaultValue = "30")]
        public int UnlockPeriod
        {
            get
            {
                return (int)this["UnlockPeriod"];
            }
            set
            {
                this["UnlockPeriod"] = value;
            }
        }
    }


}
