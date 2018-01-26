﻿namespace Website.Models {
    public class ClockThumbModel {
        public string Link { get; set; }
        public string Image { get; set; }
        public string ImageAlt { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class ClockBrowseModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public int ImageId { get; set; }
        public string ImageName { get; set; }
        public string PintrestLink { get; set; }
    }
}