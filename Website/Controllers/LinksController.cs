using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers {
    public class LinksController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}