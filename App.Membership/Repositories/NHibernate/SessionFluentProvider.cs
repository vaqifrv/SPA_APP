using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using App.Membership.Domain;
using App.Membership.Providers;
using App.Membership.Repositories.Abstract;
using App.Membership.Repositories.NHibernate.Mappings.Fluent.Oracle;
using App.Repository.Infrastructure;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using log4net.Config;
using NHibernate;
using NHibernate.Event;
using NHibernate.Tool.hbm2ddl;

namespace App.Membership.Repositories.NHibernate
{
    public class SessionFluentProvider
    {
        private static readonly Dictionary<string, ISessionFactory> SessionFactories =
             new Dictionary<string, ISessionFactory>();

        public static IDictionary<object, IDictionary<string, ISession>> Sessions =
            new Dictionary<object, IDictionary<string, ISession>>();

        private static readonly object Locker = new object();

        private static void Init(string schema = "")
        {
            XmlConfigurator.Configure();

            var cfg = Fluently.Configure()
           .Database(MsSqlConfiguration.MsSql2012.ConnectionString(c => c.FromConnectionStringWithKey("appConectionString")))
           .Mappings(m =>
           {
               m.FluentMappings.AddFromAssemblyOf<UserMap>();
           })
           //shcema update ucun
           .ExposeConfiguration(c => new SchemaUpdate(c).Execute(false, true)) 
           //schema create ucun   
           //.ExposeConfiguration(c => new SchemaExport(c).Create(true, true)) 
           .BuildConfiguration();

            cfg.EventListeners.MergeEventListeners = new IMergeEventListener[]
           {new CustomNHibernateMergeEventListener()};

            SessionFactories.Add(schema, cfg.BuildSessionFactory());

            CustomQueryOverExtensions.Register();

            CreateDefaultUserAndRole();
        }

        private static void CreateDefaultUserAndRole()
        {

            if (RepositoryFactory.GetUserRepository().IsNewDatabase().Value)
            {
                AgMembershipProvider provider = new AgMembershipProvider();
                System.Web.Security.MembershipCreateStatus status;
                provider.CreateUser("admin", "admin", String.Empty, String.Empty, string.Empty, true, "admin", out status);

                AgRoleProvider roleProvider = new AgRoleProvider();
                if (!roleProvider.RoleExists("Administrators"))
                {
                    roleProvider.CreateRole("Administrators");
                }
                if (!roleProvider.IsUserInRole("admin", "Administrators"))
                {
                    roleProvider.AddUsersToRoles(new[] { "admin" }, new string[] { "Administrators" });
                }

                ILogActionRepository repository = RepositoryFactory.GetLogActionRepository();
                if (repository.GetAllLogActions().TotalItems == 0)
                {
                    repository.Add(new LogAction { Id = 1, Name = "ValidateUser" });
                    repository.Add(new LogAction { Id = 2, Name = "ChangePassword" });
                    repository.Add(new LogAction { Id = 3, Name = "HasAccess" });
                    repository.Add(new LogAction { Id = 4, Name = "Login" });
                    repository.Add(new LogAction { Id = 5, Name = "Logout" });
                    repository.Add(new LogAction { Id = 6, Name = "ChangeUserEnabledStatus" });
                }
            }
        }

        public static ISessionFactory GetSessionFactory(string schema)
        {
            if (!SessionFactories.ContainsKey(schema))
                Init(schema);
            return SessionFactories[schema];
        }

        public static ISessionFactory GetSessionFactory()
        {
            return GetSessionFactory("");
        }

        public static ISession GetSession(string schema)
        {
            object scope = HttpContext.Current.Request;
            if (scope != null)
            {
                if (!Sessions.ContainsKey(scope))
                {
                    lock (Locker)
                    {
                        Sessions.Add(scope, new Dictionary<string, ISession>());
                    }
                }
                if (Sessions.ContainsKey(scope) && !Sessions[scope].ContainsKey(schema))
                {
                    lock (Locker)
                    {
                        ISession newSession = GetSessionFactory(schema).OpenSession();
                        if (!Sessions[scope].ContainsKey(schema))
                        {
                            Sessions[scope].Add(schema, newSession);
                        }
                    }
                }
                return Sessions[scope][schema];
            }
            else
            {
                return GetSessionFactory(schema).OpenSession();
            }
        }

        public static void RemoveSession(object scope)
        {
            lock (Locker)
            {
                if (Sessions.ContainsKey(scope))
                    Sessions.Remove(scope);
            }
        }

        public static ISession GetSession()
        {
            return GetSession("");
        }
    }
}
