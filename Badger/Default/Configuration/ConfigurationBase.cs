using Badger.Common.Diagnostics;
using System;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Badger.Default.Configuration {
    
    [Serializable]
    [DebuggerDisplay(DebugView.Default)]
    public abstract class ConfigurationBase {
        protected abstract string GetDebuggerDisplay();


    }

}
