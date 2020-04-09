namespace Badger.Installer {
    public class DefaultsConfiguration : ConfigurationBase {
        public SigningConfiguration SignUsing { get; set; } = new SigningConfiguration();

        protected override string DebuggerDisplay {
            get {
                return SignUsing.DebuggerDisplay();
            }
        }
    }

}
