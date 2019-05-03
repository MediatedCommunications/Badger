using System;
using System.Runtime.InteropServices;

// All of this code is from http://vbaccelerator.com/home/NET/Code/Libraries/Shell_Projects/Creating_and_Modifying_Shortcuts/article.asp

namespace System.Windows.Shell {
    public static class Win32
    {
        public const int MAX_PATH = 260;
        public const int MAX_BUFFER_SIZE = 256;
        public const int MAX_DESCRIPTION = 1024;

        [DllImport("Shell32", CharSet = CharSet.Auto)]
        public static extern int ExtractIconEx([MarshalAs(UnmanagedType.LPTStr)] string lpszFile, int nIconIndex, IntPtr[] phIconLarge, IntPtr[] phIconSmall, int nIcons);

        [DllImport("user32")]
        public static extern int DestroyIcon(IntPtr hIcon);

        [DllImport("shell32")]
        private static extern IntPtr SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        public static IntPtr SHGetFileInfo(string pszPath, int dwFileAttributes, uint uFlags, out SHFILEINFO psfi) {
            var shfi = new SHFILEINFO();
            var shfiSize = (uint)Marshal.SizeOf(shfi.GetType());

            var ret = Win32.SHGetFileInfo(pszPath, dwFileAttributes, ref shfi, shfiSize, uFlags);

            psfi = shfi;

            return ret;
        }


        [DllImport("kernel32")]
        private static extern int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, string lpBuffer, uint nSize, IntPtr argumentsLong);

        public static string FormatMessage(FormatMessageFlags dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, IntPtr argumentsLong) {
            var ret = new String('\0', MAX_BUFFER_SIZE);

            var Length = FormatMessage((int)dwFlags, lpSource, dwMessageId, dwLanguageId, ret, (uint)ret.Length, argumentsLong);


            ret = ret.Substring(0, Length);

            return ret;
        }

        [DllImport("kernel32")]
        public static extern int GetLastError();

    }

}
