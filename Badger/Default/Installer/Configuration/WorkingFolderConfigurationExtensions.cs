using StringTokenFormatter;
using System;
using Badger.Common;

namespace Badger.Default.Installer {
    public static class WorkingFolderConfigurationExtensions {

        public static bool Delete_OnStart(this WorkingFolderConfiguration This) {
            return (This?.Delete_OnStart).TrueWhenNull();
        }

        public static bool Delete_OnFinish(this WorkingFolderConfiguration This) {
            return (This?.Delete_OnFinish).TrueWhenNull();
        }

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
