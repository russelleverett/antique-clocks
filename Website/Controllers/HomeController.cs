using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using Website.Infrastructure.Services;

namespace Website.Controllers {
    public class HomeController : Controller {
        private IDomainContext _context;

        public HomeController(IDomainContext context) {
            _context = context;
        }

        public IActionResult Index() {
            var clocks = new List<ClockFeatureModel>();

            var featuredClocks = _context.Clocks.Where(p => p.Featured);
            foreach (var feature in featuredClocks) {
                var image = _context.Resources.FirstOrDefault(p => p.ClockId == feature.Id && p.Default);
                clocks.Add(new ClockFeatureModel {
                    Id = feature.Id,
                    ImageId = image?.Id,
                    ImageAlt = image?.Name,
                    Name = feature.Name,
                    Description = new string(feature.Description.Take(100).ToArray()) + "..."
                });
            }

            return View(clocks);
        }
    }
}
