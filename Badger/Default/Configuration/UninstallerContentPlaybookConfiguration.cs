using System.Collections.Generic;

namespace Badger.Default.Configuration {
    public class UninstallerContentPlaybookConfiguration : ConfigurationBase {
        public bool? DeleteOldVersions { get; set; }
        public bool? CloseOldVersions { get; set; }
        public List<ExternalToolConfiguration> Scripts { get; set; } = new List<ExternalToolConfiguration>();

        protected override string GetDebuggerDisplay() {
                return this.DebuggerDisplay();
        }
    }

}
