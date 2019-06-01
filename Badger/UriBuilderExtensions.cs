using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger {
    public static class UriBuilderExtensions {
        public static void AppendPath(this UriBuilder This, string Path) {
            This.Path = !This.Path.EndsWith("/")
                ? This.Path
                : This.Path.Substring(0, This.Path.Length - 2)
                ;

            Path = !Path.EndsWith("/")
                ? Path
                : Path.Substring(0, This.Path.Length - 2)
                ;

            This.Path = This.Path + "/" + Path;

        }

        public static void AppendQuery(this UriBuilder This, IEnumerable<KeyValuePair<string, string>> AdditionalParameters) {
            var Query = System.Web.HttpUtility.ParseQueryString(This.Query);
            foreach (var item in AdditionalParameters) {
                Query[item.Key] = item.Value;
            }

            This.Query = Query.ToString();

        }

    }
}
