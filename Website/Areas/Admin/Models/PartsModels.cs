using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Website.Infrastructure.Data.Entities;

namespace Website.Areas.Admin.Models {
    public class PartCreateEditModel {
        public int Id { get; set; }
        [Required, StringLength(100, ErrorMessage = "Title must be less than 100 characters.")]
        public string Title { get; set; }
        [Required, StringLength(500, ErrorMessage = "Description must be less than 500 characters.")]
        public string Description { get; set; }
        [Required]
        public decimal? Price { get; set; }
        [Required]
        public decimal? Shipping { get; set; }
        [StringLength(20, ErrorMessage = "Buy Now Id must be less than 20 characters.")]
        public string BuyNowId { get; set; }
        [DisplayName("Image Files")]
        public List<IFormFile> FileUploads { get; set; }
        [DisplayName("Is Lenzkirch?")]
        public bool IsLenzkirch { get; set; }
        [DisplayName("Show in List")]
        public bool Active { get; set; }
        public dynamic Images { get; set; }
    }

    public class PartSortModel {
        public int id { get; set; }
        public int sortOrder { get; set; }
    }
}
