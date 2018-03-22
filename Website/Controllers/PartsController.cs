using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Website.Infrastructure.Services;

namespace Website.Controllers {
    public class PartsController : Controller {
        private readonly IDomainContext _context;

        public PartsController(IDomainContext context) {
            _context = context;
        }

        public IActionResult Index(string filter = null) {
            var lenzkirch = filter == "lenzkirch";

            var parts = _context.Parts.Where(p => p.IsLenzkirch == lenzkirch).ToList();
            parts.ForEach(p => {
                p.Resources = _context.Resources.Where(x => x.ClockId == p.Id && x.ParentTypeId == 1);
            });
            return View(parts);
        }

        public IActionResult Carvings() {
            return View();
        }
    }
}