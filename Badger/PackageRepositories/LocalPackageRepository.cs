using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.PackageRepositories {
    public class LocalPackageRepository : PackageRepository {

        public string RepositoryFolder { get; private set; }
        public string ReleasesFullPath { get; private set; }

        public LocalPackageRepository(string Folder) {
            Initialize($@"{Folder}", $@"{Folder}/{Environment.ReleasesFileName}");
        }

        public LocalPackageRepository(string RepositoryFolder, string ReleasesFullPath) {
            Initialize(RepositoryFolder, ReleasesFullPath);
        }

        private void Initialize(string RepositoryFolder, string ReleasesFullPath) {
            this.RepositoryFolder = RepositoryFolder;
            this.ReleasesFullPath = ReleasesFullPath;
        }

        public override Task<List<PackageEntry>> AvailablePackages() {
            var ret = new List<PackageEntry>();

            if (System.IO.File.Exists(ReleasesFullPath)) {
                var Content = System.IO.File.ReadAllText(ReleasesFullPath);
                ret.AddRange(PackageEntry.ParseMany(Content));
            }

            return Task.FromResult(ret);
        }

        protected override Task<Stream> AcquirePackageInteral(PackageEntry Entry) {
            var Source = $@"{RepositoryFolder}/{Entry.FileName}";

            var ret = (Stream)System.IO.File.OpenRead(Source);

            return Task.FromResult(ret);
        }

    }
}
