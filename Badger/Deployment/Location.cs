using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Deployment {

    public abstract class Location {
        public static Location Current { get; private set; } = DefaultLocation.FromEntryAssembly();
        public static Location FromFolder(string Location) => DefaultLocation.FromFolder(Location);


        public VersionFolder VersionFolder { get; protected set; }

        /// <summary>
        /// Something like:C:\Users\TonyValenti\AppData\Local\Clio\
        /// </summary>
        public string InstallFolder { get; protected set; }

        /// Something like:C:\Users\TonyValenti\AppData\Local\Clio\packages
        public string LocalRepositoryFolder { get; protected set; }

        public string InstanceIdFolder { get; protected set; }
        public string InstanceIdFullPath { get; protected set; }
        public abstract Guid? InstanceId { get; set; }

        public string TempFolder { get; protected set; }
    }

}
