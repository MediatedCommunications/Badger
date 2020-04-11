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

namespace Badger.Default.Installer.StubExecutable {
    public static class Program {


        [STAThread]
        static async Task Main(string[] args) {
            Badger.Common.Diagnostics.Logging.ApplySimpleConfiguation();

            var Logger = log4net.LogManager.GetLogger("Installer");

            var Config = Badger.Default.Installer.ConfigurationResource.Get();

            SplashScreen.StartThread(Config);

            if (Config is { }) {

                if (Config.Install_Subfolder is { } Subfolder) {
                    var Root = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
                    var InstallLocation = System.IO.Path.Combine(Root, Subfolder);
                    var VersionFolder = System.IO.Path.Combine(InstallLocation, Badger.Deployment.VersionFolder.Name(Config.Package_Version));

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

                    Config.Install_Location_Scripts.Run(VersionFolder, PreviousVersion, Config.Package_Version);

                }

            }

            //This does a hard stop.  Does not "properly" shutdown the Window thread but that's OK.
            System.Environment.Exit(0);
        }


        private static async Task Install_Location_Kill(Configuration Config, string InstallLocation) {
            if (Config.Install_Location_Kill) {
                static List<WmiProcess> AppsToKill(string InstallLocation) {
                    var AllApps = Badger.Common.Management.WmiProcess.Enumerate();
                    var tret = (
                        from x in AllApps
                        where !string.IsNullOrWhiteSpace(x.ExecutablePath)
                        where x.ExecutablePath.ToLower().StartsWith(InstallLocation.ToLower())
                        select x
                        ).ToList();

                    return tret;
                }

                var ToKill = AppsToKill(InstallLocation);

                foreach (var item in ToKill) {
                    try {
                        var Proc = System.Diagnostics.Process.GetProcessById(item.Handle);
                        Proc.CloseMainWindow();
                    } catch {

                    }
                }

                for (int i = 0; i < 10 && ToKill.Count > 0; i++) {
                    await Task.Delay(1_000);
                    ToKill = AppsToKill(InstallLocation);
                }


                for (int i = 0; i < 10 && ToKill.Count > 0; i++) {
                    foreach (var item in ToKill) {
                        try {
                            var Proc = System.Diagnostics.Process.GetProcessById(item.Handle);
                            Proc.Kill();
                        } catch {

                        }
                    }

                    ToKill = AppsToKill(InstallLocation);
                    if(ToKill.Count > 0) {
                        await Task.Delay(1_000);
                    }
                }

                
            }
        }

        public static Task Install_Location_Clean(Configuration Config, string InstallLocation) {

            if (Config.Install_Location_Clean) {
                Badger.Common.IO.Directory.Delete(InstallLocation);
            }

            return Task.CompletedTask;
        }

        public static Task Install_Content_Deploy(Configuration Config, string InstallLocation) {
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

        public static Task Install_Package_Preserve(Configuration Config, string LocalRepositoryFolder, ILog Logger) {
            var OriginalInstaller = System.Reflection.Assembly.GetEntryAssembly().Location;
            var DestFile = System.IO.Path.Combine(LocalRepositoryFolder, System.IO.Path.GetFileName(OriginalInstaller));

            Folder.Create(nameof(LocalRepositoryFolder), LocalRepositoryFolder, Logger);
            File.Copy(nameof(OriginalInstaller), OriginalInstaller, nameof(LocalRepositoryFolder), DestFile, Logger);
            
            return Task.CompletedTask;
        }

    }

}
