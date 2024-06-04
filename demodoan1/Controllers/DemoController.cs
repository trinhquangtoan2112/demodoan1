using Microsoft.AspNetCore.Mvc;

namespace demodoan1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DemoController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {

            var html11 = "rrrrdddd";
            return Content(html11);
        }
        [HttpGet("toan2002",Name ="helllo12")]
        public ActionResult Index1()
        {

            var html11 = "tesesas";
            return Content(html11);
        }
    }
}
