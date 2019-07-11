using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Installer._7Zip {
    public static class Packager {
        public static void Execute(this IConfiguration This) {
            if(This.WorkingFolder_Clean == true) {
                Directory2.Delete(This.WorkingFolder_Path);
            }

            Directory2.Create(This.WorkingFolder_Path);

            var ArchiveRoot = System.IO.Path.Combine(This.WorkingFolder_Path, "Archive");
            Directory2.Create(ArchiveRoot);

            //Copy all the files that we'll need to deploy into our "Version" folder.
            var VersionRoot = System.IO.Path.Combine(ArchiveRoot, Badger.EnvironmentHelpers.VersionFolderName(This.Package_Version));
            Directory2.Create(VersionRoot);

            Directory2.Copy(This.Package_Folder, VersionRoot);

            //Create Stub executables for each EXE in the version root.
            var StubsToCreate = System.IO.Directory.GetFiles(VersionRoot, "*.exe");
            foreach (var item in StubsToCreate) {
                var FN = System.IO.Path.GetFileName(item);
                var Dest = System.IO.Path.Combine(ArchiveRoot, FN);

                System.IO.File.Copy(This.Stub_Path, FN, true);
                FileMetaData.Copy(This.Stub_Path, FN);



            }



            var ArchiveOutput = System.IO.Path.Combine(This.WorkingFolder_Path, "Archive.exe");

            Process2.Start(This.Archive_Command, This.Archive_Parameters, new ArchiveCommandParameters() {
                Source_Folder = ArchiveRoot,
                Dest_File = ArchiveOutput,
            });


            if(This.Archive_Sign == true) {
                Process2.Start(This.Archive_Sign_Command, This.Archive_Sign_Parameters, new SignCommandParameters() {
                    File = ArchiveOutput
                });
            }




        }
    }
}
