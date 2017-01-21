using System.Collections.Generic;
using System.Web.Http;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new[] {"value3", "value4"};
        }
    }
}