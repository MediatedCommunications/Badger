using Badger.Common.Diagnostics;
using Badger.Default.Installer;
using StringTokenFormatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Default.Packager.Utilities.SevenZip {
    public static class CommandLine {
        public static string FolderName => "7Zip";
        public static string ExecutableName => "7z.exe";

        public static string FolderPath => Utility.Path(FolderName);
        public static string ExecutablePath => Utility.Path(FolderName, ExecutableName);

        public static class SelfExtractingArchive {
            public static string CreateParameterTemplate => $@"a -mx9 -sfx7zcon.sfx ""{{{nameof(CreateArchiveParameters.Dest_File)}}}"" ""{{{nameof(CreateArchiveParameters.Source_Folder)}}}\*""";
            public static string ExtractParameterTemplate => $@"x -y -o""{{{nameof(ExtractArchiveParameters.Dest_Folder)}}}""";
        }
    

    }

}
