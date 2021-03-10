using Badger.Common.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Common.Diagnostics {
    public static class Processes {
        public static void KillProcessesInPath(string Path) {
            static List<WmiProcess> AppsToKill(string Path) {
                var AllApps = Badger.Common.Management.WmiProcess.Enumerate();
                var tret = (
                    from x in AllApps
                    where !string.IsNullOrWhiteSpace(x.ExecutablePath)
                    where x.ExecutablePath.ToLower().StartsWith(Path.ToLower())
                    select x
                    ).ToList();

                return tret;
            }
            var MaxDelay = TimeSpan.FromSeconds(1);

            var ToKill = AppsToKill(Path);

            foreach (var item in ToKill) {
                try {
                    var Proc = System.Diagnostics.Process.GetProcessById(item.Handle);
                    Proc.CloseMainWindow();
                } catch {

                }
            }

            for(
                var SW = System.Diagnostics.Stopwatch.StartNew(); 
                SW.Elapsed < MaxDelay && ToKill.Count > 0; 
                ToKill = AppsToKill(Path)
            ) { }
            

            for (var SW = System.Diagnostics.Stopwatch.StartNew(); SW.Elapsed < MaxDelay && ToKill.Count > 0;) {
                foreach (var item in ToKill) {
                    try {
                        var Proc = System.Diagnostics.Process.GetProcessById(item.Handle);
                        Proc.Kill();
                    } catch {

                    }
                }

                ToKill = AppsToKill(Path);
                
            }
        }

    }
}
