using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using StringTokenFormatter;

namespace Badger.Diagnostics {
    public static class Utility {


        public static string Quote(params string[] Values) {
            return Quote((IEnumerable<string>)Values);
        }

        public static string Quote(IEnumerable<string> Values) {
            var ret =
                string.Join(" ",
                from x in Values
                select $@"""{x}"""
                );

            return ret;
        }

        public static Process Run<T>(string FullPath, string ArgumentTemplate, T ArgumentValues, CancellationToken? ctoken = default) {
            var NewArguments = ArgumentTemplate.FormatToken(ArgumentValues);
            return Run(FullPath, NewArguments, ctoken);
        }

        public static Process Run(string FullPath, string Arguments, CancellationToken? ctoken = default) {
            var PSI = new ProcessStartInfo() {
                FileName = FullPath,
                Arguments = Arguments,
                CreateNoWindow = true,

                RedirectStandardError = true,
                RedirectStandardOutput = true,

                ErrorDialog = false,

                WindowStyle = ProcessWindowStyle.Hidden,

                UseShellExecute = false,
            };

            return Run(PSI, ctoken);
        }

        public static Process Run(ProcessStartInfo PSI, CancellationToken? ctoken = default) {
            var token = ctoken ?? CancellationToken.None;
            
            var ret = System.Diagnostics.Process.Start(PSI);

            ret.ErrorDataReceived += Ret_ErrorDataReceived;
            ret.OutputDataReceived += Ret_OutputDataReceived;

            ret.BeginOutputReadLine();
            ret.BeginErrorReadLine();

            while (!token.IsCancellationRequested && !ret.WaitForExit(1000)) {
                
            }

            if (token.IsCancellationRequested) {
                ret.Kill();
            }

            return ret;
        }

        private static void Ret_OutputDataReceived(object sender, DataReceivedEventArgs e) {
            Console.WriteLine(e.Data);
            System.Diagnostics.Debug.WriteLine(e.Data);
        }

        private static void Ret_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
            Console.WriteLine(e.Data);
            System.Diagnostics.Debug.WriteLine(e.Data);
        }
    }
}
