using System;
using Badger.Deployment.Servicing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Badger.Tests {
    [TestClass]
    public class ReleaseEntryTests {

        [TestMethod]
        public void Test0() {
            var Entry = TextPackageDefinition.ParseOne("");
            var Expected = default(TextPackageDefinition);

            Assert.AreEqual(Entry, Expected);
        }

        [TestMethod]
        public void Test1() {
            var Entry = TextPackageDefinition.ParseOne("0d777ea94c612e8bf1ea7379164caefba6e24463 myapp-1.0.1-delta.nupkg 6030# 50%");
            var Expected = new TextPackageDefinition() {
                SHA1 = "0d777ea94c612e8bf1ea7379164caefba6e24463",
                FileName = "myapp-1.0.1-delta.nupkg",
                FileSize = 6030,
                StagingPercentage = 50,
            };

            Assert.AreEqual(Entry, Expected);
        }

        [TestMethod]
        public void Test2() {
            var Entry = TextPackageDefinition.ParseOne("0d777ea94c612e8bf1ea7379164caefba6e24463 myapp-1.0.1-delta.nupkg 6030 # 50%");
            var Expected = new TextPackageDefinition() {
                SHA1 = "0d777ea94c612e8bf1ea7379164caefba6e24463",
                FileName = "myapp-1.0.1-delta.nupkg",
                FileSize = 6030,
                StagingPercentage = 50,
            };

            Assert.AreEqual(Entry, Expected);
        }

        [TestMethod]
        public void Test3() {
            var Entry = TextPackageDefinition.ParseOne("0d777ea94c612e8bf1ea7379164caefba6e24463 myapp-1.0.1-delta.nupkg 6030");
            var Expected = new TextPackageDefinition() {
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
