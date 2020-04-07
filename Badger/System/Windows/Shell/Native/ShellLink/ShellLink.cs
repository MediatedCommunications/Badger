using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

// All of this code is from http://vbaccelerator.com/home/NET/Code/Libraries/Shell_Projects/Creating_and_Modifying_Shortcuts/article.asp

namespace Badger.Windows.Shell.Native {
    /// <summary>
    /// Summary description for ShellLink.
    /// </summary>
    public partial class ShellLink : IDisposable {

        // Use Unicode (W) under NT, otherwise use ANSI    
        IShellLink Wrapped;

        /// <summary>
        /// Creates an instance of the Shell Link object.
        /// </summary>
        public ShellLink() {

            if (System.Environment.OSVersion.Platform == PlatformID.Win32NT) {
                var linkW = (IShellLinkW)new CShellLink();
                Wrapped = new ShellLinkW(linkW);
            } else {
                var linkA = (IShellLinkA)new CShellLink();
                Wrapped = new ShellLinkA(linkA);
            }
        }

        /// <summary>
        /// Call dispose just in case it hasn't happened yet
        /// </summary>
        ~ShellLink() {
            Dispose();
        }

        /// <summary>
        /// Dispose the object, releasing the COM ShellLink object
        /// </summary>
        public void Dispose() {
            Wrapped?.Dispose();
        }

        /// <summary>
        /// Gets a System.Drawing.Icon containing the icon for this
        /// ShellLink object.
        /// </summary>
        public Icon LargeIcon {
            get { return getIcon(true); }
        }

        public Icon SmallIcon {
            get { return getIcon(false); }
        }

        Icon getIcon(bool large) {
            // Get icon index and path:
            var iconPath = new StringBuilder(Win32.MAX_PATH, Win32.MAX_PATH);

            Wrapped.GetIconLocation(iconPath, iconPath.Capacity, out var iconIndex);

            var iconFile = iconPath.ToString();

            // If there are no details set for the icon, then we must use
            // the shell to get the icon for the target:
            if (iconFile.Length == 0) {
                // Use the FileIcon object to get the icon:
                var flags =
                    SHGetFileInfoConstants.SHGFI_ICON |
                    SHGetFileInfoConstants.SHGFI_ATTRIBUTES;
                if (large) {
                    flags = flags | SHGetFileInfoConstants.SHGFI_LARGEICON;
                } else {
                    flags = flags | SHGetFileInfoConstants.SHGFI_SMALLICON;
                }
                var fileIcon = FileIcon.FromFile(Target, flags);
                return fileIcon.ShellIcon;
            } else {
                // Use ExtractIconEx to get the icon:
                var hIconEx = new IntPtr[1] { IntPtr.Zero };
                var iconCount = 0;
                if (large) {
                    iconCount = Win32.ExtractIconEx(
                        iconFile,
                        iconIndex,
                        hIconEx,
                        null,
                        1);
                } else {
                    iconCount = Win32.ExtractIconEx(
                        iconFile,
                        iconIndex,
                        null,
                        hIconEx,
                        1);
                }
                // If success then return as a GDI+ object
                Icon icon = null;
                if (hIconEx[0] != IntPtr.Zero) {
                    icon = Icon.FromHandle(hIconEx[0]);
                    //UnManagedMethods.DestroyIcon(hIconEx[0]);
                }
                return icon;
            }
        }

        /// <summary>
        /// Gets the path to the file containing the icon for this shortcut.
        /// </summary>
        public string IconPath {
            get {
                var iconPath = new StringBuilder(Win32.MAX_PATH, Win32.MAX_PATH);
                Wrapped.GetIconLocation(iconPath, iconPath.Capacity, out var iconIndex);
                return iconPath.ToString();
            }
            set {
                var iconPath = new StringBuilder(Win32.MAX_PATH, Win32.MAX_PATH);
                Wrapped.GetIconLocation(iconPath, iconPath.Capacity, out var iconIndex);
                Wrapped.SetIconLocation(value, iconIndex);
            }
        }

        /// <summary>
        /// Gets the index of this icon within the icon path's resources
        /// </summary>
        public int IconIndex {
            get {
                var iconPath = new StringBuilder(Win32.MAX_PATH, Win32.MAX_PATH);
                Wrapped.GetIconLocation(iconPath, iconPath.Capacity, out var iconIndex);
                return iconIndex;
            }
            set {
                var iconPath = new StringBuilder(Win32.MAX_PATH, Win32.MAX_PATH);
                Wrapped.GetIconLocation(iconPath, iconPath.Capacity, out var iconIndex);
                Wrapped.SetIconLocation(iconPath.ToString(), value);
            }
        }

        /// <summary>
        /// Gets/sets the fully qualified path to the link's target
        /// </summary>
        public string Target {
            get {
                var target = new StringBuilder(Win32.MAX_PATH, Win32.MAX_PATH);
                var fd = new _WIN32_FIND_DATA();
                Wrapped.GetPath(target, target.Capacity, ref fd, (uint)EShellLinkGP.SLGP_UNCPRIORITY);
                return Target.ToString();
            }
            set {
                Wrapped.SetPath(value);
            }
        }

        /// <summary>
        /// Gets/sets the Working Directory for the Link
        /// </summary>
        public string WorkingDirectory {
            get {
                var path = new StringBuilder(Win32.MAX_PATH, Win32.MAX_PATH);
                Wrapped.GetWorkingDirectory(path, path.Capacity);
                return path.ToString();
            }
            set {
                Wrapped.SetWorkingDirectory(value);
            }
        }

