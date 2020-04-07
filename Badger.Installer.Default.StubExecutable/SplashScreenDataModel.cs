using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Badger.Installer.Default.StubExecutable {
    public class SplashScreenDataModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private BitmapImage __Image;
        public BitmapImage Image {
            get {
                return __Image;
            }
            set {
                Set(ref __Image, value);
                RaisePropertyChanged(nameof(Image_Exists));
                RaisePropertyChanged(nameof(Image_Null));
            }
        }

        public bool Image_Exists {
            get {
                return Image is { };
            }
        }

        public bool Image_Null => !Image_Exists;


        private string __Application_Name;
        public string Application_Name {
            get {
                return __Application_Name;
            }
            set {
                Set(ref __Application_Name, value);
                RaisePropertyChanged(nameof(Installer_Status));
            }
        }

        public string Installer_Status => $@"Installing {Application_Name}...";

        private void Set<T>(ref T Field, T Value, [CallerMemberName] string Property = default) {
            Field = Value;
            RaisePropertyChanged(Property);
        }


        protected void RaisePropertyChanged([CallerMemberName] string Property = default) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }

    }
}
