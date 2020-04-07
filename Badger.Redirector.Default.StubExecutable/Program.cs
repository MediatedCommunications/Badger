using System;
using System.Linq;
using System.Collections.Generic;

namespace Badger.Redirector.Default.StubExecutable {
    public static class Program {

        public static void Main(string[] args) {

            var FullPath = Badger.Diagnostics.Application.EntryAssembly.Location;
            var App = System.IO.Path.GetFileName(FullPath);
            var AppFolder = System.IO.Path.GetDirectoryName(FullPath);

            var Folders = new Dictionary<Version, string>();
            foreach (var Folder in System.IO.Directory.GetDirectories(AppFolder)) {
                if (Badger.EnvironmentHelpers.IsVersionFolder(Folder, out var V)) {
                    Folders[V] = Folder;
                }
            }

            var FoundFile = (
                from x in Folders
                orderby x.Key descending
                let Path = GetPathToFile(x.Value, App)
                where !String.IsNullOrEmpty(Path)
                select Path
                ).FirstOrDefault();

            if (!String.IsNullOrEmpty(FoundFile)) {
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
