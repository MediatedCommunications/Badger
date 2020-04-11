using System;

namespace Badger.Common {
    public static class ExceptionExtensions {
        internal static void Ignore(this Exception ex) {
            ex.Equals(ex);
        }
    }
}
