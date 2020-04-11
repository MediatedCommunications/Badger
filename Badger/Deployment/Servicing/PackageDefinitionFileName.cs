using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Badger.Deployment.Servicing {
    public class PackageDefinitionFileName {
        public string FileName { get; private set; }

        public string DisplayName { get; private set; }
        public string Package { get; private set; }
        public string Edition { get; private set; }
        public string Extension { get; private set; }

        public string Version { get; private set; }
        public Version VersionNumber { get; private set; }
        

        private const string PACKAGE = "PACKAGE";
        private const string VERSION = "VERSION";
        private const string EDITION = "EDITION";
        private const string EXTENSION = "EXTENSION";

        private static readonly string RegexPattern = $@"^(?<{PACKAGE}>.*?)(-?)(?<{VERSION}>([0-9]+\.[0-9]+\.[0-9]+))(-?)(?<{EDITION}>.*?)(?<{EXTENSION}>\..*)$";
        private static readonly Regex Regex = new Regex(RegexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public PackageDefinitionFileName(string FileName) {
            this.FileName = FileName;
            this.DisplayName = FileName.Split(new[] { $@"\", @"//" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault().Trim();

            var Match = Regex.Match(DisplayName);
            if (Match.Success) {
                Package = Match.Groups[PACKAGE].Value;
                Edition = Match.Groups[EDITION].Value;
                Version = Match.Groups[VERSION].Value;
                Extension = Match.Groups[EDITION].Value;
            }

            if (!string.IsNullOrWhiteSpace(Version)) {
                if(System.Version.TryParse(Version, out var V)) {
                    VersionNumber = V;
                }
            }

        }

    }
}
