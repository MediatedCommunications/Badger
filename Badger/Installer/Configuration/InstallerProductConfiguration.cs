using System;

namespace Badger.Installer {
    public class ProductConfiguration : ConfigurationBase {
        public string Name { get; set; }
        public Version Version { get; set; }
        public string Version_FromFile { get; set; }

        protected override string DebuggerDisplay {
            get {
                return this.DebuggerDisplay();
            }
        }
    }

}
