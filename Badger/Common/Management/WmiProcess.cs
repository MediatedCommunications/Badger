using Badger.Common.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Common.Management {

    [DebuggerDisplay(DebugView.Default)]
    public class WmiProcess {
        public string ExecutablePath { get; private set; }
        public int Handle { get; private set; }
        public string Name { get; private set; }
        public string CommandLine { get; private set; }

        protected virtual string DebuggerDisplay {
            get {
                return $@"[{Handle}] {Name} => {CommandLine}";
            }
        }

        public static List<WmiProcess> Enumerate() {
            var ret = Query.Invoke(@"root\cimv2", "select * from win32_process", x => new WmiProcess() {
                CommandLine = x["CommandLine"]?.ToString(),
                ExecutablePath = x["ExecutablePath"]?.ToString(),
                Handle = int.Parse(x["Handle"]?.ToString() ??"0"),
                Name = x["Name"]?.ToString()
            });

            return ret;
        }

    }

}
