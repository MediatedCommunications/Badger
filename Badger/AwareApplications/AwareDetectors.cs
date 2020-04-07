using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.AwareApplications {
    class AwareDetectors : AwareDetector {
        public static AwareDetector Instance { get; } = new AwareDetectors();

        public override IEnumerable<int?> SupportedVersions(string ExecutableFullPath) {
            var ret = new List<int?>();
            ret.AddRange(AwareDetector_Assembly.Instance.SupportedVersions(ExecutableFullPath));
            ret.AddRange(AwareDetector_VersionInfo.Instance.SupportedVersions(ExecutableFullPath));

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
