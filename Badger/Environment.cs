using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger {
    
    public abstract class Environment {
        public static Environment Default { get; private set; } = new DefaultEnvironment();


        public const string VersionFolderPrefix = "app-";
        public const string PackagesSubfolder = "packages";
        public const string InstallIdFileName = ".betaId";
        public const string ReleasesFileName = "RELEASES";

        protected static bool IsVersionFolder(string FolderName, out Version Version) {
            var ret = false;

            Version = default(Version);

            if (FolderName.StartsWith(VersionFolderPrefix, StringComparison.InvariantCultureIgnoreCase)) {

                var PotentialVersion = FolderName.Substring(VersionFolderPrefix.Length);
                ret = Version.TryParse(PotentialVersion, out Version);
            }


            return ret;
        }

        /// <summary>
        /// The version folder would be something like:
        /// C:\Users\TonyValenti\AppData\Local\Clio\app-19.4.25\
        /// </summary>
        public string VersionFolder { get; protected set; }
        /// <summary>
        /// The verion would be something like: 19.4.25
        /// </summary>
        public Version Version { get; protected set; }

        /// <summary>
        /// Something like:C:\Users\TonyValenti\AppData\Local\Clio\
        /// </summary>
        public string InstallFolder { get; protected set; }

        /// Something like:C:\Users\TonyValenti\AppData\Local\Clio\packages
        public string PackagesFolder { get; protected set; }

        public string InstallIdFolder { get; protected set; }
        public string InstallIdFullPath { get; protected set; }
        public abstract Guid? InstallId { get; set; }

        public string ReleasesFolder { get; protected set; }
        public string ReleasesFullPath { get; protected set; }
    }


    class DefaultEnvironment : Environment  {
        public DefaultEnvironment() {
            //Set the version folder and the version number.
            {
                var ret = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                var pret = ret;

                while (string.IsNullOrEmpty(pret)) {
                    var CurrentFolder = System.IO.Path.GetFileName(pret);
                    if (IsVersionFolder(CurrentFolder, out var Version)) {
                        ret = pret;
                        this.Version = Version;
                        break;
                    } else {
                        pret = System.IO.Path.GetDirectoryName(pret);
                    }
                }

                this.VersionFolder = ret;
            }

            //Set the installation folder.
            {
                this.InstallFolder = System.IO.Path.GetDirectoryName(VersionFolder);
            }

            {
                this.PackagesFolder = System.IO.Path.Combine(InstallFolder, PackagesSubfolder);
            }

            {
                this.InstallIdFolder = PackagesFolder;
                this.InstallIdFullPath = System.IO.Path.Combine(InstallIdFolder, InstallIdFileName);
            }

            {
                this.ReleasesFolder = PackagesFolder;
                this.ReleasesFullPath = System.IO.Path.Combine(ReleasesFolder, ReleasesFileName);
            }

        }

        public override Guid? InstallId {
            get {
                var ret = default(Guid?);

                try {
                    if (System.IO.File.Exists(InstallIdFullPath)) {
                        var Contents = System.IO.File.ReadAllText(InstallIdFullPath);
                        if(Guid.TryParse(Contents, out var value)) {
                            ret = value;
                        }
                    }
                } catch (Exception ex) {
                    ex.Ignore();
                }

                return ret;
            }

            set {
                if (!System.IO.Directory.Exists(InstallIdFolder)) {
                    System.IO.Directory.CreateDirectory(InstallIdFolder);
                }

                if(value == null) {
                    if (System.IO.File.Exists(InstallIdFullPath)) {
                        System.IO.File.Delete(InstallIdFullPath);
                    }
                } else {
                    System.IO.File.WriteAllText(InstallIdFullPath, value.ToString());
                }

            }
        }

    }

}
