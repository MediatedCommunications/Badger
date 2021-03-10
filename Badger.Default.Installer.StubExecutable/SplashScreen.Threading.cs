using Badger.Default.Installer;
using Badger.Default.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Badger.Default.Installer.StubExecutable {
    public partial class SplashScreen {

        public static void StartThread(EmbeddedInstallerConfiguration Config) {
            var T = new System.Threading.Thread(() => WindowThread(Config));
            T.SetApartmentState(System.Threading.ApartmentState.STA);
            T.Start();
        }

        static void WindowThread(EmbeddedInstallerConfiguration Config) {

            var W = new SplashScreen();

            try {
                if (SpashScreenResource.Resource.GetStream() is { } Stream) {
                    var B = new BitmapImage();
                    B.BeginInit();
                    B.StreamSource = Stream;
                    B.CacheOption = BitmapCacheOption.OnLoad;
                    B.EndInit();
                    B.Freeze();

                    W.DataModel.Image = B;
                }

                if (Config?.Product_Name is { } AppName) {
                    W.DataModel.Application_Name = AppName;
                }

            } catch (Exception ex) {
                
            }

            W.ShowDialog();
        }

    }
}
