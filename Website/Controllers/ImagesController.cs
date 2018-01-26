using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Website.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Website.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace Website.Controllers {
    public class ImagesController : Controller {
        private readonly IDomainContext _context;
        private readonly IConfiguration _config;

        public ImagesController(IDomainContext context, IConfiguration config) {
            _context = context;
            _config = config;
        }

        public IActionResult Index(int id = 0) {
            // check the referrer
            var accepted = _config.GetValue<string>("Referer");
            var referrer = Request.Headers["Host"].ToString();
            if (referrer != accepted && !referrer.Contains("pintrest.com")) {
                return Json(new {
                    message = "These images are the property of antique-clock.com."
                });
            }
            //else {
            //    var filePath = @"./wwwroot/images/coming-soon.png";
            //    using (var ms = new MemoryStream(System.IO.File.ReadAllBytes(filePath))) {
            //        return File(ms.ToArray(), "image/png", "coming-soon.png");
            //    }
            //}

            var resource = _context.Resources.FirstOrDefault(p => p.Id == id);
            if (resource != null) {
                return File(resource.File, resource.ContentType, resource.FileName);
            }
            else {
                var filePath = @"./wwwroot/images/coming-soon.png";
                using (var ms = new MemoryStream(System.IO.File.ReadAllBytes(filePath))) {
                    return File(ms.ToArray(), "image/png", "coming-soon.png");
                }
            }
        }

        [HttpDelete, Authorize]
        public IActionResult Remove(int id = 0) {
            var resource = _context.Resources.FirstOrDefault(p => p.Id == id);
            if (resource != null) {
                _context.Delete<Resource>(id);
                _context.SaveChanges();

                var resources = _context.Resources.Where(p => p.ClockId == resource.ClockId).Select(p => new {
                    id = p.Id,
                    fileName = p.FileName
                }).ToList();
                return Json(resources);
            }
            return Json(new { });
        }
    }
}