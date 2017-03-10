using App.Membership.Repositories.Abstract;
using App.Membership.Services;

namespace App.Membership.Repositories
{
    public class RepositoryFactory
    {
        public static IUserRepository GetUserRepository()
        {
            return ClassFactory.Current.GetService<IUserRepository>();
        }

        public static IRoleRepository GetRoleRepository()
        {
            return ClassFactory.Current.GetService<IRoleRepository>();
        }

       public static IRightRepository GetRightRepository() 
       {
           return ClassFactory.Current.GetService<IRightRepository>();
       }

       public static ILoggingRepository GetLoggingRepository()
       {
           return ClassFactory.Current.GetService<ILoggingRepository>();
       }

       public static ILogActionRepository GetLogActionRepository()
       {
           return ClassFactory.Current.GetService<ILogActionRepository>();
       }

       public static IProfileRepository GetProfileRepository()
       {
           return ClassFactory.Current.GetService<IProfileRepository>();
       }

       public static IProfilePropertyRepository GetProfilePropertyRepository()
       {
           return ClassFactory.Current.GetService<IProfilePropertyRepository>();
       }
    }
}
