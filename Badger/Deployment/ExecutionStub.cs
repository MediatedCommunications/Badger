using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Deployment {
    public class ExecutionStub {
        public string Assembly { get; private set; }
        public string Stub { get; private set; }


        public static ExecutionStub Current { get; private set; } = From(Badger.Common.Diagnostics.Application.EntryAssembly);

        public static ExecutionStub From(string FileName) {
            var ret = default(ExecutionStub);

            var FN = System.IO.Path.GetFileName(FileName);
            if(Badger.Deployment.Location.Current?.InstallFolder is { } V2) {
                var Dest = System.IO.Path.Combine(V2, FN);
                if (System.IO.File.Exists(Dest)) {
                    ret = new ExecutionStub() {
                        Assembly = FileName,
                        Stub = Dest,
                    };
                }
            }


            return ret;
        }

        public static ExecutionStub From(Assembly Asm) {
            return From(Asm.Location);
        }

    }
}
