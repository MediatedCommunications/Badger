﻿using Badger.Default.Configuration;
using System;
using System.Collections.Generic;

namespace Badger.Default.Installer {
    [Serializable]
    public class EmbeddedInstallerConfiguration {
        public string Product_Name { get; set; }
        public string Product_Publisher { get; set; }
        public Version Product_Version { get; set; }
        public string Product_Code { get; set; }


        public bool SpashScreen_Visible { get; set; }

        public string Install_Subfolder { get; set; }
        public bool Install_Location_Kill { get; set; }
        public bool Install_Location_Clean { get; set; }
        public string Install_Content_ParameterTemplate { get; set; }
        public List<ExternalToolConfiguration> Install_Location_Scripts { get; private set; } = new List<ExternalToolConfiguration>();

        public bool Uninstaller_Create { get; set; }
        
    }

}
