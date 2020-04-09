using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger {
    
    public abstract class Environment {
        public static Environment Default { get; private set; } = DefaultEnvironment.FromEntryAssembly();
        public static Environment FromFolder(string Location) => DefaultEnvironment.FromFolder(Location);


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
        public abstract Guid? InstanceId { get; set; }

        public string TempFolder { get; protected set; }
    }


    public class DefaultEnvironment : Environment  {


        public static DefaultEnvironment FromEntryAssembly() {
            return FromFolder(Badger.Diagnostics.Application.EntryAssembly.Location);
        }


        public static new DefaultEnvironment FromFolder(string Location) {
            var This = default(DefaultEnvironment);

            //Set the version folder and the version number.
            {
                var Version = default(Version);
                var VersionFolder = default(string);

                var Start = Location;

                while (!string.IsNullOrEmpty(Start)) {
                    try {
                        var CurrentFolder = System.IO.Path.GetFileName(Start);
                        if (Badger.VersionFolder.IsVersionFolder(CurrentFolder, out var V1)) {
                            VersionFolder = Start;
                            Version = V1;
                            break;
                        } else {
                            Start = System.IO.Path.GetDirectoryName(Start);
                        }
                    } catch (Exception ex) {
                        ex.Ignore();
                        break;
                    }
                }
                if (Version is { }) {
                    This = new DefaultEnvironment();

                    This.Version = Version;
                    This.VersionFolder = VersionFolder;
                }
            }

            if (This is { }) {

                //Set the installation folder.
                {
                    This.InstallFolder = System.IO.Path.GetDirectoryName(This.VersionFolder);
                }

                {
                    This.LocalRepositoryFolder = System.IO.Path.Combine(This.InstallFolder, Badger.EnvironmentHelpers.PackagesSubFolderPath);
                }

                {
                    This.TempFolder = System.IO.Path.Combine(This.InstallFolder, Badger.EnvironmentHelpers.TempSubFolderPath);

                    try {
                        if (Badger.IO.Directory.IsWritable(This.TempFolder)) {
                            This.TempFolder = System.IO.Path.GetTempPath();
                        }
                    } catch (Exception ex) {
                        ex.Ignore();
                    }

                }

                {
                    This.InstallIdFolder = This.LocalRepositoryFolder;
                    This.InstallIdFullPath = System.IO.Path.Combine(This.InstallIdFolder, Badger.EnvironmentHelpers.InstallIdFileName);
                }
            }

            return This;
        }

        public override Guid? InstanceId {
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
