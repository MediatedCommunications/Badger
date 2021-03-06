﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace Badger.Common.Management {
    public static class Query {

        public static List<T> Invoke<T>(string Query, Func<ManagementObject, T> Generator) {
            return Invoke<T>(null, Query, Generator);
        }

        public static List<T> Invoke<T>(string Namespace, string Query, Func<ManagementObject, T> Generator) {
            var ret = new List<T>();

            try {
                using (var Searcher = new ManagementObjectSearcher(Namespace, Query)) {
                    var Items = Searcher.Get().OfType<ManagementObject>().ToList();

                    foreach (var item in Items) {
                        try {
                            ret.Add(Generator(item));
                        } finally {
                            item.Dispose();
                        }
                    }

                }
            } catch (Exception ex) {
                
            }

            return ret;
        }

    }

}
