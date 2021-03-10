namespace Badger.Default.Configuration {
    public class DefaultsConfiguration : ConfigurationBase, ISignUsing {
        public SigningConfiguration SignUsing { get; set; } = new SigningConfiguration();

        protected override string GetDebuggerDisplay() {
                return SignUsing.DebuggerDisplay();
        }
    }

}
