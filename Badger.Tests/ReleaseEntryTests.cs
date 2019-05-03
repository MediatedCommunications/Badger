using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Badger.Tests {
    [TestClass]
    public class ReleaseEntryTests {

        [TestMethod]
        public void Test0() {
            var Entry = PackageEntry.ParseOne("");
            var Expected = default(PackageEntry);

            Assert.AreEqual(Entry, Expected);
        }

        [TestMethod]
        public void Test1() {
            var Entry = PackageEntry.ParseOne("0d777ea94c612e8bf1ea7379164caefba6e24463 myapp-1.0.1-delta.nupkg 6030# 50%");
            var Expected = new PackageEntry() {
                SHA1 = "0d777ea94c612e8bf1ea7379164caefba6e24463",
                FileName = "myapp-1.0.1-delta.nupkg",
                FileSize = 6030,
                StagingPercentage = 50,
            };

            Assert.AreEqual(Entry, Expected);
        }

        [TestMethod]
        public void Test2() {
            var Entry = PackageEntry.ParseOne("0d777ea94c612e8bf1ea7379164caefba6e24463 myapp-1.0.1-delta.nupkg 6030 # 50%");
            var Expected = new PackageEntry() {
                SHA1 = "0d777ea94c612e8bf1ea7379164caefba6e24463",
                FileName = "myapp-1.0.1-delta.nupkg",
                FileSize = 6030,
                StagingPercentage = 50,
            };

            Assert.AreEqual(Entry, Expected);
        }

        [TestMethod]
        public void Test3() {
            var Entry = PackageEntry.ParseOne("0d777ea94c612e8bf1ea7379164caefba6e24463 myapp-1.0.1-delta.nupkg 6030");
            var Expected = new PackageEntry() {
                SHA1 = "0d777ea94c612e8bf1ea7379164caefba6e24463",
                FileName = "myapp-1.0.1-delta.nupkg",
                FileSize = 6030,
            };

            Assert.AreEqual(Entry, Expected);
        }

        /*
         
         

0d777ea94c612e8bf1ea7379164caefba6e24463 myapp-1.0.1-delta.nupkg 6030 # 50%
0d777ea94c612e8bf1ea7379164caefba6e24463 myapp-1.0.1-delta.nupkg 6030
         
         */

    }
}
