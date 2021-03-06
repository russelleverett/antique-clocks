using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Website.Infrastructure.Services;
using Website.Infrastructure.Data.Entities;
using Website.Areas.Admin.Models;
using System.IO;
using System.Collections.Generic;
using Website.Infrastructure.Data.Caches;

namespace Website.Areas.Admin.Controllers {
    [Area("Admin"), Authorize]
    public class ClocksController : Controller {
        private readonly IDomainContext _context;

        public ClocksController(IDomainContext context) {
            _context = context;
        }

        public IActionResult Index() {
            var clocks = _context.Clocks.Select(clock => new ClockViewModel {
                SortOrder = clock.SortOrder,
                Id = clock.Id,
                Number = clock.Number,
                Name = clock.Name,
                Price = clock.Price,
                Featured = clock.Featured,
                IsSold = clock.IsSold,
            }).ToList();

            return View(clocks);
        }

        public IActionResult Add() {
            return View(new ClockCreateModel {
                Active = true,
                Filters = new ClockFilters()
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
                Features = model.Features.TrimEnd('\r', '\n'),
                Caveats = model.Caveats.TrimEnd('\r', '\n'),
                Description = model.Description,
                Featured = model.Featured,
                Active = model.Active,
                Filters = model.Filters.ToString()
            };
            _context.Add(clock);
            _context.SaveChanges();

            // add the resources
            if (model.FileUploads != null) {
                foreach (var file in model.FileUploads) {
                    var resource = new Resource {
                        Active = true,
                        Name = clock.Name,
                        FileName = file.FileName,
                        ContentType = file.ContentType,
                        ClockId = clock.Id,
                        FileType = FileType.Image
                    };

                    using (var ms = new MemoryStream()) {
                        file.CopyTo(ms);
                        resource.File = ms.ToArray();
                    }
                    _context.Add(resource);
                }
                _context.SaveChanges();
            }

            // ensure we have a default image
            EnsureDefaultImage(clock.Id);

            // add audio file
            if (model.AudioUpload != null) {
                var resource = new Resource {
                    Active = true,
                    Name = string.Format("{0}-Audio", clock.Name),
                    FileName = model.AudioUpload.FileName,
                    ContentType = model.AudioUpload.ContentType,
                    ClockId = clock.Id,
                    FileType = FileType.Audio
                };

                using (var ms = new MemoryStream()) {
                    model.AudioUpload.CopyTo(ms);
                    resource.File = ms.ToArray();
                }
                _context.Add(resource);
                _context.SaveChanges();
            }

            return Redirect("/admin/clocks");
        }

        public IActionResult Edit(int id = 0) {
            var clock = _context.Clocks.FirstOrDefault(p => p.Id == id);
            if (clock != null) {
                var resources = _context.Resources.Where(p => p.ClockId == clock.Id && p.FileType == FileType.Image && p.ParentTypeId == 0).Select(p => new {
                    id = p.Id,
                    fileName = p.FileName,
                    isDefault = p.Default,
                    parentTypeId = p.ParentTypeId
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
                    Featured = clock.Featured,
                    Filters = ClockFilters.FromCollection(clock.FullFilters),
                    BuyNowId = clock.BuyNowId
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
                clock.Features = model.Features?.TrimEnd('\r', '\n');
                clock.Caveats = model.Caveats?.TrimEnd('\r', '\n');
                clock.Description = model.Description;
                clock.Active = model.Active;
                clock.Featured = model.Featured;
                clock.Filters = model.Filters.ToString();
                clock.BuyNowId = model.BuyNowId;
            }
            _context.SaveChanges();

            // images
            if (model.FileUploads != null) {
                foreach (var file in model.FileUploads) {
                    var resource = new Resource {
                        Name = clock.Name,
                        FileName = file.FileName,
                        ContentType = file.ContentType,
                        Active = true,
                        ClockId = clock.Id,
                        FileType = FileType.Image
                    };

                    using (var ms = new MemoryStream()) {
                        file.CopyTo(ms);
                        resource.File = ms.ToArray();
                    }
                    _context.Add(resource);
                }
                _context.SaveChanges();
            }

            // ensure we have a default image
            EnsureDefaultImage(clock.Id);

            // edit audio file
            if (model.AudioUpload != null) {
                var audio = _context.Resources.FirstOrDefault(p => p.ClockId == clock.Id && p.FileType == FileType.Audio);
                if (audio == null) {
                    audio = new Resource {
                        Active = true,
                        Name = string.Format("{0}-Audio", clock.Name),
                        ClockId = clock.Id,
                        FileType = FileType.Audio
                    };
                    _context.Add(audio);
                }                

                using (var ms = new MemoryStream()) {
                    model.AudioUpload.CopyTo(ms);
                    audio.FileName = model.AudioUpload.FileName;
                    audio.ContentType = model.AudioUpload.ContentType;
                    audio.File = ms.ToArray();
                }
                _context.SaveChanges();
            }

            return Redirect("/admin/clocks");
        }

        [HttpPost]
        public IActionResult Sort([FromBody]ClockSortModel[] models) {
            int numberSorted = 0;
            var clocks = _context.Clocks.ToList();
            foreach (var model in models) {
                var clock = clocks.FirstOrDefault(p => p.Number == model.number);
                if (clock != null && clock.SortOrder != model.sortOrder) {
                    clock.SortOrder = model.sortOrder;
                    numberSorted++;
                }
            }
            _context.SaveChanges();
            return Json(new { message = "sorted: " + numberSorted + " clocks" });
        }

        private void EnsureDefaultImage(int id) {
            var resources = _context.Resources.Where(p => p.ClockId == id).ToList();

            // check existing
            var defaultImage = resources.FirstOrDefault(p => p.Default);
            if (defaultImage != null) {
                return;
            }

            // check for a dash 1 image
            defaultImage = resources.FirstOrDefault(p => p.FileName.Contains("-1."));
            if (defaultImage == null) {
                defaultImage = _context.Resources.FirstOrDefault();
            }

            // if we have an image make it the default
            if (defaultImage != null) {
                defaultImage.Default = true;
                _context.SaveChanges();
            }
        }
    }
}