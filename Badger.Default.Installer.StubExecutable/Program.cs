using Badger.Common.Diagnostics;
using Badger.Default.Installer;
using Badger.Common.Management;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using static Badger.Common.Diagnostics.Log;
using Badger.Deployment;
using Badger.Default.Resources;
using Badger.Default.Configuration;
using Badger.Common;

namespace Badger.Default.Installer.StubExecutable {
    public static class Program {


        [STAThread]
        public static async Task Main(string[] args) {
            Badger.Common.Diagnostics.Logging.ApplySimpleConfiguation();
            var Logger = log4net.LogManager.GetLogger("Installer");

            var Config = Badger.Default.Resources.EmbeddedConfigurationResource<EmbeddedInstallerConfiguration>.Get();

            if (Config is { }) {

                var Options = new Mono.Options.OptionSet() {
                    {$@"{nameof(EmbeddedInstallerConfiguration.SpashScreen_Visible)}=", "Toggle spash screen visibility", x => {
                        Logger.Info($@"Setting SpashScreen Visibility to {x}:");
                        if(bool.TryParse(x, out var v)) {
                            Config.SpashScreen_Visible = v;
                            Logger.Info($@"  Success!");
                        } else {
                            Logger.Info($@"  Invalid Value: {x}");
                        }
                    }},
                };

                Options.Parse(args);

                if (Config.SpashScreen_Visible) {
                    SplashScreen.StartThread(Config);
                }


                if (Config.Install_Subfolder is { } Subfolder) {
                    var Root = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
                    var InstallLocation = System.IO.Path.Combine(Root, Subfolder);
                    var VersionFolder = System.IO.Path.Combine(InstallLocation, Badger.Deployment.VersionFolder.Name(Config.Product_Version));

                    var PreviousVersion = (
                        from x in Badger.Deployment.VersionFolder.List(InstallLocation)
                        orderby x.Version descending
                        select x.Version
                        ).FirstOrDefault();

                    var CurrentEnvironment = Location.FromFolder(VersionFolder);
                    var CurrentInstanceId = CurrentEnvironment.InstanceId;

                    await Install_Location_Kill(Config, InstallLocation);
                    await Install_Location_Clean(Config, InstallLocation);
                    await Install_Content_Deploy(Config, InstallLocation);

                    await Install_Package_Preserve(Config, CurrentEnvironment.LocalRepositoryFolder, Logger);

                    CurrentEnvironment.InstanceId = CurrentInstanceId;

                    await Uninstaller_Registry_Add(Config, InstallLocation);

                    Config.Install_Location_Scripts.Run(VersionFolder, PreviousVersion, Config.Product_Version);

                }

            } else {
                Logger.Error($@"This file does not seem to have the necessary resources embedded into it.");
            }

            //This does a hard stop.  Does not "properly" shutdown the Window thread but that's OK.
            System.Environment.Exit(0);
        }


        private static Task Uninstaller_Registry_Add(EmbeddedInstallerConfiguration Config, string InstallLocation) {

            var Uninstaller = System.IO.Path.Combine(InstallLocation, "Uninstall.exe");
            
            if (Config.Uninstaller_Create && System.IO.File.Exists(Uninstaller) && Config.Product_Code.IsNotBlank()) {
                var UninstallerConfig = new Windows.Installation.UninstallerRegistryConfiguration() { 
                    InstallDate = DateTime.Now,
                    
                    CanModify = Windows.Installation.CanModifyValue.False,
                    CanRepair = Windows.Installation.CanRepairValue.False,
                    InstallLocation = InstallLocation,
                    DisplayIcon = Uninstaller,
                    DisplayVersion = Config.Product_Version.ToString(),
                    DisplayName = Config.Product_Name,
                    Publisher= Config.Product_Publisher,
                    UninstallString = Uninstaller,
                    EstimatedSizeInKb = Badger.Common.IO.Directory.Size(InstallLocation) / 1024,
                };

                Badger.Windows.Installation.UninstallerRegistry.Default.AddOrUpdate(Config.Product_Code, UninstallerConfig);

            }

            return Task.CompletedTask;

        }

        private static Task Install_Location_Kill(EmbeddedInstallerConfiguration Config, string InstallLocation) {
            if (Config.Install_Location_Kill) {
                Badger.Common.Diagnostics.Processes.KillProcessesInPath(InstallLocation);
            }

            return Task.CompletedTask;
        }

        public static Task Install_Location_Clean(EmbeddedInstallerConfiguration Config, string InstallLocation) {

            if (Config.Install_Location_Clean) {
                Badger.Common.IO.Directory.Delete(InstallLocation);
            }

            return Task.CompletedTask;
        }

        public static Task Install_Content_Deploy(EmbeddedInstallerConfiguration Config, string InstallLocation) {
            if (PackageContentResource.Resource.GetStream() is { } Content) {
                var TempFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $@"{Config.Install_Subfolder}-{Guid.NewGuid()}.exe");
                var Writer = System.IO.File.OpenWrite(TempFile);
                Content.CopyTo(Writer);
                Writer.Close();
                var ParameterValues = new ExtractArchiveParameters() {
                    Dest_Folder = InstallLocation
                };

                Utility.Run(TempFile, Config.Install_Content_ParameterTemplate, ParameterValues);
            }

            return Task.CompletedTask;
        }

        public static Task Install_Package_Preserve(EmbeddedInstallerConfiguration Config, string LocalRepositoryFolder, ILog Logger) {
            var OriginalInstaller = System.Reflection.Assembly.GetEntryAssembly().Location;
            var DestFile = System.IO.Path.Combine(LocalRepositoryFolder, System.IO.Path.GetFileName(OriginalInstaller));

            Folder.Create(nameof(LocalRepositoryFolder), LocalRepositoryFolder, Logger);
            File.Copy(nameof(OriginalInstaller), OriginalInstaller, nameof(LocalRepositoryFolder), DestFile, Logger);
            
            return Task.CompletedTask;
        }

    }

}
