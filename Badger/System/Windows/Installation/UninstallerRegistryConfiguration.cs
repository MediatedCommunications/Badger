using Badger;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Installation {
    public class UninstallerRegistryConfiguration {
        public string DisplayIcon { get; set; }
        public string DisplayName { get; set; }
        public string DisplayVersion { get; set; }
        public long EstimatedSize { get; set; }
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

        public static UninstallerRegistry Default { get; private set; } = new UninstallerRegistry();

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

        public void AddOrUpdate(string Application, UninstallerRegistryConfiguration Configuration, bool DeleteNullValues = true) {
            var ValuesToWrite = new Dictionary<String, object>() {
                ["DisplayIcon"] = Configuration.DisplayIcon,
                ["DisplayName"] = Configuration.DisplayName,
                ["DisplayVersion"] = Configuration.DisplayVersion,
                ["EstimatedSize"] = Configuration.EstimatedSize,
                ["InstallDate"] = Configuration.InstallDate?.ToString("yyyyMMdd"),
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
