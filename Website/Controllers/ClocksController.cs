using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using System.Net;
using Website.Infrastructure.Services;
using Website.Infrastructure.Data.Entities;

namespace Website.Controllers {
    public class ClocksController : Controller {
        private readonly IDomainContext _context;

        public ClocksController(IDomainContext context) {
            _context = context;
        }

        public IActionResult Index(string filter = null) {
            var models = new List<ClockBrowseModel>();

            var clocks = _context.Clocks.Where(p => p.Active).ToList();
            foreach (var clock in clocks) {
                // filter results
                if (filter != null) {
                    if (filter.Contains("sold")) {
                        if(!clock.IsSold)
                            continue;
                    }   
                    else if (!clock.Filters.Contains(filter))
                        continue;
                }

                var image = _context.Resources.FirstOrDefault(p => p.ClockId == clock.Id && p.Default);
                var imageId = (image != null) ? image.Id : 0;
                models.Add(new ClockBrowseModel {
                    Id = clock.Id,
                    Name = clock.Name,
                    ImageId = imageId,
                    ImageName = image?.Name,
                    Number = clock.Number,
                    PintrestLink = clock.PintrestLink(imageId)
                });
            }

            return View(models);
        }

        public IActionResult Details(int id = 0) {
            // get the clock
            var clock = _context.Clocks.FirstOrDefault(p => p.Id == id);
            if (clock != null) {
                clock.Resources = _context.Resources.Where(p => p.ClockId == clock.Id && p.FileType == FileType.Image).ToList();
                clock.ClockAudio = _context.Resources.FirstOrDefault(p => p.ClockId == clock.Id && p.FileType == FileType.Audio);
                return View(clock);
            }

            // redirect back to index page
            return RedirectToAction("Index");
        }
    }
}