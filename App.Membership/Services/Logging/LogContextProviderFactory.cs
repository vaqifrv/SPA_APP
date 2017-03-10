namespace App.Membership.Services.Logging
{
    public static class LogContextProviderFactory
    {

        public static ILogContextProvider GetLogContextProvider()
        {
            return ClassFactory.Current.GetService<ILogContextProvider>();
        }

    }
}
