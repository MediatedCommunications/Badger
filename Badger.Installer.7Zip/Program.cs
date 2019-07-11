using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Badger.Installer._7Zip {
    class Program {




        static void Main(string[] args) {
            var Options = new Configuration();

            var OptionSpecified = "  Option Specified: ";
            var OptionValue = "    ";

            var Parser = new Mono.Options.OptionSet() {
                {
                    nameof(Configuration.Default_Sign_Command),
                    "The full path to the executable that will be used for signing executables.",
                    x => {
                        Options.Default_Sign_Command = x.Trim();
                        Options.Default_Sign = true;
                    }
                },
                {
                    nameof(Configuration.Default_Sign_Parameters),
                    "The parameters used for signing",
                    x => {
                        Options.Default_Sign_Parameters = x.Trim();
                        Options.Default_Sign = true;
                    }
                },

                {
                    nameof(Configuration.Package_Name),
                    "The name of the package that is being created.",
                    x => {
                        Options.Package_Name = x.Trim();
                    }
                },

                {
                    nameof(Configuration.Package_Folder),
                    "The source folder that contains all of the files that will be distributed",
                    x => {
                        Options.Package_Folder = x.Trim();
                    }
                },
                {
                    nameof(Configuration.Package_Version),
                    "The version number of the application",
                    x => {
                        if(Version.TryParse(x, out var V)) {
                            Options.Package_Version = V;
                        } else {
                            Console.WriteLine($@"Unable to parse '{x}' as a valid version number.");
                        }
                    }
                },
                {
                    nameof(Configuration.Stub_Path),
                    "The full path to the stub executable that will be used as the template for launching applications.",
                    x => {
                        Options.Stub_Path = x.Trim();
                    }
                },
                {
                    nameof(Configuration.Application_Sign_Command),
                    "The command that will be used for signing",
                    x => {
                        Options.Application_Sign_Command = x.Trim();
                    }
                },

                {
                    nameof(Configuration.Installer_Icon),
                    "An ICO file to be used as the installer icon.",
                    x => {
                        Options.Installer_Icon = x.Trim();
                    }
                },
                {
                    nameof(Configuration.Installer_Sign_Command),
                    "The command that will be used for signing",
                    x => {
                        Options.Installer_Sign_Command = x.Trim();
                    }
                },

                {
                    nameof(Configuration.WorkingFolder_Path),
                    "The temporary folder that files will be copied to.",
                    x => {
                        Options.WorkingFolder_Path = x.Trim();
                    }
                },

                {
                    nameof(Configuration.WorkingFolder_Clean),
                    "Whether to erase the contents of the working folder before starting",
                    x => {
                        if(bool.TryParse(x, out var v)) {
                            Options.WorkingFolder_Clean = v;
                        } else {
                            Console.WriteLine($@"Unable to parse '{x}' as a valid boolean.");
                        }
                        
                    }
                },
                {
                    nameof(Configuration.Archive_Command),
                    "The full path to the application that will create a self-extracting EXE archive.  If not provided, the included 7-Zip package will be used.",
                    x => {
                        Options.Archive_Command = x;
                    }
                },
                {
                    nameof(Configuration.Archive_Parameters),
                    $@"The command line arguments that will be provided to the {nameof(Configuration.Archive_Command)}.  The following variables should be used in the command: {{{nameof(ArchiveCommandParameters.Source_Folder)}}} {{{nameof(ArchiveCommandParameters.Dest_File)}}}",
                    x => {
                        Options.Archive_Parameters = x;
                    }
                }


            };


            Options.Package_Version = new Version(3, 2, 1);
            Options.Package_Name = "Faster Suite";
            Options.Package_Folder = $@"C:\Users\TonyValenti\source\repos\MediatedCommunications\AlphaDrive\__BUILD\Current";

            Options.Default_Sign = true;
            Options.Default_Sign_Parameters = $@"sign /f ""C:\Users\TonyValenti\source\repos\MediatedCommunications\AlphaDrive\ASSEMBLIES\__CODESIGNINGCERTIFICATES\mediated_communications_inc.pfx"" /tr http://timestamp.digicert.com /td SHA256 /fd SHA256 /v ""{{{nameof(SignCommandParameters.File)}}}""";

            Options.ApplyDefaults();


            var Errors = Options.Validate();
            if(Errors.Count != 0) {
                foreach (var item in Errors) {
                    Console.WriteLine(item.Message);
                }
            } else {
                Options.Execute();
            }
          

            

        }
    }

}
