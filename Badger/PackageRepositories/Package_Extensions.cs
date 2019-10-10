using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger {
    public static class Package_Extensions {

        public static string SHA1(this Package This) {
            var ret = "";

            try {
                if (System.IO.File.Exists(This.FileName)) {
                    using (var FS = System.IO.File.OpenRead(This.FileName)) 
                    using (var sha1 = System.Security.Cryptography.SHA1.Create()) {
                        ret = BitConverter.ToString(sha1.ComputeHash(FS)).Replace("-", String.Empty);
                    }
                }
            } catch (Exception ex) {
                ex.Ignore();
            }

            return ret;
        }

        public static Process Execute(this Package This) {
            var ret = System.Diagnostics.Process.Start(This.FileName);

            return ret;
        }

    }
}
