using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.IO {
    public static class File {
        public static bool Copy(string Source, string Destination, bool Overwrite) {
            return Actions.Try(() => System.IO.File.Copy(Source, Destination, Overwrite));
        }

        public static bool Replace(string Source, string Destination) {
            return Copy(Source, Destination, true);
        }

        public static bool Exists(string Source) {
            var ret = false;

            try {
                ret = System.IO.File.Exists(Source);
            } catch { 
            
            }


            return ret;
            
        }

    }
}
