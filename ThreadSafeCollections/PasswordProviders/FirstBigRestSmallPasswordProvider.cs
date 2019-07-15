namespace ThreadSafeCollections.PasswordProviders
{
    using System.Collections.Generic;
    using Extensions;

    public class FirstBigRestSmallPasswordProvider : IPasswordProvider
    {
        private readonly string[] _dictionary;

        public FirstBigRestSmallPasswordProvider(string[] dictionary)
        {
            _dictionary = dictionary;
        }

        public IEnumerable<string> Produce()
        {
            foreach (var word in _dictionary)
            {
                var password = word.FirstCharToUpper();
                yield return password;
            }
        }
    }
}
