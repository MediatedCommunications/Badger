using Mono.Cecil;
using StringTokenFormatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Badger.Collections;
using log4net;

using static Badger.Diagnostics.Log;

namespace Badger.Installer.Default {
    public static class Packager {

       

        public static void Execute(this PackagerConfiguration This) {
            var Logger = log4net.LogManager.GetLogger("Packager");

            var WorkingFolder_Path = This.WorkingFolder.ResolvePath(This.Product);

            var Archive_Root = System.IO.Path.Combine(WorkingFolder_Path, "Archive");
            var Version_Root = System.IO.Path.Combine(Archive_Root, Badger.VersionFolder.Name(This.Product.Version));
            var Archive_Output = System.IO.Path.Combine(WorkingFolder_Path, "Archive.exe");
            var Installer_Output_Name = This.Installer_Output_Name_ParameterTemplate.FormatToken(new InstallerOutputParameters() {
                PackageName = This.Product.Name,
                Version = This.Product.Version,
            });
            var InstallerOutput = System.IO.Path.Combine(WorkingFolder_Path, Installer_Output_Name);
            var InstallerOutputSymbols = System.IO.Path.ChangeExtension(InstallerOutput, ".pdb");
            var Installer_Stub_Path = This.Installer.Stub.Source;
            var Installer_Stub_Symbols_Path = System.IO.Path.ChangeExtension(Installer_Stub_Path, ".pdb");

            var Package_Source = This.Installer.Content.Source;
            var Redirector_Stub_Path = This.Redirector.Stub.Source;

            var Installer_Output_Directory = This.Installer_Output_Directory;


            {
                var Locations = new Dictionary<string, string>() {
                    { nameof(Archive_Root), Archive_Root },
                    { nameof(Version_Root), Version_Root },
                    { nameof(Archive_Output), Archive_Output },
                    { nameof(Installer_Output_Name), Installer_Output_Name },
                    { nameof(InstallerOutput), InstallerOutput },
                    { nameof(InstallerOutputSymbols), InstallerOutputSymbols },
                    { nameof(Installer_Stub_Symbols_Path), Installer_Stub_Symbols_Path },
                    { nameof(Package_Source), Package_Source },
                    { nameof(Redirector_Stub_Path), Redirector_Stub_Path },
                    { nameof(Installer_Output_Directory), Installer_Output_Directory },
                };
                Logger.Info("Inputs resolved to the following locations (not all may be actually used):");
                foreach (var item in Locations.OrderBy(x => x.Key)) {
                    Logger.Debug($@"  {item.Key} =>");
                    Logger.Debug($@"    {item.Value}");
                }
                
            }

            


            if (This.WorkingFolder.Delete_OnStart == true) {
                Folder.Delete(nameof(WorkingFolder_Path), WorkingFolder_Path, Logger);
            }


            Folder.Create(nameof(WorkingFolder_Path), WorkingFolder_Path, Logger);

            Folder.Create(nameof(Archive_Root), Archive_Root, Logger);




            //Copy all the files that we'll need to deploy into our "Version" folder.

            Folder.Create(nameof(Version_Root), Version_Root, Logger);

            Folder.Copy(nameof(Package_Source), Package_Source, nameof(Version_Root), Version_Root, nameof(This.Installer.Content.Include), ()=> This.Installer.Content.Include, Logger);

            if (This.Redirector.Stub.Include == true) {
                Logger.Info($@"Creating Redirector executables for each EXE in {nameof(Version_Root)}");
                
                var ActualFiles = System.IO.Directory.GetFiles(Version_Root, "*.exe").OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase).ToList();

                foreach (var ActualFile in ActualFiles) {
                    var FN = System.IO.Path.GetFileName(ActualFile);
                    var StubExecutable = System.IO.Path.Combine(Archive_Root, FN);

                    File.Copy(nameof(Redirector_Stub_Path), Redirector_Stub_Path, nameof(StubExecutable), StubExecutable, Logger);

                    Logger.Info($@"  Copying Metadata from {nameof(ActualFile)} => {nameof(StubExecutable)}");

                    This.ExternalTools.VersionString.Copy(ActualFile, Utilities.RCEdit.VersionStrings.Default, StubExecutable);

                    Logger.Info($@"  Copying Icon from {nameof(ActualFile)} => {nameof(StubExecutable)}");


                    This.ExternalTools.Icon.Copy(ActualFile, StubExecutable);

                    This.Redirector.SignUsing.Sign(StubExecutable);

                }
            } else {
                Logger.Info($@"Creating redirector executables skipped.");
            }


