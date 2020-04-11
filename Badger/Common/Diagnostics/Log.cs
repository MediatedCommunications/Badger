using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Common.Diagnostics {
    public static class Log {
        public static class Folder {
            public static void Create(string Description, string FolderPath, ILog Logger) {
                Logger.Debug($@"Create {Description} if it does not exist");
                Logger.Debug($@"       {FolderPath}");

                if (!Badger.Common.IO.Directory.Create(FolderPath, out var ex)) {
                    Logger.Error(ex);
                }
            }

            public static void Delete(string Description, string FolderPath, ILog Logger) {
                Logger.Debug($@"Delete {Description} if it does not exist");
                Logger.Debug($@"       {FolderPath}");

                if (!Badger.Common.IO.Directory.Delete(FolderPath, out var ex)) {
                    Logger.Error(ex);
                }
            }

            public static void Copy(string SourceDescription, string SourcePath, string DestDescription, string DestPath, ILog Logger) {

                Logger.Debug($@"Copy {SourceDescription} => {DestDescription}");
                Logger.Debug($@"  From: {SourcePath}");
                Logger.Debug($@"  To:   {DestPath}");
                if (!Badger.Common.IO.Directory.Copy(SourcePath, DestPath, out var ex)) {
                    Logger.Error(ex);
                }
            }

            public static void Copy(string SourceDescription, string SourcePath, string DestDescription, string DestPath, string ConditionDescription, Func<bool?> Condition, ILog Logger) {
                var ConditionValue = Condition?.Invoke();

                if (ConditionValue == true) {
                    Copy(SourceDescription, SourcePath, DestDescription, DestPath, Logger);
                } else {
                    Logger.Debug($@"Skipped: Copy: {SourceDescription} => {DestDescription} ({ConditionDescription} != true)");
                    Logger.Debug($@"           From: {SourcePath}");
                    Logger.Debug($@"           To:   {DestPath}");
                }
            }
        }

        public static class File {
            public static void Copy(string SourceDescription, string SourcePath, string DestDescription, string DestPath, ILog Logger) {

                Logger.Debug($@"Copy {SourceDescription} => {DestDescription}");
                Logger.Debug($@"  From: {SourcePath}");
                Logger.Debug($@"  To:   {DestPath}");
                if (!Badger.Common.IO.File.Copy(SourcePath, DestPath, true, out var ex)) {
                    Logger.Error(ex);
                }
            }

            public static void Delete(string Description, string FilePath, ILog Logger) {
                Logger.Debug($@"Delete {Description} if it does not exist");
                Logger.Debug($@"       {FilePath}");

                if (!Badger.Common.IO.File.Delete(FilePath, out var ex)) {
                    Logger.Error(ex);
                }
            }

        }

    }
}
