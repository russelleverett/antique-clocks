using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using Website.Infrastructure.Services;
using System;

namespace Website.Controllers {
    public class HomeController : Controller {
        private IDomainContext _context;

        public HomeController(IDomainContext context) {
            _context = context;
        }

        public IActionResult Index() {
            var clocks = new List<ClockFeatureModel>();

            // get the clocks
            var featuredClocks = _context.Clocks.Where(p => p.Featured);
            foreach (var feature in featuredClocks) {
                var image = _context.Resources.FirstOrDefault(p => p.ClockId == feature.Id && p.Default && p.ParentTypeId == 0);
                clocks.Add(new ClockFeatureModel {
                    Id = feature.Id,
                    ImageId = image?.Id,
                    ImageAlt = image?.Name,
                    Name = feature.Name,
                    Description = new string(feature.Description.Take(100).ToArray()) + "..."
                });
            }

            // get the MOTD (message of the day)
            var messages = _context.Messages.Where(p => 
                p.StartDate <= DateTime.Today &&
                (p.EndDate != null ? p.EndDate > DateTime.Today : true)
            ).ToList();

            return View(new HomePageModel {
                Messages = messages.Select(p => p.Message),
                FeaturedClocks = clocks
            });
        }

        public IActionResult Test() {
            return View();
        }

        public IActionResult Error(int statusCode = 0) {
            return View();
        }
    }
}
