using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Badger.Installer {
    public static class ResourceSerializer {
        public static byte[] ToByte<T>(T This) {
            var S = new BinaryFormatter();

            var MS = new MemoryStream();
            S.Serialize(MS, This);
            MS.Position = 0;

            var ret = new byte[MS.Length];
            MS.Read(ret, 0, ret.Length);

            return ret;
        }

        public static T From<T>(byte[] Content) {
            var MS = new MemoryStream(Content);
            return From<T>(MS);
        }

        public static T From<T>(Stream Content) {
            var S = new BinaryFormatter();
            var ret = default(T);
            if (S.Deserialize(Content) is T V1) {
                ret = V1;
            }

            return ret;
        }

    }

}
