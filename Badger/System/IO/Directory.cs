using Badger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.IO {
    public class Directory {

        public static bool Create(string Directory) => Create(Directory, out _);

        public static bool Create(string Directory, out Exception ex) {
            ex = default;

            var ret = true;
            if (!Exists(Directory)) {
                ret &= Actions.Try(() => System.IO.Directory.CreateDirectory(Directory), out ex);
            }
            return ret;
        }

        public static bool Exists(string Directory) {
            return System.IO.Directory.Exists(Directory);
        }

        public static bool Delete(string Path) {
            return Delete(Path, true);
        }

        public static bool Delete(string Path, bool Recursive) {
            return Delete(Path, Recursive, out _);
        }

        public static bool Delete(string Path, out Exception ex) {
            return Delete(Path, true, out ex);
        }

        public static bool Delete(string Path, bool Recursive, out Exception ex) {
            ex = default;

            var ret = true;

            if (System.IO.Directory.Exists(Path)) {

                if (Recursive) {
                    foreach (var item in System.IO.Directory.GetFiles(Path)) {
                        var tret = Actions.Try(() => System.IO.File.Delete(item), out var exx);
                        ret &= tret;
                        ex ??= exx;
                    }

                    foreach (var item in System.IO.Directory.GetDirectories(Path)) {
                        var tret = Delete(item, Recursive, out var exx);
                        ret &= tret;
                        ex ??= exx;
                    }
                }
                {
                    var tret = Actions.Try(() => System.IO.Directory.Delete(Path), out var exx);
                    ret &= tret;
                    ex ??= exx;
                }
            }

            return ret;
        }

        public static bool IsWritable(string Path) {
            var ret = false;
            if (Create(Path)) {

                ret = Actions.Try(() => {
                    var FN = System.IO.Path.Combine(Path, Guid.NewGuid().ToString());
                    using (var FS = System.IO.File.Create(FN)) {

                    }

                    System.IO.File.Delete(FN);
                });
            }

            return ret;
        }

        public static bool EnsureExists(string path) {
            var ret = false;

            try {
                if (System.IO.Directory.Exists(path)) {
                    ret = true;
                } else {
                    System.IO.Directory.CreateDirectory(path);
                    ret = true;
                }
            } catch (Exception ex) {
                ex.Ignore();
            }

            return ret;
        }

        public static bool Copy(string SourcePath, string DestPath, out Exception ex) {
            return Copy(SourcePath, DestPath, true, true, out ex);
        }

        public static bool Copy(string SourcePath, string DestPath, bool Overwrite, bool Recursive, out Exception ex) {
            ex = default(Exception);

            var ret = true;

            if (Exists(SourcePath)) {
                {
                    var Files = System.IO.Directory.EnumerateFiles(SourcePath);
                    foreach (var FileName in Files) {
                        var SourceName = System.IO.Path.GetFileName(FileName);
                        var Dest = System.IO.Path.Combine(DestPath, SourceName);

                        var tret = File.Copy(FileName, Dest, Overwrite, out var exx);
                        ret &= tret;
                        ex ??= exx;
                    }
                }

                {
                    var Folders = System.IO.Directory.EnumerateDirectories(SourcePath);
                    foreach (var Folder in Folders) {
                        var SourceName = System.IO.Path.GetFileName(Folder);
                        var Dest = System.IO.Path.Combine(DestPath, SourceName);
                        {
                            var tret = Create(Dest);
                            ret &= tret;
                        }

                        if (Recursive) {
                            var tret = Copy(Folder, Dest, Overwrite, Recursive, out var exx);
                            ret &= tret;
                            ex ??= exx;
                        }
                    }
                }

            }

            return ret;
        }

    }
}
