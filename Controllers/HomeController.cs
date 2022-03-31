using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController: ControllerBase
    {
        [HttpGet("")]
        //[ApiKey] podemos criar um atributo para autenticação
        public IActionResult Get()
        {
            return Ok();
        }
    }
}