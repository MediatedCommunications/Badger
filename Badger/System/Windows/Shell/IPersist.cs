using System;
using System.Runtime.InteropServices;

// All of this code is from http://vbaccelerator.com/home/NET/Code/Libraries/Shell_Projects/Creating_and_Modifying_Shortcuts/article.asp

namespace System.Windows.Shell {
    public partial class ShellLink
    {
        [ComImport()]
        [Guid("0000010C-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IPersist
        {
            [PreserveSig]
            //[helpstring("Returns the class identifier for the component object")]
            void GetClassID(out Guid pClassID);
        }
    }
}
