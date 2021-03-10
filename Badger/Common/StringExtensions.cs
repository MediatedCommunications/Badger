using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Common {
    public static class StringExtensions {
        public static bool IsNullOrEmpty(this string This) {
            return String.IsNullOrEmpty(This);
        }

        public static bool IsNotNullOrEmpty(this string This) {
            return !String.IsNullOrEmpty(This);
        }

        public static bool IsBlank(this string This) {
            return string.IsNullOrWhiteSpace(This);
        }

        public static bool IsNotBlank(this string This) {
            return !string.IsNullOrWhiteSpace(This);
        }

        public static string Join(this IEnumerable<string> This, string Separator) {
            var Query = This.Where(x => x.IsNotNullOrEmpty());
            
            var ret = string.Join(Separator, Query);

            return ret;
        }

        public static string JoinSpace(this IEnumerable<string> This) {
            return This.Join(" ");
        }
        public static string JoinDot(this IEnumerable<string> This) {
            return This.Join(".");
        }

    }

}
