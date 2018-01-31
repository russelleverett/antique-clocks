using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website.Infrastructure.Data.Entities {
    [Table("Messages")]
    public class Messages : IEntity {
        public int Id { get; set; }
        [Required]
        public string Message { get; set; }
        [Required, DisplayName("Start Date")]
        public DateTime StartDate { get; set; }
        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }
        [NotMapped]
        public bool Delete { get; set; }
    }
}
