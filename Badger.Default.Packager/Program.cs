using Badger.Default.Installer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Badger.Default.Packager {
    public static class Program {
        
        public static void Main(string[] args) {
            Badger.Common.Diagnostics.Logging.ApplySimpleConfiguation();

            var Configuration = Defaults.Configuration();

            var MyConfiguration = new PackagerConfiguration() {
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
                        ExtractContentToSubfolder = "Clio",
                        Scripts = new List<ExternalToolConfiguration> {
                            new ExternalToolConfiguration() {
                                Enabled = true,
                                Application = "FasterSuite.Windows.exe",
                                Visible = true,
                                Async = true,
                            }
                        }
                    },
                },

                Output = new OutputConfiguration() {
                    Path_NameTemplate = $@"C:\Users\TonyValenti\source\repos\MediatedCommunications\Badger\Badger.Installer.Default.StubExecutable\bin\net472"
                }

            };

            Configuration = Configurations.Coalesce(MyConfiguration, Configuration);




            var Parser = new Mono.Options.OptionSet() {
                {
                    "from-json=",
                    "Specify a json file to load packaging options from",
                    x => { 
                        try {
                            var Instance = EasyJsonSerializer.Instance.FromFile<PackagerConfiguration>(x);
                            Configuration = Configurations.Coalesce(Instance, Configuration);

                        } catch (Exception ex) {

                        }
                    
                    }
                },
                {
                    "create-package",
                    "create the package specified by all the options",
                    x => {
                        Configuration.Configuration();
                    }
                },
                {
                    "verbosity=",
                    "set the desired log verbosity",
                    x => {

                    }
                },
                {
                    "to-json=",
                    "Specify a json file to save packaging options to.",
                    x => { 
                        try {
                            EasyJsonSerializer.Instance.ToFile(Configuration, x);
                        } catch (Exception ex) {

                        }
                    }
                }
            };


            args = new[] {
                $@"-to-json=C:\Users\TonyValenti\Documents\test.json"
            };

            var F = Parser.Parse(args);


            
 

        }
    }

}
