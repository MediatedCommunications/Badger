using Badger.Diagnostics;
using System.Diagnostics;

namespace Badger.Installer {
    [DebuggerDisplay(DebugView.Default)]
    public abstract class ConfigurationBase {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected abstract string DebuggerDisplay { get; }

    }

}
