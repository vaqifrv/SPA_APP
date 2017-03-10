using System;
using App.Membership.Services.Logging.Enums;
using NHB = global::NHibernate;

namespace App.Membership.Repositories.NHibernate.Enum
{
    public class LogActionResultEnumStringType : NHB.Type.EnumStringType
    {
        public LogActionResultEnumStringType()
            : base(typeof(LogActionResult))
        {
        }

        public override object GetValue(object enm)
        {
            if (null == enm)
                return String.Empty;
            if (enm is LogActionResult)
            {
                return ((LogActionResult)enm).ToString();
            }
            else
                throw new ArgumentException("Invalid ActionResult.");
        }


        public override object GetInstance(object code)
        {
            return System.Enum.Parse(typeof(LogActionResult), (string)code);
        }
    }
}