        /// <summary>
        /// Gets/sets the description of the link
        /// </summary>
        public string Description {
            get {
                var description = new StringBuilder(Win32.MAX_DESCRIPTION, Win32.MAX_DESCRIPTION);
                Wrapped.GetDescription(description, description.Capacity);
                return description.ToString();
            }
            set {
                Wrapped.SetDescription(value);
            }
        }

        /// <summary>
        /// Gets/sets any command line arguments associated with the link
        /// </summary>
        public string Arguments {
            get {
                var arguments = new StringBuilder(Win32.MAX_DESCRIPTION, Win32.MAX_DESCRIPTION);
                Wrapped.GetArguments(arguments, arguments.Capacity);
                return arguments.ToString();
            }
            set {
                Wrapped.SetArguments(value);
            }
        }

        /// <summary>
        /// Gets/sets the initial display mode when the shortcut is
        /// run
        /// </summary>
        public LinkDisplayMode DisplayMode {
            get {
                Wrapped.GetShowCmd(out var cmd);
                var ret = (LinkDisplayMode)cmd;
                return ret;
            }
            set {
                Wrapped.SetShowCmd((uint)value);
            }
        }

        /// <summary>
        /// Gets/sets the HotKey to start the shortcut (if any)
        /// </summary>
        public short HotKey {
            get {
                Wrapped.GetHotkey(out var key);
                return key;
            }
            set {
                Wrapped.SetHotkey(value);
            }
        }

        /// <summary>
        /// Sets the appUserModelId
        /// </summary>
        public void SetAppUserModelId(string appId) {
            if (Wrapped.IsPropertyStore(out var propStore)) {
                var pkey = PROPERTYKEYS.PKEY_AppUserModel_ID;
                var str = PropVariant.FromString(appId);
                propStore.SetValue(ref pkey, ref str);
            }
        }

        /// <summary>
        /// Sets the ToastActivatorCLSID
        /// </summary>
        public void SetToastActivatorCLSID(string clsid) {
            var guid = Guid.Parse(clsid);
            SetToastActivatorCLSID(guid);
        }

        /// <summary>
        /// Sets the ToastActivatorCLSID
        /// </summary>
        public void SetToastActivatorCLSID(Guid clsid) {

            if (Wrapped.IsPropertyStore(out var propStore)) {
                var pkey = PROPERTYKEYS.PKEY_AppUserModel_ToastActivatorCLSID;

                var varGuid = PropVariant.FromGuid(clsid);
                try {
                    int errCode = propStore.SetValue(ref pkey, ref varGuid);
                    Marshal.ThrowExceptionForHR(errCode);

                    errCode = propStore.Commit();
                    Marshal.ThrowExceptionForHR(errCode);
                } finally {
                    varGuid.Clear();
                }
            }
        }

        /// <summary>
        /// Saves the shortcut to the specified file
        /// </summary>
        /// <param name="linkFile">The shortcut file (.lnk)</param>
        public void Save(string linkFile) {
            if (Wrapped.IsPersistFile(out var Persist)) {
                Persist.Save(linkFile, true);
            }
        }



        /// <summary>
        /// Loads a shortcut from the specified file
        /// </summary>
        /// <param name="linkFile">The shortcut file (.lnk) to load</param>
        public static ShellLink FromFile(string linkFile) {
            return FromFile(linkFile, IntPtr.Zero, (EShellLinkResolveFlags.SLR_ANY_MATCH | EShellLinkResolveFlags.SLR_NO_UI), 1);
        }

        /// <summary>
        /// Loads a shortcut from the specified file, and allows flags controlling
        /// the UI behaviour if the shortcut's target isn't found to be set.
        /// </summary>
        /// <param name="linkFile">The shortcut file (.lnk) to load</param>
        /// <param name="hWnd">The window handle of the application's UI, if any</param>
        /// <param name="resolveFlags">Flags controlling resolution behaviour</param>
        public static ShellLink FromFile(string linkFile, IntPtr hWnd, EShellLinkResolveFlags resolveFlags) {
            return FromFile(linkFile, hWnd, resolveFlags, 1);
        }

        /// <summary>
        /// Loads a shortcut from the specified file, and allows flags controlling
        /// the UI behaviour if the shortcut's target isn't found to be set.  If
        /// no SLR_NO_UI is specified, you can also specify a timeout.
        /// </summary>
        /// <param name="linkFile">The shortcut file (.lnk) to load</param>
        /// <param name="hWnd">The window handle of the application's UI, if any</param>
        /// <param name="resolveFlags">Flags controlling resolution behaviour</param>
        /// <param name="timeOut">Timeout if SLR_NO_UI is specified, in ms.</param>
        public static ShellLink FromFile(string linkFile, IntPtr hWnd, EShellLinkResolveFlags resolveFlags, ushort timeOut ) {
            uint flags;

            if ((resolveFlags & EShellLinkResolveFlags.SLR_NO_UI) == EShellLinkResolveFlags.SLR_NO_UI) {
                flags = (uint)((int)resolveFlags | (timeOut << 16));
            } else {
                flags = (uint)resolveFlags;
            }
            var ret = new ShellLink();
            if (ret.Wrapped.IsPersistFile(out var Persist)) {
                Persist.Load(linkFile, 0); //STGM_DIRECT)
                ret.Wrapped.Resolve(hWnd, flags);
            }

            return ret;
        }
    }
}
