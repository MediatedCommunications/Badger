using Mono.Cecil;
using StringTokenFormatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Badger.Common;
using static Badger.Common.Diagnostics.Log;
using log4net;


using Badger.Default.Installer;
using Badger.Deployment.Servicing;
using Badger.Deployment;
using Badger.Default.Resources;
using Badger.Default.Configuration;

namespace Badger.Default.Packager {
    public static class PackageConfigurationExtensions {

        public static void InvokeConfiguration(this PackagerConfiguration This) {
            var Logger = log4net.LogManager.GetLogger("Packager");

            var WorkingFolder_Path = This.WorkingFolder.ResolvePath(This.Product);

            var Archive_Root = System.IO.Path.Combine(WorkingFolder_Path, "Archive");
            

            var Version_Root = System.IO.Path.Combine(Archive_Root, Badger.Deployment.VersionFolder.Name(This.Product.Version));
            var Archive_Output = System.IO.Path.Combine(WorkingFolder_Path, "Archive.exe");

            var OutputParameters = new InstallerOutputParameters() {
                PackageName = This.Product.Name,
                Version = This.Product.Version,
            };

            var Output_Directory = This.Output.Path_NameTemplate.FormatToken(OutputParameters);
            var Installer_Output_Name = This.Output.Installer_NameTemplate.FormatToken(OutputParameters);
            var Installer_Output_FullPath = System.IO.Path.Combine(Output_Directory, Installer_Output_Name);

            var Releases_Output_Name = This.Output.Releases_NameTemplate.FormatToken(OutputParameters);
            var Releases_Output_FullPath = System.IO.Path.Combine(Output_Directory, Releases_Output_Name);

            var InstallerOutput = System.IO.Path.Combine(WorkingFolder_Path, Installer_Output_Name);
            var InstallerOutputSymbols = System.IO.Path.ChangeExtension(InstallerOutput, ".pdb");
            var Installer_Stub_Path = This.Installer.Stub.Source;
            var Installer_Stub_Symbols_Path = System.IO.Path.ChangeExtension(Installer_Stub_Path, ".pdb");


            var UninstallerOutput = System.IO.Path.Combine(Archive_Root, "Uninstall.exe");
            var UninstallerOutputSymbols = System.IO.Path.ChangeExtension(InstallerOutput, ".pdb");
            var Uninstaller_Stub_Path = This.Uninstaller.Stub.Source;
            var Uninstaller_Stub_Symbols_Path = System.IO.Path.ChangeExtension(Uninstaller_Stub_Path, ".pdb");

            var Package_Source = This.Installer.Content.Source;
            var Redirector_Stub_Path = This.Redirector.Stub.Source;

            


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
                    { nameof(Output_Directory), Output_Directory },
                };
                Logger.Info("Inputs resolved to the following locations (not all may be actually used):");
                foreach (var item in Locations.OrderBy(x => x.Key)) {
                    Logger.Debug($@"  {item.Key} =>");
                    Logger.Debug($@"    {item.Value}");
                }
                
            }

            


            if (This.WorkingFolder.Delete_OnStart()) {
                Folder.Delete(nameof(WorkingFolder_Path), WorkingFolder_Path, Logger);
            }


            Folder.Create(nameof(WorkingFolder_Path), WorkingFolder_Path, Logger);

            Folder.Create(nameof(Archive_Root), Archive_Root, Logger);




            //Copy all the files that we'll need to deploy into our "Version" folder.

            Folder.Create(nameof(Version_Root), Version_Root, Logger);

            Folder.Copy(nameof(Package_Source), Package_Source, nameof(Version_Root), Version_Root, nameof(This.Installer.Content.Include), ()=> This.Installer.Content.Include(), Logger);

            if (This.Redirector.Stub.Include()) {
                Logger.Info($@"Creating Redirector executables for each EXE in {nameof(Version_Root)}");
                
                var ActualFiles = System.IO.Directory.GetFiles(Version_Root, "*.exe").OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase).ToList();

