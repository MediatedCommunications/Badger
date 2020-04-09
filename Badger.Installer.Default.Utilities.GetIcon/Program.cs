using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Installer.Default.Utilities.GetIcon {
    public static class Program {
        static void Main(string[] args) {
            Badger.Diagnostics.Logging.ApplySimpleConfiguation();

            var Logger = log4net.LogManager.GetLogger("GetIcon");

            try {
                var Options = new GetFileIconParameters();


                var Parser = new Mono.Options.OptionSet() { 
                    {
                        nameof(Installer.GetFileIconParameters.Source_File),
                        "The full path to the executable that will be used for signing executables.",
                        x => {
                            Options.Source_File = x;
                        }
                    },

                    {
                        nameof(Installer.GetFileIconParameters.Dest_File),
                        "The full path to the executable that will be used for signing executables.",
                        x => {
                            Options.Dest_File = x;
                        }
                    },
                
                };

                Parser.Parse(args);

                Parser.WriteOptionDescriptions(Console.Out);

                ExtractTo(Options);




            } catch (Exception ex) {
                Logger.Error(ex);
            }


        }

        private static void ExtractTo(GetFileIconParameters options) {
            ExtractTo(options.Source_File, options.Dest_File);
        }

        public static void ExtractTo(string Source, string Destination) {

            using (var Output = System.IO.File.OpenWrite(Destination)) {
                Toolbelt.Drawing.IconExtractor.Extract1stIconTo(Source, Output);
            }

        }


    }
}
