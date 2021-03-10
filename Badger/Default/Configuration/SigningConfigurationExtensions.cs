namespace Badger.Default.Configuration {
    public static class SigningConfigurationExtensions {

        public static bool Sign(this ISignUsing This, string Assembly) {
            return This.Sign(Assembly, This.SignUsing.Certificate);
        }

        public static bool Sign(this ISignUsing This, string Assembly, string Certificate) {
            var Args = new SignAssemblyParameters() {
                Certificate = Certificate,
                Assembly = Assembly,
            };

            return This.Sign(Args);
        }

        public static bool Sign(this ISignUsing This, SignAssemblyParameters Args) {
            return This.SignUsing.Run(Args);
        }


    }

}
