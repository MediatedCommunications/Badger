using Badger.Common;
using StringTokenFormatter;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Badger.Default.Configuration {
    public static class ExternalToolConfigurationExtensions {

        public static bool Enabled(this ExternalToolConfiguration This) {
            return (This?.Enabled).TrueWhenNull() && (This?.FullPath).IsNotNullOrEmpty();
        }

        public static ProcessStartInfo Prepare<T>(this ExternalToolConfiguration This, T Parameters) {
            var ret = default(ProcessStartInfo);
            if (This.Enabled()) {
                if (Badger.Common.IO.File.Exists(This.FullPath)) {
                    var FileName = This.FullPath;

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


        public static bool Run<T>(this IEnumerable<ExternalToolConfiguration> This, T Parameters) {
            var ret = true;
            if(This is { } ) {
                foreach (var item in This) {
                    var tret = item.Run(Parameters);
                    ret &= tret;
                }
            }


            return ret;
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
