using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Website.Areas.Admin.Models {
    public class ClockViewModel {
        public int SortOrder { get; set; }
        public int Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public bool IsSold { get; set; }
        public bool Featured { get; set; }
    }

    public class ClockCreateModel {
        [Required]
        public string Name { get; set; }

        [Required, DisplayName("Clock Number")]
        [StringLength(10, ErrorMessage = "Number must be less than 10 characters.")]
        public string Number { get; set; }

        [StringLength(200, ErrorMessage = "Caveats must be less than 200 characters.")]
        public string Caveats { get; set; }

        public string Description { get; set; }

        [StringLength(400, ErrorMessage = "Features must be less than 400 characters.")]
        public string Features { get; set; }

        [DisplayName("Show In Listing")]
        public bool Active { get; set; }

        [DisplayName("Feature On Home Page")]
        public bool Featured { get; set; }

        [DisplayName("Buy Now Id"), StringLength(20, ErrorMessage = "Buy Now Id must be less than 20 characters.")]
        public string BuyNowId { get; set; }

        public decimal? Price { get; set; }

        [DisplayName("Image Files")]
        public List<IFormFile> FileUploads { get; set; }

        public ClockFilters Filters { get; set; }

        [DisplayName("Audio File")]
        public IFormFile AudioUpload { get; set; }
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

        [DisplayName("Feature On Home Page")]
        public bool Featured { get; set; }

        [DisplayName("Buy Now Id")]
        public string BuyNowId { get; set; }

        public decimal? Price { get; set; }

        [DisplayName("Image Files")]
        public List<IFormFile> FileUploads { get; set; }

        public dynamic Images { get; set; }

        public ClockFilters Filters { get; set; }

        [DisplayName("Audio File")]
        public IFormFile AudioUpload { get; set; }
    }

    public class ClockFilters {
        public bool Lenzkirch { get; set; }

        [DisplayName("Vienna Regulator")]
        public bool ViennaRegulator { get; set; }

        [DisplayName("Wall Clock")]
        public bool WallClock { get; set; }

        [DisplayName("Table Clock")]
        public bool TableClock { get; set; }

        [DisplayName("Miniature Clock")]
        public bool MiniatureClock { get; set; }

        [DisplayName("Grandfather Clock")]
        public bool GrandfatherClock { get; set; }

        public bool Sold { get; set; }

        public override string ToString() {
            var sb = new StringBuilder();

            sb.Append((Lenzkirch) ? "lenzkirch|" : "");
            sb.Append((ViennaRegulator) ? "vienna|" : "");
            sb.Append((WallClock) ? "wall|" : "");
            sb.Append((TableClock) ? "table|" : "");
            sb.Append((MiniatureClock) ? "miniature|" : "");
            sb.Append((GrandfatherClock) ? "grandfather|" : "");
            sb.Append((Sold) ? "sold|" : "");

            return sb.ToString().TrimEnd('|');
        }

        public static ClockFilters FromCollection(List<string> collection) {
            return new ClockFilters {
                Lenzkirch = collection.Contains("lenzkirch"),
                ViennaRegulator = collection.Contains("vienna"),
                WallClock = collection.Contains("wall"),
                TableClock = collection.Contains("table"),
                MiniatureClock = collection.Contains("miniature"),
                GrandfatherClock = collection.Contains("grandfather"),
                Sold = collection.Contains("sold")
            };
        }
    }

    public class ClockSortModel {
        public string number { get; set; }
        public int sortOrder { get; set; }
    }
}
