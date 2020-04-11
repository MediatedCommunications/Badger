using System.Runtime.InteropServices;

// All of this code is from http://vbaccelerator.com/home/NET/Code/Libraries/Shell_Projects/Creating_and_Modifying_Shortcuts/article.asp

namespace Badger.Windows.Shell.Native {
        [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 0)]
        public struct _FILETIME
        {
            public uint dwLowDateTime;
            public uint dwHighDateTime;
        }
}
