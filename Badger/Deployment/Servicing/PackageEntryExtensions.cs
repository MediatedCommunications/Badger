using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Deployment.Servicing {
    public static class PackageEntryExtensions {

        public static PackageDefinitionFileName FileName(this PackageDefinition This) {
            return new PackageDefinitionFileName(This.FileName);
        }

    }
}
