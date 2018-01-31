using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers {
    public class BookController : Controller {
        public IActionResult Index() {
            return View();
        }

        public IActionResult Order() {
            return View();
        }
    }
}