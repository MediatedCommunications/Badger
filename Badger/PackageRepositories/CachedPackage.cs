using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCompress;
using SharpCompress.Readers;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using System.IO;

namespace Badger.PackageRepositories {

    public class CachedPackage : IDisposable {

        public static CachedPackage Create(Stream Source, string ExpectedSha1) {
            var TempFileName = Path.GetTempFileName();
            using (var FS = File.OpenWrite(TempFileName)) {
                Source.CopyTo(FS);
            }
            Source.Close();

            var ActualSha1 = Badger.Security.SHA1.FromFile(TempFileName);

            Badger.Security.SHA1.Assert(ActualSha1, ExpectedSha1);


            return new CachedPackage(TempFileName);
        }

        public string CacheFile { get; private set; }

        private CachedPackage(string CacheFile) {
            this.CacheFile = CacheFile;
        }

        ~CachedPackage() {
            Dispose(false);
        }

        private bool Disposed = false;
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool Disposing) {
            if (!Disposed) {
                try {
                    if (File.Exists(CacheFile)) {
                        File.Delete(CacheFile);
                    }
                } catch (Exception ex) {
                    ex.Ignore();
                }

                Disposed = true;
            }
        }


        public void ExtractTo(string Folder) {
            var Options = new ExtractionOptions() {
                Overwrite = true,
                PreserveFileTime = true,
                PreserveAttributes = true,
                ExtractFullPath = true,
            };

            using (var SA = ZipArchive.Open(CacheFile)) {
                using (var reader = SA.ExtractAllEntries()) {

                    reader.WriteAllToDirectory(Folder, Options);

                }
            }
        }

    }
}
