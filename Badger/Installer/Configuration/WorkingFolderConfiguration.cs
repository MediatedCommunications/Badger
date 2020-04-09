namespace Badger.Installer {
    public class WorkingFolderConfiguration : ConfigurationBase {
        public string PathRoot { get; set; }
        public string PathTemplate { get; set; }
        public bool? Delete_OnStart { get; set; }
        public bool? Delete_OnFinish { get; set; }

        protected override string DebuggerDisplay {
            get {
                return this.DebuggerDisplay();
            }
        }
    }

}
