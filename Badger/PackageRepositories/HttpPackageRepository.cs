using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Badger.PackageRepositories {
    public class HttpPackageRepository : PackageRepository {
        public string RepositoryUrl { get; private set; }
        public string ReleasesUrl { get; private set; }

        public HttpPackageRepository(string Url) {
            Initialize($@"{Url}", $@"{Url}/{Environment.ReleasesFileName}");
        }

        public HttpPackageRepository(string RepositoryUrl, string ReleasesUrl) {

        }

        private void Initialize(string RepositoryUrl, string ReleasesUrl) {
            this.RepositoryUrl = RepositoryUrl;
            this.ReleasesUrl = ReleasesUrl;
        }

        protected Lazy<HttpClient> LazyHttpClient => new Lazy<HttpClient>(() => CreateHttpClient());
        protected HttpClient HttpClient => LazyHttpClient.Value;
        protected virtual HttpClient CreateHttpClient() {
            var ret = new HttpClient();

            return ret;
        }


        public override async Task<List<PackageEntry>> AvailablePackages() {
            var ret = new List<PackageEntry>();

                var ReleasesContent = await HttpClient.GetStringAsync(ReleasesUrl)
                    .DefaultAwait()
                    ;

                var ToAdd = PackageEntry.ParseMany(ReleasesContent);
                ret.AddRange(ToAdd);

            return ret;
        }

        protected override async Task<Stream> AcquirePackageInteral(PackageEntry Entry) {

            var ret = await HttpClient.GetStreamAsync($@"{RepositoryUrl}/{Entry.FileName}")
                .DefaultAwait()
                ;

            return ret;
        }
    }
}
