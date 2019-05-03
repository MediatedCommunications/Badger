using System;

// All of this code is from http://vbaccelerator.com/home/NET/Code/Libraries/Shell_Projects/Creating_and_Modifying_Shortcuts/article.asp

namespace System.Windows.Shell {

    /// <summary>
    /// Flags determining how the links with missing
    /// targets are resolved.
    /// </summary>
    [Flags]
    public enum EShellLinkResolveFlags : uint {
        /// <summary>
        /// Allow any match during resolution.  Has no effect
        /// on ME/2000 or above, use the other flags instead.
        /// </summary>
        SLR_ANY_MATCH = 0x2,

        /// <summary>
        /// Call the Microsoft Windows Installer. 
        /// </summary>
        SLR_INVOKE_MSI = 0x80,

        /// <summary>
        /// Disable distributed link tracking. By default, 
        /// distributed link tracking tracks removable media 
        /// across multiple devices based on the volume name. 
        /// It also uses the UNC path to track remote file 
        /// systems whose drive letter has changed. Setting 
        /// SLR_NOLINKINFO disables both types of tracking.
        /// </summary>
        SLR_NOLINKINFO = 0x40,

        /// <summary>
        /// Do not display a dialog box if the link cannot be resolved. 
        /// When SLR_NO_UI is set, a time-out value that specifies the 
        /// maximum amount of time to be spent resolving the link can 
        /// be specified in milliseconds. The function returns if the 
        /// link cannot be resolved within the time-out duration. 
        /// If the timeout is not set, the time-out duration will be 
        /// set to the default value of 3,000 milliseconds (3 seconds). 
        /// </summary>                                  
        SLR_NO_UI = 0x1,

        /// <summary>
        /// Not documented in SDK.  Assume same as SLR_NO_UI but 
        /// intended for applications without a hWnd.
        /// </summary>
        SLR_NO_UI_WITH_MSG_PUMP = 0x101,

        /// <summary>
        /// Do not update the link information. 
        /// </summary>
        SLR_NOUPDATE = 0x8,

        /// <summary>
        /// Do not execute the search heuristics. 
        /// </summary>                                                        
        SLR_NOSEARCH = 0x10,

        /// <summary>
        /// Do not use distributed link tracking. 
        /// </summary>
        SLR_NOTRACK = 0x20,

        /// <summary>
        /// If the link object has changed, update its path and list 
        /// of identifiers. If SLR_UPDATE is set, you do not need to 
        /// call IPersistFile::IsDirty to determine whether or not 
        /// the link object has changed. 
        /// </summary>
        SLR_UPDATE = 0x4
    }

}