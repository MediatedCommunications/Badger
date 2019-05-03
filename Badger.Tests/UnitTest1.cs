using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Badger.Tests {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestMethod1() {
            Badger.Shell.Shortcuts.Create("The NotePad", System.Environment.SpecialFolder.Desktop, "The best app in the world!", $@"C:\Windows\System32\notepad.exe");

        }
    }
}
