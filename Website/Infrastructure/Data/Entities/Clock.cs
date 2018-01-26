using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website.Infrastructure.Data.Entities {
    public class Clock : IEntity {
        public int Id { get; set; }
        public string BuyNowId { get; set; }
        public string Caveats { get; set; }
        public string Description { get; set; }
        public string Features { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public decimal? Price { get; set; }
        public bool Active { get; set; }
        public virtual IEnumerable<Resource> Resources { get; set; }

        [NotMapped]
        public List<string> FullFeatures {
            get {
                var features = new List<string>();
                if (Features != null) {
                    foreach (var feature in Features.Split('\r')) {
                        if(!string.IsNullOrEmpty(feature))
                            features.Add(feature);
                    }
                }
                else features.Add("None");
                return features;
            }
        }

        [NotMapped]
        public List<string> FullCaveats {
            get {
                var caveats = new List<string>();
                if (Caveats != null) {
                    foreach (var caveat in Caveats.Split('\r')) {
                        if (!string.IsNullOrEmpty(caveat))
                            caveats.Add(caveat);
                    }
                }
                else caveats.Add("None");
                return caveats;
            }
        }
    }
}
