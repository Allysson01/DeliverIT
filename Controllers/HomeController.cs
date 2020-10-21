using Microsoft.AspNetCore.Mvc;

namespace DeliverIt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        public HomeController()
        {
        }

        [HttpGet]
        public string HomeIndex()
        {
            return ("API DeliverIt pronta e rodando ...");
        }
    }
}
