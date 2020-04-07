using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Badger.Tests {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestMethod1() {
            Badger.Windows.Shell.Shortcuts.Create("The NotePad", System.Environment.SpecialFolder.Desktop, "The best app in the world!", $@"C:\Windows\System32\notepad.exe");
        }

        [TestMethod]
        public async Task TestDownload() {
            var Repo = new Badger.HttpPackageRepository("http://Get.FasterLaw.com/AlphaDrive/Windows/Stable");
            var Update = await Repo.CheckForUpdate();
            var Entries = await Repo.AvailablePackages();
            var ToDownload = Entries.FirstOrDefault();

            var Package = await Repo.AcquirePackage(ToDownload);
            Package.Execute();

        }

    }
}
