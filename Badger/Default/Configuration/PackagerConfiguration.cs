namespace Badger.Default.Configuration {
    public class PackagerConfiguration : ConfigurationBase {
        public ProductConfiguration Product { get; set; } = new ProductConfiguration();

        public ExternalToolsConfiguration ExternalTools { get; set; }

        public DefaultsConfiguration Defaults { get; set; }

        public InstallerConfiguration Installer { get; set; }
        public UninstallerConfiguration Uninstaller { get; set; }

        public RedirectorConfiguration Redirector { get; set; }


        public ArchiveConfiguration Archive { get; set; }

        public OutputConfiguration Output {get; set;}

        public WorkingFolderConfiguration WorkingFolder { get; set; }

        protected override string GetDebuggerDisplay() {
                return this.DebuggerDisplay();
        }
    }

    

}
