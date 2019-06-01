namespace Badger.Security {
    public class InvalidSHA1Exception : System.Exception {
        public InvalidSHA1Exception(string message) : base(message) {
        }

        public InvalidSHA1Exception(string Expected, string Provided) : base($@"The SHA1 was expected to be {Expected} but it was actually {Provided}.") {

        }
    }

}
