using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System {
    internal static class Directory2 {

        public static void Delete(string Directory, bool Recursive = true) {
            if (Exists(Directory)) {
                System.IO.Directory.Delete(Directory, Recursive);
            }
        }

        public static void Create(string Directory) {
            if (!Exists(Directory)) {
                System.IO.Directory.CreateDirectory(Directory);
            }
        }

        public static bool Exists(string Directory) {
            return System.IO.Directory.Exists(Directory);
        }

        public static void Copy(string SourcePath, string DestPath, bool Overwrite = true, bool Recursive = true) {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(SourcePath);

            if (Exists(SourcePath)) {
                {
                    var Files = System.IO.Directory.EnumerateFiles(SourcePath);
                    foreach (var File in Files) {
                        var SourceName = System.IO.Path.GetFileName(File);
                        var Dest = System.IO.Path.Combine(DestPath, SourceName);

                        System.IO.File.Copy(File, Dest, Overwrite);
                    }
                }

                {
                    var Folders = System.IO.Directory.EnumerateDirectories(SourcePath);
                    foreach (var Folder in Folders) {
                        var SourceName = System.IO.Path.GetFileName(Folder);
                        var Dest = System.IO.Path.Combine(DestPath, SourceName);
                        Create(Dest);

                        if (Recursive) {
                            Copy(Folder, Dest, Overwrite, Recursive);
                        }
                    }
                }

            }
        }

    }
}
