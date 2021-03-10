namespace Badger.Default.Configuration {
    public static class IconExtensions {

        public static bool GetIcon(this ExternalToolConfiguration This, string Source, string Destination) {
            var Args = new GetFileIconParameters() {
                Source_File = Source,
                Dest_File = Destination
            };

            return This.GetIcon(Args);
        }


        public static bool GetIcon(this ExternalToolConfiguration This, GetFileIconParameters Args) {
            return This.Run(Args);
        }

        public static bool SetIcon(this ExternalToolConfiguration This, string Source, string Destination) {
            var Args = new SetFileIconParameters() {
                Source_File = Source,
                Dest_File = Destination
            };

            return This.SetIcon(Args);
        }

        public static bool SetIcon(this ExternalToolConfiguration This, SetFileIconParameters Args) {
            return This.Run(Args);
        }

    }

}
