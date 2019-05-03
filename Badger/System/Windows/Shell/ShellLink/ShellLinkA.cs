using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Windows.Shell {
    public class ShellLinkA : IShellLink {
        public IShellLinkA Wrapped { get; private set; }
        
        public ShellLinkA(IShellLinkA Wrapped) {
            this.Wrapped = Wrapped;
        }

        public void GetArguments(StringBuilder pszArgs, int cchMaxPath) 
            => Wrapped.GetArguments(pszArgs, cchMaxPath);

        public void GetDescription(StringBuilder pszFile, int cchMaxName) 
            => Wrapped.GetDescription(pszFile, cchMaxName);

        public void GetHotkey(out short pwHotkey) 
            => Wrapped.GetHotkey(out pwHotkey);

        public void GetIconLocation(StringBuilder pszIconPath, int cchIconPath, out int piIcon)
            => Wrapped.GetIconLocation(pszIconPath, cchIconPath, out piIcon);

        public void GetIDList(out IntPtr ppidl)
            => Wrapped.GetIDList(out ppidl);

        public void GetPath(StringBuilder pszFile, int cchMaxPath, ref _WIN32_FIND_DATA pfd, uint fFlags) {
            var Converted = pfd.To_WIN32_FIND_DATAA();
            Wrapped.GetPath(pszFile, cchMaxPath, ref Converted, fFlags);

            var UnConverted = Converted.To_WIN32_FIND_DATA();
            pfd = UnConverted;
        }

        public void GetShowCmd(out uint piShowCmd)
            => Wrapped.GetShowCmd(out piShowCmd);

        public void GetWorkingDirectory(StringBuilder pszDir, int cchMaxPath)
            => Wrapped.GetWorkingDirectory(pszDir, cchMaxPath);

        public void Resolve(IntPtr hWnd, uint fFlags)
            => Wrapped.Resolve(hWnd, fFlags);

        public void SetArguments(string pszArgs)
            => Wrapped.SetArguments(pszArgs);

        public void SetDescription(string pszName)
            => Wrapped.SetDescription(pszName);

        public void SetHotkey(short pwHotkey)
            => Wrapped.SetHotkey(pwHotkey);

        public void SetIconLocation(string pszIconPath, int iIcon)
            => Wrapped.SetIconLocation(pszIconPath, iIcon);

        public void SetIDList(IntPtr pidl)
            => Wrapped.SetIDList(pidl);

        public void SetPath(string pszFile)
            => Wrapped.SetPath(pszFile);

        public void SetRelativePath(string pszPathRel, uint dwReserved)
            => Wrapped.SetRelativePath(pszPathRel, dwReserved);

        public void SetShowCmd(uint piShowCmd)
            => Wrapped.SetShowCmd(piShowCmd);

        public void SetWorkingDirectory(string pszDir)
            => Wrapped.SetWorkingDirectory(pszDir);

        public void Dispose() {
            Marshal.ReleaseComObject(Wrapped);
        }

        public bool IsPropertyStore(out IPropertyStore PropertyStore) {
            var ret = false;
            PropertyStore = null;

            if(Wrapped is IPropertyStore PS) {
                PropertyStore = PS;
                ret = true;
            }

            return ret;
        }

        public bool IsPersistFile(out IPersistFile PersistFile) {
            var ret = false;
            PersistFile = null;

            if (Wrapped is IPersistFile PS) {
                PersistFile = PS;
                ret = true;
            }

            return ret;
        }
    }
}
