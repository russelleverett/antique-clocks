using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

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
        public bool Featured { get; set; }
        public string Filters { get; set; }
        public virtual IEnumerable<Resource> Resources { get; set; }
        public int SortOrder { get; set; }

        [NotMapped]
        public List<string> FullFeatures {
            get {
                var features = new List<string>();
                if (Features != null) {
                    foreach (var feature in Features.Replace("\n", "").Split('\r')) {
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
                    foreach (var caveat in Caveats.Replace("\n", "").Split('\r')) {
                        if (!string.IsNullOrEmpty(caveat))
                            caveats.Add(caveat);
                    }
                }
                else caveats.Add("None");
                return caveats;
            }
        }

        [NotMapped]
        public List<string> FullFilters {
            get {
                var filters = new List<string>();
                if (Filters != null) {
                    foreach (var filter in Filters.Split('|')) {
                        if (!string.IsNullOrEmpty(filter))
                            filters.Add(filter);
                    }
                }
                return filters;
            }
        }

        [NotMapped]
        public bool IsSold {
            get {
                if(Filters != null)
                    return Filters.Contains("sold");
                return false;
            }
        }

        [NotMapped]
        public Resource ClockAudio { get; set; }

        public string PintrestLink(int? imageId) {
            return string.Format("http://www.pinterest.com/pin/create/button/?url={0}&media={1}&description={1}",
                WebUtility.UrlEncode("http://antique-clock.com"),
                WebUtility.UrlEncode(string.Format("http://antique-clock.com/images/{0}", imageId ?? 0)),
                WebUtility.UrlEncode(Name + " at antique-clock.com"));
        }
    }
}
