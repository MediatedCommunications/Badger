using System.Runtime.InteropServices;

// All of this code is from http://vbaccelerator.com/home/NET/Code/Libraries/Shell_Projects/Creating_and_Modifying_Shortcuts/article.asp

namespace System.Windows.Shell {

    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 0, CharSet = CharSet.Unicode)]
    public struct _WIN32_FIND_DATAW {
        public uint dwFileAttributes;
        public _FILETIME ftCreationTime;
        public _FILETIME ftLastAccessTime;
        public _FILETIME ftLastWriteTime;
        public uint nFileSizeHigh;
        public uint nFileSizeLow;
        public uint dwReserved0;
        public uint dwReserved1;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] // MAX_PATH
        public string cFileName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
        public string cAlternateFileName;

        public _WIN32_FIND_DATA To_WIN32_FIND_DATA() {
            return new _WIN32_FIND_DATA() {
                cAlternateFileName = cAlternateFileName,
                cFileName = cFileName,
                dwFileAttributes = dwFileAttributes,
                dwReserved0 = dwReserved0,
                dwReserved1 = dwReserved1,
                ftCreationTime = ftCreationTime,
                ftLastAccessTime = ftLastAccessTime,
                ftLastWriteTime = ftLastWriteTime,
                nFileSizeHigh = nFileSizeHigh,
                nFileSizeLow = nFileSizeLow,
            };
        }
    }

}
