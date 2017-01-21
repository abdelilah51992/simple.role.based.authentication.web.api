using System.Collections.Generic;
using System.Web.Http;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin2")]
    public class Roles2Controller : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new[] {"value5", "value6"};
        }
    }
}