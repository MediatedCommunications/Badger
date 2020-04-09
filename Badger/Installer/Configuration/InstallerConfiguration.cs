﻿namespace Badger.Installer {
    public class InstallerConfiguration : ConfigurationBase {
        public InstallerContentConfiguration Content { get; set; } = new InstallerContentConfiguration();
        public InstallerContentConfiguration Stub { get; set; } = new InstallerContentConfiguration();
        public InstallerContentConfiguration Icon { get; set; } = new InstallerContentConfiguration();
        public InstallerContentConfiguration SplashScreen { get; set; } = new InstallerContentConfiguration();

        public InstallerContentPlaybookConfiguration Playbook { get; set; } = new InstallerContentPlaybookConfiguration();

        public SigningConfiguration SignUsing { get; set; } = new SigningConfiguration();

        protected override string DebuggerDisplay {
            get {
                return this.DebuggerDisplay();
            }
        }
    }

}
