namespace Badger.Default.Installer {
    public class DefaultsConfiguration : ConfigurationBase, ISignUsing {
        public SigningConfiguration SignUsing { get; set; } = new SigningConfiguration();

        protected override string DebuggerDisplay {
            get {
                return SignUsing.DebuggerDisplay();
            }
        }
    }

}
