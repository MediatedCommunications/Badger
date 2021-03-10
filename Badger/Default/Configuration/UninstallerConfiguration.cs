namespace Badger.Default.Configuration {
    public class UninstallerConfiguration : ConfigurationBase, ISignUsing {
        public InstallerContentConfiguration Stub { get; set; } = new InstallerContentConfiguration();
        public InstallerContentConfiguration Icon { get; set; } = new InstallerContentConfiguration();

        public UninstallerContentPlaybookConfiguration Playbook { get; set; } = new UninstallerContentPlaybookConfiguration();

        public SigningConfiguration SignUsing { get; set; } = new SigningConfiguration();

        protected override string GetDebuggerDisplay() {
                return this.DebuggerDisplay();
        }
    }

}
