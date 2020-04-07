using Badger.Diagnostics;
using StringTokenFormatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Installer.Default.Utilities {
    public static class SevenZip {
        public static string FolderName => "7Zip";
        public static string ExecutableName => "7z.exe";

        public static string FolderPath => Utility.Path(FolderName);
        public static string ExecutablePath => Utility.Path(FolderName, ExecutableName);

        public static string CreateSelfExtractingArchiveParameterTemplate => $@"a -mx9 -sfx7zcon.sfx ""{{{nameof(ArchiveCommandParameters.Dest_File)}}}"" ""{{{nameof(ArchiveCommandParameters.Source_Folder)}}}\*""";

        public static void CreateSelfExtractingArchive(string ExecutablePath, string ParameterTemplate, string Source_Folder, string Dest_File) {
            CreateSelfExtractingArchive(ExecutablePath, ParameterTemplate, new ArchiveCommandParameters() {
                Source_Folder = Source_Folder,
                Dest_File = Dest_File,
            });
        }

        public static void CreateSelfExtractingArchive(string ExecutablePath, string ParameterTemplate, ArchiveCommandParameters ParameterValues) {
            Diagnostics.Utility.Run(ExecutablePath, ParameterTemplate, ParameterValues);
        }

        public static string ExtractArchiveToLocationParameterTemplate => $@"x -y -o""{{{nameof(Badger.Installer.Default.StubExecutable.ExtractParameters.Location)}}}""";

    }

}
