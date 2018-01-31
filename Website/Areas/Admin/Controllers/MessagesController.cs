using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Website.Infrastructure.Services;
using Website.Infrastructure.Data.Entities;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Website.Areas.Admin.Controllers {
    [Area("Admin"), Authorize]
    public class MessagesController : Controller {
        private readonly IDomainContext _context;

        public MessagesController(IDomainContext context) {
            _context = context;
        }

        public IActionResult Index() {
            var messages = _context.Messages.ToList();
            return View(messages);
        }

        public IActionResult Add() {
            return View(new Messages {
                StartDate = DateTime.Now
            });
        }

        [HttpPost]
        public IActionResult Add(Messages model) {
            if (!ModelState.IsValid)
                return View(model);

            _context.Add(model);
            _context.SaveChanges();

            return Redirect("/admin/messages");
        }

        public IActionResult Edit(int id = 0) {
            var model = _context.Messages.FirstOrDefault(p => p.Id == id);
            if (model == null)
                return Redirect("/admin/messages");

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Messages model) {
            if (!ModelState.IsValid)
                return View(model);

            var message = _context.Messages.FirstOrDefault(p => p.Id == model.Id);
            if (message == null)
                return Redirect("/admin/messages");

            message.Message = model.Message;
            message.StartDate = model.StartDate;
            message.EndDate = model.EndDate;
            _context.SaveChanges();

            return Redirect("/admin/messages");
        }
    }
}