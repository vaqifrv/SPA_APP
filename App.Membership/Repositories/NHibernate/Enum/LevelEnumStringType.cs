using System;
using App.Membership.Services.Logging.Enums;
using NHB=global::NHibernate;

namespace App.Membership.Repositories.NHibernate.Enum
{
    public class LevelEnumStringType:NHB.Type.EnumStringType  
    {
        public LevelEnumStringType()
            : base(typeof(Level))
        {
        }

        public override object GetValue(object enm)
        {
            if (null == enm)
                return String.Empty;
            if (enm is Level)
            {
                return ((Level)enm).ToString();
            }
            else
                throw new ArgumentException("Invalid Level.");  
        }


        public override object GetInstance(object code)
        {
            return System.Enum.Parse(typeof(Level), (string)code);
        }  
    }
}
