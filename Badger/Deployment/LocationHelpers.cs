using System.Text;

namespace Badger.Deployment {

    public static class LocationHelpers {

        
        public const string PackagesSubFolderName = "packages";
        public const string PackagesSubFolderPath = PackagesSubFolderName;

        public const string InstallIdFileName = ".betaId";
        public const string ReleasesFileName = "RELEASES.txt";

        public const string TempSubFolderName = "SquirrelTemp";
        public const string TempSubFolderPath = PackagesSubFolderPath + @"\" + TempSubFolderName;

    }
}
