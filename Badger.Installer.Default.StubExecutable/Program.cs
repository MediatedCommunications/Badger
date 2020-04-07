using Badger.Diagnostics;
using Badger.Management;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Badger.Installer.Default.StubExecutable {
    public static class Program {


        [STAThread]
        static async Task Main(string[] args) {
            var Config = Badger.Installer.ConfigurationResource.Get();

            SplashScreen.StartThread(Config);

            if (Config is { }) {

                if (Config.Install_Subfolder is { } Subfolder) {
                    var Root = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
                    var InstallLocation = System.IO.Path.Combine(Root, Subfolder);
                    var VersionFolder = System.IO.Path.Combine(InstallLocation, EnvironmentHelpers.VersionFolderName(Config.Package_Version));

                    var CurrentEnvironment = Badger.Environment.FromFolder(VersionFolder);
                    var CurrentInstanceId = CurrentEnvironment.InstanceId;

                    await Install_Location_Kill(Config, InstallLocation);
                    await Install_Location_Clean(Config, InstallLocation);
                    await Install_Content_Deploy(Config, InstallLocation);

                    await Install_Package_Preserve(Config, CurrentEnvironment.LocalRepositoryFolder);

                    CurrentEnvironment.InstanceId = CurrentInstanceId;

                    Config.Install_Location_Scripts.Run(VersionFolder);

                }

            }

            //This does a hard stop.  Does not "properly" shutdown the Window thread but that's OK.
            System.Environment.Exit(0);
        }


        private static async Task Install_Location_Kill(Configuration Config, string InstallLocation) {
            if (Config.Install_Location_Kill) {
                static List<WmiProcess> AppsToKill(string InstallLocation) {
                    var AllApps = Badger.Management.WmiProcess.Enumerate();
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
                Badger.IO.Directory.Delete(InstallLocation);
            }

            return Task.CompletedTask;
        }

        public static Task Install_Content_Deploy(Configuration Config, string InstallLocation) {
            if (PackageContentResource.Resource.GetStream() is { } Content) {
                var TempFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $@"{Config.Install_Subfolder}-{Guid.NewGuid()}.exe");
                var Writer = System.IO.File.OpenWrite(TempFile);
                Content.CopyTo(Writer);
                Writer.Close();
                var ParameterValues = new ExtractParameters() {
                    Location = InstallLocation
                };

                Utility.Run(TempFile, Config.Install_Content_ParameterTemplate, ParameterValues);
            }

            return Task.CompletedTask;
        }

        public static Task Install_Package_Preserve(Configuration Config, string LocalRepositoryFolder) {
            var SourceFile = System.Reflection.Assembly.GetEntryAssembly().Location;
            var DestFile = System.IO.Path.Combine(LocalRepositoryFolder, System.IO.Path.GetFileName(SourceFile));
            Badger.IO.Directory.Create(LocalRepositoryFolder);
            Badger.IO.File.Replace(SourceFile, DestFile);

            return Task.CompletedTask;
        }

    }

}
