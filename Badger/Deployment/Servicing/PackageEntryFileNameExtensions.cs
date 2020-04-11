using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Deployment.Servicing {
    public static class PackageEntryFileNameExtensions {

        private const string IsFull_Tag1 = "";
        private const string IsFull_Tag2 = "full";

        public static bool IsEdition(this PackageDefinitionFileName This, string Edition) {
            return false
                || string.Equals(This.Edition, Edition, StringComparison.InvariantCultureIgnoreCase)
                ;
        }

        public static bool IsFull(this PackageDefinitionFileName This) {
            return false
                || This.IsEdition(IsFull_Tag1)
                || This.IsEdition(IsFull_Tag2)
                ;
        }

        private const string IsDelta_Tag1 = "delta";
        public static bool IsDelta(this PackageDefinitionFileName This) {
            return false
                || This.IsEdition(IsDelta_Tag1)
                ;
        }

    }
}
