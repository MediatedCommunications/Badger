using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Common {
    public static class Actions {

        public static bool Try(Action T) {
            return Try(T, out _ );
        }

        public static bool Try(Action T, out Exception ex) {
            ex = default;

            var ret = false;

            try {
                T?.Invoke();
                ret = true;
            } catch(Exception exx) {
                ex = exx;
            }

            return ret;
        }

    }
}
