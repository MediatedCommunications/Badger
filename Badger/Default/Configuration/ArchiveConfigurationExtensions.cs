namespace Badger.Default.Configuration {
    public static class ArchiveConfigurationExtensions {

        public static bool Create(this ArchiveConfiguration This, string Source_Folder, string Dest_File) {
            var args = new CreateArchiveParameters() {
                Source_Folder = Source_Folder,
                Dest_File = Dest_File
            };

            return This.Create(args);
        }

        public static bool Create(this ArchiveConfiguration This, CreateArchiveParameters Args) {
            return This.CreateUsing.Run(Args);
        }
    }

}
