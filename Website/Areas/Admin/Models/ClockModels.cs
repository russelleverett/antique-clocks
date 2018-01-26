using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Website.Infrastructure.Data.Entities;

namespace Website.Areas.Admin.Models {
    public class ClockCreateModel {
        [Required]
        public string Name { get; set; }

        [Required, DisplayName("Clock Number")]
        public string Number { get; set; }

        public string Caveats { get; set; }

        public string Description { get; set; }

        public string Features { get; set; }

        [DisplayName("Show In Listing")]
        public bool Active { get; set; }

        [DisplayName("Buy Now Id")]
        public string BuyNowId { get; set; }

        public decimal? Price { get; set; }

        [DisplayName("Image Files")]
        public List<IFormFile> FileUploads { get; set; }
    }

    public class ClockEditModel {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, DisplayName("Clock Number")]
        public string Number { get; set; }

        public string Caveats { get; set; }

        public string Description { get; set; }

        public string Features { get; set; }

        [DisplayName("Show In Listing")]
        public bool Active { get; set; }

        [DisplayName("Buy Now Id")]
        public string BuyNowId { get; set; }

        public decimal? Price { get; set; }

        [DisplayName("Image Files")]
        public List<IFormFile> FileUploads { get; set; }

        public dynamic Images { get; set; }
    }
}
