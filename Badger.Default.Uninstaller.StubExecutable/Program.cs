using System;
using System.Linq;
using System.Collections.Generic;
using Badger.Common;
using Badger.Deployment;
using Badger.Default.Installer;
using Badger.Default.Resources;
using System.Threading.Tasks;
using Badger.Default.Configuration;

namespace Badger.Default.Uninstaller.StubExecutable {
    public static class Program {

        public static async Task Main(string[] args) {
            Badger.Common.Diagnostics.Logging.ApplySimpleConfiguation();

            var Logger = log4net.LogManager.GetLogger("Uninstaller");

            var Config = EmbeddedConfigurationResource<EmbeddedUninstallerConfiguration>.Get();

            if(Config is { }) {
                
                if(Config.Install_Subfolder is { } Subfolder) {
                    var Root = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
                    var InstallLocation = System.IO.Path.Combine(Root, Subfolder);

                    await Install_Location_Scripts_Run(Config, InstallLocation);
                    await Install_Location_Kill(Config, InstallLocation);
                    await Install_Location_Clean(Config, InstallLocation);
                    await Uninstaller_Registry_Remove(Config, InstallLocation);

                }

            }

        }

        private static Task Install_Location_Kill(EmbeddedUninstallerConfiguration Config, string InstallLocation) {
            if (Config.Install_Location_Kill) {
                Badger.Common.Diagnostics.Processes.KillProcessesInPath(InstallLocation);
            }

            return Task.CompletedTask;
        }

        public static Task Install_Location_Clean(EmbeddedUninstallerConfiguration Config, string InstallLocation) {

            if (Config.Install_Location_Clean) {
                Badger.Common.IO.Directory.Delete(InstallLocation);
            }

            return Task.CompletedTask;
        }

        private static Task Uninstaller_Registry_Remove(EmbeddedUninstallerConfiguration Config, string InstallLocation) {

            if (Config.Product_Code.IsNotBlank()) {
                Badger.Windows.Installation.UninstallerRegistry.Default.Remove(Config.Product_Code);
            }
            
            return Task.CompletedTask;

        }


        private static Task Install_Location_Scripts_Run(EmbeddedUninstallerConfiguration Config, string InstallLocation) {
            var BaseConfig = new ExternalToolConfiguration() {
                Path = InstallLocation
            };
            var NewScripts = (
                from x in Config.Install_Location_Scripts
                let v = ConfigurationMerger.Merge(x, BaseConfig)
                select v
                ).ToList();

            var Params = new UninstallScriptParameters() {

            };

            NewScripts.Run(Params);

            return Task.CompletedTask;
        }


    }
}
