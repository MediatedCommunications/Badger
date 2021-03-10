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


        public static T Try<T>(Func<T> F) {
            return Try(F, default);
        }

        public static T Try<T>(Func<T> F, T Default) {
            Try(F, Default, out var ret, out _);
            return ret;
        }

        public static bool Try<T>(Func<T> F, out T Value, out Exception ex) {
            return Try(F, default(T), out Value, out ex);
        }

        public static bool Try<T>(Func<T> F, T Default, out T Value, out Exception ex) {
            var ret = false;
            Value = Default;
            ex = default;

            try {
                Value = F.Invoke();
                ret = true;
            } catch (Exception exx) {
                ex = exx;
            }


            return ret;
        }

    }
}
