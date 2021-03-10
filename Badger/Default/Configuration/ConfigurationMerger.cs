using Badger.Default.Installer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Badger.Default.Configuration {

    public static class ConfigurationMerger {

        public static T Many<T>(Func<T, T, T> Merger, IEnumerable<T> Values) {
            var ret = default(T);
            if (Values is { }) {
                foreach (var item in Values) {
                    ret = Merger(ret, item);
                }
            }

            return ret;
        }


        public static string Merge(string V1, string V2) {
            return V1 ?? V2;
        }

        public static bool? Merge(bool? V1, bool? V2) {
            return V1 ?? V2;
        }

        private static Version Merge(Version V1, Version V2) {
            return V1 ?? V2;
        }

        public static List<T> Merge<T>(IEnumerable<T> V1, IEnumerable<T> V2) {
            var ret = new List<T>();
            if(V1 is { }) {
                ret.AddRange(V1);
            } else if (V2 is { }) {
                ret.AddRange(V2);
            }

            return ret;
        }


        public static PackagerConfiguration Merge(PackagerConfiguration V1, PackagerConfiguration V2) {
            var ret = new PackagerConfiguration() {
                Archive = Merge(V1?.Archive, V2?.Archive),
                Defaults = Merge(V1?.Defaults, V2?.Defaults),
                ExternalTools = Merge(V1?.ExternalTools, V2?.ExternalTools),
                Installer = Merge(V1?.Installer, V2?.Installer),
                Uninstaller = Merge(V1?.Uninstaller, V2?.Uninstaller),
                Product = Merge(V1?.Product, V2?.Product),
                Redirector = Merge(V1?.Redirector, V2?.Redirector),
                WorkingFolder = Merge(V1?.WorkingFolder, V2?.WorkingFolder),
                Output = Merge(V1?.Output, V2?.Output)
            };

            return ret;
        }

        public static UninstallerConfiguration Merge(UninstallerConfiguration V1, UninstallerConfiguration V2) {
            return new UninstallerConfiguration() {
                Icon = Merge(V1?.Icon, V2?.Icon),
                Playbook = Merge(V1?.Playbook, V2?.Playbook),
                SignUsing = Merge(V1?.SignUsing, V2?.SignUsing),
                Stub = Merge(V1?.Stub, V2?.Stub),
            };
        }

        public static UninstallerContentPlaybookConfiguration Merge(UninstallerContentPlaybookConfiguration V1, UninstallerContentPlaybookConfiguration V2) {
            return new UninstallerContentPlaybookConfiguration() {
                CloseOldVersions = Merge(V1?.CloseOldVersions, V2?.CloseOldVersions),
                DeleteOldVersions = Merge(V1?.DeleteOldVersions, V2?.DeleteOldVersions),
                Scripts = Merge(V1?.Scripts, V2?.Scripts),
            };
        }


        public static OutputConfiguration Merge(OutputConfiguration V1, OutputConfiguration V2) {
            return new OutputConfiguration() {
                Installer_NameTemplate = Merge(V1?.Installer_NameTemplate, V2?.Installer_NameTemplate),
                Releases_NameTemplate = Merge(V1?.Releases_NameTemplate, V2?.Releases_NameTemplate),
                Path_NameTemplate = Merge(V1?.Path_NameTemplate, V2?.Path_NameTemplate),
            };
        }

        public static ArchiveConfiguration Merge(ArchiveConfiguration V1, ArchiveConfiguration V2) {
            return new ArchiveConfiguration() {
                CreateUsing = Merge(V1?.CreateUsing, V2?.CreateUsing),
                SignUsing = Merge(V1?.SignUsing, V2?.SignUsing)
            };
        }

        public static ExternalToolConfiguration Merge(ExternalToolConfiguration V1, ExternalToolConfiguration V2) {
            return new ExternalToolConfiguration() {
                Application = Merge(V1?.Application, V2?.Application),
                Path = Merge(V1?.Path, V2?.Path),
                ArgumentTemplate = Merge(V1?.ArgumentTemplate, V2?.ArgumentTemplate),
                Async = Merge(V1?.Async, V2?.Async),
                Enabled = Merge(V1?.Enabled, V2?.Enabled),
                Visible  = Merge(V1?.Visible, V2?.Visible)
            };
        }

        public static WorkingFolderConfiguration Merge(WorkingFolderConfiguration V1, WorkingFolderConfiguration V2) {
            return new WorkingFolderConfiguration() {
                Delete_OnFinish = Merge(V1?.Delete_OnFinish, V2?.Delete_OnFinish),
                Delete_OnStart = Merge(V1?.Delete_OnStart, V2?.Delete_OnStart),
                PathRoot = Merge(V1?.PathRoot, V2?.PathRoot),
                PathTemplate = Merge(V1?.PathTemplate, V2?.PathTemplate),
            };
        }

        public static RedirectorConfiguration Merge(RedirectorConfiguration V1, RedirectorConfiguration V2) {
            return new RedirectorConfiguration() {
                SignUsing = Merge(V1?.SignUsing, V2?.SignUsing),
                Stub = Merge(V1?.Stub, V2?.Stub)
            };
        }

        public static InstallerConfiguration Merge(InstallerConfiguration V1, InstallerConfiguration V2) {
            return new InstallerConfiguration() {
                Content = Merge(V1?.Content, V2?.Content),
                Icon = Merge(V1?.Icon, V2?.Icon),
                Playbook = Merge(V1?.Playbook, V2?.Playbook),
                SignUsing = Merge(V1?.SignUsing, V2?.SignUsing),
                SplashScreen = Merge(V1?.SplashScreen, V2?.SplashScreen),
                Stub = Merge(V1?.Stub, V2?.Stub)
            };
        }

        public static SigningConfiguration Merge(SigningConfiguration V1, SigningConfiguration V2) {
            return new SigningConfiguration() {
                Application = Merge(V1?.Application, V2?.Application),
                Path = Merge(V1?.Path, V2?.Path),
                ArgumentTemplate = Merge(V1?.ArgumentTemplate, V2?.ArgumentTemplate),
                Async = Merge(V1?.Async, V2?.Async),
                Certificate = Merge(V1?.Certificate, V2?.Certificate),
                Enabled = Merge(V1?.Enabled, V2?.Enabled),
                Visible = Merge(V1?.Visible, V2?.Visible)
            };
        }

        public static ProductConfiguration Merge(ProductConfiguration V1, ProductConfiguration V2) {
            return new ProductConfiguration() {
                Name = Merge(V1?.Name, V2?.Name),
                Publisher = Merge(V1?.Publisher, V2?.Publisher),
                Code = Merge(V1?.Code, V2?.Code),
                Version = Merge(V1?.Version, V2?.Version),
                Version_FromFile = Merge(V1?.Version_FromFile, V2?.Version_FromFile),
            };
        }

        public static InstallerContentPlaybookConfiguration Merge(InstallerContentPlaybookConfiguration V1, InstallerContentPlaybookConfiguration V2) {
            return new InstallerContentPlaybookConfiguration() {
                CloseOldVersions = Merge(V1?.CloseOldVersions, V2?.CloseOldVersions),
                DeleteOldVersions = Merge(V1?.DeleteOldVersions, V2?.DeleteOldVersions),
                ExtractContent = Merge(V1?.ExtractContent, V2?.ExtractContent),
                ExtractContentToSubfolder = Merge(V1?.ExtractContentToSubfolder, V2?.ExtractContentToSubfolder),
                Scripts = Merge(V1?.Scripts, V2?.Scripts)
            };
        }

        public static List<ExternalToolConfiguration> Merge(List<ExternalToolConfiguration> V1, List<ExternalToolConfiguration> V2) {
            var ret = new List<ExternalToolConfiguration>();

            if( (V1 ?? V2) is { } V) {
                ret.AddRange(from x in V select Merge(x, default));
            }


            return ret;
        }

        public static InstallerContentConfiguration Merge(InstallerContentConfiguration V1, InstallerContentConfiguration V2) {
            return new InstallerContentConfiguration() {
                Include = Merge(V1?.Include, V2?.Include),
                Source = Merge(V1?.Source, V2?.Source)
            };
        }

        public static ExternalToolsConfiguration Merge(ExternalToolsConfiguration V1, ExternalToolsConfiguration V2) {
            return new ExternalToolsConfiguration() {
                Icon = Merge(V1?.Icon, V2?.Icon),
                VersionString = Merge(V1?.VersionString, V2?.VersionString)
            };
        }

        public static VersionStringExternalToolConfiguration Merge(VersionStringExternalToolConfiguration V1, VersionStringExternalToolConfiguration V2) {
            return new VersionStringExternalToolConfiguration() {
                Get = Merge(V1?.Get, V2?.Get),
                Set = Merge(V1?.Set, V2?.Set),
                Values = Merge(V1?.Values, V2?.Values)
            };
        }

        public static IconExternalToolConfiguration Merge(IconExternalToolConfiguration V1, IconExternalToolConfiguration V2) {
            return new IconExternalToolConfiguration() {
                Get = Merge(V1?.Get, V2?.Get),
                Set = Merge(V1?.Set, V2?.Set),
            };
        }

        public static DefaultsConfiguration Merge(DefaultsConfiguration V1, DefaultsConfiguration V2) {
            return new DefaultsConfiguration() {
                SignUsing = Merge(V1?.SignUsing, V2?.SignUsing)
            };
        }

        
    }

}
