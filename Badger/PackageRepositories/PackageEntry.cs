using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Badger {
    [DebuggerDisplay(Debugger2.DISPLAY)]
    public class PackageEntry {
        public string SHA1 { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public int? StagingPercentage { get; set; }


        private const string SHA = "SHA";
        private const string FILENAME = "FILENAME";
        private const string SIZE = "SIZE";
        private const string STAGINGPERCENTAGE = "PERCENT";
            
        private static readonly string RegexPattern = $@"(?<{SHA}>[0-9a-f]{{5,40}})(\s*)(?<{FILENAME}>\S+)(\s*)(?<{SIZE}>[0-9]+)(\s*)(#(\s*)(?<{STAGINGPERCENTAGE}>[0-9]{{2}})\s*%\s*)?";
        private static readonly Regex Regex = new Regex(RegexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static IEnumerable<PackageEntry> ParseMany(string Text) {
            var LineChars = new[] { '\r', '\n' };
            var Lines = Text.Split(LineChars, StringSplitOptions.RemoveEmptyEntries);
            return ParseMany(Lines);
        }

        public static IEnumerable<PackageEntry> ParseMany(string[] Lines) {
            var ret = (
                from Line in Lines
                let Parsed = ParseOne(Line)
                where Parsed != null
                select Parsed
                );

            return ret;
        }

        public static PackageEntry ParseOne(string Line) {
            var ret = default(PackageEntry);

            var Match = Regex.Match(Line);
            if (Match.Success) {
                try {
                    var pret = new PackageEntry();
                    pret.SHA1 = Match.Groups[SHA].Value;

                    pret.FileName = Match.Groups[FILENAME].Value;

                    if (long.TryParse(Match.Groups[SIZE].Value, out var PFileSize)){
                        pret.FileSize = PFileSize;
                    }

                    if (int.TryParse(Match.Groups[STAGINGPERCENTAGE].Value, out var PStagingPercentage)) {
                        pret.StagingPercentage = PStagingPercentage;
                    }

                    ret = pret;
                } catch (Exception ex) {
                    ex.Ignore();
                }
            }

            return ret;
        }

        public override string ToString() {
            var ret = StagingPercentage == null
                ? $@"{SHA1} {FileName} {FileSize}"
                : $@"{SHA1} {FileName} {FileSize}# {StagingPercentage:00}%"
                ;

            return ret;
        }

        protected virtual string DebuggerDisplay {
            get {
                return ToString();
            }
        }


        public static bool operator ==(PackageEntry A, PackageEntry B) {
            var ret = true
                && A?.SHA1 == B?.SHA1
                && A?.FileName == B?.FileName
                && A?.FileSize == B?.FileSize
                && A?.StagingPercentage == B?.StagingPercentage
                ;

            return ret;
        }

        public static bool operator !=(PackageEntry A, PackageEntry B) {
            return !(A == B);
        }

        public override bool Equals(object obj) {
            return obj is PackageEntry entry &&
                   this.SHA1 == entry.SHA1 &&
                   this.FileName == entry.FileName &&
                   this.FileSize == entry.FileSize &&
                   EqualityComparer<int?>.Default.Equals(this.StagingPercentage, entry.StagingPercentage);
        }

        public override int GetHashCode() {
            var hashCode = 631203604;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.SHA1);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.FileName);
            hashCode = hashCode * -1521134295 + EqualityComparer<long>.Default.GetHashCode(this.FileSize);
            hashCode = hashCode * -1521134295 + EqualityComparer<int?>.Default.GetHashCode(this.StagingPercentage);
            return hashCode;
        }
    }
}
