using System.IO;

namespace Badger.Default.Installer {
    public abstract class EasySerializer {

        public abstract void ToStream<T>(T This, Stream Output);
        public abstract T From<T>(Stream Content);


        public T From<T>(byte[] Content) {
            var MS = new MemoryStream(Content);
            return From<T>(MS);
        }

        public byte[] ToByte<T>(T This) {

            var MS = new MemoryStream();
            ToStream(This, MS);
            MS.Position = 0;

            var ret = new byte[MS.Length];
            MS.Read(ret, 0, ret.Length);

            return ret;
        }

        public T FromFile<T>(string FileName) {
            using (var S = System.IO.File.OpenRead(FileName)) {
                return From<T>(S);
            }
        }

        public void ToFile<T>(T This, string FileName) {
            using (var S = System.IO.File.Create(FileName)) {
                ToStream(This, S);
            }
        }


    }

}
