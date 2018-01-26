using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using System.Net;
using Website.Infrastructure.Services;
using Website.Infrastructure.Data.Entities;
using System;

namespace Website.Controllers {
    public class ClocksController : Controller {
        private readonly IDomainContext _context;

        public ClocksController(IDomainContext context) {
            _context = context;
        }

        public IActionResult Index() {
            var models = new List<ClockBrowseModel>();

            var clocks = _context.Clocks.Where(p => p.Active).ToList();
            foreach (var clock in clocks) {
                var image = _context.Resources.FirstOrDefault(p => p.ClockId == clock.Id && p.Default);
                var imageId = (image != null) ? image.Id : 0;
                models.Add(new ClockBrowseModel {
                    Id = clock.Id,
                    Name = clock.Name,
                    ImageId = imageId,
                    ImageName = image?.Name,
                    Number = clock.Number,
                    PintrestLink = string.Format("http://www.pinterest.com/pin/create/button/?url={0}&media={1}&description={1}",
                    WebUtility.UrlEncode("http://antique-clock.com"),
                    WebUtility.UrlEncode(string.Format("http://antique-clock.com/images/{0}", imageId)),
                    WebUtility.UrlEncode(clock.Name + " at antique-clock.com"))
                });
            }

            return View(models);
        }

        public IActionResult Details(int id = 0) {
            // get the clock
            var clock = _context.Clocks.FirstOrDefault(p => p.Id == id);
            if (clock != null) {
                clock.Resources = _context.Resources.Where(p => p.ClockId == clock.Id).ToList();
                return View(clock);
            }

            //var clock = new Clock {
            //    Id = 0,
            //    Name = "Amber's Clock",
            //    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec efficitur tortor sed nunc accumsan, sed cursus sem euismod. Quisque sollicitudin elit vitae erat malesuada facilisis. Quisque eu nulla vel risus aliquam consequat. Curabitur sit amet magna leo. Ut euismod neque vel sapien blandit, sed viverra nunc vestibulum. Aenean vitae mauris quis ligula laoreet lobortis sed a sem. Morbi sed est et odio sodales vestibulum eu sed urna. Duis in elit elit. Curabitur mattis neque ut quam vehicula convallis.In et varius magna.Mauris volutpat tincidunt ornare.Vestibulum at felis eu enim tristique tincidunt at ut risus.Nullam non volutpat diam.Fusce sit amet est turpis.Nulla egestas id dui nec pellentesque.Sed enim risus, tempor quis pulvinar sed, semper non libero.Donec quis placerat orci.",
            //    Features = "Feature #1\r\nFeature #2\r\nFeature #3",
            //    Number = "RNA001",
            //    Price = 15000000m,
            //    Active = true,
            //    Resources = new List<Resource> {
            //        new Resource {
            //            Id = 5,
            //            Default = true,
            //            Active = true,
            //            ClockId = 0,
            //            Name = "Amber's default picture."
            //        }
            //    }
            //};
            //return View(clock);

            // redirect back to index page
            return RedirectToAction("Index");
        }
    }
}