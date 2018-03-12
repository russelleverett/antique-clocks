using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Website.Infrastructure.Services;
using Website.Areas.Admin.Models;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Website.Areas.Admin.Controllers {
    [Area("Admin")]
    public class AccountController : Controller {
        private readonly IDomainContext _context;

        public AccountController(IDomainContext context) {
            _context = context;
        }

        public IActionResult Login(string returnUrl) {
            ViewBag.returnUrl = WebUtility.UrlEncode(returnUrl);
            return View(new LoginModel());
        }

        [HttpPost]
        public IActionResult Login(LoginModel model, string returnUrl) {
            if (!ModelState.IsValid)
                return View(model);

            var user = _context.Users.FirstOrDefault(p => p.Username == model.Username);
            if (user != null) {
                var passwordHash = Hash(user.Salt + model.Password);
                HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Username)
                }, "formsAuthentication")));

                return Redirect(returnUrl ?? "/admin/clocks");
            }

            return View(model);
        }

        public IActionResult Logout() {
            HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/admin/account/login");
        }

        #region Helper methods
        public string Hash(string password) {
            using (var hasher = MD5.Create()) {
                var result = hasher.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(result);
            }
        }
        #endregion
    }
}