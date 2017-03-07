using App.Core.Domain;
using App.Repository.Repositories.Abstract;
using App.Services.Services.Abstract;

namespace App.Services.Services.Concrete
{
    public class TestService: GuidServiceBase<Test>, ITestService
    {
        public TestService(ITestRepository repository)
        {
            this.Repository = repository;
        }
    }
}
