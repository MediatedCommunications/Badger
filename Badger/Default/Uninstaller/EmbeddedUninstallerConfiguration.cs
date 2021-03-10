using Badger.Default.Configuration;
using System;
using System.Collections.Generic;

namespace Badger.Default.Installer {
    [Serializable]
    public class EmbeddedUninstallerConfiguration {
        public string Product_Name { get; set; }
        public Version Product_Version { get; set; }
        public string Product_Code { get; set; }

        public string Install_Subfolder { get; set; }

        public bool Install_Location_Kill { get; set; }
        public List<ExternalToolConfiguration> Install_Location_Scripts { get; private set; } = new List<ExternalToolConfiguration>();
        public bool Install_Location_Clean { get; set; }

        

    }

}
