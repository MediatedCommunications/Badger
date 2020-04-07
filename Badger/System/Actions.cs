using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger {
    public static class Actions {
        public static bool Try(Action T) {
            var ret = false;

            try {
                T?.Invoke();
                ret = true;
            } catch {

            }

            return ret;
        }

    }
}
