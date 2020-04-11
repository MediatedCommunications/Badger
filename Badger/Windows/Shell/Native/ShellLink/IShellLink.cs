using System;
using System.Runtime.InteropServices;
using System.Text;

// All of this code is from http://vbaccelerator.com/home/NET/Code/Libraries/Shell_Projects/Creating_and_Modifying_Shortcuts/article.asp

namespace Badger.Windows.Shell.Native {

    public interface IShellLink : IDisposable {
        //[helpstring("Retrieves the path and filename of a shell link object")]
        void GetPath(
            StringBuilder pszFile,
            int cchMaxPath,
            ref _WIN32_FIND_DATA pfd,
            uint fFlags);

        //[helpstring("Retrieves the list of shell link item identifiers")]
        void GetIDList(out IntPtr ppidl);

        //[helpstring("Sets the list of shell link item identifiers")]
        void SetIDList(IntPtr pidl);

        //[helpstring("Retrieves the shell link description string")]
        void GetDescription(
            StringBuilder pszFile,
            int cchMaxName);

        //[helpstring("Sets the shell link description string")]
        void SetDescription(string pszName);

        //[helpstring("Retrieves the name of the shell link working directory")]
        void GetWorkingDirectory(StringBuilder pszDir, int cchMaxPath);

        //[helpstring("Sets the name of the shell link working directory")]
        void SetWorkingDirectory(string pszDir);

        //[helpstring("Retrieves the shell link command-line arguments")]
        void GetArguments(StringBuilder pszArgs, int cchMaxPath);

        //[helpstring("Sets the shell link command-line arguments")]
        void SetArguments(string pszArgs);

        //[propget, helpstring("Retrieves or sets the shell link hot key")]
        void GetHotkey(out short pwHotkey);
        //[propput, helpstring("Retrieves or sets the shell link hot key")]
        void SetHotkey(short pwHotkey);

        //[propget, helpstring("Retrieves or sets the shell link show command")]
        void GetShowCmd(out uint piShowCmd);
        //[propput, helpstring("Retrieves or sets the shell link show command")]
        void SetShowCmd(uint piShowCmd);

        //[helpstring("Retrieves the location (path and index) of the shell link icon")]
        void GetIconLocation(StringBuilder pszIconPath, int cchIconPath, out int piIcon);

        //[helpstring("Sets the location (path and index) of the shell link icon")]
        void SetIconLocation(string pszIconPath, int iIcon);

        //[helpstring("Sets the shell link relative path")]
        void SetRelativePath(string pszPathRel, uint dwReserved);

        //[helpstring("Resolves a shell link. The system searches for the shell link object and updates the shell link path and its list of identifiers (if necessary)")]
        void Resolve(IntPtr hWnd, uint fFlags);

        //[helpstring("Sets the shell link path and filename")]
        void SetPath(string pszFile);


        bool IsPropertyStore(out IPropertyStore PropertyStore);

        bool IsPersistFile(out IPersistFile PersistFile);

    }
}

