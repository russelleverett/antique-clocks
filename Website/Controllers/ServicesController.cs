using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers {
    public class ServicesController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}