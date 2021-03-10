using Badger.Common.Diagnostics;
using System;
using System.Diagnostics;

namespace Badger.Deployment.Servicing {
    [DebuggerDisplay(DebugView.Default)]
    public class CheckForUpdateResponse {
        public Version FutureVersion { get; set; }

        public PackageDefinition FuturePackageEntry { get; set; }

        protected virtual string GetDebuggerDisplay() {
                var V2 = FutureVersion?.ToString() ?? "(null)";

                var ret = $@"{V2} ({FuturePackageEntry})";

                return ret;
        }

    }
}
