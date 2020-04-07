using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Windows.Shell {
    public static class BadgerFolderExtensions {

        public static string Path(this BadgerFolder This, params string[] SubFolders) {
            var ret = GetFolderPath(This);
            foreach (var item in SubFolders) {
                ret = System.IO.Path.Combine(ret, item);
            }

            return ret;
        }

        public static string GetFolderPath(this BadgerFolder This) {
            var ret = default(string);

            switch (This) {
                case BadgerFolder.StartMenuPrograms:
                    ret = System.Environment.SpecialFolder.DesktopDirectory.Path("Programs");
                    break;
                case BadgerFolder.TaskBarPins:
                    ret = System.Environment.SpecialFolder.ApplicationData.Path("Microsoft", "Internet Explorer", "Quick Launch", "User Pinned", "TaskBar");
                    break;
                case BadgerFolder.AppVersionFolder:
                    ret = Badger.Environment.Default.VersionFolder;
                    break;
                case BadgerFolder.AppInstallFolder:
                    ret = Badger.Environment.Default.InstallFolder;
                    break;
                default:
                    break;
            }


            return ret;
        }

    }
}
