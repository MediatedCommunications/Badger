using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger {
    public static class StringExtensions {
        public static bool IsNullOrEmpty(this string This) {
            return String.IsNullOrEmpty(This);
        }
    }
}
