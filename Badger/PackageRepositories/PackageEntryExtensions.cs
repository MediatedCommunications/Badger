using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger {
    public static class PackageEntryExtensions {

        public static PackageEntryFileName FileName(this PackageEntry This) {
            return new PackageEntryFileName(This.FileName);
        }

    }
}
