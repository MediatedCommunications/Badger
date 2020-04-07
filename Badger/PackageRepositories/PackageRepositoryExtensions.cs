using Badger.Diagnostics;
using Badger.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Badger {

    [DebuggerDisplay(DebugView.Default)]
    public class CheckForUpdateResponse {
        public Version CurrentVersion { get; set; }
        public Version FutureVersion { get; set; }

        public PackageEntry FuturePackageEntry { get; set; }

        protected virtual string DebuggerDisplay {
            get {
                var V1 = CurrentVersion?.ToString() ?? "(null)";
                var V2 = FutureVersion?.ToString() ?? "(null)";

                var ret = $@"{V1} -> {V2} ({FuturePackageEntry})";

                return ret;
            }
        }

    }

    public static class PackageRepositoryExtensions {
        public static async Task<CheckForUpdateResponse> CheckForUpdate(this PackageRepository This, Environment Environment = null) {
            Environment = Environment ?? Environment.Default;

            var ret = new CheckForUpdateResponse();

            var AvailableReleases = await This.AvailablePackages()
                .ConfigureAwait(false)
                ;

            ret.CurrentVersion = Environment.Version;

            //Find the newest available version
            var PotentialFutureVersion = (
                from x in AvailableReleases
                let FN = x.FileName()
                let VN = FN.VersionNumber
                where VN != null && (ret.CurrentVersion == null || VN > ret.CurrentVersion)
                orderby VN descending
                select new {
                    PackageEntry = x,
                    FileName = FN,
                    Version = VN
                })
                .FirstOrDefault();

            ret.FuturePackageEntry = PotentialFutureVersion?.PackageEntry;
            ret.FutureVersion = PotentialFutureVersion?.Version;
            
            return ret;
        }


        public static async Task<Package> AcquirePackage(this PackageRepository This, PackageEntry Entry, Badger.Environment Environment = null) {
            Environment = Environment ?? Badger.Environment.Default;

            var Success = false;
            var ret = default(Package);
            var Exception = default(Exception);

            try {
                var Stream = await This.AcquirePackageStream(Entry)
                    .DefaultAwait()
                    ;

                ret = Package.FromPath(Environment.TempFolder, System.IO.Path.GetExtension(Entry.FileName));
                using (var FS = System.IO.File.OpenWrite(ret.FileName)) {
                    Stream.CopyTo(FS);
                }

                var Expected = Entry.SHA1;
                var Actual = ret.SHA1();

                if (!String.Equals(Expected, Actual, StringComparison.InvariantCultureIgnoreCase)) {
                    throw new InvalidSHA1Exception(Expected, Actual);
                }
                Success = true;
            } catch (Exception ex) {
                Exception = ex;
                Success = false;
            }

            if (!Success) {
                ret?.Dispose();
                ret = null;
            }

            if(Exception != null) {
                throw Exception;
            }


            return ret;
        }

        

    }
}
