using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger {
    public static class SpecialFoldersExtensions {

        public static string Path(this System.Environment.SpecialFolder This, params string[] SubFolders) {
            var ret = System.Environment.GetFolderPath(This);
            foreach (var item in SubFolders) {
                ret = System.IO.Path.Combine(ret, item);
            }

            return ret;
        }

    }
}
