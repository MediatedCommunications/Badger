using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Default.Installer.StubExecutable {
    public static class References {
        public static void Ensure() {
            var Name = typeof(WpfAnimatedGif.ImageBehavior).Name;
        }
    }
}
