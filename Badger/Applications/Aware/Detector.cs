using System;
using System.Collections.Generic;
using System.Linq;

namespace Badger.Applications.Aware {

    abstract class Detector {
        /// <summary>
        /// Returns all squirrel versions that the specified executable is aware of.
        /// </summary>
        /// <param name="Executable">The full path to the executable to query</param>
        /// <returns></returns>
        public abstract IEnumerable<int?> SupportedVersions(string Executable);

        public bool IsAware(string Executable, Func<int, bool> VersionCondition, out List<int> MatchingVersions) {
            MatchingVersions = (
                from Version in SupportedVersions(Executable)
                where Version != null
                let VersionValue = Version.Value
                where VersionCondition?.Invoke(VersionValue) ?? true
                select VersionValue
                ).ToList();
               
            var ret = MatchingVersions.Any();

            return ret;
        }

        public bool IsAware(string Executable, Func<int, bool> VersionCondition) {
            return IsAware(Executable, VersionCondition, out _);
        }


        public bool IsAware(string Executable, int MinimumVersion, out List<int> MatchingVersions) {
            return IsAware(Executable, x => x >= MinimumVersion, out MatchingVersions);
        }

        public bool IsAware(string Executable, int MinimumVersion) {
            return IsAware(Executable, MinimumVersion, out _);
        }

        public bool IsAware(string Executable) {
            return IsAware(Executable, null, out _);
        }

        static Detector Instance { get; } = Detectors.Instance;
    }
}
