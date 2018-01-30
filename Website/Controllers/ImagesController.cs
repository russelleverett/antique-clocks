using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Website.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Website.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Collections.Generic;

namespace Website.Controllers {
    public class ImagesController : Controller {
        private readonly IDomainContext _context;
        private readonly IConfiguration _config;

        public ImagesController(IDomainContext context, IConfiguration config) {
            _context = context;
            _config = config;
        }

        public IActionResult Index(int id = 0) {
            var acceptedHosts = new List<string> { "pintrest.com", "facebook.com" };
            acceptedHosts.Add(_config.GetValue<string>("Host"));

            // check the host is acceptes
            var host = Request.Headers["Host"].ToString();
            if (!acceptedHosts.Contains(host)) {
                return Json(new {
                    message = "These images are the property of antique-clock.com."
                });
            }
            
            // check that it's not being accessed directly
            var referer = Request.Headers["Referer"].ToString();
            if (string.IsNullOrEmpty(referer) || referer.ToLower().Contains("/images/"))
                return new NotFoundResult();

            // local hosts so serve the default image
            if (host.Contains("localhost")) {
                var filePath = @"./wwwroot/images/coming-soon.png";
                using (var ms = new MemoryStream(System.IO.File.ReadAllBytes(filePath))) {
                    return File(ms.ToArray(), "image/png", "coming-soon.png");
                }
            }

            // serve the image
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