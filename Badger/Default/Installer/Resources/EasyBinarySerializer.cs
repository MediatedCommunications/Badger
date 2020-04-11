using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Badger.Default.Installer {
    public class EasyBinarySerializer : EasySerializer {
        public static EasyBinarySerializer Instance { get; private set; } = new EasyBinarySerializer();

        public override void ToStream<T>(T This, Stream Output) {
            var S = new BinaryFormatter();
            S.Serialize(Output, This);
        }

        public override T From<T>(Stream Content) {
            var S = new BinaryFormatter();
            var ret = default(T);
            if (S.Deserialize(Content) is T V1) {
                ret = V1;
            }

            return ret;
        }
    }

}
