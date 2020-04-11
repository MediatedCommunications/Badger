using Badger.Common.Diagnostics;
using System;
using System.Diagnostics;

namespace Badger.Deployment.Servicing {
    [DebuggerDisplay(DebugView.Default)]
    public class CheckForUpdateResponse {
        public Version CurrentVersion { get; set; }
        public Version FutureVersion { get; set; }

        public PackageDefinition FuturePackageEntry { get; set; }

        protected virtual string DebuggerDisplay {
            get {
                var V1 = CurrentVersion?.ToString() ?? "(null)";
                var V2 = FutureVersion?.ToString() ?? "(null)";

                var ret = $@"{V1} -> {V2} ({FuturePackageEntry})";

                return ret;
            }
        }

    }
}
