namespace App.Membership.Services.Logging
{
    public interface ILogContextProvider
    {
        LoggingContext GetLogContext();
    }
}
