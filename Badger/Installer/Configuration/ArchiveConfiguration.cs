namespace Badger.Installer {

    public class ArchiveConfiguration : ConfigurationBase {
        public ExternalToolConfiguration CreateUsing { get; set; } = new ExternalToolConfiguration();
        public SigningConfiguration SignUsing { get; set; } = new SigningConfiguration();

        protected override string DebuggerDisplay {
            get {
                return CreateUsing.DebuggerDisplay();
            }
        }
    }

}
