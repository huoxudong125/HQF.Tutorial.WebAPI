using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HQF.Tutorial.WebAPI.Models;

namespace HQF.Tutorial.WebAPI.Controllers
{
    public class ExceptionController : ApiController
    {
        [Route("api/Exception")]
        public HttpResponseMessage Get()
        {
            throw new Exception("Ooops!");
        }

        public Product GetProduct()
        {
            throw new NotImplementedException();
        }
    }
}
