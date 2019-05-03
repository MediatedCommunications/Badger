using System.Runtime.InteropServices;

// All of this code is from http://vbaccelerator.com/home/NET/Code/Libraries/Shell_Projects/Creating_and_Modifying_Shortcuts/article.asp

namespace System.Windows.Shell {

    public struct _WIN32_FIND_DATA {
        public uint dwFileAttributes;
        public _FILETIME ftCreationTime;
        public _FILETIME ftLastAccessTime;
        public _FILETIME ftLastWriteTime;
        public uint nFileSizeHigh;
        public uint nFileSizeLow;
        public uint dwReserved0;
        public uint dwReserved1;

        public string cFileName;

        public string cAlternateFileName;

        public _WIN32_FIND_DATAA To_WIN32_FIND_DATAA() {
            return new _WIN32_FIND_DATAA() {
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

        public _WIN32_FIND_DATAW To_WIN32_FIND_DATAW() {
            return new _WIN32_FIND_DATAW() {
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
