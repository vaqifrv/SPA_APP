using App.Core.Domain;
using App.Repository.Repositories.Abstract;

namespace App.Repository.Repositories.Concrete
{
    public class TestRepository : RepositoryBase<Test, string>, ITestRepository
    {
    }
}