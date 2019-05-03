using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.PackageRepositories {
    public abstract class PackageRepository {

        public abstract Task<List<PackageEntry>> AvailablePackages();

        public async Task<List<PackageEntry>> AvailablePackagesSafe() {
            var ret = new List<PackageEntry>();
            try {
                var Items = await AvailablePackages()
                    .DefaultAwait()
                    ;

                ret.AddRange(Items);

            } catch (Exception ex) {
                ex.Ignore();
            }


            return ret;
        }

        protected abstract Task<Stream> AcquirePackageInteral(PackageEntry Entry);

        public async Task<CachedPackage> AcquirePackage(PackageEntry Entry) {
            var Stream = await AcquirePackageInteral(Entry)
                .DefaultAwait()
                ;

            var Package = CachedPackage.Create(Stream, Entry.SHA1);

            return Package;
        }
    }
}
