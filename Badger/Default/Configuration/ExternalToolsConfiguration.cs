using System.Collections.Generic;

namespace Badger.Default.Configuration {
    public class ExternalToolsConfiguration : ConfigurationBase {
        public VersionStringExternalToolConfiguration VersionString { get; set; }
        public IconExternalToolConfiguration Icon { get; set; }

        protected override string GetDebuggerDisplay() {
                return this.DebuggerDisplay();
        }
    }

    public class VersionStringExternalToolConfiguration : ConfigurationBase {
        public ExternalToolConfiguration Get { get; set; }
        public ExternalToolConfiguration Set { get; set; }

        public List<string> Values { get; set; }

        protected override string GetDebuggerDisplay() {
                return this.DebuggerDisplay();
        }
    }

    public class IconExternalToolConfiguration : ConfigurationBase {
        public ExternalToolConfiguration Get { get; set; }
        public ExternalToolConfiguration Set { get; set; }

        protected override string GetDebuggerDisplay() {
                return this.DebuggerDisplay();
        }
    }

}
