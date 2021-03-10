using Badger.Default.Configuration;
using Badger.Default.Installer;

namespace Badger.Default.Packager.Utilities.RcEdit {
    public static partial class CommandLine {
        public static string FolderName => "RCEdit";
        public static string ExecutableName => "rcedit-x86.exe";

        public static string FolderPath => Utility.Path(FolderName);
        public static string ExecutablePath => Utility.Path(FolderName, ExecutableName);

        public static partial class Icons {
            public static string SetParameterTemplate => $@"""{{{nameof(SetFileIconParameters.Dest_File)}}}"" --set-icon ""{{{nameof(SetFileIconParameters.Source_File)}}}""";
        }

        public static partial class VersionStrings {
            public static string GetParametersTemplate => $@"""{{{nameof(GetVersionStringParameters.Source_File)}}}"" --get-version-string {{{nameof(GetVersionStringParameters.Key)}}}";
            public static string SetParametersTemplate => $@"""{{{nameof(SetVersionStringParameters.Dest_File)}}}"" --set-version-string ""{{{nameof(SetVersionStringParameters.Key)}}}"" ""{{{nameof(SetVersionStringParameters.Value)}}}""";
        }


    }


}
