using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Badger.Installer.Default {







    public static class ConfigurationExtensions {




        public static bool Validate(this PackagerConfiguration This, out List<Exception> Errors) {
            Errors = new List<Exception>();

            /*
            if (This.Installer.Product .Package_Name.IsNullOrEmpty()) {
                Errors.Add(new ArgumentNullException(nameof(This.Package_Name)));
            }

            if (This.Pack .Package_Source.IsNullOrEmpty()) {
                Errors.Add(new ArgumentNullException(nameof(This.Package_Source)));
            }

            if (This.Package_Version == null) {
                Errors.Add(new ArgumentNullException(nameof(This.Package_Version)));
            }

            if (!System.IO.Directory.Exists(This.Package_Source)) {
                Errors.Add(new System.IO.DirectoryNotFoundException($@"The folder specified for {nameof(This.Package_Source)} ({This.Package_Source}) does not exist"));
            }
            */

            var ret = Errors.Count == 0;

            return ret;
        }

    }

}
