using System.IO;
using System.Reflection;

namespace Badger.Default.Installer {
    public class Resource {
        public string Name { get; private set; }
        public Assembly Assembly { get; private set; }


        public Resource(string Name) : this(Name, Badger.Common.Diagnostics.Application.EntryAssembly) { 
        
        }

        public Resource(string Name, Assembly Assembly) {
            this.Name = Name;
            this.Assembly = Assembly;
        }

        public Stream GetStream() {
            return Assembly.GetManifestResourceStream(Name);
        }

        public StreamReader GetReader() {
            return new StreamReader(GetStream());
        }

        public bool Exists() {
            var Info = Assembly.GetManifestResourceInfo(Name);
            var ret = Info is { };

            return ret;
        }

    }

}
