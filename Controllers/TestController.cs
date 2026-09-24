using Microsoft.AspNetCore.Mvc;

namespace weather.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Hello, Test Api!";
        }
    }
}