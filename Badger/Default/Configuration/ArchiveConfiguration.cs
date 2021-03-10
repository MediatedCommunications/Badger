namespace Badger.Default.Configuration {

    public class ArchiveConfiguration : ConfigurationBase, ISignUsing {
        public ExternalToolConfiguration CreateUsing { get; set; } = new ExternalToolConfiguration();
        public SigningConfiguration SignUsing { get; set; } = new SigningConfiguration();

        protected override string GetDebuggerDisplay() {
                return CreateUsing.DebuggerDisplay();
        }
    }

}
