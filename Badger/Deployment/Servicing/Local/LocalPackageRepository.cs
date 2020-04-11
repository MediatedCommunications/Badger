using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Deployment.Servicing.Local {
    public class LocalPackageRepository : PackageRepository {

        public string RepositoryFolder { get; private set; }
        public string ReleasesFullPath { get; private set; }

        public LocalPackageRepository(string Folder) {
            Initialize($@"{Folder}", $@"{Folder}/{LocationHelpers.ReleasesFileName}");
        }

        public LocalPackageRepository(string RepositoryFolder, string ReleasesFullPath) {
            Initialize(RepositoryFolder, ReleasesFullPath);
        }

        private void Initialize(string RepositoryFolder, string ReleasesFullPath) {
            this.RepositoryFolder = RepositoryFolder;
            this.ReleasesFullPath = ReleasesFullPath;
        }

        public override Task<List<TextPackageDefinition>> AvailablePackages() {
            var ret = new List<TextPackageDefinition>();

            if (System.IO.File.Exists(ReleasesFullPath)) {
                var Content = System.IO.File.ReadAllText(ReleasesFullPath);
                ret.AddRange(TextPackageDefinition.ParseMany(Content));
            }

            return Task.FromResult(ret);
        }

        public override Task<Stream> AcquirePackageStream(TextPackageDefinition Entry) {
            var Source = $@"{RepositoryFolder}/{Entry.FileName}";

            var ret = (Stream)System.IO.File.OpenRead(Source);

            return Task.FromResult(ret);
        }

    }
}
