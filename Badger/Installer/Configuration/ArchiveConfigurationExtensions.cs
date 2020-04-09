namespace Badger.Installer {
    public static class ArchiveConfigurationExtensions {

        public static bool CreateArchive(this ExternalToolConfiguration This, string Source_Folder, string Dest_File) {
            var args = new CreateArchiveParameters() {
                Source_Folder = Source_Folder,
                Dest_File = Dest_File
            };

            return This.Create(args);
        }

        public static bool Create(this ExternalToolConfiguration This, CreateArchiveParameters Args) {
            return This.Run(Args);
        }
    }

}
