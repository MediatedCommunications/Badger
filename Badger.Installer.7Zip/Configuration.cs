using System;
using System.Collections.Generic;

namespace Badger.Installer._7Zip {

    public class ArchiveCommandParameters {
        public string Source_Folder { get; set; }
        public string Dest_File { get; set; }
    }

    public class SignCommandParameters {
        public string File { get; set; }
    }

    public class Configuration : IConfiguration {


    }

    public class IConfiguration {
        public bool? Default_Sign { get; set; }
        public string Default_Sign_Command { get; set; }
        public string Default_Sign_Parameters { get; set; }

        public string Package_Folder { get; set; }
        public Version Package_Version { get; set; }
        public string Package_Name { get; set; }

        public string Stub_Path { get; set; }
        public bool? Stub_Sign { get; set; }
        public string Stub_Sign_Command { get; set; }
        public string Stub_Sign_Parameters { get; set; }

        public bool? Application_Sign { get; set; }
        public string Application_Sign_Command { get; set; }
        public string Application_Sign_Parameters { get; set; }

        public string Installer_Icon { get; set; }
        public string Installer_Stub { get; set; }

        public bool? Installer_Sign { get; set; }
        public string Installer_Sign_Command { get; set; }
        public string Installer_Sign_Parameters { get; set; }


        public string WorkingFolder_Path { get; set; }
        public bool? WorkingFolder_Clean { get; set; }

        public string Archive_Command { get; set; }
        public string Archive_Parameters { get; set; }

        public bool? Archive_Sign { get; set; }
        public string Archive_Sign_Command { get; set; }
        public string Archive_Sign_Parameters { get; set; }


    }




    public static class ConfigurationExtensions {
        public static void ApplyDefaults(this Configuration This) {
            var AppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            var ResourcesPath = System.IO.Path.Combine(AppPath, "Resources");

            var Default_Sign_Command = System.IO.Path.Combine(ResourcesPath, "SignTool", "SignTool.exe");
            //This usually won't work because additional options will need to be specified.
            //Sign using the best available certificate
            var Default_Sign_Parameters = $@"sign /a ""{{{nameof(SignCommandParameters.File)}}}""";
            
            //Default Signing
            if(This.Default_Sign == null) {
                This.Default_Sign = false;
            }

            if (This.Default_Sign_Command.IsNullOrEmpty()) {
                This.Default_Sign_Command = Default_Sign_Command;
            }

            if (This.Default_Sign_Parameters.IsNullOrEmpty()) {
                This.Default_Sign_Parameters = Default_Sign_Parameters;
            }

            //App Signing
            if(This.Application_Sign == null) {
                This.Application_Sign = This.Default_Sign;
            }

            if (This.Application_Sign_Command.IsNullOrEmpty()) {
                This.Application_Sign_Command = This.Default_Sign_Command;
            }

            if (This.Application_Sign_Parameters.IsNullOrEmpty()) {
                This.Application_Sign_Parameters = This.Default_Sign_Parameters;
            }

            //Archive Signing
            if(This.Archive_Sign == null) {
                This.Archive_Sign = This.Default_Sign;
            }

            if (This.Archive_Sign_Command.IsNullOrEmpty()) {
                This.Archive_Sign_Command = This.Default_Sign_Command;
            }

            if (This.Archive_Sign_Parameters.IsNullOrEmpty()) {
                This.Archive_Sign_Parameters = This.Default_Sign_Parameters;
            }

            //Installer Signing
            if(This.Installer_Sign == null) {
                This.Installer_Sign = This.Default_Sign;
            }

            if (This.Installer_Sign_Command.IsNullOrEmpty()) {
                This.Installer_Sign_Command = This.Default_Sign_Command;
            }

            if (This.Installer_Sign_Parameters.IsNullOrEmpty()) {
                This.Installer_Sign_Parameters = This.Default_Sign_Parameters;
            }






            if (This.WorkingFolder_Clean == null) {
                This.WorkingFolder_Clean = true;
            }

            if (This.WorkingFolder_Path.IsNullOrEmpty()) {
                var LocalPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

                This.WorkingFolder_Path = System.IO.Path.Combine(LocalPath, "WorkingFolder", This.Package_Name, This.Package_Version.ToString());
            }

            if (This.Archive_Command.IsNullOrEmpty()) {
                This.Archive_Command = System.IO.Path.Combine(ResourcesPath, "7Zip", "7z.exe");
            }

            if (This.Archive_Parameters.IsNullOrEmpty()) {
                //Create a self extracting console EXE file.
                This.Archive_Parameters = $@"a -sfx7zcon.sfx ""{{{nameof(ArchiveCommandParameters.Dest_File)}}}"" ""{{{nameof(ArchiveCommandParameters.Source_Folder)}}}""";
            }



        }


        public static void Display(this IConfiguration This, System.IO.TextWriter TW, string Prefix = null) {
            Prefix = Prefix ?? "";

            Prefix = "--" + Prefix;

            TW.WriteLine($"{Prefix}{nameof(IConfiguration.Default_Sign_Command)}:\t{This.Default_Sign_Command}");

            TW.WriteLine($"{Prefix}{nameof(IConfiguration.Package_Name)}:\t{This.Package_Name}");
            TW.WriteLine($"{Prefix}{nameof(IConfiguration.Package_Folder)}:\t{This.Package_Folder}");
            TW.WriteLine($"{Prefix}{nameof(IConfiguration.Package_Version)}:\t{This.Package_Version}");

            TW.WriteLine($"{Prefix}{nameof(IConfiguration.Stub_Path)}:\t{This.Stub_Path}");
            TW.WriteLine($"{Prefix}{nameof(IConfiguration.Application_Sign_Command)}:\t{This.Application_Sign_Command}");

            TW.WriteLine($"{Prefix}{nameof(IConfiguration.Installer_Icon)}:\t{This.Installer_Icon}");
            TW.WriteLine($"{Prefix}{nameof(IConfiguration.Installer_Stub)}:\t{This.Installer_Stub}");
            TW.WriteLine($"{Prefix}{nameof(IConfiguration.Installer_Sign_Command)}:\t{This.Installer_Sign_Command}");

            TW.WriteLine($"{Prefix}{nameof(IConfiguration.WorkingFolder_Path)}:\t{This.WorkingFolder_Path}");
            TW.WriteLine($"{Prefix}{nameof(IConfiguration.WorkingFolder_Clean)}:\t{This.WorkingFolder_Clean}");
        }


        public static List<Exception> Validate(this IConfiguration This) {
            var ret = new List<Exception>();

            if (This.Package_Name.IsNullOrEmpty()) {
                ret.Add(new ArgumentNullException(nameof(This.Package_Name)));
            }

            if (This.Package_Folder.IsNullOrEmpty()) {
                ret.Add(new ArgumentNullException(nameof(This.Package_Folder)));
            }

            if (This.Package_Version == null) {
                ret.Add(new ArgumentNullException(nameof(This.Package_Version)));
            }

            if (!System.IO.Directory.Exists(This.Package_Folder)) {
                ret.Add(new System.IO.DirectoryNotFoundException($@"The folder specified for {nameof(This.Package_Folder)} ({This.Package_Folder}) does not exist"));
            }

            if (!System.IO.File.Exists(This.Archive_Command)) {
                ret.Add(new System.IO.FileNotFoundException($@"The application specified for {nameof(This.Archive_Command)} ({This.Archive_Command}) does not exist"));
            }

            return ret;
        }

    }

}
