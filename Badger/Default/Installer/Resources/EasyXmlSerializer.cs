using System.IO;
using System.Xml.Serialization;

namespace Badger.Default.Installer {



    public class EasyXmlSerializer : EasySerializer {
        public static EasyXmlSerializer Instance { get; private set; } = new EasyXmlSerializer();


        private XmlSerializerNamespaces Namespaces() {
            var ret = new XmlSerializerNamespaces();
            ret.Add("", "");

            return ret;
        }

        private XmlSerializer Serializer<T>() {
            var ret = new XmlSerializer(typeof(T),"");
            return ret;
        }
        
        public override T From<T>(Stream Content) {
            var ret = default(T);

            var S = Serializer<T>();

            if (S.Deserialize(Content) is T V1){
                ret = V1;
            }

            return ret;
        }

        public override void ToStream<T>(T This, Stream Output) {
            var S = Serializer<T>();
            S.Serialize(Output, This, Namespaces());
        }

    }

}
