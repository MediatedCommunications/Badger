using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Security {
    public static class SHA1 {

        public static string FromFile(string filePath) {
            using (var stream = File.OpenRead(filePath)) {
                return FromStream(stream);
            }
        }

        public static string FromStream(Stream file) {
            using (var sha1 = System.Security.Cryptography.SHA1.Create()) {
                return BitConverter.ToString(sha1.ComputeHash(file)).Replace("-", String.Empty);
            }
        }


    }

}
