using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website.Infrastructure.Data.Entities {
    public class Part : IEntity {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? Shipping { get; set; }
        public string BuyNowId { get; set; }
        public bool IsLenzkirch { get; set; }
        public int SortOrder { get; set; }
    }
}
