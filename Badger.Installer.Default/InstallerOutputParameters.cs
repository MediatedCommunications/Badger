using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Installer.Default {
    public class InstallerOutputParameters {
        public string Name { get; set; }
        public Version Version { get; set; }

        public static string DefaultTemplate => $@"{{{nameof(Name)}}}-{{{nameof(Version)}}}-Installer.exe";

    }
}
