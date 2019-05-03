// All of this code is from http://vbaccelerator.com/home/NET/Code/Libraries/Shell_Projects/Creating_and_Modifying_Shortcuts/article.asp

namespace System.Windows.Shell {
    [Flags]
    public enum FormatMessageFlags {
        ALLOCATE_BUFFER = 0x100,
        ARGUMENT_ARRAY = 0x2000,
        FROM_HMODULE = 0x800,
        FROM_STRING = 0x400,
        FROM_SYSTEM = 0x1000,
        IGNORE_INSERTS = 0x200,
        MAX_WIDTH_MASK = 0xFF,
    }

}
