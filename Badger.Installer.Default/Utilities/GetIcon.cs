using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Installer.Default {
    public static class GetIcon {

        public static string FolderName => "7Zip";
        public static string ExecutableName => "7z.exe";

        public static string FolderPath => System.IO.Path.GetDirectoryName(ExecutablePath);
        public static string ExecutablePath => typeof(Badger.Installer.Default.Utilities.GetIcon.Program).Assembly.Location;

        public static string ArgumentTemplate => $@"-{nameof(GetFileIconParameters.Source_File)} ""{{{nameof(GetFileIconParameters.Source_File)}}}"" -{nameof(GetFileIconParameters.Dest_File)} ""{{{nameof(GetFileIconParameters.Dest_File)}}}""";


    }
}
