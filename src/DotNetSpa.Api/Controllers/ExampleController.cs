using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DotNetSpa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExampleController : ControllerBase
    {
        [HttpGet(), Route("foo", Name = "Foo")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public ActionResult<string> Foo()
        {
            return Ok("foo");
        }

        [HttpGet(), Route("bar", Name = "Bar")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public ActionResult<string> Bar()
        {
            return Ok("bar");
        }
    }
}
