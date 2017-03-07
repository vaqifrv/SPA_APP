using App.Core.Domain;
using App.Services.Services.Abstract;
using App.Web.UI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}