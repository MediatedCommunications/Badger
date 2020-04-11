using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.IO;

namespace Badger.Default.Installer {
    public class EasyJsonSerializer : EasySerializer {
        public static EasyJsonSerializer Instance { get; private set; } = new EasyJsonSerializer();

        private JsonSerializer Serializer() {
            var Settings = new Newtonsoft.Json.JsonSerializerSettings() {
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter>() { 
                    new VersionConverter(),
                    new StringEnumConverter(),
                }
            };
            var S = Newtonsoft.Json.JsonSerializer.Create(Settings);

            return S;
        }


        public override T From<T>(Stream Content) {
            using (var SR = new StreamReader(Content)) {
                using (var JR = new JsonTextReader(SR)) {
                    return Serializer().Deserialize<T>(JR);
                }
            }
                
        }

        public override void ToStream<T>(T This, Stream Output) {

            using (var TW = new StreamWriter(Output)) {
                Serializer().Serialize(TW, This);
            }
        }
    }

}
