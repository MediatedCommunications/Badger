using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Common {
    public static class TypeExtensions {
            public static string GetFriendlyName(this Type type) {
                var friendlyName = type.Name;
                if (!type.IsGenericType) return friendlyName;

                var iBacktick = friendlyName.IndexOf('`');
                if (iBacktick > 0) friendlyName = friendlyName.Remove(iBacktick);

                var genericParameters = type.GetGenericArguments().Select(x => x.GetFriendlyName());
                friendlyName += "<" + genericParameters.Join(", ") + ">";

                return friendlyName;
        }

    }
}
