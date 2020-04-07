using System;
using System.Drawing;
using System.Runtime.InteropServices;

// All of this code is from http://vbaccelerator.com/home/NET/Code/Libraries/Shell_Projects/Creating_and_Modifying_Shortcuts/article.asp

namespace Badger.Windows.Shell.Native {

    /// <summary>
    /// Enables extraction of icons for any file type from
    /// the Shell.
    /// </summary>
    public class FileIcon {

        private const SHGetFileInfoConstants Flags_Default =
            SHGetFileInfoConstants.SHGFI_ICON |
            SHGetFileInfoConstants.SHGFI_DISPLAYNAME |
            SHGetFileInfoConstants.SHGFI_TYPENAME |
            SHGetFileInfoConstants.SHGFI_ATTRIBUTES |
            SHGetFileInfoConstants.SHGFI_EXETYPE
            ;


        /// <summary>
        /// Gets/sets the filename to get the icon for
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets the icon for the chosen file
        /// </summary>
        public Icon ShellIcon { get; set; }

        /// <summary>
        /// Gets the display name for the selected file
        /// if the SHGFI_DISPLAYNAME flag was set.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets the type name for the selected file
        /// if the SHGFI_TYPENAME flag was set.
        /// </summary>
        public string TypeName { get; set; }


        /// <summary>
        /// Constructs a new, default instance of the FileIcon
        /// class.  Specify the filename and call GetInfo()
        /// to retrieve an icon.
        /// </summary>
        public FileIcon() {

        }


        public static FileIcon FromFile(string FileName, SHGetFileInfoConstants Flags = Flags_Default) {
            var ret = default(FileIcon);

            if (Win32.SHGetFileInfo(FileName, 0, (uint)(Flags), out var shfi) == IntPtr.Zero) {
                var ErrorNumber = Win32.GetLastError();
                var ErrorMessage = Win32.FormatMessage(FormatMessageFlags.FROM_SYSTEM | FormatMessageFlags.IGNORE_INSERTS, IntPtr.Zero, ErrorNumber, 0, IntPtr.Zero);

                var ExceptionMessage = $@"Error #{ErrorNumber}: {ErrorMessage}";
                throw new Exception(ExceptionMessage);

            } else {
                ret = new FileIcon() {
                    TypeName = shfi.szTypeName,
                    DisplayName = shfi.szDisplayName,
                    ShellIcon = shfi.hIcon == IntPtr.Zero ? null : System.Drawing.Icon.FromHandle(shfi.hIcon)
                };

            }
            
            return ret;
        }

    }
}
