using Badger.Default.Installer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Badger.Default.Packager {
    public static partial class Configurations {

        private static string Coalesce(string V1, string V2) {
            return V1 ?? V2;
        }

        private static bool? Coalesce(bool? V1, bool? V2) {
            return V1 ?? V2;
        }

        private static Version Coalesce(Version V1, Version V2) {
            return V1 ?? V2;
        }

        private static List<T> Coalesce<T>(IEnumerable<T> V1, IEnumerable<T> V2) {
            var ret = new List<T>();
            if(V1 is { }) {
                ret.AddRange(V1);
            } else if (V2 is { }) {
                ret.AddRange(V2);
            }

            return ret;
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
                Output = Coalesce(V1?.Output, V2?.Output)
            };

            return ret;
        }

        public static OutputConfiguration Coalesce(OutputConfiguration V1, OutputConfiguration V2) {
            return new OutputConfiguration() {
                Installer_NameTemplate = Coalesce(V1?.Installer_NameTemplate, V2?.Installer_NameTemplate),
                Releases_NameTemplate = Coalesce(V1?.Releases_NameTemplate, V2?.Releases_NameTemplate),
                Path_NameTemplate = Coalesce(V1?.Path_NameTemplate, V2?.Path_NameTemplate),
            };
        }

        public static ArchiveConfiguration Coalesce(ArchiveConfiguration V1, ArchiveConfiguration V2) {
            return new ArchiveConfiguration() {
                CreateUsing = Coalesce(V1?.CreateUsing, V2?.CreateUsing),
                SignUsing = Coalesce(V1?.SignUsing, V2?.SignUsing)
            };
        }

        public static ExternalToolConfiguration Coalesce(ExternalToolConfiguration V1, ExternalToolConfiguration V2) {
            return new ExternalToolConfiguration() {
                Application = Coalesce(V1?.Application, V2?.Application),
                ArgumentTemplate = Coalesce(V1?.ArgumentTemplate, V2?.ArgumentTemplate),
                Async = Coalesce(V1?.Async, V2?.Async),
                Enabled = Coalesce(V1?.Enabled, V2?.Enabled),
                Visible  = Coalesce(V1?.Visible, V2?.Visible)
            };
        }

        public static WorkingFolderConfiguration Coalesce(WorkingFolderConfiguration V1, WorkingFolderConfiguration V2) {
            return new WorkingFolderConfiguration() {
                Delete_OnFinish = Coalesce(V1?.Delete_OnFinish, V2?.Delete_OnFinish),
                Delete_OnStart = Coalesce(V1?.Delete_OnStart, V2?.Delete_OnStart),
                PathRoot = Coalesce(V1?.PathRoot, V2?.PathRoot),
                PathTemplate = Coalesce(V1?.PathTemplate, V2?.PathTemplate),
            };
        }

        public static RedirectorConfiguration Coalesce(RedirectorConfiguration V1, RedirectorConfiguration V2) {
            return new RedirectorConfiguration() {
                SignUsing = Coalesce(V1?.SignUsing, V2?.SignUsing),
                Stub = Coalesce(V1?.Stub, V2?.Stub)
            };
        }

        public static InstallerConfiguration Coalesce(InstallerConfiguration V1, InstallerConfiguration V2) {
            return new InstallerConfiguration() {
                Content = Coalesce(V1?.Content, V2?.Content),
                Icon = Coalesce(V1?.Icon, V2?.Icon),
                Playbook = Coalesce(V1?.Playbook, V2?.Playbook),
                SignUsing = Coalesce(V1?.SignUsing, V2?.SignUsing),
                SplashScreen = Coalesce(V1?.SplashScreen, V2?.SplashScreen),
                Stub = Coalesce(V1?.Stub, V2?.Stub)
            };
        }

        public static SigningConfiguration Coalesce(SigningConfiguration V1, SigningConfiguration V2) {
            return new SigningConfiguration() {
                Application = Coalesce(V1?.Application, V2?.Application),
                ArgumentTemplate = Coalesce(V1?.ArgumentTemplate, V2?.ArgumentTemplate),
                Async = Coalesce(V1?.Async, V2?.Async),
                Certificate = Coalesce(V1?.Certificate, V2?.Certificate),
                Enabled = Coalesce(V1?.Enabled, V2?.Enabled),
                Visible = Coalesce(V1?.Visible, V2?.Visible)
            };
        }

        public static ProductConfiguration Coalesce(ProductConfiguration V1, ProductConfiguration V2) {
            return new ProductConfiguration() {
                Name = Coalesce(V1?.Name, V2?.Name),
                Version = Coalesce(V1?.Version, V2?.Version),
                Version_FromFile = Coalesce(V1?.Version_FromFile, V2?.Version_FromFile),
            };
        }

        public static InstallerContentPlaybookConfiguration Coalesce(InstallerContentPlaybookConfiguration V1, InstallerContentPlaybookConfiguration V2) {
            return new InstallerContentPlaybookConfiguration() {
                CloseOldVersions = Coalesce(V1?.CloseOldVersions, V2?.CloseOldVersions),
                DeleteOldVersions = Coalesce(V1?.DeleteOldVersions, V2?.DeleteOldVersions),
                ExtractContent = Coalesce(V1?.ExtractContent, V2?.ExtractContent),
                ExtractContentToSubfolder = Coalesce(V1?.ExtractContentToSubfolder, V2?.ExtractContentToSubfolder),
                Scripts = Coalesce(V1?.Scripts, V2?.Scripts)
            };
        }

        public static List<ExternalToolConfiguration> Coalesce(List<ExternalToolConfiguration> V1, List<ExternalToolConfiguration> V2) {
            var ret = new List<ExternalToolConfiguration>();

            if( (V1 ?? V2) is { } V) {
                ret.AddRange(from x in V select Coalesce(x, default));
            }


            return ret;
        }

        public static InstallerContentConfiguration Coalesce(InstallerContentConfiguration V1, InstallerContentConfiguration V2) {
            return new InstallerContentConfiguration() {
                Include = Coalesce(V1?.Include, V2?.Include),
                Source = Coalesce(V1?.Source, V2?.Source)
            };
        }

        public static ExternalToolsConfiguration Coalesce(ExternalToolsConfiguration V1, ExternalToolsConfiguration V2) {
            return new ExternalToolsConfiguration() {
                Icon = Coalesce(V1?.Icon, V2?.Icon),
                VersionString = Coalesce(V1?.VersionString, V2?.VersionString)
            };
        }

        public static VersionStringExternalToolConfiguration Coalesce(VersionStringExternalToolConfiguration V1, VersionStringExternalToolConfiguration V2) {
            return new VersionStringExternalToolConfiguration() {
                Get = Coalesce(V1?.Get, V2?.Get),
                Set = Coalesce(V1?.Set, V2?.Set),
                Values = Coalesce(V1?.Values, V2?.Values)
            };
        }

        public static IconExternalToolConfiguration Coalesce(IconExternalToolConfiguration V1, IconExternalToolConfiguration V2) {
            return new IconExternalToolConfiguration() {
                Get = Coalesce(V1?.Get, V2?.Get),
                Set = Coalesce(V1?.Set, V2?.Set),
            };
        }

        public static DefaultsConfiguration Coalesce(DefaultsConfiguration V1, DefaultsConfiguration V2) {
            return new DefaultsConfiguration() {
                SignUsing = Coalesce(V1?.SignUsing, V2?.SignUsing)
            };
        }

        
    }

}
