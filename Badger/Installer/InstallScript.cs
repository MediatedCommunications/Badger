using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Badger.Installer {
    [Serializable]
    public class InstallScript {
        public string FileName { get; set; }
        public bool RunHidden { get; set; }
        public bool RunAsync { get; set; }
        public bool IncludeParameters { get; set; }
    }

    public static class InstallScriptExtensions {

        public static bool Run(this IEnumerable<InstallScript> This, string VersionFolder) {
            var ret = true;

            if (This is { }) {
                foreach (var item in This) {
                    var tret = item.Run(VersionFolder);
                    ret &= tret;
                }
            }


            return ret;
        }

        public static bool Run(this InstallScript This, string VersionFolder) {
            var ret = false;

            var FileName = System.IO.Path.Combine(VersionFolder, This.FileName);
            if (Badger.IO.File.Exists(FileName)) {

                var PSI = new ProcessStartInfo() {
                    FileName = FileName,
                };

                if (This.RunHidden) {
                    PSI.CreateNoWindow = true;
                    PSI.WindowStyle = ProcessWindowStyle.Hidden;
                }

                ret = Actions.Try(() => {
                    var Proc = System.Diagnostics.Process.Start(PSI);
                    if (!This.RunAsync) {
                        Proc.WaitForExit();
                    }
                });

            }


            return ret;
        }

    }

    public class InstallScriptParameters {
        public Version FromVersion { get; set; }
        public Version ToVersion { get; set; }
    }
    

}
