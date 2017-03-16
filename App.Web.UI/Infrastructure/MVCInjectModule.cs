using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ninject.Modules;
using App.Core.Validation.Abstract;
using App.Core.Infrastructure.Abstract;
using App.Repository.Repositories.Abstract;
using App.Services.Services.Abstract;
using App.Repository.Repositories.Concrete;
using App.Services.Services.Concrete;
using Moq;
using NHibernate;
using App.Repository.Infrastructure;
using App.Core.Validation;

namespace App.Web.UI.Infrastructure
{
    public class MvcInjectModule : NinjectModule
    {
        public override void Load()
        {
            //session
            Bind<ISessionScopeProvider>().To<MVCSessionScopeProvider>();
            Bind<ISession>().ToMethod(ctx => SessionProvider.GetSession("appConnectionString"));



            #region Dependency Resolver 
            //repositories
            Bind<ITestRepository>().To<TestRepository>();
            
            //services
            Bind<ITestService>().To<TestService>();
            #endregion 

            //validations
            Bind<ICustomValidation>().ToMethod(x => ICustomValidationServiceMock()).Named("default");
          
        }


        private ICustomValidation ICustomValidationServiceMock()
        {
            Mock<ICustomValidation> mock = new Mock<ICustomValidation>();
            mock.Setup(x => x.Validate(It.IsAny<ValidationContext>())).Returns((ValidationContext context) => new List<BrokenRule>());
            return mock.Object;
        }


    }
}
