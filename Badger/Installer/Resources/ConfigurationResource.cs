using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Installer {
    public static class ConfigurationResource {

        public static string ResourceName => nameof(ConfigurationResource);

        public static Configuration Get() {
            var ret = default(Configuration);

            var R = new Resource(ResourceName);

            using (var Stream = R.GetStream()) {
                if (Stream is { }) {
                    ret = ResourceSerializer.From<Configuration>(Stream);
                }
            }
            return ret;
        }

    }
}
