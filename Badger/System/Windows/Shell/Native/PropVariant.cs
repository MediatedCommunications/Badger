using System;
using System.Runtime.InteropServices;

// All of this code is from http://vbaccelerator.com/home/NET/Code/Libraries/Shell_Projects/Creating_and_Modifying_Shortcuts/article.asp

namespace Badger.Windows.Shell.Native {

    [StructLayout(LayoutKind.Sequential)]
    public struct PropVariant {
        public short variantType;
        public short Reserved1, Reserved2, Reserved3;
        public IntPtr pointerValue;

        public static PropVariant FromString(string str) {
            var pv = new PropVariant() {
                variantType = 31,  // VT_LPWSTR
                pointerValue = Marshal.StringToCoTaskMemUni(str),
            };

            return pv;
        }

        public static PropVariant FromGuid(Guid guid) {
            byte[] bytes = guid.ToByteArray();
            var pv = new PropVariant() {
                variantType = 72,  // VT_CLSID
                pointerValue = Marshal.AllocCoTaskMem(bytes.Length),
            };
            Marshal.Copy(bytes, 0, pv.pointerValue, bytes.Length);

            return pv;
        }

        /// <summary>
        /// Called to properly clean up the memory referenced by a PropVariant instance.
        /// </summary>
        [DllImport("ole32.dll")]
        private extern static int PropVariantClear(ref PropVariant pvar);

        /// <summary>
        /// Called to clear the PropVariant's referenced and local memory.
        /// </summary>
        /// <remarks>
        /// You must call Clear to avoid memory leaks.
        /// </remarks>
        public void Clear() {
            // Can't pass "this" by ref, so make a copy to call PropVariantClear with
            PropVariant tmp = this;
            PropVariantClear(ref tmp);

            // Since we couldn't pass "this" by ref, we need to clear the member fields manually
            // NOTE: PropVariantClear already freed heap data for us, so we are just setting
            //       our references to null.
            variantType = (short)VarEnum.VT_EMPTY;
            Reserved1 = Reserved2 = Reserved3 = 0;
            pointerValue = IntPtr.Zero;
        }
    }
}