using Mono.Cecil;
using StringTokenFormatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Badger.Collections;

namespace Badger.Installer.Default {
    public static class Packager {
        public static void Execute(this IConfiguration This) {
            var ArchiveRoot = System.IO.Path.Combine(This.WorkingFolder_Path, "Archive");
            var VersionRoot = System.IO.Path.Combine(ArchiveRoot, Badger.EnvironmentHelpers.VersionFolderName(This.Package_Version));
            var ArchiveOutput = System.IO.Path.Combine(This.WorkingFolder_Path, "Archive.exe");
            var InstallerOutputName = This.Installer_Output_Name_ParameterTemplate.FormatToken(new InstallerOutputParameters() {
                Name = This.Package_Name,
                Version = This.Package_Version
            });
            var InstallerOutput = System.IO.Path.Combine(This.WorkingFolder_Path, InstallerOutputName);
            var InstallerOutputSymbols = System.IO.Path.ChangeExtension(InstallerOutput, ".pdb");

            var InstallerStubSymbols = System.IO.Path.ChangeExtension(This.Installer_Stub_Path, ".pdb");

            if (This.WorkingFolder_Clean == true) {
                Badger.IO.Directory.Delete(This.WorkingFolder_Path);
            }

            Badger.IO.Directory.Create(This.WorkingFolder_Path);

            Badger.IO.Directory.Create(ArchiveRoot);

            //Copy all the files that we'll need to deploy into our "Version" folder.

            Badger.IO.Directory.Create(VersionRoot);

            if (This.Package_Folder_Copy == true) {
                Badger.IO.Directory.Copy(This.Package_Folder, VersionRoot);
            }

            if (This.Redirector_Stub_Create == true) {
                //Create Redirector executables for each EXE in the version root.
                var ActualFiles = System.IO.Directory.GetFiles(VersionRoot, "*.exe").OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase).ToList();
                foreach (var ActualFile in ActualFiles) {
                    var FN = System.IO.Path.GetFileName(ActualFile);
                    var StubExecutable = System.IO.Path.Combine(ArchiveRoot, FN);

                    Badger.IO.File.Replace(This.Redirector_Stub_Path, StubExecutable);

                    Utilities.RCEdit.VersionStrings.Copy(ActualFile, StubExecutable);
                    Utilities.RCEdit.Icons.Copy(ActualFile, StubExecutable);

                    if (This.Redirector_Sign == true) {
                        Utilities.SignTool.Sign(This.Redirector_Sign_Command, This.Redirector_Sign_ParameterTemplate, StubExecutable, This.Redirector_Sign_Certificate);
                    }
                }
            }


            if (This.Archive_Create == true) {

                if (System.IO.File.Exists(ArchiveOutput)) {
                    System.IO.File.Delete(ArchiveOutput);
                }

                //Create our SevenZip Installer
                Utilities.SevenZip.CreateSelfExtractingArchive(This.Archive_Create_Command, This.Archive_Create_ParameterTemplate, ArchiveRoot, ArchiveOutput);

                if (This.Archive_Sign == true) {
                    Utilities.SignTool.Sign(This.Archive_Sign_Command, This.Archive_Sign_ParameterTemplate, ArchiveOutput, This.Archive_Sign_Certificate);
                }
            }

            if (This.Installer_Create == true) {

                Badger.IO.File.Replace(This.Installer_Stub_Path, InstallerOutput);

                var InstallerIcon = This.Installer_Stub_Icon;
                if (InstallerIcon == null) {
                    var FirstFile = System.IO.Directory.GetFiles(VersionRoot, "*.exe").OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase).FirstOrDefault();

                    if (FirstFile is { }) {
                        InstallerIcon = Utilities.RCEdit.Icons.Get(FirstFile);
                    }

                }

                if (!InstallerIcon.IsNullOrEmpty()) {
                    Utilities.RCEdit.Icons.Set(InstallerOutput, InstallerIcon);
                }

                try {

                    using (var AssemblyDef = Mono.Cecil.AssemblyDefinition.ReadAssembly(InstallerOutput, new Mono.Cecil.ReaderParameters {
                        ReadingMode = Mono.Cecil.ReadingMode.Immediate,
                        InMemory = true
                    })) {

                        if (System.IO.File.Exists(InstallerStubSymbols)) {
                            var Symbols = new Mono.Cecil.Cil.DefaultSymbolReaderProvider();
                            var Reader = Symbols.GetSymbolReader(AssemblyDef.MainModule, InstallerStubSymbols);
                            AssemblyDef.MainModule.ReadSymbols(Reader);
                        }



                        var Resources = AssemblyDef.MainModule.Resources;

                        Resources.AddFromFile(Badger.Installer.PackageContentResource.ResourceName, ArchiveOutput);
                        Resources.AddFromFile(Badger.Installer.SpashScreenResource.ResourceName, This.Installer_SplashScreen_Image);


                        var InstallerConfig = new Badger.Installer.Configuration() {
                            Package_Name = This.Package_Name,
                            Package_Version = This.Package_Version,
                            Install_Location_Clean = This.Installer_Install_Location_Clean == true,
                            Install_Location_Kill = This.Installer_Install_Location_Kill == true,

                            Install_Subfolder = This.Installer_Install_Subfolder,
                            Install_Content_ParameterTemplate = This.Installer_Install_Content_ParameterTemplate,

                            Install_Location_Scripts = { This.Installer_Install_Location_Scripts }
                        };

                        Resources.AddFromObject(Badger.Installer.ConfigurationResource.ResourceName, InstallerConfig);

                        AssemblyDef.Write(InstallerOutput, new WriterParameters() {
                            WriteSymbols = true
                        });
                    }
                } catch (Exception ex) {

                }

                if (This.Installer_Sign == true) {
                    Utilities.SignTool.Sign(This.Installer_Sign_Command, This.Installer_Sign_ParameterTemplate, InstallerOutput, This.Installer_Sign_Certificate);
                }

                var ReleasesFileName = System.IO.Path.Combine(This.WorkingFolder_Path, EnvironmentHelpers.ReleasesFileName);
                var ReleasesEntry = new Badger.PackageEntry() {
                    FileName =  System.IO.Path.GetFileName(InstallerOutput),
                    FileSize = new System.IO.FileInfo(InstallerOutput).Length,
                    SHA1 = Badger.Security.SHA1.FromFile(InstallerOutput)
                };
                System.IO.File.WriteAllText(ReleasesFileName, ReleasesEntry.ToString());



                var FilesToCopy = new[] {
                    InstallerOutput,
                    InstallerOutputSymbols,
                    ReleasesFileName
                };

                foreach (var item in FilesToCopy) {
                    
                    if (Badger.IO.File.Exists(item)) {
                        var NewLocation = System.IO.Path.Combine(This.Installer_Output_Directory, System.IO.Path.GetFileName(item));
                        Badger.IO.File.Replace(item, NewLocation);
                    }
                    
                }

            }

        }
    }
}
