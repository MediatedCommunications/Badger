using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Default.Installer {
    public static class ConfigurationResource {

        public static string ResourceName => nameof(ConfigurationResource);

        public static Configuration Get() {
            var ret = default(Configuration);

            var R = new Resource(ResourceName);

            using (var Stream = R.GetStream()) {
                if (Stream is { }) {
                    ret = EasyBinarySerializer.Instance.From<Configuration>(Stream);
                }
            }
            return ret;
        }

    }
}