                foreach (var ActualFile in ActualFiles) {
                    var FN = System.IO.Path.GetFileName(ActualFile);
                    var StubExecutable = System.IO.Path.Combine(Archive_Root, FN);

                    File.Copy(nameof(Redirector_Stub_Path), Redirector_Stub_Path, nameof(StubExecutable), StubExecutable, Logger);

                    Logger.Info($@"  Copying Metadata from {nameof(ActualFile)} => {nameof(StubExecutable)}");

                    This.ExternalTools.VersionString.Copy(ActualFile, StubExecutable);

                    Logger.Info($@"  Copying Icon from {nameof(ActualFile)} => {nameof(StubExecutable)}");


                    This.ExternalTools.Icon.Copy(ActualFile, StubExecutable);

                    This.Redirector.Sign(StubExecutable);

                }
            } else {
                Logger.Info($@"Creating redirector executables skipped.");
            }

            if (This.Uninstaller.Stub.Include()) {
                Logger.Info($@"Creating Uninstaller EXE");
                File.Copy("Uninstaller Stub", This.Uninstaller.Stub.Source, "Uninstaller", UninstallerOutput, Logger);

                if (This.Uninstaller.Icon.Include()) {
                    This.ExternalTools.Icon.Copy(This.Uninstaller.Icon.Source, UninstallerOutput);
                }



                try {
                    Logger.Info("Adding resources to Uninstaller...");
                    using (var AssemblyDef = Mono.Cecil.AssemblyDefinition.ReadAssembly(UninstallerOutput, new Mono.Cecil.ReaderParameters {
                        ReadingMode = Mono.Cecil.ReadingMode.Immediate,
                        InMemory = true
                    })) {

                        var UninstallerConfig = new EmbeddedUninstallerConfiguration() {
                            Install_Location_Clean = true,
                            Install_Location_Kill = true,
                            Install_Location_Scripts = { This.Uninstaller.Playbook.Scripts },
                            
                            Product_Name = This.Product.Name,
                            Product_Version = This.Product.Version,
                            Product_Code = This.Product.Code,
                            Install_Subfolder = This.Installer.Playbook.ExtractContentToSubfolder,
                            
                        };

                        if (System.IO.File.Exists(Uninstaller_Stub_Symbols_Path)) {
                            Logger.Info("Uninstaller symbols exist so they are being loaded");
                            var Symbols = new Mono.Cecil.Cil.DefaultSymbolReaderProvider();
                            var Reader = Symbols.GetSymbolReader(AssemblyDef.MainModule, Uninstaller_Stub_Symbols_Path);
                            AssemblyDef.MainModule.ReadSymbols(Reader);
                        }



                        var Resources = AssemblyDef.MainModule.Resources;
                        

                        Logger.Info("Adding Installer Playbook...");
                        Resources.AddFromObject(EmbeddedConfigurationResource<EmbeddedUninstallerConfiguration>.ResourceName, UninstallerConfig);

                        Logger.Info("Creating Uninstaller...");
                        AssemblyDef.Write(UninstallerOutput, new WriterParameters() {
                            WriteSymbols = true
                        });
                    }
                } catch (Exception ex) {
                    Logger.Error(ex);
                }


                This.Uninstaller.Sign(UninstallerOutput);

            }



            if (This.Archive.CreateUsing.Enabled()) {
                Logger.Info($@"Creating the self-extracting archive.");

                File.Delete(nameof(Archive_Output), Archive_Output, Logger);


                This.Archive.Create(Archive_Root, Archive_Output);

                This.Archive.Sign(Archive_Output);
            }

            if (This.Installer.Stub.Include()) {
                File.Copy(nameof(Installer_Stub_Path), Installer_Stub_Path, nameof(InstallerOutput), InstallerOutput, Logger);

                if(This.Installer.Icon.Include()) {
                    This.ExternalTools.Icon.Copy(This.Installer.Icon.Source, InstallerOutput);
                }

                try {
                    Logger.Info("Adding resources to Installer...");
                    using (var AssemblyDef = Mono.Cecil.AssemblyDefinition.ReadAssembly(InstallerOutput, new Mono.Cecil.ReaderParameters {
                        ReadingMode = Mono.Cecil.ReadingMode.Immediate,
                        InMemory = true
                    })) {

                        var InstallerConfig = new EmbeddedInstallerConfiguration() {
                            Product_Name = This.Product.Name,
                            Product_Publisher = This.Product.Publisher,
                            Product_Version = This.Product.Version,
                            Product_Code = This.Product.Code,

                            Install_Location_Kill = This.Installer.Playbook.CloseOldVersions.TrueWhenNull(),
                            Install_Location_Clean = This.Installer.Playbook.DeleteOldVersions.TrueWhenNull(),

                            Install_Subfolder = This.Installer.Playbook.ExtractContentToSubfolder,
                            Install_Content_ParameterTemplate = This.Installer.Playbook.ExtractContent.ArgumentTemplate,

                            Install_Location_Scripts = { This.Installer.Playbook.Scripts },
                            
                            Uninstaller_Create = This.Uninstaller.Stub.Include()
                        };

                        if (System.IO.File.Exists(Installer_Stub_Symbols_Path)) {
                            Logger.Info("Installer symbols exist so they are being loaded");
                            var Symbols = new Mono.Cecil.Cil.DefaultSymbolReaderProvider();
                            var Reader = Symbols.GetSymbolReader(AssemblyDef.MainModule, Installer_Stub_Symbols_Path);
                            AssemblyDef.MainModule.ReadSymbols(Reader);
                        }



                        var Resources = AssemblyDef.MainModule.Resources;

                        Logger.Info("Adding Archive Content...");
                        Resources.AddFromFile(Badger.Default.Resources.PackageContentResource.ResourceName, Archive_Output);

                        if (This.Installer.SplashScreen.Include()) {
                            Logger.Info("Adding Splash Screen Content...");
                            Resources.AddFromFile(Badger.Default.Resources.SpashScreenResource.ResourceName, This.Installer.SplashScreen.Source);
                            InstallerConfig.SpashScreen_Visible = true;
                        }



                        Logger.Info("Adding Installer Playbook...");
                        Resources.AddFromObject(EmbeddedConfigurationResource<EmbeddedInstallerConfiguration>.ResourceName, InstallerConfig);

                        Logger.Info("Creating Installer...");
                        AssemblyDef.Write(InstallerOutput, new WriterParameters() {
                            WriteSymbols = true
                        });
                    }
                } catch (Exception ex) {
                    Logger.Error(ex);
                }


                This.Installer.Sign(InstallerOutput);

                Logger.Info("Generating Releases file...");
                var ReleasesFileName = System.IO.Path.Combine(WorkingFolder_Path, LocationHelpers.ReleasesFileName);
                var ReleasesEntry = new TextPackageDefinition() {
                    FileName =  System.IO.Path.GetFileName(InstallerOutput),
                    FileSize = new System.IO.FileInfo(InstallerOutput).Length,
                    SHA1 = Badger.Common.Security.SHA1.FromFile(InstallerOutput)
                };
                System.IO.File.WriteAllText(ReleasesFileName, ReleasesEntry.ToString());


                Logger.Info("Copying Output files...");
                var FilesToCopy = new Dictionary<string, string> {
                    {nameof(InstallerOutput), InstallerOutput },
                    {nameof(InstallerOutputSymbols), InstallerOutputSymbols },
                    {nameof(ReleasesFileName), ReleasesFileName },
                };

                foreach (var item in FilesToCopy) {
                    
                    if (Badger.Common.IO.File.Exists(item.Value)) {
                        var NewLocation = System.IO.Path.Combine(Output_Directory, System.IO.Path.GetFileName(item.Value));

                        File.Copy(item.Key, item.Value, nameof(Output_Directory), NewLocation, Logger);

                    }
                    
                }

            }

            if (This.WorkingFolder.Delete_OnFinish()) {
                Folder.Delete(nameof(WorkingFolder_Path), WorkingFolder_Path, Logger);
            }

        }
    }
}
