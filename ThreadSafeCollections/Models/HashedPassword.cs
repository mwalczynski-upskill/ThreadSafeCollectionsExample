namespace ThreadSafeCollections.Models
{
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public class HashedPassword
    {
        private readonly string _password;
        public string HashedPass { get; }

        public HashedPassword(string pass)
        {
            _password = pass;
            HashedPass = Md5Hash(pass);
        }

        public string Md5Hash(string input)
        {
            using (var md5Provider = new MD5CryptoServiceProvider())
            {
                var bytes = md5Provider.ComputeHash(new UTF8Encoding().GetBytes(input));

                var hashedString = bytes.Aggregate(new StringBuilder(), (result, b) => result.Append(b.ToString("x2"))).ToString();
                return hashedString;
            }
        }

        public override string ToString()
        {
            return _password + " (" + HashedPass + ")";
        }
    }
}
