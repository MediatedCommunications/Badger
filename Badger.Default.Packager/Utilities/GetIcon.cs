using Badger.Default.Configuration;
using Badger.Default.Installer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Default.Packager.Utilities.GetIcon {
    public static class CommandLine {

        public static string FolderPath => System.IO.Path.GetDirectoryName(ExecutablePath);
        public static string ExecutablePath => typeof(Badger.Default.Packager.Utilities.GetIcon.Program).Assembly.Location;
        public static string ExecutableName => System.IO.Path.GetFileName(ExecutablePath);

        public static string ArgumentTemplate => $@"-{nameof(GetFileIconParameters.Source_File)} ""{{{nameof(GetFileIconParameters.Source_File)}}}"" -{nameof(GetFileIconParameters.Dest_File)} ""{{{nameof(GetFileIconParameters.Dest_File)}}}""";


    }
}
