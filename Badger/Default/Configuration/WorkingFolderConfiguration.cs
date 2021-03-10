namespace Badger.Default.Configuration {
    public class WorkingFolderConfiguration : ConfigurationBase {
        public string PathRoot { get; set; }
        public string PathTemplate { get; set; }
        public bool? Delete_OnStart { get; set; }
        public bool? Delete_OnFinish { get; set; }

        protected override string GetDebuggerDisplay() {
                return this.DebuggerDisplay();
        }
    }

}
