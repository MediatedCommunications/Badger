using System;
using System.Collections.Generic;
using System.Linq;

namespace Badger.Installer.Default {

    public static partial class CoalesceExtensions {

        private static string Coalesce(string V1, string V2) {
            return V1 ?? V2;
        }

        private static bool? Coalesce(bool? V1, bool? V2) {
            return V1 ?? V2;
        }

        private static Version Coalesce(Version V1, Version V2) {
            return V1 ?? V2;
        }


        public static PackagerConfiguration Coalesce(PackagerConfiguration V1, PackagerConfiguration V2) {
            var ret = new PackagerConfiguration() {
                Archive = Coalesce(V1?.Archive, V2?.Archive),
                Defaults = Coalesce(V1?.Defaults, V2?.Defaults),
                ExternalTools = Coalesce(V1?.ExternalTools, V2?.ExternalTools),
                Installer = Coalesce(V1?.Installer, V2?.Installer),
                Product = Coalesce(V1?.Product, V2?.Product),
                Redirector = Coalesce(V1?.Redirector, V2?.Redirector),
                WorkingFolder = Coalesce(V1?.WorkingFolder, V2?.WorkingFolder),
            };

            return ret;
        }

        private static ArchiveConfiguration Coalesce(ArchiveConfiguration V1, ArchiveConfiguration V2) {
            return new ArchiveConfiguration() {
                CreateUsing = Coalesce(V1?.CreateUsing, V2?.CreateUsing),
                SignUsing = Coalesce(V1?.SignUsing, V2?.SignUsing)
            };
        }

        private static ExternalToolConfiguration Coalesce(ExternalToolConfiguration V1, ExternalToolConfiguration V2) {
            return new ExternalToolConfiguration() {
                Application = Coalesce(V1?.Application, V2?.Application),
                ArgumentTemplate = Coalesce(V1?.ArgumentTemplate, V2?.ArgumentTemplate),
                Async = Coalesce(V1?.Async, V2?.Async),
                Enabled = Coalesce(V1?.Enabled, V2?.Enabled),
                Visible  = Coalesce(V1?.Visible, V2?.Visible)
            };
        }

        private static WorkingFolderConfiguration Coalesce(WorkingFolderConfiguration V1, WorkingFolderConfiguration V2) {
            return new WorkingFolderConfiguration() {
                Delete_OnFinish = Coalesce(V1?.Delete_OnFinish, V2?.Delete_OnFinish),
                Delete_OnStart = Coalesce(V1?.Delete_OnStart, V2?.Delete_OnStart),
                PathRoot = Coalesce(V1?.PathRoot, V2?.PathRoot),
                PathTemplate = Coalesce(V1?.PathTemplate, V2?.PathTemplate),
            };
        }

        private static RedirectorConfiguration Coalesce(RedirectorConfiguration V1, RedirectorConfiguration V2) {
            return new RedirectorConfiguration() {
                SignUsing = Coalesce(V1?.SignUsing, V2?.SignUsing),
                Stub = Coalesce(V1?.Stub, V2?.Stub)
            };
        }

        private static InstallerConfiguration Coalesce(InstallerConfiguration V1, InstallerConfiguration V2) {
            return new InstallerConfiguration() {
                Content = Coalesce(V1?.Content, V2?.Content),
                Icon = Coalesce(V1?.Icon, V2?.Icon),
                Playbook = Coalesce(V1?.Playbook, V2?.Playbook),
                SignUsing = Coalesce(V1?.SignUsing, V2?.SignUsing),
                SplashScreen = Coalesce(V1?.SplashScreen, V2?.SplashScreen),
                Stub = Coalesce(V1?.Stub, V2?.Stub)
            };
        }

        private static SigningConfiguration Coalesce(SigningConfiguration V1, SigningConfiguration V2) {
            return new SigningConfiguration() {
                Application = Coalesce(V1?.Application, V2?.Application),
                ArgumentTemplate = Coalesce(V1?.ArgumentTemplate, V2?.ArgumentTemplate),
                Async = Coalesce(V1?.Async, V2?.Async),
                Certificate = Coalesce(V1?.Certificate, V2?.Certificate),
                Enabled = Coalesce(V1?.Enabled, V2?.Enabled),
                Visible = Coalesce(V1?.Visible, V2?.Visible)
            };
        }

