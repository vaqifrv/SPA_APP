using System;
using System.Collections.Generic;
using System.Security.Principal;
using App.Core.Infrastructure.Abstract;
using Ninject;
using Ninject.Modules;
using App.Core.Validation;
using NLog;

namespace App.Core.Infrastructure
{
    public class IoC : StandardKernel
    {
        //ILog log;
        private Logger _log;

        public IoC(params INinjectModule[] modules)
            : base(modules)
        {
        }

        public static IoC Current { get; set; }
        public string ApplicationDirectory { get; set; }

        public string CurrentUser
        {
            get
            {
                return (Principal != null) && (Principal.Identity != null) ? Principal.Identity.Name.ToUpper() : null;
            }
        }

        public IPrincipal Principal
        {
            get { return Current.GetObject<ISessionScopeProvider>().Principal; }
        }

        public T GetObject<T>()
        {
            return this.Get<T>();
        }

        public T GetObject<T>(string namedParam)
        {
            try
            {
                return this.Get<T>(namedParam);
            }
            catch (Exception exc)
            {
                try
                {
                    return this.Get<T>("default");
                }
                catch
                {
                    throw exc;
                }
            }
        }

        public object GetObject(Type service)
        {
            return this.Get(service);
        }

        public bool UserHasAccess(string right)
        {
            var x = Current.GetObject<ISessionScopeProvider>().UserHasAccess(right);
            return x;
        }

        public IList<BrokenRule> LogSysError(Exception exc)
        {
            IList<BrokenRule> list = new List<BrokenRule>();
#if DEBUG
            list = ExtractFromException(exc);
#endif

            list.Add(new BrokenRule { Message = "Server error occured. Try again later." });
            _log = LogManager.GetLogger("filelogger");
            _log.Error(exc.Message + "---" + exc.StackTrace, (exc.InnerException != null) ? exc.InnerException : exc);

            //log = LogManager.GetLogger(typeof(IoC));
            //XmlConfigurator.Configure();
            //log.Error(string.Format("{0} - {1}", Util.GetCurrentMethod(), exc.Message + "---" + exc.StackTrace));

            return list;
        }

        public IList<BrokenRule> ExtractFromException(Exception exc)
        {
            var list = new List<BrokenRule>();
            list.Add(new BrokenRule { Message = exc.Message });
            if (exc.InnerException != null)
                list.AddRange(ExtractFromException(exc.InnerException));

            return list;
        }
    }
}
