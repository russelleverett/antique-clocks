using System.Linq;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
using System.Reflection;
using Website.Infrastructure.Extensions;

namespace Website.Infrastructure.Data.Entities {
    public enum FileType {
        Image = 0,
        Audio = 1
    }

    public class Resource : IEntity {
        public int Id { get; set; }
        public int ClockId { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] File { get; set; }
        public bool Active { get; set; }
        public bool Default { get; set; }
        public FileType FileType { get; set; }

        public HtmlString ImageTag(dynamic props = null) {
            // build the property collection
            var properties = new Dictionary<string, string>();
            var iconString = string.Empty;

            // check for dynamic properties
            if (props != null) {
                var _props = props.GetType().GetProperties();
                foreach (PropertyInfo _prop in _props) {
                    properties.Add(_prop.Name.Replace('_', '-'), _prop.GetValue(props));
                }
            }

            properties.Add("src", "/Images/" + Id);
            properties.Add("alt", FileName);

            // condense and 
            var propString = properties.Keys.Select(p => "{0}=\"{1}\"".FormatWith(p, properties[p])).Combine();
            return new HtmlString("<img {0} />".FormatWith(propString));
        }
    }
}
