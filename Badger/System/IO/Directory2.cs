using Badger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO {
    internal class Directory2 {

        public static bool EnsureWritable(string Path) {
            var ret = false;
            if (EnsureExists(Path)) {
                try {
                    var FN = System.IO.Path.Combine(Path, Guid.NewGuid().ToString());
                    using (var FS = System.IO.File.Create(FN)) {

                    }

                    System.IO.File.Delete(FN);

                } catch (Exception ex) {
                    ex.Ignore();
                }
            }

            return ret;
        }

        public static bool EnsureExists(string path) {
            var ret = false;

            try {
                if (System.IO.Directory.Exists(path)) {
                    ret = true;
                } else {
                    System.IO.Directory.CreateDirectory(path);
                    ret = true;
                }
            } catch (Exception ex) {
                ex.Ignore();
            }

            return ret;
        }

    }
}
