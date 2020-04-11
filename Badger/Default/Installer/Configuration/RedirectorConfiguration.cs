namespace Badger.Default.Installer {
    public class RedirectorConfiguration : ConfigurationBase, ISignUsing {
        public InstallerContentConfiguration Stub { get; set; } = new InstallerContentConfiguration();
        public SigningConfiguration SignUsing { get; set; } = new SigningConfiguration();

        protected override string DebuggerDisplay {
            get {
                return this.DebuggerDisplay();
            }
        }
    }

}
