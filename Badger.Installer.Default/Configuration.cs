using System;
using System.Collections.Generic;

namespace Badger.Installer.Default {

    public class ArchiveCommandParameters {
        public string Source_Folder { get; set; }
        public string Dest_File { get; set; }
    }

    public class SignCommandParameters {
        public string Files { get; set; }
        public string Certificate { get; set; }
    }

    public class Configuration : IConfiguration {


    }

    public class IConfiguration {
        public bool? Default_Sign { get; set; }
        public string Default_Sign_Command { get; set; }
        public string Default_Sign_Certificate { get; set; }
        public string Default_Sign_ParameterTemplate { get; set; }

        public string Package_Folder { get; set; }
        public bool? Package_Folder_Copy { get; set; }
        public Version Package_Version { get; set; }
        public string Package_Name { get; set; }

        public string Redirector_Stub_Path { get; set; }
        public bool? Redirector_Stub_Create { get; set; }

        public bool? Redirector_Sign { get; set; }
        public string Redirector_Sign_Command { get; set; }
        public string Redirector_Sign_Certificate { get; set; }
        public string Redirector_Sign_ParameterTemplate { get; set; }

        public bool? Archive_Create { get; set; }
        public string Archive_Create_Command { get; set; }
        public string Archive_Create_ParameterTemplate { get; set; }

        public bool? Archive_Sign { get; set; }
        public string Archive_Sign_Command { get; set; }
        public string Archive_Sign_Certificate { get; set; }
        public string Archive_Sign_ParameterTemplate { get; set; }

        public bool? Application_Sign { get; set; }
        public string Application_Sign_Command { get; set; }
        public string Application_Sign_Certificate { get; set; }
        public string Application_Sign_ParameterTemplate { get; set; }


        public bool? Installer_Create { get; set; }
        public string Installer_Stub_Icon { get; set; }
        public string Installer_Stub_Path { get; set; }

        public bool? Installer_Sign { get; set; }
        public string Installer_Sign_Command { get; set; }
        public string Installer_Sign_Certificate { get; set; }
        public string Installer_Sign_ParameterTemplate { get; set; }

        public string Installer_SplashScreen_Image { get; set; }
        public string Installer_Install_Subfolder { get; set; }
        public bool? Installer_Install_Location_Kill { get; set; }
        public bool? Installer_Install_Location_Clean { get; set; }
        public string Installer_Install_Content_ParameterTemplate { get; set; }
        public List<InstallScript> Installer_Install_Location_Scripts { get; private set; } = new List<InstallScript>();

        public string Installer_Output_Name_ParameterTemplate { get; set; }
        public string Installer_Output_Directory { get; set; }


        public string WorkingFolder_Path { get; set; }
        public bool? WorkingFolder_Clean { get; set; }


    }




