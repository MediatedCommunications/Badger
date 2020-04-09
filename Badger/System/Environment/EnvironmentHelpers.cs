using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Badger {

    public class VersionFolder {
        public string FullPath { get; private set; }
        public Version Version { get; private set; }

        public const string VersionFolderPrefix = "app-";

        public static string Name(Version ForVersion) {
            var ret = $@"{VersionFolderPrefix}{ForVersion}";
            return ret;
        }

        public static Version IsVersionFolder(string FolderName) {
            var ret = default(Version);

            if (IsVersionFolder(FolderName, out var V1)) {
                ret = V1;
            }

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

        public static List<VersionFolder> List(string Folder) {
            var ret = new List<VersionFolder>();
            Actions.Try(() => {
                if (Directory.Exists(Folder)) {
                    ret = (
                        from x in Directory.GetDirectories(Folder)
                        let Version = IsVersionFolder(x)
                        where Version is { }
                        select new VersionFolder() {
                            Version = Version,
                            FullPath = x
                        }).ToList();
                }
            });


            return ret;
        }


    }

    public static class EnvironmentHelpers {

        
        public const string PackagesSubFolderName = "packages";
        public const string PackagesSubFolderPath = PackagesSubFolderName;

        public const string InstallIdFileName = ".betaId";
        public const string ReleasesFileName = "RELEASES.txt";

        public const string TempSubFolderName = "SquirrelTemp";
        public const string TempSubFolderPath = PackagesSubFolderPath + @"\" + TempSubFolderName;

    }
}