        private static ProductConfiguration Coalesce(ProductConfiguration V1, ProductConfiguration V2) {
            return new ProductConfiguration() {
                Name = Coalesce(V1?.Name, V2?.Name),
                Version = Coalesce(V1?.Version, V2?.Version),
                Version_FromFile = Coalesce(V1?.Version_FromFile, V2?.Version_FromFile),
            };
        }

        private static InstallerContentPlaybookConfiguration Coalesce(InstallerContentPlaybookConfiguration V1, InstallerContentPlaybookConfiguration V2) {
            return new InstallerContentPlaybookConfiguration() {
                CloseOldVersions = Coalesce(V1?.CloseOldVersions, V2?.CloseOldVersions),
                DeleteOldVersions = Coalesce(V1?.DeleteOldVersions, V2?.DeleteOldVersions),
                ExtractContent = Coalesce(V1?.ExtractContent, V2?.ExtractContent),
                ExtractContentToSubfolder = Coalesce(V1?.ExtractContentToSubfolder, V2?.ExtractContentToSubfolder),
                Scripts = Coalesce(V1?.Scripts, V2?.Scripts)
            };
        }

        private static List<ExternalToolConfiguration> Coalesce(List<ExternalToolConfiguration> V1, List<ExternalToolConfiguration> V2) {
            var ret = new List<ExternalToolConfiguration>();

            if( (V1 ?? V2) is { } V) {
                ret.AddRange(from x in V select Coalesce(x, default));
            }


            return ret;
        }

        private static InstallerContentConfiguration Coalesce(InstallerContentConfiguration V1, InstallerContentConfiguration V2) {
            return new InstallerContentConfiguration() {
                Include = Coalesce(V1?.Include, V2?.Include),
                Source = Coalesce(V1?.Source, V2?.Source)
            };
        }

        private static ExternalToolsConfiguration Coalesce(ExternalToolsConfiguration V1, ExternalToolsConfiguration V2) {
            return new ExternalToolsConfiguration() {
                Icon = Coalesce(V1?.Icon, V2?.Icon),
                VersionString = Coalesce(V1?.VersionString, V2?.VersionString)
            };
        }

        private static VersionStringExternalToolConfiguration Coalesce(VersionStringExternalToolConfiguration V1, VersionStringExternalToolConfiguration V2) {
            return new VersionStringExternalToolConfiguration() {
                Get = Coalesce(V1?.Get, V2?.Get),
                Set = Coalesce(V1?.Set, V2?.Set),
            };
        }

        private static IconExternalToolConfiguration Coalesce(IconExternalToolConfiguration V1, IconExternalToolConfiguration V2) {
            return new IconExternalToolConfiguration() {
                Get = Coalesce(V1?.Get, V2?.Get),
                Set = Coalesce(V1?.Set, V2?.Set),
            };
        }

        private static DefaultsConfiguration Coalesce(DefaultsConfiguration V1, DefaultsConfiguration V2) {
            return new DefaultsConfiguration() {
                SignUsing = Coalesce(V1?.SignUsing, V2?.SignUsing)
            };
        }

        
    }
    


    public static class ConfigDefaults {
        public static SigningConfiguration SigningConfiguration() {
            var ret = new SigningConfiguration() {
                Application = Utilities.SignTool.ExecutablePath,
                ArgumentTemplate = Utilities.SignTool.SignParameterTemplateDefault,
                Enabled = false,
            };

            return ret;
        }

        public static DefaultsConfiguration DefaultsConfiguration() {
            var ret = new DefaultsConfiguration {
                SignUsing = SigningConfiguration()
            };

            return ret;
        }

        public static ExternalToolsConfiguration ExternalToolsConfiguration() {
            var ret = new ExternalToolsConfiguration() {
                VersionString = VersionStringExternalToolConfiguration(),
                Icon = IconExternalToolConfiguration(),
            };

            return ret;

        }

