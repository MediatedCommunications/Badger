using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Default.Packager.Utilities {
    public static class Utility {
        public static string AppPath {
            get {
                var ret = Badger.Common.Diagnostics.Application.Path;
                return ret;
            }
        }


        public static string RootPath {
            get {
                var ret = System.IO.Path.Combine(AppPath, "Utilities");
                return ret;
            }
        }

        public static string Path(params string[] RelativePath) {
            var ret = RootPath;
            foreach (var item in RelativePath) {
                ret = System.IO.Path.Combine(ret, item);
            }

            return ret;
        }

    }
}
