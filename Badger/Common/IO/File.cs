using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Common.IO {
    public static class File {
        public static bool Copy(string Source, string Destination, bool Overwrite, out Exception ex) {
            return Actions.Try(() => System.IO.File.Copy(Source, Destination, Overwrite), out ex);
        }

        public static bool Replace(string Source, string Destination, out Exception ex) {
            return Copy(Source, Destination, true, out ex);
        }
        public static bool Delete(string Location, out Exception ex) {
            return Actions.Try(() => {

                if (Exists(Location)) {
                    System.IO.File.Delete(Location) ;
                }

            }, out ex);
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
