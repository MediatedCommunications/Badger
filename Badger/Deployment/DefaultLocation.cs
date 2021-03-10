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
                var MyVersionFolder = Badger.Deployment.VersionFolder.Find(Location);

                if (MyVersionFolder is { } V1) {
                    This = new DefaultLocation();

                    This.VersionFolder = V1;
                }
            }

            if (This is { }) {

                //Set the installation folder.
                {
                    This.InstallFolder = System.IO.Path.GetDirectoryName(This.VersionFolder.FullPath);
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
                    This.InstanceIdFolder = This.LocalRepositoryFolder;
                    This.InstanceIdFullPath = System.IO.Path.Combine(This.InstanceIdFolder, LocationHelpers.InstallIdFileName);
                }
            }

            return This;
        }

        public override Guid? InstanceId {
            get {
                var ret = default(Guid?);

                try {
                    if (System.IO.File.Exists(InstanceIdFullPath)) {
                        var Contents = System.IO.File.ReadAllText(InstanceIdFullPath);
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
                if (!System.IO.Directory.Exists(InstanceIdFolder)) {
                    System.IO.Directory.CreateDirectory(InstanceIdFolder);
                }

                if(value == null) {
                    if (System.IO.File.Exists(InstanceIdFullPath)) {
                        System.IO.File.Delete(InstanceIdFullPath);
                    }
                } else {
                    System.IO.File.WriteAllText(InstanceIdFullPath, value.ToString());
                }

            }
        }

    }

}
