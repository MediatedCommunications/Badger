using StringTokenFormatter;
using System;
using System.Diagnostics;

namespace Badger.Installer {
    public static class ExternalToolConfigurationExtensions {

        public static ProcessStartInfo Prepare<T>(this ExternalToolConfiguration This, T Parameters) {
            var ret = default(ProcessStartInfo);
            if (This?.Enabled == true) {
                if (Badger.IO.File.Exists(This.Application)) {
                    var FileName = This.Application;

                    ret = new ProcessStartInfo() {
                        FileName = FileName,
                        Arguments = This.ArgumentTemplate.FormatToken(Parameters),

                        ErrorDialog = false,
                        UseShellExecute = false,
                    };

                    if (This.Visible != true) {
                        ret.CreateNoWindow = true;
                        ret.WindowStyle = ProcessWindowStyle.Hidden;
                    }
                }
            }


            return ret;
        }

        public static Process RunProcess<T>(this ExternalToolConfiguration This, T Parameters) {
            return This.Run(Parameters, Proc => Proc);
        }


        public static bool Run<T>(this ExternalToolConfiguration This, T Parameters) {
            return This.Run(Parameters, Proc => {
                var tret = false;
                if (Proc is { }) {
                    tret = true;
                    if (This.Async != true) { 
                        Proc.WaitForExit();
                    }
                }

                return tret;
            });
        }

        public static ProcessOutputResult RunOutput<T>(this ExternalToolConfiguration This, T Parameters) {
            return This.Run(Parameters, PSI => {
                PSI.RedirectStandardError = true;
                PSI.RedirectStandardOutput = true;

            }, Proc => {
                Proc.WaitForExit();
                var ret = new ProcessOutputResult() {
                    ExitCode = Proc.ExitCode,
                    Error = Proc.StandardError.ReadToEnd(),
                    Output = Proc.StandardOutput.ReadToEnd(),
                };


                return ret;

            });
        }


        public static TValue Run<T, TValue>(this ExternalToolConfiguration This, T Parameters, Func<Process, TValue> Result) {
            return This.Run(Parameters, default, Result);
        }

        public static TValue Run<T, TValue>(this ExternalToolConfiguration This, T Parameters, Action<ProcessStartInfo> Configure, Func<Process, TValue> Result) {
            var ret = default(TValue);

            if (This.Prepare(Parameters) is { } PSI) {
                Configure?.Invoke(PSI);

                Actions.Try(() => {
                    var Proc = System.Diagnostics.Process.Start(PSI);
                    ret = Result(Proc);
                });

            }

            return ret;
        }

    }

}
