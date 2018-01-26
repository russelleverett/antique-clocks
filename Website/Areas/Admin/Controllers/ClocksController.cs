using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Website.Infrastructure.Services;
using Website.Infrastructure.Data.Entities;
using Website.Areas.Admin.Models;
using System.IO;

namespace Website.Areas.Admin.Controllers {
    [Area("Admin"), Authorize]
    public class ClocksController : Controller {
        private readonly IDomainContext _context;

        public ClocksController(IDomainContext context) {
            _context = context;
        }

        public IActionResult Index() {
            var clocks = _context.Clocks.ToList();
            return View(clocks);
        }

        public IActionResult Add() {
            return View(new ClockCreateModel {
                Active = true
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(ClockCreateModel model) {
            if (!ModelState.IsValid)
                return View(model);

            // add the clock
            var clock = new Clock {
                Number = model.Number,
                Name = model.Name,
                Price = model.Price,
                BuyNowId = model.BuyNowId,
                Features = model.Features,
                Caveats = model.Caveats,
                Description = model.Description,
                Featured = model.Featured,
                Active = model.Active
            };
            _context.Add(clock);
            _context.SaveChanges();

            // add the resources
            if (model.FileUploads != null) {
                var _default = true;
                foreach (var file in model.FileUploads) {
                    var resource = new Resource {
                        Active = true,
                        Name = clock.Name,
                        FileName = file.FileName,
                        ContentType = file.ContentType,
                        ClockId = clock.Id
                    };

                    // tag the initial one as the default
                    if (_default) {
                        resource.Default = true;
                        _default = false;
                    }

                    using (var ms = new MemoryStream()) {
                        file.CopyTo(ms);
                        resource.File = ms.ToArray();
                    }
                    _context.Add(resource);
                }
                _context.SaveChanges();
            }

            return Redirect("/admin/clocks");
        }

        public IActionResult Edit(int id = 0) {
            var clock = _context.Clocks.FirstOrDefault(p => p.Id == id);
            if (clock != null) {
                var resources = _context.Resources.Where(p => p.ClockId == clock.Id).Select(p => new {
                    id = p.Id,
                    fileName = p.FileName
                }).ToList();

                return View(new ClockEditModel {
                    Id = clock.Id,
                    Number = clock.Number,
                    Name = clock.Name,
                    Price = clock.Price,
                    Features = clock.Features,
                    Caveats = clock.Caveats,
                    Description = clock.Description,
                    Active = clock.Active,
                    Images = resources,
                    Featured = clock.Featured
                });
            }   
            return Redirect("/admin/clocks");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(ClockEditModel model) {
            if (!ModelState.IsValid)
                return View(model);

            var clock = _context.Clocks.FirstOrDefault(p => p.Id == model.Id);
            if (clock != null) {
                clock.Number = model.Number;
                clock.Name = model.Name;
                clock.Price = model.Price;
                clock.Features = model.Features;
                clock.Caveats = model.Caveats;
                clock.Description = model.Description;
                clock.Active = model.Active;
                clock.Featured = model.Featured;

                _context.SaveChanges();
            }

            if (model.FileUploads != null) {
                var _default = _context.Resources.FirstOrDefault(p => p.ClockId == clock.Id && p.Default) != null;
                foreach (var file in model.FileUploads) {
                    var resource = new Resource {
                        Name = clock.Name,
                        FileName = file.FileName,
                        ContentType = file.ContentType,
                        Active = true,
                        ClockId = clock.Id
                    };

                    if (_default) {
                        resource.Default = true;
                        _default = false;
                    }

                    using (var ms = new MemoryStream()) {
                        file.CopyTo(ms);
                        resource.File = ms.ToArray();
                    }
                    _context.Add(resource);
                }
                _context.SaveChanges();
            }

            return Redirect("/admin/clocks");
        }
    }
}