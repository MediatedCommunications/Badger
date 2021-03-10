using Badger.Common;
using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Badger.Default.Configuration {
    [Serializable]
    public class ExternalToolConfiguration : ConfigurationBase {

        public bool? Enabled { get; set; }

        public string Path { get; set; }
        public string Application { get; set; }

        public string FullPath {
            get {
                var ret = Application;
                if(Path is { }) {
                    ret = System.IO.Path.Combine(Path, ret);
                }

                return ret;
            }
        }

        
        public string ArgumentTemplate { get; set; }

        public bool? Visible { get; set; }

        public bool? Async { get; set; }

        protected override string GetDebuggerDisplay() {
                return this.DebuggerDisplay();
        }
    }

    public static class DebuggerDisplayExtensions {

        static string Attribute(bool? Status, string Enabled, string Disabled) {
            return Status == true ? Enabled : Disabled;
        }

        static string WhenTrue(bool? Status, string Enabled) {
            return Attribute(Status, Enabled, null);
        }

        static string WhenFalse(bool? Status, string Disabled) {
            return Attribute(Status, null, Disabled);
        }

        static string Attributes(params string[] Values) {

            var ret = Join(Values);

            if(ret.Length > 0) {
                ret = $@"({ret})";
            }

            return ret;
        }

        private static string Join(params string[] Values) {
            var ret = "";
            var V = (
                from x in Values where x.IsNotNullOrEmpty()
                select x
                ).ToList();

            if (V.Count > 0) {
                ret = V.JoinSpace();
            }


            return ret;
        }


        public static string NullDebuggerDisplay(object V) {
            var ret = "{null}";
            if(V is { }) {
                ret = null;
            }

            return ret;
        }

        private static string DebuggerDisplay<T>(this T Value, Func<T, string> Generate) {
            var ret = "{null}";

            if(Value is { }) {
                ret = Generate(Value);
            }
            return ret;
        }
        

        public static string DebuggerDisplay(this ExternalToolConfiguration This) {
            return This.DebuggerDisplay(This => {
                var ret = "";
                
                var MyAttributes = Attributes(
                        WhenFalse(This.Enabled, "Disabled"),
                        WhenTrue(This.Async, "Async"),
                        WhenFalse(This.Visible, "Invisible")
                        );

                ret = Join(This.FullPath, This.ArgumentTemplate, MyAttributes);

                return ret;
            });
        }

        public static string DebuggerDisplay(this ExternalToolsConfiguration This) {
            return This.DebuggerDisplay(This => {
                return This.ToString();
            });
        }

        public static string DebuggerDisplay(this VersionStringExternalToolConfiguration This) {
            return This.DebuggerDisplay(This => {
                return This.ToString();
            });
        }

        public static string DebuggerDisplay(this IconExternalToolConfiguration This) {
            return This.DebuggerDisplay(This => {
                return This.ToString();
            });
        }

        public static string DebuggerDisplay(this WorkingFolderConfiguration This) {
            return This.DebuggerDisplay(This => {
                return $@"{This.PathRoot}\{This.PathTemplate}";
            });
        }

        public static string DebuggerDisplay(this ProductConfiguration This) {
            return This.DebuggerDisplay(This => {
                var VersionString = "";
                if(This.Version is { } V1) {
                    VersionString = V1.ToString();
                } else if (This.Version_FromFile.IsNotNullOrEmpty()) {
                    VersionString = $@"from {This.Version_FromFile}";
                }
                return Join(This.Name, Attributes(VersionString));
            });
        }


        public static string DebuggerDisplay(this InstallerConfiguration This) {
            return This.DebuggerDisplay(This => {
                return This.Content.DebuggerDisplay();
            });
        }

        public static string DebuggerDisplay(this InstallerContentConfiguration This) {
            return This.DebuggerDisplay(This => {

                var attr = Attributes(WhenFalse(This.Include, "Excluded"));

                return Join(This.Source, attr);
            });
        }

        public static string DebuggerDisplay(this UninstallerConfiguration This) {
            return This.DebuggerDisplay(This => {
                return This.Stub.DebuggerDisplay();
            });
        }

        public static string DebuggerDisplay(this RedirectorConfiguration This) {
            return This.DebuggerDisplay(This => {
                return This.Stub.DebuggerDisplay();
            });
        }


        public static string DebuggerDisplay(this PackagerConfiguration This) {
            return This.DebuggerDisplay(This => {
                return This.Product.DebuggerDisplay();
            });
        }

        public static string DebuggerDisplay(this InstallerContentPlaybookConfiguration This) {
            return This.DebuggerDisplay(This => {
                return This.ToString();
            });
        }

        public static string DebuggerDisplay(this UninstallerContentPlaybookConfiguration This) {
            return This.DebuggerDisplay(This => {
                return This.ToString();
            });
        }

        public static string DebuggerDisplay(this OutputConfiguration This) {
            return This.DebuggerDisplay(This => {
                return This.Path_NameTemplate;
            });
        }


    }

}
