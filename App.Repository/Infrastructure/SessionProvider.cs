using System.Collections.Generic;
using App.Core.Infrastructure;
using App.Core.Infrastructure.Abstract;
using App.Mappings.Maps;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using log4net.Config;
using NHibernate;
using NHibernate.Event;
using NHibernate.Tool.hbm2ddl;

namespace App.Repository.Infrastructure
{
    public static class SessionProvider
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
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(c => c.FromConnectionStringWithKey("appConnectionString")))
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssemblyOf<TestMap>();
                })
                //shcema update ucun
                .ExposeConfiguration(c => new SchemaUpdate(c).Execute(false, true)) 
                  //schema create ucun   
                //.ExposeConfiguration(c => new SchemaExport(c).Create(true, true)) 
                .BuildConfiguration();

            //var cfg = Fluently.Configure()
            //    .Database(OracleClientConfiguration.Oracle10
            //        .Dialect<CustomOracleDialect>()
            //        .ConnectionString(c => c.FromConnectionStringWithKey("appConnectionString")))
            //    .Mappings(m =>
            //    {
            //        m.FluentMappings.AddFromAssemblyOf<TestMap>();
            //    })
            //    .ExposeConfiguration(c => new SchemaUpdate(c).Execute(false, true)) //shcema update ucun
            //                                                                        //.ExposeConfiguration(c => new SchemaExport(c).Create(true, true)) //schema create ucun                    
            //    .BuildConfiguration();


            cfg.EventListeners.MergeEventListeners = new IMergeEventListener[]
            {new CustomNHibernateMergeEventListener()};

            SessionFactories.Add(schema, cfg.BuildSessionFactory());

            CustomQueryOverExtensions.Register();
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
            var scope = IoC.Current.GetObject<ISessionScopeProvider>().Scope;
            if (scope != null)
            {
                if (!Sessions.ContainsKey(scope))
                {
                    lock (Locker)
                    {
                        Sessions.Add(scope, new Dictionary<string, ISession>());
                        Sessions[scope].Add(schema, GetSessionFactory(schema).OpenSession());
                    }
                }
                return Sessions[scope][schema];
            }
            return GetSessionFactory(schema).OpenSession();
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