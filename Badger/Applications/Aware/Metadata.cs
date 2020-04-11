using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;



namespace Badger.Applications.Aware {
    /// <summary>
    /// This class contains the metadata attribute and value that you should apply to your main EXE assemblies that are aware.
    /// It should be applied by using:
    /// [assembly: AssemblyMetadata(Metadata.AssemblyMetadataAttributeKey, Metadata.AssemblyMetadataAttributeValue)]
    /// </summary>
    public static class Metadata {
        public const string AssemblyMetadataAttributeKey = "SquirrelAwareVersion";
        public const string AssemblyMetadataAttributeValue = "1";
    }
}
