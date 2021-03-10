using Badger.Common;
using Badger.Common.Security;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Badger.Deployment.Servicing {

    public static class PackageRepositoryExtensions {
        public static async Task<CheckForUpdateResponse> CheckForUpdate(this PackageRepository This, Location Environment = null) {
            Environment = Environment ?? Location.Current;

            var ret = new CheckForUpdateResponse();

            var AvailableReleases = await This.AvailablePackages()
                .ConfigureAwait(false)
                ;

            //Find the newest available version
            var PotentialFutureVersion = (
                from x in AvailableReleases
                let FN = x.FileName()
                let VN = FN.VersionNumber
                where VN != null 
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


        public static async Task<Package> AcquirePackage(this PackageRepository This, TextPackageDefinition Entry, Location Environment = null) {
            Environment = Environment ?? Location.Current;

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
