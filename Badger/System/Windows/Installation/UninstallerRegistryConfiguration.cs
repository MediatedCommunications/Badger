using Badger;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Installation {
    public class UninstallerRegistryConfiguration {
        public string DisplayIcon { get; set; }
        public string DisplayName { get; set; }
        public string DisplayVersion { get; set; }
        public long? EstimatedSize { get; set; }
        public DateTime? InstallDate { get; set; }
        public string InstallLocation { get; set; }
        public long? Language { get; set; }
        public long? NoModify { get; set; }
        public long? NoRepair { get; set; }
        public string Publisher { get; set; }
        public string QuietUninstallString { get; set; }
        public string UninstallString { get; set; }
        public string URLUpdateInfo { get; set; }
    }

    public class UninstallerRegistry {
        public const string UninstallRegSubKey = @"Software\Microsoft\Windows\CurrentVersion\Uninstall";

        public static UninstallerRegistry Default => User;

        public static UninstallerRegistry User { get; private set; } = new UninstallerRegistry(RegistryHive.CurrentUser, RegistryView.Default);
        public static UninstallerRegistry User32 { get; private set; } = new UninstallerRegistry(RegistryHive.CurrentUser, RegistryView.Registry32);
        public static UninstallerRegistry User64 { get; private set; } = new UninstallerRegistry(RegistryHive.CurrentUser, RegistryView.Registry64);

        public static UninstallerRegistry System { get; private set; } = new UninstallerRegistry(RegistryHive.LocalMachine, RegistryView.Default);
        public static UninstallerRegistry System32 { get; private set; } = new UninstallerRegistry(RegistryHive.LocalMachine, RegistryView.Registry32);
        public static UninstallerRegistry System64 { get; private set; } = new UninstallerRegistry(RegistryHive.LocalMachine, RegistryView.Registry64);


        public UninstallerRegistry(): this(RegistryHive.CurrentUser, RegistryView.Default) {
            
        }

        public RegistryHive Hive { get; private set; }
        public RegistryView View { get; private set; }
        public UninstallerRegistry(RegistryHive Hive, RegistryView View) {
            this.Hive = Hive;
            this.View = View;
        }

        public String[] Applications() {
            var ret = new string[] { };

            try {
                using (var Key = RegistryKey.OpenBaseKey(Hive, View)) {
                    using (var Key2 = Key.OpenSubKey(UninstallRegSubKey)) {
                        ret = Key2.GetSubKeyNames();
                    }
                }
            } catch (Exception ex) {
                ex.Ignore();
            }

            return ret;
        }


        private string GetString(RegistryKey Key, string Name) {
            var ret = "";

            try {
                ret = (Key.GetValue(Name) ?? "").ToString();
            } catch { }

            return ret;
        }

        private long? GetLong(RegistryKey Key, string Name) {
            var ret = default(long?);

            try {
                if(Key.GetValue(Name) is long V) {
                    ret = V;
                }
            } catch { }

            return ret;
        }

        private string DateFormat = "yyyyMMdd";
        private DateTime? GetDate(RegistryKey Key, string Name) {
            var ret = default(DateTime?);
            try {
                var V = GetString(Key, Name);
                if (!string.IsNullOrWhiteSpace(V)) {
                    ret = DateTime.ParseExact(V, DateFormat, CultureInfo.InvariantCulture);
                }

            } catch { }

            return ret;
        }

        public UninstallerRegistryConfiguration Get(string Application) {
            var ret = default(UninstallerRegistryConfiguration);

            try {
                using (var Key = RegistryKey.OpenBaseKey(Hive, View)) {
                    using (var Key2 = Key.OpenSubKey(UninstallRegSubKey)) {
                        using (var Key3 = Key2.OpenSubKey(Application)) {
                            var tret = new UninstallerRegistryConfiguration() {
                                DisplayIcon = GetString(Key3, "DisplayIcon"),
                                DisplayName = GetString(Key3, "DisplayName"),
                                DisplayVersion = GetString(Key3, "DisplayVersion"),
                                InstallLocation = GetString(Key3, "InstallLocation"),
                                Publisher = GetString(Key3, "Publisher"),
                                QuietUninstallString = GetString(Key3, "QuietUninstallString"),
                                UninstallString = GetString(Key3, "UninstallString"),
                                URLUpdateInfo = GetString(Key3, "URLUpdateInfo"),
                                EstimatedSize = GetLong(Key3, "EstimatedSize"),
                                Language = GetLong(Key3, "Language"),
                                NoModify = GetLong(Key3, "NoModify"),
                                NoRepair = GetLong(Key3, "NoRepair"),
                                InstallDate = GetDate(Key3, "InstallDate"),
                            };
                            


                            ret = tret;
                        }
                    }
                }

            } catch { }


            return ret;
        }


        public void AddOrUpdate(string Application, UninstallerRegistryConfiguration Configuration, bool DeleteNullValues = true) {
            var ValuesToWrite = new Dictionary<String, object>() {
                ["DisplayIcon"] = Configuration.DisplayIcon,
                ["DisplayName"] = Configuration.DisplayName,
                ["DisplayVersion"] = Configuration.DisplayVersion,
                ["EstimatedSize"] = Configuration.EstimatedSize,
                ["InstallDate"] = Configuration.InstallDate?.ToString(DateFormat),
                ["InstallLocation"] = Configuration.InstallLocation,
                ["Language"] = Configuration.Language,
                ["NoModify"] = Configuration.NoModify,
                ["NoRepair"] = Configuration.NoRepair,
                ["Publisher"] = Configuration.Publisher,
                ["QuietUninstallString"] = Configuration.QuietUninstallString,
                ["UninstallString"] = Configuration.UninstallString,
                ["URLUpdateInfo"] = Configuration.URLUpdateInfo,
            };

            using (var Key = RegistryKey.OpenBaseKey(Hive, View)) {
                using (var Key2 = Key.CreateSubKey(UninstallRegSubKey, true)) {
                    using (var Key3 = Key2.CreateSubKey(Application, true)) {
                        foreach (var item in ValuesToWrite) {
                            var Name = item.Key;
                            var Value = item.Value;
                            if(Value == null) {
                                Key3.DeleteValue(Name, false);
                            } else if (Value is string) {
                                Key3.SetValue(Name, Value, RegistryValueKind.String);
                            } else if (Value is long) {
                                Key3.SetValue(Name, Value, RegistryValueKind.DWord);
                            }
                            
                        }
                    }
                }
            }

        }

        public void Remove(string Application) {
            using (var Key = RegistryKey.OpenBaseKey(Hive, View)) {
                using (var Key2 = Key.OpenSubKey(UninstallRegSubKey, true)) {
                    Key2.DeleteSubKeyTree(Application,false);
                }
            }
        }

    }
}
