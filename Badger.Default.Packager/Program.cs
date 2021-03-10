using Badger.Common.Serialization;
using Badger.Default.Configuration;
using Badger.Default.Installer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Badger.Default.Packager {

    public static class Program {
        

        private static PackagerConfiguration MyConfig() {
            var ret = new PackagerConfiguration() {
                Defaults = new DefaultsConfiguration {
                    SignUsing = new SigningConfiguration {
                        Certificate = @"C:\Users\TonyValenti\source\repos\MediatedCommunications\AlphaDrive\ASSEMBLIES\__CODESIGNINGCERTIFICATES\mediated_communications_inc.pfx",
                        ArgumentTemplate = $@"sign /f {{{nameof(SignAssemblyParameters.Certificate)}}} /tr http://timestamp.digicert.com /td SHA256 /fd SHA256 /v {{{nameof(SignAssemblyParameters.Assembly)}}}"
                    }
                },
                Product = new ProductConfiguration {
                    Name = "Faster Suite",
                    Version = new Version(3, 2, 1),
                    Publisher = "Mediated Communications",
                    Code = "Clio",
                },

                Installer = new InstallerConfiguration {
                    Content = new InstallerContentConfiguration() {
                        Source = $@"C:\Users\TonyValenti\source\repos\MediatedCommunications\AlphaDrive\__BUILD\Current",
                    },
                    Icon = new InstallerContentConfiguration() {
                        Source = $@"C:\Users\TonyValenti\source\repos\MediatedCommunications\AlphaDrive\__BUILD\Current\FasterSuite.Windows.x32.exe"
                    },
                    SplashScreen = new InstallerContentConfiguration() {
                        Source = @"C:\Users\TonyValenti\source\repos\MediatedCommunications\AlphaDrive\FasterSuite.Windows.Package\InstallerResources\LoadingGif-assets\LoadingGif.gif"
                    },
                    Playbook = new InstallerContentPlaybookConfiguration {
                        ExtractContentToSubfolder = "Clio",
                        Scripts = new List<ExternalToolConfiguration> {
                            new ExternalToolConfiguration() {
                                Application = "FasterSuite.Windows.exe",
                                Visible = true,
                                Async = true,
                            }
                        }
                    },
                },

                Output = new OutputConfiguration() {
                    Path_NameTemplate = $@"C:\Users\TonyValenti\source\repos\MediatedCommunications\Badger\Badger.Default.Installer.StubExecutable\bin\net472"
                }
            };

            return ret;
        }

        public static void Main(string[] args) {
            Badger.Common.Diagnostics.Logging.ApplySimpleConfiguation();



            var Configurations = new List<PackagerConfiguration>() {
                MyConfig(),
                MyConfig().WithDefaults(),
                Defaults.Configuration(),

            };

            var Parser = new Mono.Options.OptionSet() {
                {
                    "from-json=",
                    "Specify a json file to load packaging options from",
                    x => { 
                        try {
                            var Instance = Badger.Common.Serialization.EasyJsonSerializer.Instance.FromFile<PackagerConfiguration>(x);
                            var Defaults = Instance.WithDefaults();

                            Configurations.InsertRange(0, new[]{Instance, Defaults });

                        } catch (Exception ex) {

                        }
                    
                    }
                },
                {
                    "create-package",
                    "create the package specified by all the options",
                    x => {
                        var Config = ConfigurationMerger.Many((x,y)=>ConfigurationMerger.Merge(x,y), Configurations);

                        Config.InvokeConfiguration();
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
                            var Config = ConfigurationMerger.Many((x,y)=>ConfigurationMerger.Merge(x,y), Configurations);

                            EasyJsonSerializer.Instance.ToFile(Config, x);
                        } catch (Exception ex) {

                        }
                    }
                }
            };


            args = new[] {
                $@"-to-json=C:\Users\TonyValenti\Documents\test.json",
                $@"-create-package"
            };

            var F = Parser.Parse(args);


            
 

        }
    }

}
