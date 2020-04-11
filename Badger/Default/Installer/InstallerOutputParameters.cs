using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Default.Installer {
    public class InstallerOutputParameters {
        public string PackageName { get; set; }
        public Version Version { get; set; }

        public static string DefaultTemplate => $@"{{{nameof(PackageName)}}}-{{{nameof(Version)}}}-Installer.exe";

    }

    public class WorkingFolderTemplateParameters {
        public string PackageName { get; set; }
        public Version Version { get; set; }
    }

}
