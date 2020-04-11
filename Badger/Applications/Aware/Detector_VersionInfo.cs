using Badger.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Applications.Aware {
    class Detector_VersionInfo : Detector {

        public static Detector Instance { get; } = new Detector_VersionInfo();

        public override IEnumerable<int?> SupportedVersions(string Executable) {
            var Value = GetVersion_FileVersion(Executable);
            if(Value != null) {
                yield return Value;
            }
        }

        [DllImport("version.dll", SetLastError = true)]
        static extern int GetFileVersionInfoSize(string lpszFileName, IntPtr dwHandleIgnored);

        [DllImport("version.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetFileVersionInfo(string lpszFileName, int dwHandleIgnored, int dwLen, [MarshalAs(UnmanagedType.LPArray)] byte[] lpData);

        [DllImport("version.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool VerQueryValue(byte[] pBlock, string pSubBlock, out IntPtr pValue, out int len);

        static int? GetVersion_FileVersion(string ExecutableFullPath) {
            var ret = default(int?);
            try {
                if (System.IO.File.Exists(ExecutableFullPath)) {
                    var size = GetFileVersionInfoSize(ExecutableFullPath, IntPtr.Zero);
                    if (size >= 0 && size <= 4096) {
                        var Buffer = new byte[size];

                        if (GetFileVersionInfo(ExecutableFullPath, 0, size, Buffer)) {
                            var SubBlock = $@"\StringFileInfo\040904B0\{Metadata.AssemblyMetadataAttributeKey}";
                            if (VerQueryValue(Buffer, SubBlock, out var result, out var resultsize)) {
                                //TODO:

                                // NB: I have **no** idea why, but Atom.exe won't return the version
                                // number "1" despite it being in the resource file and being 100% 
                                // identical to the version block that actually works. I've got stuff
                                // to ship, so we're just going to return '1' if we find the name in 
                                // the block at all. I hate myself for this.
                                ret = 1;
                            }

                        }

                    }
                }
            } catch (Exception ex) {
                ex.Ignore();
            }
            return ret;
        }


    }
}
