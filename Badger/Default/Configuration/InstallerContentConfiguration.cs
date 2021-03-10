using Badger.Common;

namespace Badger.Default.Configuration {
    public class InstallerContentConfiguration : ConfigurationBase {
        public string Source { get; set; }
        public bool? Include { get; set; }

        protected override string GetDebuggerDisplay() {
                return this.DebuggerDisplay();
        }
    }

    public static class InstallerContentConfigurationExtensions {
        public static bool Include(this InstallerContentConfiguration This) {
            return (This?.Include).TrueWhenNull() && This.Source.IsNotNullOrEmpty();
        }
    }

}
