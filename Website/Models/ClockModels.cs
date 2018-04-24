using System.Collections.Generic;

namespace Website.Models {
    public class ClockFeatureModel {
        public int Id { get; set; }
        public int? ImageId { get; set; }
        public string ImageAlt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ClockBrowseModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public int? ImageId { get; set; }
        public string ImageName { get; set; }
        public string PintrestLink { get; set; }
        public int SortOrder { get; set; }
        public bool ShowDetails { get; set; }
    }

    public class HomePageModel {
        public IEnumerable<string> Messages { get; set; }
        public IEnumerable<ClockFeatureModel> FeaturedClocks { get; set; }
    }
}
