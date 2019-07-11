using System;
using System.Collections.Generic;
using System.Text;

namespace Badger {
    public static class EnvironmentHelpers {

        public const string VersionFolderPrefix = "app-";
        public const string PackagesSubFolderName = "packages";
        public const string PackagesSubFolderPath = PackagesSubFolderName;

        public const string InstallIdFileName = ".betaId";
        public const string ReleasesFileName = "RELEASES.txt";

        public const string TempSubFolderName = "SquirrelTemp";
        public const string TempSubFolderPath = PackagesSubFolderPath + @"\" + TempSubFolderName;

        public static string VersionFolderName(Version ForVersion) {
            var ret = $@"{VersionFolderPrefix}{ForVersion}";
            return ret;
        }

        public static bool IsVersionFolder(string FolderName, out Version Version) {
            var ret = false;

            Version = default(Version);

            if (FolderName.StartsWith(VersionFolderPrefix, StringComparison.InvariantCultureIgnoreCase)) {

                var PotentialVersion = FolderName.Substring(VersionFolderPrefix.Length);
                ret = Version.TryParse(PotentialVersion, out Version);
            }


            return ret;
        }

    }
}
