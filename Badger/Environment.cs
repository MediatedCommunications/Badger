using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger {
    
    public abstract class Environment {
        public static Environment Default { get; private set; } = new DefaultEnvironment();


        /// <summary>
        /// The version folder would be something like:
        /// C:\Users\TonyValenti\AppData\Local\Clio\app-19.4.25\
        /// </summary>
        public string VersionFolder { get; protected set; }
        /// <summary>
        /// The version would be something like: 19.4.25
        /// </summary>
        public Version Version { get; protected set; }

        /// <summary>
        /// Something like:C:\Users\TonyValenti\AppData\Local\Clio\
        /// </summary>
        public string InstallFolder { get; protected set; }

        /// Something like:C:\Users\TonyValenti\AppData\Local\Clio\packages
        public string LocalRepositoryFolder { get; protected set; }

        public string InstallIdFolder { get; protected set; }
        public string InstallIdFullPath { get; protected set; }
        public abstract Guid? InstallId { get; set; }

        public string TempFolder { get; protected set; }
    }


    public class DefaultEnvironment : Environment  {

        private System.Reflection.Assembly EntryAssembly() {
            var ret = System.Reflection.Assembly.GetEntryAssembly();

            ret = ret ?? new System.Diagnostics.StackTrace().GetFrames().Last().GetMethod().Module.Assembly;

            return ret;
        }

        public DefaultEnvironment() {
            //Set the version folder and the version number.
            {
                var ret = System.IO.Path.GetDirectoryName(EntryAssembly().Location);
                var pret = ret;

                while (string.IsNullOrEmpty(pret)) {
                    try {
                        var CurrentFolder = System.IO.Path.GetFileName(pret);
                        if (Badger.EnvironmentHelpers.IsVersionFolder(CurrentFolder, out var Version)) {
                            ret = pret;
                            this.Version = Version;
                            break;
                        } else {
                            pret = System.IO.Path.GetDirectoryName(pret);
                        }
                    } catch (Exception ex) {
                        ex.Ignore();
                        break;
                    }
                }

                this.VersionFolder = ret;
            }

            //Set the installation folder.
            {
                this.InstallFolder = System.IO.Path.GetDirectoryName(VersionFolder);
            }

            {
                this.LocalRepositoryFolder = System.IO.Path.Combine(InstallFolder, Badger.EnvironmentHelpers.PackagesSubFolderPath);
            }

            {
                this.TempFolder = System.IO.Path.Combine(InstallFolder, Badger.EnvironmentHelpers.TempSubFolderPath);

                try {
                    if (System.IO.Directory2.EnsureWritable(TempFolder)) {
                        TempFolder = System.IO.Path.GetTempPath();
                    }
                } catch(Exception ex) {
                    ex.Ignore();
                }

            }

            {
                this.InstallIdFolder = LocalRepositoryFolder;
                this.InstallIdFullPath = System.IO.Path.Combine(InstallIdFolder, Badger.EnvironmentHelpers.InstallIdFileName);
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
