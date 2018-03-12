using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website.Infrastructure.Services;
using Website.Areas.Admin.Models;
using Website.Infrastructure.Data.Entities;
using System.IO;

namespace Website.Areas.Admin.Controllers {
    [Area("Admin"), Authorize]
    public class PartsController : Controller {
        private readonly IDomainContext _context;

        public PartsController(IDomainContext context) {
            _context = context;
        }

        public IActionResult Index() {
            var parts = _context.Parts.ToList();
            return View(parts);
        }

        public IActionResult Add() {
            return View(new PartCreateEditModel { });
        }

        [HttpPost]
        public IActionResult Add(PartCreateEditModel model) {
            if (!ModelState.IsValid)
                return View(model);

            // add the part
            var part = new Part {
                Title = model.Title,
                Price = model.Price,
                Shipping = model.Shipping,
                BuyNowId = model.BuyNowId,
                Description = model.Description,
                IsLenzkirch = model.IsLenzkirch,
            };
            _context.Add(part);
            _context.SaveChanges();

            // add the resources
            if (model.FileUploads != null) {
                foreach (var file in model.FileUploads) {
                    var resource = new Resource {
                        Active = true,
                        Name = part.Title,
                        FileName = file.FileName,
                        ContentType = file.ContentType,
                        ClockId = part.Id,
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
            EnsureDefaultImage(part.Id);

            return Redirect("/admin/parts");
        }

        public IActionResult Edit(int id = 0) {
            var part = _context.Parts.FirstOrDefault(p => p.Id == id);
            if (part != null) {
                return View(new PartCreateEditModel {
                    Id = part.Id,
                    Title = part.Title,
                    Price = part.Price,
                    Shipping = part.Shipping,
                    BuyNowId = part.BuyNowId,
                    Description = part.Description,
                    IsLenzkirch = part.IsLenzkirch,
                });
            }
            return Redirect("/admin/parts");
        }

        [HttpPost]
        public IActionResult Edit(PartCreateEditModel model) {
            if (!ModelState.IsValid)
                return View(model);

            var part = _context.Parts.FirstOrDefault(p => p.Id == model.Id);
            if (part != null) {
                part.Title = model.Title;
                part.Price = model.Price;
                part.Shipping = model.Shipping;
                part.Description = model.Description;
                part.BuyNowId = model.BuyNowId;
                part.IsLenzkirch = model.IsLenzkirch;

                _context.SaveChanges();
            }

            // images
            if (model.FileUploads != null) {
                foreach (var file in model.FileUploads) {
                    var resource = new Resource {
                        Name = part.Title,
                        FileName = file.FileName,
                        ContentType = file.ContentType,
                        Active = true,
                        ClockId = part.Id,
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
            EnsureDefaultImage(part.Id);

            return Redirect("/admin/parts");
        }

        [HttpPost]
        public IActionResult Sort([FromBody]PartSortModel[] models) {
            foreach (var model in models) {
                var part = _context.Parts.FirstOrDefault(p => p.Id == model.id);
                if (part != null)
                    part.SortOrder = model.sortOrder;
            }
            _context.SaveChanges();

            return Json(new { message = "this happened." });
        }

        private void EnsureDefaultImage(int id) {
            var resources = _context.Resources.Where(p => p.ClockId == id).ToList();

            // check existing
            var defaultImage = resources.FirstOrDefault(p => p.Default);
            if (defaultImage != null)
                return;

            // check for a dash 1 image
            defaultImage = resources.FirstOrDefault(p => p.FileName.Contains("-1."));
            if (defaultImage == null)
                defaultImage = _context.Resources.FirstOrDefault();

            // if we have an image make it the default
            if (defaultImage != null) {
                defaultImage.Default = true;
                _context.SaveChanges();
            }
        }
    }
}