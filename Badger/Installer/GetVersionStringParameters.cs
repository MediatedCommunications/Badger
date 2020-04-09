namespace Badger.Installer {
    public class GetVersionStringParameters {
        public string Source_File { get; set; }
        public string Key { get; set; }
    }

    public class SetVersionStringParameters {
        public string Dest_File { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class GetFileIconParameters {
        public string Source_File { get; set; }
        public string Dest_File { get; set; }
    }

    public class SetFileIconParameters {
        public string Source_File { get; set; }
        public string Dest_File { get; set; }
    }

}
