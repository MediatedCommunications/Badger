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
            public static string SetParameterTemplate => $@"""{{{nameof(SetFileIconParameters.Dest_File)}}}"" --set-icon ""{{{nameof(SetFileIconParameters.Source_File)}}}""";
          

        }
    }

    public static partial class RCEdit {
        public static partial class VersionStrings {

            
            public static string[] Default = new[] {
                "CompanyName",
                "LegalCopyright",
                "FileDescription",
                "ProductName",

                "OriginalFilename",

                "FileVersion",
                "ProductVersion",
            };

            private static string InvalidVersionString => "Fatal error: Unable to get version string";

            public static string GetParametersTemplate => $@"""{{{nameof(GetVersionStringParameters.Source_File)}}}"" --get-version-string {{{nameof(GetVersionStringParameters.Key)}}}";
            public static string SetParametersTemplate => $@"""{{{nameof(SetVersionStringParameters.Dest_File)}}}"" --set-version-string ""{{{nameof(SetVersionStringParameters.Key)}}}"" ""{{{nameof(SetVersionStringParameters.Value)}}}""";


        }
    }

}
