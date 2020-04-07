using Badger.Windows.Shell.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Badger.Windows.Shell {
    public static class Shortcuts {

        public const string ICON_FROM_DESTINATION = null;
        public const int ICON_INDEX_DEFAULT = 0;
        public const string DEST_ARGUMENTS_NONE = null;

        public const string LINK_EXTENSION = ".lnk";

        /// <summary>
        /// Ensure that the link name ends with ".lnk".
        /// </summary>
        /// <param name="LinkName"></param>
        /// <returns></returns>
        private static string EnsureLinkName(string LinkName) {
            if (!LinkName.EndsWith(LINK_EXTENSION, StringComparison.InvariantCultureIgnoreCase)) {
                LinkName += LINK_EXTENSION;
            }

            return LinkName;
        }

        public static ShellLink Create(BadgerFolder LinkFolder, string DestPath, string DestArguments = DEST_ARGUMENTS_NONE, string IconSource = ICON_FROM_DESTINATION, int IconSourceIndex = ICON_INDEX_DEFAULT) {
            var FolderPath = LinkFolder.Path();
            return Create(FolderPath, DestPath, DestArguments, IconSource, IconSourceIndex);
        }


        public static ShellLink Create(System.Environment.SpecialFolder LinkFolder, string DestPath, string DestArguments = DEST_ARGUMENTS_NONE, string IconSource = ICON_FROM_DESTINATION, int IconSourceIndex = ICON_INDEX_DEFAULT) {
            var FolderPath = LinkFolder.Path();
            return Create(FolderPath, DestPath, DestArguments, IconSource, IconSourceIndex);
        }

        public static ShellLink Create(string LinkFolder, string DestPath, string DestArguments = DEST_ARGUMENTS_NONE, string IconSource = ICON_FROM_DESTINATION, int IconSourceIndex = ICON_INDEX_DEFAULT) {
            var FileInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(DestPath);
            var LinkName = FileInfo.ProductName;
            var LinkDescription = FileInfo.FileDescription;

            return Create(LinkName, LinkFolder, LinkDescription, DestPath, DestArguments, IconSource, IconSourceIndex);
        }

        /// <summary>
        /// Creates a shortcut file for the given executable
        /// </summary>
        /// <param name="LinkName">The file name to be used for the shortcut.  If it does not end with .lnk, then .lnk will be appended.</param>
        /// <param name="LinkFolder">The folder the shortcut will be created in.</param>
        /// <param name="LinkDescription">The description for the shortcut.</param>
        /// <param name="DestPath">The full path to the file the shortcut will open.</param>
        /// <param name="DestArguments">Any arguments that will be used when launching the file.</param>
        /// <param name="IconSource">The icon that will be used.  If not provided, the icon will be the default icon for the DestPath.</param>
        /// <param name="IconSourceIndex">The icon index that will be used.  If not provided, the default icon index will be used.</param>
        /// <returns>The created shortcut file</returns>
        public static ShellLink Create(string LinkName, BadgerFolder LinkFolder, string LinkDescription, string DestPath, string DestArguments = DEST_ARGUMENTS_NONE, string IconSource = ICON_FROM_DESTINATION, int IconSourceIndex = ICON_INDEX_DEFAULT) {
            var FolderPath = LinkFolder.Path();

            return Create(LinkName, FolderPath, LinkDescription, DestPath, DestArguments, IconSource, IconSourceIndex);
        }


        /// <summary>
        /// Creates a shortcut file for the given executable
        /// </summary>
        /// <param name="LinkName">The file name to be used for the shortcut.  If it does not end with .lnk, then .lnk will be appended.</param>
        /// <param name="LinkFolder">The folder the shortcut will be created in.</param>
        /// <param name="LinkDescription">The description for the shortcut.</param>
        /// <param name="DestPath">The full path to the file the shortcut will open.</param>
        /// <param name="DestArguments">Any arguments that will be used when launching the file.</param>
        /// <param name="IconSource">The icon that will be used.  If not provided, the icon will be the default icon for the DestPath.</param>
        /// <param name="IconSourceIndex">The icon index that will be used.  If not provided, the default icon index will be used.</param>
        /// <returns>The created shortcut file</returns>
        public static ShellLink Create(string LinkName, System.Environment.SpecialFolder LinkFolder, string LinkDescription, string DestPath, string DestArguments = DEST_ARGUMENTS_NONE, string IconSource = ICON_FROM_DESTINATION, int IconSourceIndex = ICON_INDEX_DEFAULT) {
            var FolderPath = LinkFolder.Path();

            return Create(LinkName, FolderPath, LinkDescription, DestPath, DestArguments, IconSource, IconSourceIndex);
        }



        /// <summary>
        /// Creates a shortcut file for the given executable
        /// </summary>
        /// <param name="LinkName">The file name to be used for the shortcut.  If it does not end with .lnk, then .lnk will be appended.</param>
        /// <param name="LinkFolder">The folder the shortcut will be created in.</param>
        /// <param name="LinkDescription">The description for the shortcut.</param>
        /// <param name="DestPath">The full path to the file the shortcut will open.</param>
        /// <param name="DestArguments">Any arguments that will be used when launching the file.</param>
        /// <param name="IconSource">The icon that will be used.  If not provided, the icon will be the default icon for the DestPath.</param>
        /// <param name="IconSourceIndex">The icon index that will be used.  If not provided, the default icon index will be used.</param>
        /// <returns>The created shortcut file</returns>
        public static ShellLink Create(string LinkName, string LinkFolder, string LinkDescription, string DestPath, string DestArguments = DEST_ARGUMENTS_NONE, string IconSource = ICON_FROM_DESTINATION, int IconSourceIndex = ICON_INDEX_DEFAULT) {
            var LinkFullPath = System.IO.Path.Combine(LinkFolder, LinkName);

            return CreateFullPath(LinkFullPath, LinkDescription, DestPath, DestArguments, IconSource, IconSourceIndex);
        }

        /// <summary>
        /// Creates a shortcut file for the given executable
        /// </summary>
        /// <param name="LinkFullPath">The full path to the file name to be used for the shortcut.  If it does not end with .lnk, then .lnk will be appended.</param>
        /// <param name="LinkDescription">The description for the shortcut.</param>
        /// <param name="DestPath">The full path to the file the shortcut will open.</param>
        /// <param name="DestArguments">Any arguments that will be used when launching the file.</param>
        /// <param name="IconSource">The icon that will be used.  If not provided, the icon will be the default icon for the DestPath.</param>
        /// <param name="IconSourceIndex">The icon index that will be used.  If not provided, the default icon index will be used.</param>
        /// <returns>The created shortcut file</returns>
        public static ShellLink CreateFullPath(string LinkFullPath, string LinkDescription, string DestPath, string DestArguments = DEST_ARGUMENTS_NONE, string IconSource = ICON_FROM_DESTINATION, int IconSourceIndex = ICON_INDEX_DEFAULT) {
            LinkFullPath = EnsureLinkName(LinkFullPath);

            var ret = new ShellLink() {
                Target = DestPath,
                IconPath = IconSource ?? DestPath,
                IconIndex = IconSourceIndex,
                WorkingDirectory = System.IO.Path.GetDirectoryName(DestPath),
                Description = LinkDescription
            };

            if (!String.IsNullOrWhiteSpace(DestArguments)) {
                ret.Arguments = $@" -a ""{DestArguments}""";
            }

            Delete(LinkFullPath);

            ret.Save(LinkFullPath);

            return ret;

        }

        /// <summary>
        /// Deletes a shortcut from the specified folder if it exists.
        /// </summary>
        /// <param name="LinkName">The name of the shortcut file.  If it does not end with .lnk then .lnk will be appended.</param>
        /// <param name="LinkFolder">The folder the shortcut file may exist in.</param>
        public static void Delete(string LinkName, System.Environment.SpecialFolder LinkFolder) {
            var FolderPath = LinkFolder.Path();

            Delete(LinkName, FolderPath);
        }

        /// <summary>
        /// Deletes a shortcut from the specified folder if it exists.
        /// </summary>
        /// <param name="LinkName">The name of the shortcut file.  If it does not end with .lnk then .lnk will be appended.</param>
        /// <param name="LinkFolder">The folder the shortcut file may exist in.</param>
        public static void Delete(string LinkName, string LinkFolder) {
            var LinkFullPath = System.IO.Path.Combine(LinkFolder, LinkName);

            Delete(LinkFullPath);
        }

        /// <summary>
        /// Deletes a shortcut if it exists.
        /// </summary>
        /// <param name="LinkFullPath">The full path to the shortcut file that should be deleted.  If it does not end with .lnk then .lnk will be appended.</param>
        public static void Delete(string LinkFullPath) {
            LinkFullPath = EnsureLinkName(LinkFullPath);

            if (System.IO.File.Exists(LinkFullPath)) {
                System.IO.File.Delete(LinkFullPath);
            }
        }


    }
}
