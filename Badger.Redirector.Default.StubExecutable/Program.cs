using System;
using System.Linq;
using System.Collections.Generic;

namespace Badger.Redirector.Default.StubExecutable {
    public static class Program {

        public static void Main(string[] args) {

            var FullPath = Badger.Diagnostics.Application.EntryAssembly.Location;
            var App = System.IO.Path.GetFileName(FullPath);
            var AppFolder = System.IO.Path.GetDirectoryName(FullPath);

            var Versions = Badger.VersionFolder.List(AppFolder);


            var FoundFile = (
                from x in Versions
                orderby x.Version descending
                let Path = GetPathToFile(x.FullPath, App)
                where !Path.IsNullOrEmpty()
                select Path
                ).FirstOrDefault();

            if (!FoundFile.IsNullOrEmpty()) {
                try {
                    System.Diagnostics.Process.Start(FoundFile);
                } catch {

                }
            }

        }

        private static string GetPathToFile(string Path, string File) {
            var ret = default(String);
            try {
                var Dest = System.IO.Path.Combine(Path, File);
                if (System.IO.File.Exists(Dest)) {
                    ret = Dest;
                }
            } catch {

            }

            return ret;
        }

    }
}
