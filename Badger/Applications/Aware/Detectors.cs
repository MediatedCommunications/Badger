using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Applications.Aware {
    class Detectors : Detector {
        public static Detector Instance { get; } = new Detectors();

        public override IEnumerable<int?> SupportedVersions(string ExecutableFullPath) {
            var ret = new List<int?>();
            ret.AddRange(Detector_Assembly.Instance.SupportedVersions(ExecutableFullPath));
            ret.AddRange(Detector_VersionInfo.Instance.SupportedVersions(ExecutableFullPath));

            ret = (
                from x in ret
                where x != null
                orderby x
                select x
                ).Distinct().ToList();

            return ret;
        }
    }
}
