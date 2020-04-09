namespace Badger.Installer {
    public class InstallerContentConfiguration : ConfigurationBase {
        public string Source { get; set; }
        public bool? Include { get; set; }

        protected override string DebuggerDisplay {
            get {
                return this.DebuggerDisplay();
            }
        }
    }

}
