namespace App.Membership.Services.Logging
{

    public class WinFormsLogContextProvider : ILogContextProvider
    {

        public LoggingContext GetLogContext()
        {
            //for testing
            return new LoggingContext("test", "test", "test");
            
        }
    }

}
