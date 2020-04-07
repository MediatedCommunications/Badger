// All of this code is from http://vbaccelerator.com/home/NET/Code/Libraries/Shell_Projects/Creating_and_Modifying_Shortcuts/article.asp

namespace Badger.Windows.Shell.Native { 
    public enum LinkDisplayMode : uint {
        edmNormal = EShowWindowFlags.SW_NORMAL,
        edmMinimized = EShowWindowFlags.SW_SHOWMINNOACTIVE,
        edmMaximized = EShowWindowFlags.SW_MAXIMIZE
    }
}

