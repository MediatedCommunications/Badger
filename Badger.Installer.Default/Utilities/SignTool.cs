using Badger.Diagnostics;
using StringTokenFormatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Installer.Default.Utilities {
    public static class SignTool {
        public static string FolderName => "SignTool";
        public static string ExecutableName => "SignTool.exe";

        public static string FolderPath => Utility.Path(FolderName);
        public static string ExecutablePath => Utility.Path(FolderName, ExecutableName);

        public static string SignParameterTemplate(string FullPath) {
            var ret = $@"sign /a ""{FullPath}""";

            return ret;
        }

        public static void Sign(string ExecutablePath, string ParameterTemplate, string File, string Certificate) {
            Sign(ExecutablePath, ParameterTemplate, new[] { File }, Certificate);
        }


        public static void Sign(string ExecutablePath, string ParameterTemplate, IEnumerable<string> Files, string Certificate) {

            Sign(ExecutablePath, ParameterTemplate, new SignCommandParameters() {
                Files = Diagnostics.Utility.Quote(Files),
                Certificate = Diagnostics.Utility.Quote(Certificate)
            });

        }


        public static void Sign(string ExecutablePath, string ParameterTemplate, SignCommandParameters ParameterValues) {

            Diagnostics.Utility.Run(ExecutablePath, ParameterTemplate, ParameterValues);

        }



    }
}
