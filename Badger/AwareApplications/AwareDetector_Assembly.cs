using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Badger.AwareApplications {
    class AwareDetector_Assembly : AwareDetector {

        public static AwareDetector Instance { get; } = new AwareDetector_Assembly();

        public override IEnumerable<int?> SupportedVersions(string ExecutableFullPath) {
            var Values = default(IEnumerable<int?>);
            try {
                if (System.IO.File.Exists(ExecutableFullPath)) {
                    var assembly = AssemblyDefinition.ReadAssembly(ExecutableFullPath);
                    Values = (
                        from x in assembly.CustomAttributes
                        where true
                            && x.AttributeType.FullName == typeof(AssemblyMetadataAttribute).FullName
                            && x.ConstructorArguments.Count == 2
                            && String.Equals(x.ConstructorArguments[0].Value.ToString(), Metadata.AssemblyMetadataAttributeKey, StringComparison.InvariantCultureIgnoreCase)
                        let Version = TryParseInt(x.ConstructorArguments[1].Value.ToString(), NumberStyles.Integer, CultureInfo.CurrentCulture)
                        where Version != null
                        select Version
                        ).ToList();
                }
            } catch (Exception ex) {
                ex.Ignore();
            }

            if (Values != null) {
                foreach (var item in Values) {
                    yield return item;
                }
            }

        }

        private static int? TryParseInt(string Value, NumberStyles NumberStyle, CultureInfo Culture) {
            var ret = default(int?);
            if (int.TryParse(Value, NumberStyle, Culture, out var parsed)) {
                ret = parsed;
            }

            return ret;
        }

    }
}
