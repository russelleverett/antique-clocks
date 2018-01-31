using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers {
    public class AboutController : Controller {
        public IActionResult Index() {
            return View();
        }

        public IActionResult Lenzkirch() {
            return View();
        }

        public IActionResult Unsigned() {
            return View();
        }

        public IActionResult Dating() {
            return View();
        }

        public IActionResult Shipping() {
            return View();
        }

        public IActionResult Sell() {
            return View();
        }
    }
}