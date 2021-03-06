﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Badger.Tests {
    [TestClass]
    public class UninstallerTests {
        [TestMethod]
        public void TestMethod1() {
            var Default = Badger.Windows.Installation.UninstallerRegistry.Default;

            var AllApps = Default.Applications();

            Assert.IsTrue(AllApps.Length > 0);
        }

        [TestMethod]
        public void TestMethod2() {
            var Default = Badger.Windows.Installation.UninstallerRegistry.Default;

            Default.AddOrUpdate("Custom App", new Badger.Windows.Installation.UninstallerRegistryConfiguration() {
                DisplayIcon = "My Icon",
                DisplayName = "My Name",
                DisplayVersion = "1.2.3",
                EstimatedSizeInKb = 100 * 1024 * 1024,
                InstallDate = DateTime.Now,
                Language = 0x0409,
                InstallLocation = @"C:\Foo\",
                CanModify = Windows.Installation.CanModifyValue.True,
                CanRepair = Windows.Installation.CanRepairValue.False,
                Publisher = "My company",
                QuietUninstallString = "Quiet Uninstall",
                UninstallString = "Not so Quiet Uninstall",
                URLUpdateInfo = "http://www.Google.com"
            });

        }


    }
}
