using Microsoft.AspNetCore.Mvc;

namespace DaVinciCollegeAuthenticationService.Controllers
{
    public class StaticContentController : Controller
    {
        public IActionResult PageNotFound()
        {
            return View();
        }

        [Route("/Documentation")]
        public IActionResult Documentation()
        {
            return View();
        }
    }
}