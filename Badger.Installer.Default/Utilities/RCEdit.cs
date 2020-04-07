using Badger.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Installer.Default.Utilities {
    public static partial class RCEdit {
        public static string FolderName => "RCEdit";
        public static string ExecutableName => "rcedit-x86.exe";

        public static string FolderPath => Utility.Path(FolderName);
        public static string ExecutablePath => Utility.Path(FolderName, ExecutableName);

    }

    public static partial class RCEdit {
        public static partial class Icons {
            public static string Get(string Path) {
                //return Icon.ExtractAssociatedIcon(Path);
                var FileName = System.IO.Path.GetTempFileName();
                using (var Output = System.IO.File.OpenWrite(FileName)) {
                    Toolbelt.Drawing.IconExtractor.Extract1stIconTo(Path, Output);
                }

                return FileName;


            }

            private static string SetParameters(string Path, string PathToIcon) {
                var ret = $@"""{Path}"" --set-icon ""{PathToIcon}""";
                return ret;
            }

            public static void Set(string Path, string PathToIcon) {
                Diagnostics.Utility.Run(ExecutablePath, SetParameters(Path, PathToIcon));
            }

            public static void Copy(string SourcePath, string DestPath) {
                var Icon = Get(SourcePath);
                Set(DestPath, Icon);
            }

        }
    }

    public static partial class RCEdit {
        public static partial class VersionStrings {

            private static string[] Default = new[] {
                "CompanyName",
                "LegalCopyright",
                "FileDescription",
                "ProductName",

                "OriginalFilename",

                "FileVersion",
                "ProductVersion",
            };

            private static string InvalidVersionString => "Fatal error: Unable to get version string";


            public static IDictionary<string, string> Get(string Path, IEnumerable<string> VersionStrings) {
                var ret = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                foreach (var item in VersionStrings) {

                    if (Get(Path, item) is { } V1) {
                        ret[item] = V1;
                    }
                }

                return ret;
            }

            public static IDictionary<string, string> Get(string Path) {
                return Get(Path, Default);
            }

            private static string GetParameters(string Path, string Key) {
                var ret = $@"""{Path}"" --get-version-string {Key}";
                return ret;
            }

            public static string Get(string Path, string Key) {
                var P = Diagnostics.Utility.Run(ExecutablePath, GetParameters(Path, Key));
                var ret = P.StandardOutput.ReadToEnd();

                return ret;
            }

            public static string SetParameters(string Path, IDictionary<string, string> VersionStrings) {
                var ret = $@"""{Path}""";

                foreach (var item in VersionStrings) {
                    ret += $@" --set-version-string ""{item.Key}"" ""{item.Value}""";
                }

                return ret;
            }

            public static void Set(string Path, IDictionary<string, string> VersionStrings) {
                Diagnostics.Utility.Run(ExecutablePath, SetParameters(Path, VersionStrings));
            }

            public static void Copy(string SourcePath, string DestinationPath) {
                Copy(SourcePath, DestinationPath, Default);
            }

            public static void Copy(string SourcePath, string DestinationPath, IEnumerable<string> VersionStrings) {
                var Values = Get(SourcePath, VersionStrings);
                Set(DestinationPath, Values);

            }


        }
    }

}