        private static IconExternalToolConfiguration IconExternalToolConfiguration() {
            return new IconExternalToolConfiguration() {
                Get = new ExternalToolConfiguration() {
                    Enabled = true,
                    Application = GetIcon.ExecutablePath,
                    ArgumentTemplate = GetIcon.ArgumentTemplate,
                },
                Set = new ExternalToolConfiguration() {
                    Enabled = true,
                    Application = Utilities.RCEdit.ExecutablePath,
                    ArgumentTemplate = Utilities.RCEdit.Icons.SetParameterTemplate,
                }
            };
        }

        private static VersionStringExternalToolConfiguration VersionStringExternalToolConfiguration() {
            return new VersionStringExternalToolConfiguration() {
                Get = new ExternalToolConfiguration() {
                    Enabled = true,
                    Application = Utilities.RCEdit.ExecutablePath,
                    ArgumentTemplate = Utilities.RCEdit.VersionStrings.GetParametersTemplate,
                },
                Set = new ExternalToolConfiguration() {
                    Enabled = true,
                    Application = Utilities.RCEdit.ExecutablePath,
                    ArgumentTemplate = Utilities.RCEdit.VersionStrings.SetParametersTemplate,
                }
            };
        }

        private static RedirectorConfiguration RedirectorConfiguration() {
            var ret = new RedirectorConfiguration() {
                Stub = new InstallerContentConfiguration() {
                    Include = true,
                    Source = typeof(Badger.Redirector.Default.StubExecutable.Program).Assembly.Location,
                },
            };

            return ret;
        }

        private static ArchiveConfiguration ArchiveConfiguration() {
            var ret = new ArchiveConfiguration() {
                CreateUsing = new ExternalToolConfiguration() {
                    Enabled = true,
                    Application = Utilities.SevenZip.ExecutablePath,
                    ArgumentTemplate = Utilities.SevenZip.CreateSelfExtractingArchiveParameterTemplate,
                },
            };


            return ret;
        }

        private static WorkingFolderConfiguration WorkingFolderConfiguration() {
            var LocalPath = System.IO.Path.GetDirectoryName(Badger.Diagnostics.Application.EntryAssembly.Location);

            var PathRoot = System.IO.Path.Combine(LocalPath, "WorkingFolder");

            var ret = new WorkingFolderConfiguration() {
                Delete_OnStart = true,
                Delete_OnFinish = true,
                PathRoot = PathRoot,
                PathTemplate = $@"{{{nameof(WorkingFolderTemplateParameters.PackageName)}}}\{{{nameof(WorkingFolderTemplateParameters.Version)}}}"
            };

            return ret;
        }



        public static PackagerConfiguration Configuration() {

            var ret = new PackagerConfiguration() {
                Defaults = ConfigDefaults.DefaultsConfiguration(),
                Product = ConfigDefaults.ProductConfiguration(),
                ExternalTools = ConfigDefaults.ExternalToolsConfiguration(),
                Redirector = ConfigDefaults.RedirectorConfiguration(),
                Archive = ConfigDefaults.ArchiveConfiguration(),
                WorkingFolder = ConfigDefaults.WorkingFolderConfiguration(),
                Installer = ConfigDefaults.InstallerConfiguration(),
                
            };

            return ret;
        }

        private static ProductConfiguration ProductConfiguration() {
            return new ProductConfiguration() {
                Name = "NoName",
                Version = new Version(0, 0, 0)
            };
        }

        private static InstallerConfiguration InstallerConfiguration() {
            var ret = new InstallerConfiguration() {
                Playbook = new InstallerContentPlaybookConfiguration() {
                    ExtractContentToSubfolder = "NoName",

                    CloseOldVersions = true,
                    DeleteOldVersions = true,
                    
                    ExtractContent = new ExternalToolConfiguration() {
                        Enabled = true,
                        ArgumentTemplate = Utilities.SevenZip.ExtractArchiveToLocationParameterTemplate
                    },
                },
                Stub = new InstallerContentConfiguration() {
                    Include = true,
                    Source = typeof(Badger.Installer.Default.StubExecutable.Program).Assembly.Location,
                }
                
            };


            return ret;
        }
    }

}
