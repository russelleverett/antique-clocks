using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers {
    public class PartsController : Controller {
        public IActionResult Lenzkirch() {
            return View();
        }

        public IActionResult Other() {
            return View();
        }

        public IActionResult Carvings() {
            return View();
        }
    }
}