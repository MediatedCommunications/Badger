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

        public static string SignParameterTemplateDefault => Utilities.SignTool.SignParameterTemplate($@"{{{nameof(SignAssemblyParameters.Assembly)}}}");

        public static string SignParameterTemplate(string FullPath) {
            var ret = $@"sign /a ""{FullPath}""";

            return ret;
        }


        public static void Sign(string ExecutablePath, string ParameterTemplate, string Assembly, string Certificate) {

            Sign(ExecutablePath, ParameterTemplate, new SignAssemblyParameters() {
                Assembly = Assembly,
                Certificate = Certificate
            });

        }


        public static void Sign(string ExecutablePath, string ParameterTemplate, SignAssemblyParameters ParameterValues) {

            Diagnostics.Utility.Run(ExecutablePath, ParameterTemplate, ParameterValues);

        }



    }
}
