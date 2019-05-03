using System;
using System.Runtime.InteropServices;

// All of this code is from http://vbaccelerator.com/home/NET/Code/Libraries/Shell_Projects/Creating_and_Modifying_Shortcuts/article.asp

namespace System.Windows.Shell {

    [StructLayout(LayoutKind.Sequential)]
    public struct PROPERTYKEY {
        public Guid fmtid;
        public UIntPtr pid;

    }

}