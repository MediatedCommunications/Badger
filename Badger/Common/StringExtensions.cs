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

    }

}
