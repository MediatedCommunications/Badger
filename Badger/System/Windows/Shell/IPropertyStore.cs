using System;
using System.Runtime.InteropServices;

// All of this code is from http://vbaccelerator.com/home/NET/Code/Libraries/Shell_Projects/Creating_and_Modifying_Shortcuts/article.asp

namespace System.Windows.Shell {

    [ComImport]
    [Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPropertyStore {
        [PreserveSig]
        int GetCount([Out] out uint cProps);
        [PreserveSig]
        int GetAt([In] uint iProp, out PROPERTYKEY pkey);
        [PreserveSig]
        int GetValue([In] ref PROPERTYKEY key, out PropVariant pv);
        [PreserveSig]
        int SetValue([In] ref PROPERTYKEY key, [In] ref PropVariant pv);
        [PreserveSig]
        int Commit();
    }
}