    public static class ConfigurationExtensions {
        public static void ApplyDefaults(this Configuration This) {

            var AppPath = Utility.AppPath;

            var Default_Sign_Command = Utilities.SignTool.ExecutablePath;
            //This usually won't work because additional options will need to be specified.
            //Sign using the best available certificate
            var Default_Sign_Parameters = Utilities.SignTool.SignParameterTemplate($@"{{{nameof(SignCommandParameters.Files)}}}");

            var Default_Sign_Certificate = "";


            //Working Folder
            if (This.WorkingFolder_Clean == null) {
                This.WorkingFolder_Clean = true;
            }

            if (This.WorkingFolder_Path.IsNullOrEmpty()) {
                var LocalPath = System.IO.Path.GetDirectoryName(Badger.Diagnostics.Application.EntryAssembly.Location);

                This.WorkingFolder_Path = System.IO.Path.Combine(LocalPath, "WorkingFolder", This.Package_Name, This.Package_Version.ToString());
            }

            //Default Signing
            This.Default_Sign ??= false;
            This.Default_Sign_Command ??= Default_Sign_Command;
            This.Default_Sign_Certificate ??= Default_Sign_Certificate;
            This.Default_Sign_ParameterTemplate ??= Default_Sign_Parameters;

            //Package Folder Stuff
            This.Package_Folder_Copy ??= true;


            //Redirector Signing
            This.Redirector_Sign ??= This.Default_Sign;
            This.Redirector_Sign_Command ??= This.Default_Sign_Command;
            This.Redirector_Sign_Certificate ??= This.Default_Sign_Certificate;
            This.Redirector_Sign_ParameterTemplate ??= This.Default_Sign_ParameterTemplate;

            //Redirector Sources
            This.Redirector_Stub_Create ??= true;
            This.Redirector_Stub_Path ??= typeof(Badger.Redirector.Default.StubExecutable.Program).Assembly.Location;


            //App Signing
            This.Application_Sign ??= This.Default_Sign;
            This.Application_Sign_Command ??= This.Default_Sign_Command;
            This.Application_Sign_Certificate ??= This.Default_Sign_Certificate;
            This.Application_Sign_ParameterTemplate ??= This.Default_Sign_ParameterTemplate;

            //Archive Creation
            This.Archive_Create ??= true;
            This.Archive_Create_Command ??= Utilities.SevenZip.ExecutablePath;
            This.Archive_Create_ParameterTemplate ??= Utilities.SevenZip.CreateSelfExtractingArchiveParameterTemplate;


            //Archive Signing
            This.Archive_Sign ??= This.Default_Sign;
            This.Archive_Sign_Command ??= This.Default_Sign_Command;
            This.Archive_Sign_Certificate ??= This.Default_Sign_Certificate;
            This.Archive_Sign_ParameterTemplate ??= This.Default_Sign_ParameterTemplate;


            //Installer Signing
            This.Installer_Sign ??= This.Default_Sign;

            This.Installer_Sign_Command ??= This.Default_Sign_Command;

            This.Installer_Sign_Certificate ??= This.Default_Sign_Certificate;
            This.Installer_Sign_ParameterTemplate ??= This.Default_Sign_ParameterTemplate;

            //Installer Stuff
            This.Installer_Create ??= true;

            This.Installer_Stub_Path ??= typeof(Badger.Installer.Default.StubExecutable.Program).Assembly.Location;

            This.Installer_Install_Subfolder ??= This.Package_Name;

            This.Installer_Install_Location_Kill ??= true;
            This.Installer_Install_Location_Clean ??= true;

            This.Installer_Install_Content_ParameterTemplate ??= Utilities.SevenZip.ExtractArchiveToLocationParameterTemplate;

            This.Installer_Output_Directory ??= System.Environment.CurrentDirectory;
            This.Installer_Output_Name_ParameterTemplate ??= InstallerOutputParameters.DefaultTemplate;

        }


        public static void Display(this IConfiguration This, System.IO.TextWriter TW, string Prefix = null) {
            Prefix = Prefix ?? "";

            Prefix = "--" + Prefix;

            TW.WriteLine($"{Prefix}{nameof(IConfiguration.Default_Sign_Command)}:\t{This.Default_Sign_Command}");

            TW.WriteLine($"{Prefix}{nameof(IConfiguration.Package_Name)}:\t{This.Package_Name}");
            TW.WriteLine($"{Prefix}{nameof(IConfiguration.Package_Folder)}:\t{This.Package_Folder}");
            TW.WriteLine($"{Prefix}{nameof(IConfiguration.Package_Version)}:\t{This.Package_Version}");

            TW.WriteLine($"{Prefix}{nameof(IConfiguration.Redirector_Stub_Path)}:\t{This.Redirector_Stub_Path}");
            TW.WriteLine($"{Prefix}{nameof(IConfiguration.Application_Sign_Command)}:\t{This.Application_Sign_Command}");

            TW.WriteLine($"{Prefix}{nameof(IConfiguration.Installer_Stub_Icon)}:\t{This.Installer_Stub_Icon}");
            TW.WriteLine($"{Prefix}{nameof(IConfiguration.Installer_Stub_Path)}:\t{This.Installer_Stub_Path}");
            TW.WriteLine($"{Prefix}{nameof(IConfiguration.Installer_Sign_Command)}:\t{This.Installer_Sign_Command}");

            TW.WriteLine($"{Prefix}{nameof(IConfiguration.WorkingFolder_Path)}:\t{This.WorkingFolder_Path}");
            TW.WriteLine($"{Prefix}{nameof(IConfiguration.WorkingFolder_Clean)}:\t{This.WorkingFolder_Clean}");
        }


        public static bool Validate(this IConfiguration This, out List<Exception> Errors) {
            Errors = new List<Exception>();

            if (This.Package_Name.IsNullOrEmpty()) {
                Errors.Add(new ArgumentNullException(nameof(This.Package_Name)));
            }

            if (This.Package_Folder.IsNullOrEmpty()) {
                Errors.Add(new ArgumentNullException(nameof(This.Package_Folder)));
            }

            if (This.Package_Version == null) {
                Errors.Add(new ArgumentNullException(nameof(This.Package_Version)));
            }

            if (!System.IO.Directory.Exists(This.Package_Folder)) {
                Errors.Add(new System.IO.DirectoryNotFoundException($@"The folder specified for {nameof(This.Package_Folder)} ({This.Package_Folder}) does not exist"));
            }


            var ret = Errors.Count == 0;

            return ret;
        }

    }

}
