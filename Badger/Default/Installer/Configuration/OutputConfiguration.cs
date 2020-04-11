namespace Badger.Default.Installer {
    public class OutputConfiguration : ConfigurationBase {
        public string Path_NameTemplate { get; set; }
        
        public string Installer_NameTemplate { get; set; }
        public string Releases_NameTemplate { get; set; }

        protected override string DebuggerDisplay {
            get {
                return this.DebuggerDisplay();
            }
        }
    }

    

}
