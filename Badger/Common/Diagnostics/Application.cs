using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Common.Diagnostics {
    public static class Application {
        public static Assembly EntryAssembly { get; private set; }
        public static string FolderPath { get; private set; }

        static Application() {
            {
                var ret = null
                    ?? System.Reflection.Assembly.GetEntryAssembly()
                    ?? new System.Diagnostics.StackTrace().GetFrames().Last().GetMethod().Module.Assembly
                    ;
                EntryAssembly = ret;
            }

            {
                FolderPath = System.IO.Path.GetDirectoryName(EntryAssembly.Location);
            }

        }
    }
}
