using Badger.Common.Diagnostics;
using Badger.Default.Packager.Utilities;
using Badger.Deployment;
using Badger.Default.Installer;
using System;
using System.Collections.Generic;
using Badger.Default.Configuration;

namespace Badger.Default.Packager {
       
    public static class Defaults {
        public static SigningConfiguration SigningConfiguration() {
            var ret = new SigningConfiguration() {
                Application = Utilities.SignTool.CommandLine.ExecutableName,
                Path = Utilities.SignTool.CommandLine.FolderPath,
                ArgumentTemplate = Utilities.SignTool.CommandLine.SignParameterTemplateDefault,
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

        public static IconExternalToolConfiguration IconExternalToolConfiguration() {
            return new IconExternalToolConfiguration() {
                Get = new ExternalToolConfiguration() {
                    Path = Utilities.GetIcon.CommandLine.FolderPath,
                    Application = Utilities.GetIcon.CommandLine.ExecutableName,
                    ArgumentTemplate = Utilities.GetIcon.CommandLine.ArgumentTemplate,
                },
                Set = new ExternalToolConfiguration() {
                    Path = Utilities.RcEdit.CommandLine.FolderPath,
                    Application = Utilities.RcEdit.CommandLine.ExecutableName,
                    ArgumentTemplate = Utilities.RcEdit.CommandLine.Icons.SetParameterTemplate,
                }
            };
        }

        public static VersionStringExternalToolConfiguration VersionStringExternalToolConfiguration() {
            return new VersionStringExternalToolConfiguration() {
                Values = new List<string>() {
                    "CompanyName",
                    "LegalCopyright",
                    "FileDescription",
                    "ProductName",

                    "OriginalFilename",

                    "FileVersion",
                    "ProductVersion",
                },
                Get = new ExternalToolConfiguration() {
                    Path = Utilities.RcEdit.CommandLine.FolderPath,
                    Application = Utilities.RcEdit.CommandLine.ExecutableName,
                    ArgumentTemplate = Utilities.RcEdit.CommandLine.VersionStrings.GetParametersTemplate,
                },
                Set = new ExternalToolConfiguration() {
                    Path = Utilities.RcEdit.CommandLine.FolderPath,
                    Application = Utilities.RcEdit.CommandLine.ExecutableName,
                    ArgumentTemplate = Utilities.RcEdit.CommandLine.VersionStrings.SetParametersTemplate,
                }
            };
        }

        public static RedirectorConfiguration RedirectorConfiguration() {
            var ret = new RedirectorConfiguration() {
                Stub = new InstallerContentConfiguration() {
                    Include = true,
                    Source = typeof(Badger.Default.Redirector.StubExecutable.Program).Assembly.Location,
                },
            };

            return ret;
        }

        public static ArchiveConfiguration ArchiveConfiguration() {
            var ret = new ArchiveConfiguration() {
                CreateUsing = new ExternalToolConfiguration() {
                    Path = Utilities.SevenZip.CommandLine.FolderPath,
                    Application = Utilities.SevenZip.CommandLine.ExecutableName,
                    ArgumentTemplate = Utilities.SevenZip.CommandLine.SelfExtractingArchive.CreateParameterTemplate,
                },
            };


            return ret;
        }

        public static WorkingFolderConfiguration WorkingFolderConfiguration() {
            var LocalPath = System.IO.Path.GetDirectoryName(Badger.Common.Diagnostics.Application.EntryAssembly.Location);

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
                Defaults = Defaults.DefaultsConfiguration(),
                Product = Defaults.ProductConfiguration(),
                ExternalTools = Defaults.ExternalToolsConfiguration(),
                Redirector = Defaults.RedirectorConfiguration(),
                Archive = Defaults.ArchiveConfiguration(),
                WorkingFolder = Defaults.WorkingFolderConfiguration(),
                Installer = Defaults.InstallerConfiguration(),
                Uninstaller = Defaults.UninstallerConfiguration(),
                Output = Defaults.OutputConfiguration(),
            };

            return ret;
        }

        public static UninstallerConfiguration UninstallerConfiguration() {
            return new UninstallerConfiguration() {
                Stub = new InstallerContentConfiguration() {
                    Include = true,
                    Source= typeof(Badger.Default.Uninstaller.StubExecutable.Program).Assembly.Location,
                }
            };
        }


        public static OutputConfiguration OutputConfiguration() {
            return new OutputConfiguration() {
                Path_NameTemplate = Application.FolderPath,
                Installer_NameTemplate = $@"{{{nameof(InstallerOutputParameters.PackageName)}}}-{{{nameof(InstallerOutputParameters.Version)}}}.exe",
                Releases_NameTemplate = LocationHelpers.ReleasesFileName
            };
        }

        public static ProductConfiguration ProductConfiguration() {
            return new ProductConfiguration() {
                Name = "NoName",
                Publisher = "NoName",
                Version = new Version(0, 0, 0)
            };
        }

        public static InstallerConfiguration InstallerConfiguration() {
            var ret = new InstallerConfiguration() {
                Playbook = new InstallerContentPlaybookConfiguration() {
                    ExtractContentToSubfolder = "NoName",
                   
                    ExtractContent = new ExternalToolConfiguration() {
                        ArgumentTemplate = Utilities.SevenZip.CommandLine.SelfExtractingArchive.ExtractParameterTemplate,
                    },
                },
                Stub = new InstallerContentConfiguration() {
                    Include = true,
                    Source = typeof(Badger.Default.Installer.StubExecutable.Program).Assembly.Location,
                }
                
            };


            return ret;
        }
    }

}
