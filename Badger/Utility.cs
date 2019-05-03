using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.CompilerServices;

namespace Badger {
    public static class Utility {
        public static void Ignore(this Exception ex) {
            ex.Equals(ex);
        }


        public static ConfiguredTaskAwaitable DefaultAwait(this Task This) {
            return This.RunOnAnyThread();
        }

        public static ConfiguredTaskAwaitable<T> DefaultAwait<T>(this Task<T> This) {
            return This.RunOnAnyThread();
        }

        private const bool __RunOnAnyThread = false;
        public static ConfiguredTaskAwaitable RunOnAnyThread(this Task This) {
            return This.ConfigureAwait(__RunOnAnyThread);
        }

        public static ConfiguredTaskAwaitable<T> RunOnAnyThread<T>(this Task<T> This) {
            return This.ConfigureAwait(__RunOnAnyThread);
        }

    }
}
