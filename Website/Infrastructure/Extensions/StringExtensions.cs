using System.Collections.Generic;
using System.Linq;

namespace Website.Infrastructure.Extensions {
    public static class StringExtentions {
        public static string FormatWith(this string str, params object[] objs) {
            return string.Format(str, objs);
        }

        public static string Combine(this IEnumerable<string> strArray, string separator = " ") {
            return string.Join(separator, strArray.ToArray());
        }
    }
}
