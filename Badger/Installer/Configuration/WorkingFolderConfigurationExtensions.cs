using StringTokenFormatter;
using System;

namespace Badger.Installer {
    public static class WorkingFolderConfigurationExtensions {

        public static string ResolvePath(this WorkingFolderConfiguration This, ProductConfiguration Config) {
            return This.ResolvePath(Config.Name, Config.Version);
        }

        public static string ResolvePath(this WorkingFolderConfiguration This, string PackageName, Version Version) {
            return This.ResolvePath(new WorkingFolderTemplateParameters() {
                PackageName = PackageName,
                Version = Version
            });
        }

        public static string ResolvePath(this WorkingFolderConfiguration This, WorkingFolderTemplateParameters Parameters) {
            var ret = System.IO.Path.Combine(This.PathRoot, This.PathTemplate.FormatToken(Parameters));

            return ret;

        }
    }

}
