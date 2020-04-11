namespace Badger.Common {
    public static class BoolExtensions {
        public static bool TrueWhenNull(this bool? This) {
            return This ?? true;
        }

        public static bool FalseWhenNull(this bool? This) {
            return This ?? false;
        }

    }

}
