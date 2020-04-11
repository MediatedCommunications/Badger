using Badger.Common;
using System;

namespace Badger.Deployment {
    public class DefaultLocation : Location  {


        public static DefaultLocation FromEntryAssembly() {
            return FromFolder(Badger.Common.Diagnostics.Application.EntryAssembly.Location);
        }


        public static new DefaultLocation FromFolder(string Location) {
            var This = default(DefaultLocation);

            //Set the version folder and the version number.
            {
                var Version = default(Version);
                var VersionFolder = default(string);

                var Start = Location;

                while (!string.IsNullOrEmpty(Start)) {
                    try {
                        var CurrentFolder = System.IO.Path.GetFileName(Start);
                        if (Badger.Deployment.VersionFolder.IsVersionFolder(CurrentFolder, out var V1)) {
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
                    This = new DefaultLocation();

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
                    This.LocalRepositoryFolder = System.IO.Path.Combine(This.InstallFolder, LocationHelpers.PackagesSubFolderPath);
                }

                {
                    This.TempFolder = System.IO.Path.Combine(This.InstallFolder, LocationHelpers.TempSubFolderPath);

                    try {
                        if (Badger.Common.IO.Directory.IsWritable(This.TempFolder)) {
                            This.TempFolder = System.IO.Path.GetTempPath();
                        }
                    } catch (Exception ex) {
                        ex.Ignore();
                    }

                }

                {
                    This.InstallIdFolder = This.LocalRepositoryFolder;
                    This.InstallIdFullPath = System.IO.Path.Combine(This.InstallIdFolder, LocationHelpers.InstallIdFileName);
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
