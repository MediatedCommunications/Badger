using Badger.Common.Diagnostics;
using Badger.Default.Installer;
using StringTokenFormatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Default.Packager.Utilities.SignTool {
    public static class CommandLine {
        public static string FolderName => "SignTool";
        public static string ExecutableName => "SignTool.exe";

        public static string FolderPath => Utility.Path(FolderName);
        public static string ExecutablePath => Utility.Path(FolderName, ExecutableName);

        public static string SignParameterTemplateDefault => $@"sign /a ""{{{nameof(SignAssemblyParameters.Assembly)}}}""";


    }
}
