using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger {
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

        public abstract Task<Stream> AcquirePackageStream(PackageEntry Entry);

    }
}
