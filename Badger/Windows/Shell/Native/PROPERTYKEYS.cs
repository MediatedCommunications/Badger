// All of this code is from http://vbaccelerator.com/home/NET/Code/Libraries/Shell_Projects/Creating_and_Modifying_Shortcuts/article.asp

using System;

namespace Badger.Windows.Shell.Native {
    public static class PROPERTYKEYS {
        public static PROPERTYKEY PKEY_AppUserModel_ID {
            get {
                return new PROPERTYKEY() {
                    fmtid = Guid.ParseExact("{9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3}", "B"),
                    pid = new UIntPtr(5),
                };
            }
        }

        public static PROPERTYKEY PKEY_AppUserModel_ToastActivatorCLSID {
            get {
                return new PROPERTYKEY() {
                    fmtid = Guid.ParseExact("{9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3}", "B"),
                    pid = new UIntPtr(26),
                };
            }
        }
    }

}