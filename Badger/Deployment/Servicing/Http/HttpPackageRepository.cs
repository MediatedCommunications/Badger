using Badger.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Deployment.Servicing.Http {
    public class HttpPackageRepository : PackageRepository {
        public Uri RepositoryRootUrl { get; private set; }
        public Uri ReleasesUrl { get; private set; }

        protected static class QueryParameters {
            public const string LocalVersion = "localVersion";
            public const string Architecture = "arch";

            public const string Architecture_Unknown = "unknown";
            public const string Architecture_x86 = "x86";
            public const string Architecture_Amd64 = "amd64";
            public const string Architecture_Arm64 = "arm64";


            public const string InstallId = "installid";

        }

        protected virtual Version Repository_QueryParameter_Version() {
            return Location.Current.VersionFolder.Version;
        }

        protected virtual string Repository_QueryParameter_Architecture() {
            var ret = QueryParameters.Architecture_Unknown;
            if (System.Environment.Is64BitOperatingSystem) {
                ret = QueryParameters.Architecture_Amd64;
            } else {
                ret = QueryParameters.Architecture_x86;
            }

            return ret;
        }


        protected virtual string Repository_QueryParameter_InstallId() {
            return Location.Current.InstanceId?.ToString();
        }

        protected virtual Dictionary<String, String> Repository_QueryParameters() {
            var ret = new Dictionary<string, string>() {
                {QueryParameters.LocalVersion, Repository_QueryParameter_Version()?.ToString() },
                {QueryParameters.Architecture, Repository_QueryParameter_Architecture() },
                {QueryParameters.InstallId, Repository_QueryParameter_InstallId()}
            };

            return ret;
        }


        public HttpPackageRepository(string RepositoryUrl, string ReleasesUrl = null) {
            var RepositoryUri = new Uri(RepositoryUrl);
            var ReleasesUri = String.IsNullOrWhiteSpace(ReleasesUrl)
                ? null
                : new Uri(ReleasesUrl)
                ;

            Initialize(RepositoryUri, ReleasesUri);
        }

        public HttpPackageRepository(Uri RepositoryUri, Uri ReleasesUri = null) {
            Initialize(RepositoryUri, ReleasesUri);
        }

        private void Initialize(Uri RepositoryUrl, Uri ReleasesUrl) {
            this.RepositoryRootUrl = RepositoryUrl;

            {
                if(ReleasesUrl == null) {
                    var Builder = new UriBuilder(RepositoryUrl);
                    Builder.AppendPath(LocationHelpers.ReleasesFileName);
                    Builder.AppendQuery(Repository_QueryParameters());

                    ReleasesUrl = Builder.Uri;
                }
            }

            this.ReleasesUrl = ReleasesUrl;
        }

        protected Lazy<HttpClient> LazyHttpClient => new Lazy<HttpClient>(() => CreateHttpClient());
        protected HttpClient HttpClient => LazyHttpClient.Value;
        protected virtual HttpClient CreateHttpClient() {
            var ret = new HttpClient();

            return ret;
        }


        public override async Task<List<TextPackageDefinition>> AvailablePackages() {
            var ret = new List<TextPackageDefinition>();

                var ReleasesContent = await HttpClient.GetStringAsync(ReleasesUrl)
                    .DefaultAwait()
                    ;

                var ToAdd = TextPackageDefinition.ParseMany(ReleasesContent);
                ret.AddRange(ToAdd);

            return ret;
        }

        public override async Task<Stream> AcquirePackageStream(TextPackageDefinition Entry) {

            var Uri = new UriBuilder(RepositoryRootUrl);
            Uri.AppendPath(Entry.FileName);

            var ret = await HttpClient.GetStreamAsync(Uri.ToString())
                .DefaultAwait()
                ;

            return ret;
        }
    }
}
