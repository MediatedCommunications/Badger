﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Default.Configuration {


    public static class ExternalToolsExtensions {
        public static bool Copy(this IconExternalToolConfiguration This, string Source, string Destination) {
            var ret = false;
            if (This.Get.Enabled() && This.Set.Enabled()) {
                ret = true;
                var Icon = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $@"{Guid.NewGuid()}.ico");

                {
                    var tret = This.Get.GetIcon(Source, Icon);
                    ret &= tret;
                }

                {
                    var tret = This.Set.SetIcon(Icon, Destination);
                    ret &= tret;
                }
                System.IO.File.Delete(Icon);
            }

            return ret;
        }

        public static string GetVersionString(this ExternalToolConfiguration This, string Source, string Key) {
            var ret = "";

            var Args = new GetVersionStringParameters() {
                Source_File = Source,
                Key = Key,
            };

            ret = This.RunOutput(Args)?.Output;


            return ret;
        }

        public static bool SetVersionString(this ExternalToolConfiguration This, string Dest, string Key, string Value) {
            var Args = new SetVersionStringParameters() {
                Dest_File = Dest,
                Key = Key,
                Value = Value,
            };

            return This.Run(Args);

        }



        public static bool Copy(this VersionStringExternalToolConfiguration This, string Source, string Destination) {
            var ret = false;

            if(This.Get.Enabled() && This.Set.Enabled()) {
                var Values = new Dictionary<string, string>();
                foreach (var item in This.Values) {
                    Values[item] = This.Get.GetVersionString(Source, item);
                }

                foreach (var item in Values) {
                    This.Set.SetVersionString(Destination, item.Key, item.Value);
                }


            }


            return ret;
        }

    }

}
