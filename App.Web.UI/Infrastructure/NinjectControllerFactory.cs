using App.Core.Infrastructure;
using Ninject;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace App.Web.UI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        public NinjectControllerFactory() { }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)IoC.Current.Get(controllerType);
        }
    }
}
