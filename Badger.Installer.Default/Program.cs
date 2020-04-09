using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace Badger.Installer.Default {
    class Program {




        static void Main(string[] args) {
            Badger.Diagnostics.Logging.ApplySimpleConfiguation();

            var Options = new Installer.PackagerConfiguration() {
                Defaults = new DefaultsConfiguration {
                    SignUsing = new SigningConfiguration {
                        Certificate = @"C:\Users\TonyValenti\source\repos\MediatedCommunications\AlphaDrive\ASSEMBLIES\__CODESIGNINGCERTIFICATES\mediated_communications_inc.pfx",
                        ArgumentTemplate = $@"sign /f {{{nameof(SignAssemblyParameters.Certificate)}}} /tr http://timestamp.digicert.com /td SHA256 /fd SHA256 /v {{{nameof(SignAssemblyParameters.Assembly)}}}"
                    }
                },
                Product = new ProductConfiguration {
                    Name = "Faster Suite",
                    Version = new Version(3, 2, 1),
                },

                Installer = new InstallerConfiguration {
                    Content = new InstallerContentConfiguration() {
                        Include = true,
                        Source = $@"C:\Users\TonyValenti\source\repos\MediatedCommunications\AlphaDrive\__BUILD\Current",
                    },
                    Icon = new InstallerContentConfiguration() {
                        Include = true,
                        Source = $@"C:\Users\TonyValenti\source\repos\MediatedCommunications\AlphaDrive\__BUILD\Current\FasterSuite.Windows.x86.exe"
                    },
                    SplashScreen = new InstallerContentConfiguration() {
                        Include = true,
                        Source = @"C:\Users\TonyValenti\source\repos\MediatedCommunications\AlphaDrive\FasterSuite.Windows.Package\InstallerResources\LoadingGif-assets\LoadingGif.gif"
                    },
                    Playbook = new InstallerContentPlaybookConfiguration {
                        Scripts = new List<ExternalToolConfiguration> {
                            new ExternalToolConfiguration() {
                                Enabled = true,
                                Application = "FasterSuite.Windows.exe"
                            }
                        }
                    },
                },
            };

            Options = CoalesceExtensions.Coalesce(Options, ConfigDefaults.Configuration());
            

            if(!Options.Validate(out var Errors)) {
                var Logger = log4net.LogManager.GetLogger("ValidationErrors");

                foreach (var item in Errors) {
                    Logger.Error(item.Message);
                }
            } else {
                Options.Execute();
            }
          

            

        }
    }

}
