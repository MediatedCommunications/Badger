using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using StringTokenFormatter;

namespace Badger.Common.Diagnostics {
    public static class Utility {

        public static Process Run<T>(string FullPath, string ArgumentTemplate, T ArgumentValues, CancellationToken? ctoken = default) {
            var PSI = Prepare(FullPath, ArgumentTemplate, ArgumentValues);
            return Run(PSI);
        }

        public static ProcessStartInfo Prepare<T>(string FullPath, string ArgumentTemplate, T ArgumentValues) {
            var NewArguments = ArgumentTemplate.FormatToken(ArgumentValues);
            return Prepare(FullPath, NewArguments);
        }

        public static ProcessStartInfo Prepare(string FullPath, string Arguments) {
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

            return PSI;
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
