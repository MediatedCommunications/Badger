using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Badger.Common;

namespace Badger.Deployment {
    public class VersionFolder {
        public string FullPath { get; private set; }
        public Version Version { get; private set; }

        public VersionFolder(string FullPath, Version Version) {
            this.FullPath = FullPath;
            this.Version = Version;
        }


        public const string VersionFolderPrefix = "app-";

        public static string Name(Version ForVersion) {
            var ret = $@"{VersionFolderPrefix}{ForVersion}";
            return ret;
        }

        public static VersionFolder Parse(string FolderPath) {
            var ret = default(VersionFolder);

            var FolderName = System.IO.Path.GetFileName(FolderPath);

            if (FolderName.StartsWith(VersionFolderPrefix, StringComparison.InvariantCultureIgnoreCase)) {

                var PotentialVersion = FolderName.Substring(VersionFolderPrefix.Length);
                if(Version.TryParse(PotentialVersion, out var V1)) {
                    ret = new VersionFolder(FolderPath, V1);
                }
            }

            return ret;
        }

        public static VersionFolder Find(string FolderPath) {
            var ret = default(VersionFolder);

            var PathToCheck = FolderPath;

            while (PathToCheck.IsNotNullOrEmpty() && ret == default) {
                try {
                    if(VersionFolder.Parse(PathToCheck) is { } V1) {
                        ret = V1;
                    } else {
                        PathToCheck = System.IO.Path.GetDirectoryName(PathToCheck);
                    }
                } catch (Exception ex) {
                    ex.Ignore();
                    break;
                }
            }

            return ret;
        }



        

        public static List<VersionFolder> List(string Folder) {
            var ret = new List<VersionFolder>();
            Actions.Try(() => {
                if (Directory.Exists(Folder)) {
                    ret = (
                        from x in Directory.GetDirectories(Folder)
                        let Version = VersionFolder.Parse(x)
                        where Version is { }
                        select Version
                        ).ToList();
                }
            });


            return ret;
        }


    }
}
