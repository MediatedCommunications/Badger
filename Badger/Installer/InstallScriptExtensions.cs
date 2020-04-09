using StringTokenFormatter;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Badger.Installer {
    public static class InstallScriptExtensions {

        public static bool Run(this IEnumerable<ExternalToolConfiguration> This, string VersionFolder, Version PreviousVersion, Version CurrentVersion) {
            var ret = true;

            if (This is { }) {
                var Parameters = new InstallScriptParameters() {
                    FromVersion = PreviousVersion,
                    ToVersion = CurrentVersion,
                };

                foreach (var item in This) {
                    var tret = item.Run(VersionFolder, Parameters);
                    ret &= tret;
                }
            }


            return ret;
        }

        public static bool Run(this ExternalToolConfiguration This, string VersionFolder, InstallScriptParameters Parameters) {
            var NewConfig = new ExternalToolConfiguration() {
                Enabled = This.Enabled,
                Application = System.IO.Path.Combine(VersionFolder, This.Application),
                ArgumentTemplate = This.ArgumentTemplate,
                Async = This.Async,
                Visible = This.Visible,
            };

            return NewConfig.Run(Parameters);

        }

    }
    

}
