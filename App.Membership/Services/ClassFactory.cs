using System;
using System.Collections.Generic;
using App.Membership.Repositories.Abstract;
using App.Membership.Repositories.NHibernate;
using App.Membership.Services.Configuration;
using App.Membership.Services.Logging;
using App.Membership.Services.Login;
using App.Membership.Services.NetworkAuthorize.Abstract;
using App.Membership.Services.NetworkAuthorize.Configuration;

namespace App.Membership.Services
{
    public class ClassFactory
    {
        private void AddBindings()
        {
            Bind<IUserRepository, UserRepository>();
            Bind<IRoleRepository, RoleRepository>();
            Bind<IRightRepository, RightRepository>();
            Bind<ILoggingRepository, LoggingRepository>();
            Bind<ILogActionRepository, LogActionRepository>();
            Bind<ILogContextProvider, AspNetLogContextProvider>(Platform == "ASPNET");
            Bind<ILogContextProvider, WinFormsLogContextProvider>(Platform == "WINFORMS");
            Bind<ILogContextProvider, WcfLogContextProvider>(Platform == "WCF");
            Bind<ILogin<LoginStatus>, AspnetLogin>(Platform == "ASPNET");
            Bind<ILogin<bool>, WinFormsLogin>(Platform == "WINFORMS");
            Bind<ILogin<LoginStatus>, WcfLogin>(Platform == "WCF");
            Bind<ILogin, AspnetLogin>(Platform == "ASPNET");
            Bind<ILogin, WinFormsLogin>(Platform == "WINFORMS");
            Bind<ILogin, WcfLogin>(Platform == "WCF");
            Bind<IAcl, Acl>();
            Bind<IProfileRepository, ProfileRepository>();
            Bind<IProfilePropertyRepository, ProfilePropertyRepository>();
        }

        private static readonly string Platform = ((LoggingSection)System.Configuration.ConfigurationManager.GetSection("LoggingGroup/Logging")).Platform.Name;

        public static ClassFactory Current { get; } = new ClassFactory();


        private readonly Dictionary<Type, Type> _dependies = new Dictionary<Type, Type>();

        public ClassFactory()
        {
            AddBindings();
        }


        public void Bind<TBind, TTo>()
        {
            Bind<TBind, TTo>(true);
        }

        public void Bind<TBind, TTo>(bool checkExpression)
        {
            if (checkExpression)
            {
                if (_dependies.ContainsKey(typeof(TBind)))
                    _dependies[typeof(TBind)] = typeof(TTo);
                else
                    _dependies.Add(typeof(TBind), typeof(TTo));
            }
        }


        public T GetService<T>()
        {
            if (_dependies.ContainsKey(typeof(T)))
            {
               return (T) Activator.CreateInstance(_dependies[typeof(T)]);
            }
            else
                return default(T);
        }
        

            
    }


}
