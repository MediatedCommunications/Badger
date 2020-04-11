namespace Badger.Deployment.Servicing {
    public class PackageDefinition {
        public string SHA1 { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public int? StagingPercentage { get; set; }
    }
}
