namespace Badger.Installer {
    public class RedirectorConfiguration : ConfigurationBase {
        public InstallerContentConfiguration Stub { get; set; } = new InstallerContentConfiguration();
        public SigningConfiguration SignUsing { get; set; } = new SigningConfiguration();

        protected override string DebuggerDisplay {
            get {
                return this.DebuggerDisplay();
            }
        }
    }

}
