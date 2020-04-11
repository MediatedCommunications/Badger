using System.Collections.Generic;

namespace Badger.Default.Installer {
    public class InstallerContentPlaybookConfiguration : ConfigurationBase {
        public string ExtractContentToSubfolder { get; set; }
        public bool? DeleteOldVersions { get; set; }
        public bool? CloseOldVersions { get; set; }
        public ExternalToolConfiguration ExtractContent { get; set; } = new ExternalToolConfiguration();
        public List<ExternalToolConfiguration> Scripts { get; set; } = new List<ExternalToolConfiguration>();

        protected override string DebuggerDisplay {
            get {
                return this.DebuggerDisplay();
            }
        }
    }

}
