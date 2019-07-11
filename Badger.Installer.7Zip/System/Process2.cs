using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

using StringTokenFormatter;

namespace System {
    internal static class Process2 {

        public static Process Start(this ProcessStartInfo This) {
            return System.Diagnostics.Process.Start(This);
        }

        public static void Start(string Command, string Parameters = null, object ParameterVariables = null) {
            var PSI = Configure(Command, Parameters, ParameterVariables);
            var Proc = PSI.Start();
            Proc.WaitForExit();
        }

        public static ProcessStartInfo Configure(string Command, string Parameters = null, object ParameterVariables = null) {
            return new ProcessStartInfo().Configure(Command, Parameters, ParameterVariables);
        }

        public static ProcessStartInfo Configure(this ProcessStartInfo This, string Command, string Parameters = null , object ParameterVariables = null) {
            Parameters = Parameters ?? "";
            if(ParameterVariables != null) {
                Parameters = Parameters.FormatToken(ParameterVariables);
            }

            This.FileName = Command;
            This.Arguments = Parameters;

            return This;            
        }
    }
}
