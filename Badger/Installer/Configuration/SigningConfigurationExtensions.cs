namespace Badger.Installer {
    public static class SigningConfigurationExtensions {

        public static bool Sign(this SigningConfiguration This, string Assembly) {
            return This.Sign(Assembly, This.Certificate);
        }

        public static bool Sign(this SigningConfiguration This, string Assembly, string Certificate) {
            var Args = new SignAssemblyParameters() {
                Certificate = Certificate,
                Assembly = Assembly,
            };

            return This.Sign(Args);
        }

        public static bool Sign(this SigningConfiguration This, SignAssemblyParameters Args) {
            return This.Run(Args);
        }


    }

}
