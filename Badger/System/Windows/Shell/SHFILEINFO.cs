using System;
using System.Runtime.InteropServices;

// All of this code is from http://vbaccelerator.com/home/NET/Code/Libraries/Shell_Projects/Creating_and_Modifying_Shortcuts/article.asp

namespace System.Windows.Shell {
    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public int iIcon;
        public int dwAttributes;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Win32.MAX_PATH)]
        public string szDisplayName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }
}
