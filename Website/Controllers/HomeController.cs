using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Website.Models;

namespace Website.Controllers {
    public class HomeController : Controller {
        public IActionResult Index() {
            var clocks = new List<ClockThumbModel>();

            clocks.Add(new ClockThumbModel {
                Link = "/clocks/GE035-1",
                Image = "/images/GE035-1-Thumb.jpg",
                ImageAlt = "French Gothic Cathedral Clock",
                Title = "High Quality Clocks For Sale",
                Description = "See this French Gothic Cathedral Clock, a Vienna table clocks with music and other fine clocks."
            });

            clocks.Add(new ClockThumbModel {
                Link = "/clocks/GE036-1",
                Image = "/images/GE036-1-Thumb.jpg",
                ImageAlt = "30-Day Vienna Regulator Inlayed Case",
                Title = "Vienna Regulators",
                Description = "See Vienna Regulators by Felsing, Schonberger, Salfer, Becker, Lenzkirch, and others famous makers."
            });

            clocks.Add(new ClockThumbModel {
                Link = "/book/order",
                Image = "/images/book-cover.jpg",
                ImageAlt = "book-cover",
                Title = "Lenzkirch Clocks, the Unsigned Story",
                Description = "My book is a technical study of Lenzkirch clocks like nothing ever published before.  It is a must have book for the Lenzkirch Clock collector."
            });

            clocks.Add(new ClockThumbModel {
                Link = "/clocks/GE039-1",
                Image = "/images/GE039-1-Thumb.jpg",
                ImageAlt = "Lenzkirch Model 310 Jeweler's Regulator",
                Title = "Lenzkirch Clocks For Sale",
                Description = "See Lenzkirch clocks for sale along with many precise regulators"
            });

            clocks.Add(new ClockThumbModel {
                Link = "/clocks/GE038-1",
                Image = "/images/GE038-1-Thumb.jpg",
                ImageAlt = "John Knibb Hodded Lantern",
                Title = "John Knibb Hooded Lantern Ca. 1680",
                Description = "See this Rare John Knibb Hooded Lantern Clock."
            });

            clocks.Add(new ClockThumbModel {
                Link = "/clocks/GE037-1",
                Image = "/images/GE037-1-Thumb.jpg",
                ImageAlt = "J. Salfer, Wien Calendar",
                Title = "Johann Salfer Calendar",
                Description = "See this one-of-a-kind J. Salfer Wien Calendar clock"
            });

            return View(clocks);
        }
    }
}
