using System;
using System.Collections.Generic;

namespace Badger.Installer {
    [Serializable]
    public class Configuration {
        public string Package_Name { get; set; }
        public Version Package_Version { get; set; }
        public string Install_Subfolder { get; set; }
        public bool Install_Location_Kill { get; set; }
        public bool Install_Location_Clean { get; set; }
        public string Install_Content_ParameterTemplate { get; set; }

        public List<ExternalToolConfiguration> Install_Location_Scripts { get; private set; } = new List<ExternalToolConfiguration>();

    }

}
