using Badger.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger {

    [DebuggerDisplay(DebugView.Default)]
    public class Package : IDisposable {
        protected string DebuggerDisplay => FileName;

        public string FileName { get; private set; }
        public bool DeleteOnDispose { get; private set; }

        public static Package FromPath(string Path, string Extension, bool DeleteOnDispose = true) { 

            if (!Extension.StartsWith(".")) {
                Extension = "." + Extension;
            }

            var FN = System.IO.Path.Combine(Path, $@"{Guid.NewGuid()}{Extension}");

            var ret = new Package() {
                FileName = FN,
                DeleteOnDispose = DeleteOnDispose
            };

            return ret;
        }

        public static Package FromFile(string FullPath, bool DeleteOnDispose = false) {
            var ret = new Package() {
                FileName = FullPath,
                DeleteOnDispose = DeleteOnDispose
            };

            return ret;
        }


        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool Disposed;
        protected virtual void Dispose(bool Disposing) {
            if (!this.Disposed) {
                try {
                    if (System.IO.File.Exists(FileName)) {
                        System.IO.File.Delete(FileName);
                    }
                } catch (Exception ex) {
                    ex.Ignore();
                }
                Disposed = true;
            }
        }

        ~Package() {
            Dispose(false);
        }

    }
}
