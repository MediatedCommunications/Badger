using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Common {
    public static class CollectionExtensions {

        public static void Add<T>(this ICollection<T> This, IEnumerable<T> Items) { 
            if(Items is { }) {
                foreach (var item in Items) {
                    This.Add(item);
                }
            }
        }

    }
}
