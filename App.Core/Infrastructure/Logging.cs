using System;
using System.Collections.Generic;
using App.Core.Validation;

namespace App.Core.Infrastructure
{
    public class Logging
    {
        public static IList<BrokenRule> LogSysError(Exception exc)
        {
            // logRepo.Add(new LogItem());

            IList<BrokenRule> list = new List<BrokenRule>();
#if DEBUG
            list = ExtractFromException(exc);
#else
            list = ExtractFromException(exc);
#endif

            list.Add(new BrokenRule { Message = "Server error occured. Try again later." });
            return list;
        }

        public static IList<BrokenRule> ExtractFromException(Exception exc)
        {
            var list = new List<BrokenRule>();
            list.Add(new BrokenRule { Message = exc.Message });
            if (exc.InnerException != null)
                list.AddRange(ExtractFromException(exc.InnerException));

            return list;
        }
    }
}
