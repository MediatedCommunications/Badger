using Badger.Common.Diagnostics;
using System;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Badger.Default.Installer {
    
    [Serializable]
    [DebuggerDisplay(DebugView.Default)]
    public abstract class ConfigurationBase {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected abstract string DebuggerDisplay { get; }


    }

}
