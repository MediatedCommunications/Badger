using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Diagnostics {
    public static class Application {
        public static Assembly EntryAssembly { get; private set; }
        public static string Path { get; private set; }

        static Application() {
            {
                var ret = null
                    ?? System.Reflection.Assembly.GetEntryAssembly()
                    ?? new System.Diagnostics.StackTrace().GetFrames().Last().GetMethod().Module.Assembly
                    ;
                EntryAssembly = ret;
            }

            {
                Path = System.IO.Path.GetDirectoryName(EntryAssembly.Location);
            }

        }
    }
}
