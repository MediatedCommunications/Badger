using Badger.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Deployment.Servicing {
    public abstract class PackageRepository {

        public abstract Task<List<TextPackageDefinition>> AvailablePackages();

        public async Task<List<TextPackageDefinition>> AvailablePackagesSafe() {
            var ret = new List<TextPackageDefinition>();
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

        public abstract Task<Stream> AcquirePackageStream(TextPackageDefinition Entry);

    }
}
