using System;
using System.Collections.Generic;
using System.Web;
using App.Membership.Domain;
using App.Membership.Providers;
using App.Membership.Repositories.Abstract;
using NHibernate;
using NHibernate.Cfg;
using Environment = NHibernate.Cfg.Environment;

namespace App.Membership.Repositories.NHibernate
{
    public class SessionProvider
    {
        private static readonly Dictionary<string, ISessionFactory> SessionFactories = new Dictionary<string, ISessionFactory>();
        public static IDictionary<object, IDictionary<string, ISession>> Sessions = new Dictionary<object, IDictionary<string, ISession>>();
        private static readonly object Locker = new object();

        /*private static ISession session = null;

        public static ISession nSession
        {
            get
            {
                if (session == null)
                {
                    session = GetSession();
                }
                session.Clear();
                return session;
            }
        }*/


        private static void Init(string schema = "")
        {

            Configuration config = new Configuration().Configure();

            string dialect = config.GetProperty(Environment.Dialect);

            string main = "Agile.Solutions.Infrastructure.Membership.Repositories.NHibernate.Mappings.";

            string driverFolder = null;
            if (dialect.ToLower().Contains("mssql"))
            {
                driverFolder = "MsSql.";
            }
            else if (dialect.ToLower().Contains("oracle"))
            {
                driverFolder = "Oracle.";
            }
            else
            {
                throw new Exception("Correct NHibernate dialect was not provided in web.config");
            }

            List<string> mappings = new List<string>();

            mappings.Add(main + driverFolder + "Log.hbm.xml");
            mappings.Add(main + driverFolder + "LogAction.hbm.xml");
            mappings.Add(main + driverFolder + "Right.hbm.xml");
            mappings.Add(main + driverFolder + "Role.hbm.xml");
            mappings.Add(main + driverFolder + "User.hbm.xml");
            mappings.Add(main + driverFolder + "Profile.hbm.xml");
            mappings.Add(main + driverFolder + "ProfileProperty.hbm.xml");


            config.AddResources(mappings, typeof(SessionProvider).Assembly);

            /*
            var schemaExport = new SchemaExport(config);
            schemaExport.Create(false, true);*/

            SessionFactories.Add(schema, config.BuildSessionFactory());


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

        public static ISession GetSessionLazy(string schema)
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
                if (SessionProvider.Sessions.ContainsKey(scope))
                    SessionProvider.Sessions.Remove(scope);
            }
        }
        public static ISession GetSessionLazy()
        {
            return GetSessionLazy("");
        }
    }
}
