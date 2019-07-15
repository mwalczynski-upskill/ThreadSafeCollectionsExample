namespace ThreadSafeCollections.PasswordProviders
{
    using System.Collections.Generic;

    public class SimpleBasePasswordProvider : IPasswordProvider
    {
        private readonly string[] _dictionary;

        public SimpleBasePasswordProvider(string[] dictionary)
        {
            _dictionary = dictionary;
        }

        public IEnumerable<string> Produce()
        {
            foreach (var word in _dictionary)
            {
                var password = word;
                yield return password;
            }
        }
    }
}
