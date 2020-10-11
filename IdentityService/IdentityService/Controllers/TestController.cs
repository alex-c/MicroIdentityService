using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [Route("api/v1/test")]
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok();
        }
    }
}