            if (This.Archive.CreateUsing.Enabled == true) {
                Logger.Info($@"Creating the self-extracting archive.");

                File.Delete(nameof(Archive_Output), Archive_Output, Logger);


                This.Archive.CreateUsing.CreateArchive(Archive_Root, Archive_Output);

                This.Archive.SignUsing.Sign(Archive_Output);
            }

            if (This.Installer.Stub.Include == true) {
                File.Copy(nameof(Installer_Stub_Path), Installer_Stub_Path, nameof(InstallerOutput), InstallerOutput, Logger);

                if(This.Installer.Icon.Include == true) {
                    This.ExternalTools.Icon.Copy(This.Installer.Icon.Source, InstallerOutput);
                }

                try {
                    Logger.Info("Adding resources to Installer...");
                    using (var AssemblyDef = Mono.Cecil.AssemblyDefinition.ReadAssembly(InstallerOutput, new Mono.Cecil.ReaderParameters {
                        ReadingMode = Mono.Cecil.ReadingMode.Immediate,
                        InMemory = true
                    })) {

                        if (System.IO.File.Exists(Installer_Stub_Symbols_Path)) {
                            Logger.Info("Installer symbols exist so they are being loaded");
                            var Symbols = new Mono.Cecil.Cil.DefaultSymbolReaderProvider();
                            var Reader = Symbols.GetSymbolReader(AssemblyDef.MainModule, Installer_Stub_Symbols_Path);
                            AssemblyDef.MainModule.ReadSymbols(Reader);
                        }



                        var Resources = AssemblyDef.MainModule.Resources;

                        Logger.Info("Adding Archive Content...");
                        Resources.AddFromFile(Badger.Installer.PackageContentResource.ResourceName, Archive_Output);

                        if (This.Installer.SplashScreen.Include == true) {
                            Logger.Info("Adding Splash Screen Content...");
                            Resources.AddFromFile(Badger.Installer.SpashScreenResource.ResourceName, This.Installer.SplashScreen.Source);
                        }



                        Logger.Info("Adding Installer Playbook...");
                        var InstallerConfig = new Badger.Installer.Configuration() {
                            Package_Name = This.Product.Name,
                            Package_Version = This.Product.Version,

                            Install_Location_Kill = This.Installer.Playbook.CloseOldVersions == true,
                            Install_Location_Clean = This.Installer.Playbook.DeleteOldVersions == true,

                            Install_Subfolder = This.Installer.Playbook.ExtractContentToSubfolder,
                            Install_Content_ParameterTemplate = This.Installer.Playbook.ExtractContent.ArgumentTemplate,

                            Install_Location_Scripts = { This.Installer.Playbook.Scripts }
                        };

                        Resources.AddFromObject(Badger.Installer.ConfigurationResource.ResourceName, InstallerConfig);

                        Logger.Info("Creating Installer...");
                        AssemblyDef.Write(InstallerOutput, new WriterParameters() {
                            WriteSymbols = true
                        });
                    }
                } catch (Exception ex) {
                    Logger.Error(ex);
                }


                This.Installer.SignUsing.Sign(InstallerOutput);

                Logger.Info("Generating Releases file...");
                var ReleasesFileName = System.IO.Path.Combine(WorkingFolder_Path, EnvironmentHelpers.ReleasesFileName);
                var ReleasesEntry = new Badger.PackageEntry() {
                    FileName =  System.IO.Path.GetFileName(InstallerOutput),
                    FileSize = new System.IO.FileInfo(InstallerOutput).Length,
                    SHA1 = Badger.Security.SHA1.FromFile(InstallerOutput)
                };
                System.IO.File.WriteAllText(ReleasesFileName, ReleasesEntry.ToString());


                Logger.Info("Copying Output files...");
                var FilesToCopy = new Dictionary<string, string> {
                    {nameof(InstallerOutput), InstallerOutput },
                    {nameof(InstallerOutputSymbols), InstallerOutputSymbols },
                    {nameof(ReleasesFileName), ReleasesFileName },
                };

                foreach (var item in FilesToCopy) {
                    
                    if (Badger.IO.File.Exists(item.Value)) {
                        var NewLocation = System.IO.Path.Combine(This.Installer_Output_Directory, System.IO.Path.GetFileName(item.Value));

                        File.Copy(item.Key, item.Value, nameof(Installer_Output_Directory), NewLocation, Logger);

                    }
                    
                }

            }

            if (This.WorkingFolder.Delete_OnFinish == true) {
                Folder.Delete(nameof(WorkingFolder_Path), WorkingFolder_Path, Logger);
            }

        }
    }
}
