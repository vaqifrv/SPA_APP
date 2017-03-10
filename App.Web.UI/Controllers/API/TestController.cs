using App.Core.Domain;
using App.Services.Services.Abstract;
using App.Web.UI.Infrastructure;
using System.Collections.Generic;
using System.Web.Http;

namespace App.Web.UI.Controllers.API
{
    [RoutePrefix("api/test")]
    public class TestController : GuidApiController<Test, ITestService>
    {
        [HttpGet, Route("getAll")]
        public IEnumerable<Test> GetAll()
        {
            var result = Service.List();
            return result.List;
        }

        public string CreateUser()
        {

            return "gdgd";
        }
    }
}