using System;

namespace Badger.Default.Configuration {
    public class ProductConfiguration : ConfigurationBase {
        public string Name { get; set; }
        public string Publisher { get; set; }
        public string Code { get; set; }

        public Version Version { get; set; }
        public string Version_FromFile { get; set; }

        protected override string GetDebuggerDisplay() {
                return this.DebuggerDisplay();
        }
    }

}
