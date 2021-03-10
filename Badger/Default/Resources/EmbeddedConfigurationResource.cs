using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Badger.Common;
using Badger.Common.Serialization;

namespace Badger.Default.Resources {
    public static class EmbeddedConfigurationResource<T> {

        public static string ResourceName => typeof(T).GetFriendlyName();

        public static T Get() {
            var ret = default(T);

            var R = new Resource(ResourceName);

            using (var Stream = R.GetStream()) {
                if (Stream is { }) {
                    ret = EasyBinarySerializer.Instance.From<T>(Stream);
                }
            }
            return ret;
        }

    }
}
