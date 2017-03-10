using System.Web;

namespace App.Membership.Services.Logging
{
    public class AspNetLogContextProvider : ILogContextProvider
    {

        public LoggingContext GetLogContext()

        {
            HttpContext currentHttpContext = HttpContext.Current;

            return new LoggingContext(currentHttpContext.User.Identity.Name,
                                                currentHttpContext.Request.UserHostAddress,
                                                currentHttpContext.Request.UserAgent);


            //for testing
            //return new LoggingContext("test", "test", "test");




        }
    }
}
