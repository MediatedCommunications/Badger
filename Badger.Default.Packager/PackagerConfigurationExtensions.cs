using Badger.Common;
using Badger.Default.Configuration;

namespace Badger.Default.Packager {
    public static class PackagerConfigurationExtensions {
        public static PackagerConfiguration WithDefaults(this PackagerConfiguration This) {
            var AlteredConfiguration = new PackagerConfiguration() {
                Product = new ProductConfiguration { 
                    Code = new[] { This.Product.Publisher, This.Product.Name }.JoinDot() 
                },
                Installer = new InstallerConfiguration { 
                    SignUsing = This?.Defaults?.SignUsing 
                },
                Redirector = new RedirectorConfiguration { 
                    SignUsing = This?.Defaults?.SignUsing 
                },
                Uninstaller = new UninstallerConfiguration { 
                    SignUsing = This?.Defaults?.SignUsing,
                    Icon = This?.Installer?.Icon,
                },
                Archive = new ArchiveConfiguration { 
                    SignUsing = This?.Defaults?.SignUsing
                }
            };

            return AlteredConfiguration;
        }
    }

}
